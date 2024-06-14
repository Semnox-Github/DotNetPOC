/********************************************************************************************
 * Project Name - Parafait Kiosk- frmHome 
 * Description  - Home screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
*2.152.0.0   12-Dec-2023      Suraj Pai           Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.logger;
using Semnox.Parafait.Transaction;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Waivers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Semnox.Parafait.User;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Device;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parafait_Kiosk
{
    public partial class frmHome : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CardDispenser cardDispenser;
        string dispenserMessage = "";
        bool isDispenserCardReaderValid = false;
        Monitor kioskMonitor;
        Monitor cardDispenserMonitor;
        Monitor billAcceptorMonitor;
        ExecutionContext machineUserContext;
        private int homeAudioInterval = 0;
        private bool paymentGatewayInitializaztionError = false;

        public CommonBillAcceptor commonBillAcceptor;

        public Utilities utilities;
        public string cardNumber;
        public Card Card;

        public string entitlementType;

        //public delegate void IsHomeForm(bool isHome);
        //public IsHomeForm isHomeForm;
        private TagNumberParser tagNumberParser;
        private bool incorrectCustomerSetupForWaiver = true;
        private string waiverSetupErrorMsg = string.Empty;
        //private bool singleFunctionMode = true;
        private bool showCartInKiosk = false;
        private bool isPaymentModeDrivenSalesInKioskEnabled;
        private string msg1;
        private string msg2;
        private List<PaymentModesContainerDTO> paymentModestWithDisplayGroups = null;

        private int enabledProductMenuBtnCount = 0;
        private string enabledProductMenuBtnName = string.Empty;
        private const string NEW_CARD_MENU = "NEW CARD";
        private const string RECHARGE_CARD_MENU = "RECHARGE CARD";
        private const string CHECKIN_MENU = "CHECKIN";
        private const string ATTRACTION_MENU = "ATTRACTION";
        private const string FOOD_N_BEV_MENU = "FOOD AND BEV";

        public frmHome()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In frmHome");
            InitiateUtils();
            InitializeComponent();
            SetStyles();
            InitKiosk();
            //GetCoverPageConfigs();
            utilities.setLanguage();

            DoubleBuffered = true;
            //utilities = KioskStatic.Utilities;
            //machineUserContext = utilities.ExecutionContext;
            //utilities.setLanguage();
            //InitializeComponent();
            //utilities.setLanguage(this);

            showCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "SHOW_CART_IN_KIOSK", false);
            isPaymentModeDrivenSalesInKioskEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_PAYMENT_MODE_DRIVEN_SALES", false);
            msg1 = MessageContainerList.GetMessage(machineUserContext, 460);//Problem in Card Dispenser. Cannot issue new card.
            msg2 = MessageContainerList.GetMessage(machineUserContext, 441);//Please contact our staff
            paymentModestWithDisplayGroups = KioskHelper.GetPaymentModesWithDisplayGroups();
            HideApplicablePurchaseButtons();
            //utilities.setLanguage(this);
            try
            {
                if (System.IO.File.Exists(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png"))
                {
                    pbClientLogo.Image = Image.FromFile(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png");
                    pbClientLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            btnLanguage.Text = utilities.ParafaitEnv.LanguageName;

            if (utilities.getParafaitDefaults("ALLOW_KIOSK_LANGUAGE_CHANGE").Equals("N"))
                btnLanguage.Visible = false;

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            //GetCoverPageConfigs();
            InitKioskHomeScreen();
            utilities.setLanguage(this);
            //tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            log.LogMethodExit();
        }
        private void InitiateUtils()
        {
            log.LogMethodEntry();
            try
            {
                utilities = KioskStatic.Utilities = new Utilities();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("DB Connection Error" + ex.Message);
                log.Error(ex);
                Program.ShowTaskbar();
                Environment.Exit(0);
            }
            log.LogMethodExit();
        }
        private void SetStyles()
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            log.LogMethodExit();
        }
        void InitKiosk()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            CheckApplicationVersion();
            ValidateLicenceKey();
            SetKioskUserNMachineInfo();
            utilities.ParafaitEnv.Initialize();
            SetMachineContext();
            SetKioskVariableTopUpProduct();
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            KioskStatic.UpdateThemeTextFile();
            KioskStatic.get_config(machineUserContext, KioskStatic.TABLETOP_KIOSK_TYPE);
            SetTheme();
            SetClientLogo();
            KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1693));
            SetupDevices();
            StartKioskTimer();
            this.Activate();
            if (KioskStatic.debugMode == false)
            {
                Cursor.Hide();
            }
            lblSiteName.Text = KioskStatic.SiteHeading;
            KioskStatic.get_receipt();

        }

        private void ValidateLicenceKey()
        {
            log.LogMethodEntry();
            string keyMessage = "";
            KeyManagement km = new KeyManagement(KioskStatic.Utilities.DBUtilities, KioskStatic.Utilities.ParafaitEnv);
            if (!km.validateLicense(ref keyMessage))
            {
                MessageBox.Show(keyMessage, MessageContainerList.GetMessage(utilities.ExecutionContext, 1698));
                flpOptions.Enabled = false;
                txtMessage.Text = keyMessage;
            }
            else
            {
                txtMessage.Text = utilities.MessageUtils.getMessage(462);
                PlayHomeScreenAudio();
            }
            log.LogMethodExit();
        }

        private void SetKioskUserNMachineInfo()
        {
            log.LogMethodEntry();
            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch (Exception ex) { log.Error(ex); }

            try
            {
                KioskStatic.Utilities.ParafaitEnv.User_Id = Convert.ToInt32(utilities.executeScalar("select user_id from users where loginId = 'External POS'"));
                KioskStatic.Utilities.ParafaitEnv.LoginID = "External POS";
                if (utilities.ParafaitEnv.User_Id <= 0)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1699));
                    Program.ShowTaskbar();
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                log.Error("Please define External POS user id in users table", ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1699));
                Program.ShowTaskbar();
                Environment.Exit(1);
            }
            utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            log.LogMethodExit();

        }

        private void SetMachineContext()
        {
            log.LogMethodEntry();
            machineUserContext = utilities.ExecutionContext;
            log.LogMethodExit(machineUserContext);
        }

        private void SetKioskVariableTopUpProduct()
        {
            log.LogMethodEntry();
            object o = utilities.getParafaitDefaults("KIOSK_VARIABLE_TOPUP_PRODUCT");
            try
            {
                KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = Convert.ToInt32(o);
            }
            catch (Exception ex)
            {
                log.Error("KIOSK_VARIABLE_TOPUP_PRODUCT", ex);
                KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = -1;
            }

            if (KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId < 0)
            {
                o = utilities.executeScalar(@"select top 1 p.product_id 
                                                from products p, product_type pt 
                                               where pt.product_type_id = p.product_type_id 
                                                 and pt.product_type = 'VARIABLECARD'
                                                 and p.active_flag = 'Y' 											 
									             and p.product_name not in ('MembershipReward-Ticket','MembershipReward-Loyalty')
                                                 and ISNULL(p.StartDate,getdate()) >= getdate()
                                                 and isnull(p.ExpiryDate,getdate()) <= getdate()
                                                 and TaxInclusivePrice = 'Y'
                                            order by p.product_id");
                if (o != null)
                {
                    KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = Convert.ToInt32(o);
                }
                else
                    KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = -1;
            }
            log.LogMethodExit(machineUserContext);
        }

        private void SetClientLogo()
        {
            log.LogMethodEntry();
            try
            {
                if (pbClientLogo.Image == null &&
                    System.IO.File.Exists(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png"))
                {
                    pbClientLogo.Image = Image.FromFile(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png");
                }
                if (pbClientLogo.Image != null)
                    pbClientLogo.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }
        private void SetTheme()
        {
            log.LogMethodEntry();
            ThemeManager.setThemeImages(KioskStatic.CurrentTheme.ThemeId.ToString());
            this.lblSiteName.Visible = KioskStatic.CurrentTheme.ShowSiteHeading;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
            pbClientLogo.Image = ThemeManager.CurrentThemeImages.HomeScreenClientLogo;
            if (ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo != null)
            {
                pbSemnox.Image = ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo;
            }
            else
            {
                pbSemnox.Image = Properties.Resources.semnox_logo;
            }
            log.LogMethodExit();
        }

        private void SetupDevices()
        {
            log.LogMethodEntry();
            SetupTopUpReader();
            SetupDispenserReader();
            //DeviceContainer.ReturnMessageToUI returnMessageToUI = new DeviceContainer.ReturnMessageToUI(UpdateDisplayMsg);
            DeviceContainer.returnMessageToUI += new DeviceContainer.ReturnMessageToUI(UpdateDisplayMsg);
            DeviceContainer.InitializeSerialPorts();


            try
            {
                if (utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y")
                    KioskStatic.InitializeFiscalPrinter();
            }
            catch (Exception ex) { log.Error(" KioskStatic.InitializeFiscalPrinter", ex); }

            if (KioskStatic.config.dispport > 0)
            {
                cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                cardDispenserMonitor = new Monitor(Monitor.MonitorAppModule.CARD_DISPENSER);
            }

            if (KioskStatic.config.baport > 0)
            {
                try
                {
                    if (!KioskStatic.DisablePurchase)
                    {
                        billAcceptorMonitor = new Monitor(Monitor.MonitorAppModule.BILL_ACCEPTOR);
                        if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_KIOSK_DIRECT_CASH") != "N")
                        {
                            commonBillAcceptor = new CommonBillAcceptor(machineUserContext, billAcceptorMonitor, KioskStatic.config.baport.ToString());
                            EnableCommonAcceptor();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    KioskStatic.logToFile(ex.Message);
                    log.Error(ex);
                }
            }

            RestartRFIDPrinter();
            log.LogMethodExit();
        }

        private void SetupTopUpReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                if (KioskStatic.config.cardAcceptorport > 0)
                {
                    KioskStatic.cardAcceptor = KioskStatic.getCardAcceptor(KioskStatic.config.cardAcceptorport.ToString());
                    string message = "";
                    if (KioskStatic.cardAcceptor.Initialize(ref message) == false)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, "Card Acceptor") + ":" + message);
                        KioskStatic.cardAcceptor = null;
                    }
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.TopUpReaderDevice = new SK310UR04((int)KioskStatic.cardAcceptor.deviceHandle, "COM" + KioskStatic.config.cardAcceptorport.ToString());
                        EventHandler CardScanCompleteEvent = new EventHandler(TopUpCardScanCompleteEventHandle);
                        KioskStatic.TopUpReaderDevice.Register(CardScanCompleteEvent);
                        List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                        int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                        KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                        log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                    }
                    utilities.getMifareCustomerKey();
                }
            }
            else
            {
                try
                {
                    log.Info("Registering top up reader");
                    KioskStatic.TopUpReaderDevice = DeviceContainer.RegisterTopupCardReader(utilities.ExecutionContext, this, TopUpCardScanCompleteEventHandle);
                    List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                    int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                    KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                    log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                }
                catch (Exception ex)
                {
                    log.Error("Error registering top up reader", ex);
                    using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + utilities.MessageUtils.getMessage(441)))
                    {
                        f.ShowDialog();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetupDispenserReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.config.dispport > 0)
            {
                try
                {
                    log.Info("Register dispenser card reader");
                    KioskStatic.DispenserReaderDevice = DeviceContainer.InitiateDispenserCardReader(KioskStatic.Utilities.ExecutionContext, this, KioskStatic.CardDispenserModel);
                    isDispenserCardReaderValid = true;
                    log.Info("Dispenser card reader is registered");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + utilities.MessageUtils.getMessage(441)))
                    {
                        f.ShowDialog();
                    }
                }
            } 
            log.LogMethodExit();
        }
        private bool CheckNewCardDependency()
        {
            log.LogMethodEntry();
            log.Debug("Disport: " + KioskStatic.config.dispport);
            if (KioskStatic.config.dispport == -1)
            {
                string mes = "Card dispenser is Disabled. Port is set as -1";
                KioskStatic.logToFile(mes);
                log.LogMethodExit(true, mes);
                return true;
            }
            bool disblePurchaseOnCardLowLevel = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "DISABLE_PURCHASE_ON_CARD_LOW_LEVEL", false);
            if (cardDispenser == null)
            {
                DisplayMessageLine(msg1);
                KioskStatic.logToFile("Card dispenser is null (not initialized)");
                frmOKMsg.ShowUserMessage(txtMessage.Text + ". " + msg2);
                log.LogVariableState("Card dispenser is null (not initialized)", cardDispenser);
                log.LogMethodExit(false);
                return false;
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                DisplayMessageLine(string.IsNullOrEmpty(dispenserMessage) ? msg1 : dispenserMessage);
                KioskStatic.logToFile(txtMessage.Text);
                frmOKMsg.ShowUserMessage(txtMessage.Text + ". " + msg2);
                log.LogVariableState(txtMessage.Text, cardDispenser);
                log.LogMethodExit(false);
                return false;
            }
            else if (disblePurchaseOnCardLowLevel && cardDispenser.cardLowlevel)
            {
                string mes = MessageContainerList.GetMessage(machineUserContext, 378) + ". " + msg2;
                //Card Low Level
                KioskStatic.logToFile(mes);
                frmOKMsg.ShowUserMessage(mes);
                log.LogVariableState(mes, cardDispenser);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InvokeNewCardBtnAction();
            log.LogMethodExit();
        }

        private void InvokeNewCardBtnAction()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("New Card click");
                string entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
                if (KioskHelper.isTimeEnabledStore())
                {
                    entitlementType = KioskTransaction.TIME_ENTITLEMENT;
                }
                log.LogVariableState("entitlementType", entitlementType);
                CardDispenserStatusCheck();

                if (CheckNewCardDependency() == false)
                {
                    log.LogMethodExit();
                    return;
                }

                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                DisableCommonBillAcceptor();
                kioskTransaction = new KioskTransaction(utilities);
                try
                {
                    kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                PaymentModesContainerDTO userSelectedPaymentModeDTO;
                try
                {
                    userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETNEWCARDTYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    frm.FormClosed += (s, ea) =>
                    {
                        this.Activate();
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnNewCard_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ClearTrxObject();
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit New Card click");
            }
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InvokeRechargeCardBtnAction();
            log.LogMethodExit();
        }

        private void InvokeRechargeCardBtnAction()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Top-up click");
                string entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
                if (KioskHelper.isTimeEnabledStore())
                {
                    entitlementType = KioskTransaction.TIME_ENTITLEMENT;
                }
                log.LogVariableState("entitlementType", entitlementType);
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                DisableCommonBillAcceptor();
                kioskTransaction = new KioskTransaction(utilities);
                try
                {
                    kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                PaymentModesContainerDTO userSelectedPaymentModeDTO;
                try
                {
                    userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETRECHAREGETYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    frm.FormClosed += (s, ea) =>
                    {
                        if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                        {
                            KioskStatic.cardAcceptor.EjectCardFront();
                            KioskStatic.cardAcceptor.BlockAllCards();
                        }
                        this.Activate();
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnRecharge_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ClearTrxObject();
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Top-up click");
            }
            log.LogMethodExit();
        }
        private void PlayHomeScreenAudio()
        {
            log.LogMethodEntry();
            try
            {
                if (this.Equals(Form.ActiveForm) == false)
                {
                    log.LogMethodExit();
                    return;
                }

                if (homeAudioInterval % 8 == 0)
                {
                    Audio.PlayAudio(Audio.ChooseOption);
                }
                else
                {
                    Audio.PlayAudio(Audio.ChooseOption);
                }

                homeAudioInterval++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in PlayHomeScreenAudio: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void InitKioskHomeScreen()
        {
            log.LogMethodEntry();
            StopKioskTimer();

            //CheckApplicationVersion();

            lblDebug.Text = "";
            //if (singleFunctionMode)
            //{
            PlayHomeScreenAudio();
            //}

            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            btnFAQ.Visible = KioskStatic.EnableFAQ;
            KioskStatic.setDefaultFont(this);

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
            tagNumberParser = new TagNumberParser(machineUserContext);
            KioskStatic.logToFile("App started");
            log.Info("App started");
            try
            {
                if (KioskStatic.DisablePurchase)
                {
                    flpOptions.Controls.Remove(btnPurchase);
                    flpOptions.Controls.Remove(btnNewCard);
                    flpOptions.Controls.Remove(btnRecharge);
                    flpOptions.Controls.Remove(btnPlaygroundEntry);
                    flpOptions.Controls.Remove(btnFNB);
                    flpOptions.Controls.Remove(btnAttractions);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (KioskStatic.DisableNewCard)
                {
                    flpOptions.Controls.Remove(btnNewCard);
                }
                else
                {
                    enabledProductMenuBtnCount += 1;
                    enabledProductMenuBtnName = NEW_CARD_MENU;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "DISABLE_RECHARGE", false))
                {
                    flpOptions.Controls.Remove(btnRecharge);
                }
                else
                {
                    enabledProductMenuBtnCount += 1;
                    enabledProductMenuBtnName = RECHARGE_CARD_MENU;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if ((!KioskStatic.RegistrationAllowed) || (KioskStatic.DisableCustomerRegistration))
                {
                    flpOptions.Controls.Remove(btnRegister);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.EnableTransfer)
                {
                    flpOptions.Controls.Remove(btnTransfer);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.EnableRedeemTokens)
                {
                    flpOptions.Controls.Remove(btnRedeemTokens);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.EnablePauseCard)
                {
                    flpOptions.Controls.Remove(btnPauseTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.AllowPointsToTimeConversion)
                {
                    flpOptions.Controls.Remove(btnPointsToTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.EnableWaiverSignInKiosk)
                {
                    flpOptions.Controls.Remove(btnSignWaiver);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (!KioskStatic.EnablePlaygroundEntry)
                {
                    flpOptions.Controls.Remove(btnPlaygroundEntry);
                }
                else
                {
                    btnPlaygroundEntry.Visible = true;
                    enabledProductMenuBtnCount += 1;
                    enabledProductMenuBtnName = CHECKIN_MENU;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                bool enableExecuteOnlineTrxOption = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "KIOSK_ENABLE_EXECUTE_TRANSACTION_OPTION");
                if (enableExecuteOnlineTrxOption == false)
                {
                    flpOptions.Controls.Remove(btnExecuteOnlineTransaction);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {

                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_FNB_PRODUCTS_IN_KIOSK", false) == false)
                {
                    flpOptions.Controls.Remove(btnFNB);
                }
                else
                {
                    btnFNB.Visible = true;
                    enabledProductMenuBtnCount += 1;
                    enabledProductMenuBtnName = FOOD_N_BEV_MENU;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_ATTRACTION_PRODUCTS_IN_KIOSK", false) == false)
                {
                    flpOptions.Controls.Remove(btnAttractions);
                }
                else
                {
                    btnAttractions.Visible = true;
                    enabledProductMenuBtnCount += 1;
                    enabledProductMenuBtnName = ATTRACTION_MENU;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "KIOSK_ENABLE_CHECK_BALANCE_OPTION", true) == true)
                {
                    btnCheckBalance.Visible = true;
                }
                else
                {
                    flpOptions.Controls.Remove(btnCheckBalance);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (enabledProductMenuBtnCount == 0)
            {
                flpOptions.Controls.Remove(btnPurchase);
            }
            bool hasValidDynamicImageConfig = HomeScreenOptionDisplay.HasValidDynamicImageConfig(machineUserContext);
            if (hasValidDynamicImageConfig)
            {
                DoDynamicDisplay();
            }
            else
            {
                ArrangeDisplayButtonImg();
            }

            GetLanguages();
            SetCustomizedFontColors();
            if (KioskStatic.DispenserReaderDevice != null || KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
            }

            lblSiteName.Text = KioskStatic.SiteHeading;

            StartKioskTimer();

            this.Activate();
            if (KioskStatic.debugMode == false)//&& singleFunctionMode)
            {
                Cursor.Hide();
            }
            ValidateWaiverSetup();
            KioskStatic.LoadTermsAndConditionsFiles();
            try
            {
                KioskStatic.LoadMasterScheduleBLList();
                KioskStatic.LoadAttractionSchedulesForTheDay();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void SetButtonMarginNonCartMode()
        {
            log.LogMethodEntry();
            if (flpOptions.Controls.Count > 5)
            {
                int buttonPosition = 1;
                foreach (Control c in flpOptions.Controls)
                {
                    if ((buttonPosition > 1 && flpOptions.Controls.Count == 10))
                    {
                        if (buttonPosition % 2 == 0) //Right side of the screen
                        {
                            c.Margin = new Padding(36, 25, 0, 0);
                        }
                        else //left side of the screen
                        {
                            c.Margin = new Padding(75, 25, 0, 0);
                        }
                    }
                    else if ((buttonPosition > 2 && (flpOptions.Controls.Count == 7 || flpOptions.Controls.Count == 9)))
                    {
                        if (buttonPosition % 2 == 0) //Left side of the screen
                        {
                            c.Margin = new Padding(75, 25, 0, 0);
                        }
                        else //Right side of the screen
                        {
                            c.Margin = new Padding(36, 25, 0, 0);
                        }
                    }
                    else if ((buttonPosition > 2 && (flpOptions.Controls.Count == 6 || flpOptions.Controls.Count == 8)))
                    {
                        if (buttonPosition % 2 == 0) //Right side of the screen
                        {
                            c.Margin = new Padding(36, 25, 0, 0);
                        }
                        else //left side of the screen
                        {
                            c.Margin = new Padding(75, 25, 0, 0);
                        }
                    }
                    buttonPosition++;
                }
            }
            log.LogMethodExit();
        }

        private void HandleBillAcceptorEvent(int noteDenominatorReceived)
        {
            log.LogMethodEntry(noteDenominatorReceived);
            try
            {
                ResetKioskTimer();
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    if (Application.OpenForms[i].Name == "frmOKSMsg"
                        || Application.OpenForms[i].Name == "frmTimeOutCounter"
                        || Application.OpenForms[i].Name == "frmTimeOut")
                    {
                        Application.OpenForms[i].Visible = false;
                        Application.OpenForms[i].Close();
                    }
                }
                using (frmCashInsert fci = new frmCashInsert(entitlementType, noteDenominatorReceived))
                {
                    fci.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in HandleBillAcceptorEvent:" + ex.Message);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            log.LogMethodExit();
        }

        private void EnableCommonAcceptor()
        {
            log.LogMethodEntry();
            log.Debug("commonBillAcceptor :" + commonBillAcceptor);
            if (commonBillAcceptor != null)
            {
                commonBillAcceptor.setReceiveAction = HandleBillAcceptorEvent;
                try
                {
                    commonBillAcceptor.Start();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in EnableCommonAcceptor:" + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void DisableCommonBillAcceptor()
        {
            log.LogMethodEntry();
            if (commonBillAcceptor != null)
            {
                if (BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                {
                    log.Error(MessageContainerList.GetMessage(machineUserContext, 1701));
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1701));
                }
                try
                {
                    commonBillAcceptor.Stop();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in DisableCommonBillAcceptor:" + ex.Message);
                }
            }
            log.LogMethodExit();
        }


        private void UpdateDisplayMsg(string message)
        {
            log.LogMethodEntry(message);
            this.dispenserMessage = this.txtMessage.Text = message;
            log.LogMethodExit();
        }


        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Check Balance click");
                DisableCommonBillAcceptor();
                Card card = null;
                using (frmTapCard ftc = new frmTapCard())
                {
                    ftc.ShowDialog();
                    if (ftc.Card == null)
                    {
                        ftc.Dispose();
                        log.LogMethodExit();
                        return;
                    }

                    card = ftc.Card;
                    ftc.Dispose();
                    AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, card.card_id, true, false);
                    if (accountBL.AccountDTO == null)
                    {
                        KioskStatic.logToFile("Customer Card detail is not available");
                        log.LogMethodExit("accountBL.AccountDTO is null");
                        return;
                    }
                    KioskHelper.CheckForCardExpiry(accountBL.AccountDTO, frmOKMsg.ShowUserMessage);
                    using (frmCheckBalance frmchkBal = new frmCheckBalance(accountBL.AccountDTO))
                    {
                        frmchkBal.ShowDialog();
                        frmchkBal.Dispose();
                    }
                }

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                this.Activate();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnCheckBalance_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit CheckBalance click");
            }
            log.LogMethodExit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Register click");
            try
            {
                DisableCommonBillAcceptor();
                string CardNumber = "";
                if (KioskStatic.AllowRegisterWithoutCard)
                {

                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(496), utilities.MessageUtils.getMessage("Yes")))
                    {
                        DialogResult dr = ftc.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            CardNumber = null;
                        }
                        else if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit();
                            return;
                        }
                        else
                        {
                            CardNumber = ftc.cardNumber;
                        }
                        log.LogVariableState("CardNumber", CardNumber);
                        ftc.Dispose();
                    }
                }
                else
                {
                    using (frmTapCard ftc = new frmTapCard(utilities.MessageUtils.getMessage(500)))
                    {
                        ftc.ShowDialog();
                        if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit();
                            return;
                        }

                        CardNumber = ftc.cardNumber;
                        log.LogVariableState("CardNumber", CardNumber);
                        ftc.Dispose();
                    }
                }

                if (!String.IsNullOrEmpty(CardNumber))
                {
                    Card custRegisterCard = new Card(CardNumber, "", KioskStatic.Utilities);
                    if (custRegisterCard.technician_card.Equals('Y'))
                    {
                        txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(197, custRegisterCard.CardNumber);
                        KioskStatic.logToFile(txtMessage.Text);
                        log.LogMethodExit();
                        return;
                    }
                }
                CustomerStatic.ShowCustomerScreen(CardNumber);

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                this.Activate();
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                log.Error(ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnRegister_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Register_Click");
            }
            log.LogMethodExit();
        }
        private void TopUpCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    TagNumber tagNumber = null;
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
                            KioskStatic.logToFile("Error in TopUpCardScanCompleteEventHandle:" + ex.Message);
                            return;
                        }
                        try
                        {
                            scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                        }
                        catch (ValidationException ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile("Error in TopUpCardScanCompleteEventHandle:" + ex.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile("Error in TopUpCardScanCompleteEventHandle:" + ex.Message);
                            return;
                        }
                    }
                    if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(scannedTagNumber);
                        KioskStatic.logToFile(message);
                        log.LogMethodExit(null, "Invalid Tag Number. " + message);
                        return;
                    }
                    string lclCardNumber = tagNumber.Value;
                    log.LogVariableState("lclCardNumber", lclCardNumber);
                    if ((sender as DeviceClass).GetType().ToString().Contains("KeyboardWedge"))
                        lclCardNumber = KioskStatic.ReverseTopupCardNumber(lclCardNumber);
                    HandleTopUpCardRead(lclCardNumber, sender as DeviceClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in TopUpCardScanCompleteEventHandle:" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HandleTopUpCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            try
            {
                //StopKioskTimer();
                KioskStatic.logToFile("HomeScreen Card Tap: " + inCardNumber);
                log.LogVariableState("HomeScreen Card Tap: ", inCardNumber);
                if (Application.OpenForms.Count > 1)
                {
                    KioskStatic.logToFile("Not in home screen. Ignored.");
                    log.LogVariableState("Not in home screen. Ignored.", Application.OpenForms.Count);
                    log.LogMethodExit();
                    return;
                }

                Card card = null;

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                {
                    try
                    {
                        if (KioskStatic.cardAcceptor != null)
                        {
                            KioskStatic.cardAcceptor.EjectCardFront();
                            KioskStatic.cardAcceptor.BlockAllCards();
                        }
                    }
                    catch (Exception ex) { log.Error(ex); }
                }
                else
                {
                    card = new Card(readerDevice, inCardNumber, "", utilities);
                }

                if (card != null)
                {
                    if (card.CardStatus == "NEW")
                    {
                        KioskStatic.logToFile("NEW Card tapped. Ignore");
                        log.LogVariableState("NEW Card tapped. Ignored.", card.CardStatus);
                        log.LogMethodExit();
                        return;
                    }

                    if (card.customerDTO != null)
                    {
                        StopKioskTimer();
                        txtMessage.Text = utilities.MessageUtils.getMessage(463, card.customerDTO.FirstName);
                    }
                    else
                        txtMessage.Text = utilities.MessageUtils.getMessage(462);

                    KioskStatic.logToFile(txtMessage.Text);

                    if (card.vip_customer == 'N')
                    {
                        card.getTotalRechargeAmount();
                        if ((card.credits_played >= utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                             || (card.TotalRechargeAmount >= utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                        {
                            using (frmOKMsg fok = new frmOKMsg(utilities.MessageUtils.getMessage(451, KioskStatic.VIPTerm)))
                            {
                                fok.ShowDialog();
                            }
                        }
                    }
                    bool isManagerCard = CheckIsThisCardIsManagerCard(machineUserContext, inCardNumber);
                    if (card.technician_card == 'Y' || isManagerCard == true)
                    {

                        KioskStatic.logToFile("Tech card OR isManagerCard: " + card.technician_card + " isManagerCard: " + (isManagerCard ? "Yes" : "No"));
                        log.LogVariableState("Tech Card", card.technician_card);
                        log.LogVariableState("isManagerCard", isManagerCard);
                        using (frmAdmin adminForm = new frmAdmin(machineUserContext, card.CardNumber, isManagerCard))
                        {
                            if (adminForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                KioskStatic.logToFile("Exit from Admin screen");
                                log.LogVariableState("Exit from Admin screen", card);
                                CloseApplication();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in HandleTopUpCardRead: " + ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CloseApplication()
        {
            log.LogMethodEntry();
            for (int openForm = Application.OpenForms.Count - 1; openForm >= 0; openForm--)
            {
                Application.OpenForms[openForm].Visible = false;
                Application.OpenForms[openForm].Close();
            }
            log.LogMethodExit();
        }

        private void frmHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //semnoxClickCount = 0;
            log.LogMethodExit();
        }

        int tickCount = 0;

        private void CardDispenserStatusCheck()
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                dispenserMessage = string.Empty;
                if (cardDispenser != null)
                {
                    string mes = string.Empty;
                    if (isDispenserCardReaderValid == false)
                    {
                        cardDispenser.dispenserWorking = false;
                        dispenserMessage = MessageContainerList.GetMessage(machineUserContext, 1696);
                    }
                    else
                    {
                        int cardPosition = -1;

                        this.Cursor = Cursors.WaitCursor;
                        bool suc = BackgroundProcessRunner.Run<bool>(() =>
                        {
                            return InvokeCardDispenserCheckStatus(ref cardPosition, ref mes);
                        }
                        );
                        this.Cursor = Cursors.WaitCursor;
                        //bool suc = cardDispenser.checkStatus(ref cardPosition, ref mes);

                        dispenserMessage = mes;
                        if (suc)
                        {
                            if (cardPosition == 3)
                            {
                                cardDispenser.dispenserWorking = false;
                                dispenserMessage = "Card at mouth positon. Please remove card.";
                                KioskStatic.logToFile("Card at mouth positon. Please remove card.");
                                log.LogVariableState("Card at mouth positon. Please remove card.", cardDispenser.dispenserWorking);
                            }
                            else if (cardPosition == 2)
                            {
                                cardDispenser.dispenserWorking = false;
                                string message = "";
                                KioskStatic.logToFile("Card at read positon. Ejecting.");
                                log.Info("Card at read positon. Ejecting.");
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.LogVariableState(message, cardDispenser.dispenserWorking);
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(mes);
                            dispenserMessage = MessageContainerList.GetMessage(machineUserContext, 377);//Card Dispenser Problem
                        }
                    }
                    cardDispenserMonitor.Post((cardDispenser.dispenserWorking ? Monitor.MonitorLogStatus.INFO : Monitor.MonitorLogStatus.ERROR), string.IsNullOrEmpty(dispenserMessage) ? "Dispenser working" : dispenserMessage + ": " + mes);
                }   

                if (string.IsNullOrWhiteSpace(dispenserMessage) == false)
                {
                    DisplayMessageLine(dispenserMessage);
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 2438));//Please choose an option
                }
                log.LogVariableState(txtMessage.Text, null);
                StartKioskTimer();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in CardDispenserStatusCheck: " + ex.Message + ": " + ex.StackTrace);
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void frmHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {

                if (KioskStatic.TopUpReaderDevice != null)
                {
                    KioskStatic.TopUpReaderDevice.UnRegister();
                    List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                    int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                    KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                    log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmHome_FormClosed: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            try
            {
                DisableCommonBillAcceptor();
                if (commonBillAcceptor != null)
                {
                    commonBillAcceptor.setReceiveAction = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmHome_FormClosed: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            //if (singleFunctionMode)
            //{
            //isHomeForm(true);
            KioskStatic.logToFile("App exiting");
            string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5190); //"Exiting TableTop Kiosk"
            string activityCode = paymentGatewayInitializaztionError ? KioskTransaction.PAYMENTMODE_ERRROR_EXITKIOSK_MSG : KioskTransaction.EXITKIOSKMSG;
            KioskStatic.UpdateKioskActivityLog(utilities.ExecutionContext, activityCode, message);
            //}
            //else
            //{
            //    log.Info(this.Name + ": Form closed");
            //}
            log.LogMethodExit();
        }
         

        private void frmHome_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (singleFunctionMode)
            //{
            if ((int)e.KeyChar == 3)
            {
                Cursor.Show();
            }
            else if ((int)e.KeyChar == 8)
            {
                Cursor.Hide();
            }
            else
            {
                e.Handled = true;
            }
            //}
            log.LogMethodExit();
        }
        //private void spCoinAcceptor_DataReceived(System.Object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    try
        //    {
        //        List<byte[]> bList = new List<byte[]>();
        //        int totalBytesToRead = 0;
        //        while (KioskStatic.spCoinAcceptor.BytesToRead > 0)
        //        {
        //            int bytesToRead = KioskStatic.spCoinAcceptor.BytesToRead;
        //            byte[] dataRec = new byte[bytesToRead];
        //            KioskStatic.spCoinAcceptor.Read(dataRec, 0, bytesToRead);
        //            if (dataRec != null && dataRec.Count() > 0)
        //            {
        //                bList.Add(dataRec);
        //            }
        //            totalBytesToRead = totalBytesToRead + bytesToRead;
        //        }
        //        if (bList != null && bList.Any())
        //        {
        //            int byteSizeVal = bList.Sum(br => br.Count());
        //            KioskStatic.coinAcceptor_rec = new byte[byteSizeVal + 5];
        //            int i = 0;
        //            foreach (byte[] item in bList)
        //            {
        //                foreach (byte byteItem in item)
        //                {
        //                    KioskStatic.coinAcceptor_rec[i] = byteItem;
        //                    i++;
        //                }
        //            }
        //            KioskStatic.logToFile("SP CoinAcceptor BytesToRead: " + totalBytesToRead.ToString());
        //            string returnValue = BitConverter.ToString(KioskStatic.coinAcceptor_rec).Replace("-", "");
        //            KioskStatic.logToFile("SP CoinAcceptor Bytes data: " + returnValue);

        //            KioskStatic.coinAcceptorDatareceived = true;

        //            if (KioskStatic.caReceiveAction != null)
        //                KioskStatic.caReceiveAction.Invoke();
        //        }
        //        else
        //        {
        //            KioskStatic.logToFile("SP CoinAcceptor Bytes data is empty");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        KioskStatic.logToFile("Error in frmHome spCoinAcceptor_DataReceived: " + ex.Message);
        //    }
        //    log.LogMethodExit();
        //}

        private void btnRegister_MouseDown(object sender, MouseEventArgs e)
        {
            //  btnRegister.BackgroundImage = Properties.Resources.regsiter_btn_pressed;
        }

        private void btnRegister_MouseUp(object sender, MouseEventArgs e)
        {
            //  btnRegister.BackgroundImage = Properties.Resources.Register_New;
        }

        private void btnRecharge_MouseDown(object sender, MouseEventArgs e)
        {
            //  btnRecharge.BackgroundImage = Properties.Resources.top_up_btn_pressed;
        }

        private void btnRecharge_MouseUp(object sender, MouseEventArgs e)
        {
            //  btnRecharge.BackgroundImage = Properties.Resources.Add_Points_New;
        }

        private void btnCheckBalance_MouseDown(object sender, MouseEventArgs e)
        {
            // btnCheckBalance.BackgroundImage = Properties.Resources.check_balance_btn_pressed;
        }

        private void btnCheckBalance_MouseUp(object sender, MouseEventArgs e)
        {
            // btnCheckBalance.BackgroundImage = Properties.Resources.Check_Balance_New;
        }

        ListBox lstLang;

        private void GetLanguages()
        {
            log.LogMethodEntry();
            DataTable dt = utilities.executeDataTable(@"select LanguageCode, LanguageName, LanguageId 
                                                        from languages 
                                                        where Active = 1
                                                        union all 
                                                        select 'en-US', 'English', -1
                                                        where not exists (select 1 from Languages where LanguageName = 'English') 
                                                        order by LanguageName");

            lstLang = new ListBox();
            lstLang.Name = "lstLang";
            lstLang.DataSource = dt;
            lstLang.ValueMember = "LanguageCode";
            lstLang.DisplayMember = "LanguageName";
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            lstLang.Font = new System.Drawing.Font(fontFamName, 24);
            this.lstLang.ForeColor = KioskStatic.CurrentTheme.HomeScreenLanguageListForeColor;
            this.Controls.Add(lstLang);
            lstLang.BringToFront();
            lstLang.Height = (int)(lstLang.CreateGraphics().MeasureString("English", lstLang.Font).Height * dt.Rows.Count);
            lstLang.MinimumSize = new System.Drawing.Size(lstLang.Width, 100);
            lstLang.Width = (int)(lstLang.CreateGraphics().MeasureString("Chinese (Taiwan)", lstLang.Font).Width);
            int verticalPoint = btnLanguage.Location.Y - lstLang.Height;
            if (verticalPoint < 0)
                verticalPoint = 10;
            lstLang.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - lstLang.Width, verticalPoint);
            lstLang.Visible = false;
            lstLang.SelectedValue = utilities.ParafaitEnv.LanguageCode;

            lstLang.SelectedIndexChanged += lstLang_SelectedIndexChanged;

            if (dt.Rows.Count == 2)
            {
                if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                    btnLanguage.Text = dt.Rows[1]["LanguageName"].ToString();
                else
                    btnLanguage.Text = dt.Rows[0]["LanguageName"].ToString();
            }
            log.LogMethodExit();
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                btnLanguage.Enabled = false;
                KioskStatic.logToFile("Language button click");
                DisableCommonBillAcceptor();
                ResetKioskTimer();
                if (lstLang.Items.Count == 2)
                {
                    DataTable dt = lstLang.DataSource as DataTable;
                    if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                        RestartOnLanguageChange((int)dt.Rows[0]["LanguageId"], dt.Rows[0]["LanguageName"].ToString());
                    else
                        RestartOnLanguageChange((int)dt.Rows[1]["LanguageId"], dt.Rows[1]["LanguageName"].ToString());

                    log.LogMethodExit("lstLang.Items.Count == 2");
                    return;
                }

                lstLang.Visible = !lstLang.Visible;
                if (lstLang.Visible)
                    Audio.PlayAudio(Audio.SelectLanguage);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnLanguage_Click: " + ex.Message);
            }
            finally
            {
                EnableCommonAcceptor();
                btnLanguage.Enabled = true;
            }
            KioskStatic.logToFile("Exit Language button click");
            log.LogMethodExit();
        }

        private void lstLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                RestartOnLanguageChange((int)(lstLang.SelectedItem as DataRowView).Row["LanguageId"], (lstLang.SelectedItem as DataRowView).Row["LanguageName"].ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in lstLang_SelectedIndexChanged: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void RestartOnLanguageChange(int languageId, string languageName)
        {
            log.LogMethodEntry(languageId, languageName);
            lstLang.Visible = false;

            KioskStatic.logToFile("Language change to " + languageName);
            log.Info("Language change to " + languageName);

            KioskStatic.Utilities.setLanguage(languageId);
            this.btnPurchase.Text = "Purchase";
            this.btnNewCard.Text = "Buy A New Card";
            this.btnRecharge.Text = "Top Up";
            this.btnPlaygroundEntry.Text = "Playground Entry";
            this.btnCheckBalance.Text = "Balance / Activity";//Modification on 17-Dec-2015 for introducing new theme
            this.btnRegister.Text = "Register";
            this.btnTransfer.Text = "Transfer";
            this.btnRedeemTokens.Text = "Redeem Tokens";
            this.btnFAQ.Text = "FAQ";
            this.btnSignWaiver.Text = "Sign Waiver";
            this.btnPauseTime.Text = "Pause Card";
            this.btnPointsToTime.Text = "Convert Points To Time";
            this.btnFNB.Text = "Food And Beverages";
            this.btnAttractions.Text = "Attractions";
            this.btnExecuteOnlineTransaction.Text = "Retrieve My Purchase";

            KioskStatic.Utilities.setLanguage(this);
            lblSiteName.Text = KioskStatic.Utilities.MessageUtils.getMessage(415, KioskStatic.Utilities.ParafaitEnv.SiteName);
            lblSiteName.Refresh();
            if (lstLang.Items.Count == 2)
            {
                DataTable dt = lstLang.DataSource as DataTable;
                if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                    btnLanguage.Text = dt.Rows[1]["LanguageName"].ToString();
                else
                    btnLanguage.Text = dt.Rows[0]["LanguageName"].ToString();
            }
            else
                btnLanguage.Text = languageName;

            log.LogMethodExit();
        }


        private void btnTransfer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Transfer balance click");
                ResetKioskTimer();

                DisableCommonBillAcceptor();
                using (frmTapCard ftp = new frmTapCard())
                {
                    ftp.ShowDialog();
                    if (ftp.Card == null)
                    {
                        ftp.Dispose();
                        log.LogMethodExit("ftp.Card == null");
                        return;
                    }
                    else
                    {
                        if (ftp.Card.CardStatus == "NEW")
                        {
                            txtMessage.Text = utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogMethodExit(txtMessage.Text);
                            return;
                        }
                    }
                    double totalCredits = ftp.Card.credits + ftp.Card.CreditPlusCardBalance + ftp.Card.CreditPlusCredits;
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ENABLE_TRANSFER").Equals("BOTH"))
                        {
                            if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0 && totalCredits != 0)
                            {
                                using (frmEntitlement frm = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1343)))
                                {
                                    if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                    {
                                        frm.Dispose();
                                        log.LogMethodExit();
                                        return;
                                    }
                                    entitlementType = frm.selectedEntitlement;
                                    frm.Dispose();
                                }
                            }
                            else if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0 && totalCredits == 0)
                            {
                                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
                                log.Info("ENTITLEMENT_TYPE is Time");
                            }
                            else
                            {
                                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
                                log.Info("ENTITLEMENT_TYPE is Credits");
                            }
                        }
                        else if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ENABLE_TRANSFER").Equals("TIME"))
                        {
                            if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0)//&& ftp.Card.credits == 0
                            {
                                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
                                log.Info("ENTITLEMENT_TYPE is Time");
                            }
                            else
                            {
                                using (frmOKMsg f = new frmOKMsg(utilities.MessageUtils.getMessage(1842, utilities.MessageUtils.getMessage("Time"))))
                                {
                                    f.ShowDialog();
                                }

                                KioskStatic.logToFile("Transfer is allowed only for Time. Card does not have any Time to transfer.");
                                log.Info("Transfer is allowed only for Time. Card does not have any Time to transfer.");
                                return;
                            }
                        }
                        else if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ENABLE_TRANSFER").Equals("POINT"))
                        {
                            if (totalCredits != 0)
                            {
                                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
                                log.Info("ENTITLEMENT_TYPE is Credits");
                            }
                            else
                            {
                                using (frmOKMsg f = new frmOKMsg(utilities.MessageUtils.getMessage(1842, utilities.MessageUtils.getMessage("Points"))))
                                {
                                    f.ShowDialog();
                                }
                                KioskStatic.logToFile("Transfer is allowed only for Points. Card does not have any Points to transfer.");
                                log.Info("Transfer is allowed only for Points. Card does not have any Points to transfer.");
                                return;
                            }
                        }
                        log.LogVariableState("Entitlement type", entitlementType);
                    }
                    using (CardTransfer.frmTransferFrom tfFrom = new CardTransfer.frmTransferFrom(ftp.Card.CardNumber, entitlementType))
                    {
                        tfFrom.ShowDialog();
                    }
                }
                Audio.Stop();

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                this.Activate();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnTransfer_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exit Transfer balance click");
            log.LogMethodExit();
        }

        private void btnTransfer_MouseDown(object sender, MouseEventArgs e)
        {
            //  btnTransfer.BackgroundImage = Properties.Resources.transfer_points_pressed;
        }

        private void btnTransfer_MouseUp(object sender, MouseEventArgs e)
        {
            //  btnTransfer.BackgroundImage = Properties.Resources.Transfer_Points_New;
        }

        private void btnRedeemTokens_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Redeem tokens click");
            try
            {
                DisableCommonBillAcceptor();
                CardDispenserStatusCheck();

                using (frmRedeemTokens frt = new frmRedeemTokens((cardDispenser != null && cardDispenser.dispenserWorking == true)))
                {
                    frt.ShowDialog();
                }
                Audio.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnRedeemTokens_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exit Redeem tokens click");
            log.LogMethodExit();
        }

        private void btnRedeemTokens_MouseDown(object sender, MouseEventArgs e)
        {
            // btnRedeemTokens.BackgroundImage = Properties.Resources.redeem_tokens_pressed;
        }

        private void btnRedeemTokens_MouseUp(object sender, MouseEventArgs e)
        {
            // btnRedeemTokens.BackgroundImage = Properties.Resources.Redeem_Tokens_New;
        }

        private void btnFAQ_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("FAQ Click");
                if (commonBillAcceptor != null && BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                {
                    KioskStatic.logToFile("Bill acceptor is processing notes. Can not proceed");
                    return;
                }
                DisableCommonBillAcceptor();
                Audio.Stop();
                using (frmFAQ faq = new frmFAQ())
                {
                    faq.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnFAQ_Click:" + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exit FAQ Click");
            log.LogMethodExit();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayAssemblyVersion();

            if (KioskHelper.isTimeEnabledStore())
                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
            else
                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;

            log.LogVariableState("entitlementType", entitlementType);
            txtMessage.Text = (utilities.MessageUtils.getMessage(462));
            //if (singleFunctionMode)
            //{
            if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null
                || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
            {
                if (KioskStatic.CurrentTheme.SplashAfterSeconds == 0)
                {
                    ResetKioskTimer();
                }
                else
                {
                    SetKioskTimerTickValue(KioskStatic.CurrentTheme.SplashAfterSeconds);
                    ResetKioskTimer();
                }
            }
            else
            {
                ResetKioskTimer();
            }

            if (!KioskStatic.questStatus)
            {
                txtMessage.Text = MessageContainerList.GetMessage(machineUserContext, 1692);
            }
            kioskMonitor = new Monitor();
            kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1693));
            this.btnHome.Visible = false;
            string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5189); //"Launching TableTop Kiosk"
            KioskStatic.UpdateKioskActivityLog(utilities.ExecutionContext, KioskTransaction.LAUNCHKIOSKMSG, message);
            if (KioskStatic.errorPayentModeDTOList != null && KioskStatic.errorPayentModeDTOList.Any())
            {
                bool shouldCloseTheKiosk = KioskHelper.CanCloseTheKiosk(machineUserContext);
                if (shouldCloseTheKiosk)
                {
                    foreach (POSPaymentModeInclusionDTO item in KioskStatic.errorPayentModeDTOList)
                    {
                        KioskStatic.logToFile("Error while initializing Payment Mode: " + (string.IsNullOrWhiteSpace(item.FriendlyName)
                                                                             ? (item.PaymentModeDTO != null ? item.PaymentModeDTO.PaymentMode : item.PaymentModeId.ToString())
                                                                             : item.FriendlyName));
                    }
                    KioskStatic.logToFile("Payment mode has error, closing the Kiosk");
                    log.Info("Payment mode has error, closing the Kiosk");

                    paymentGatewayInitializaztionError = true;
                    CloseApplication();
                }
                else
                {
                    KioskStatic.logToFile("Skipping application closure for Payment mode errors");
                    log.Info("Skipping application closure for Payment mode errors");
                }
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //if (singleFunctionMode)
                //{
                if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
                {
                    int tickSecondsRemaining = GetKioskTimerSecondsValue();
                    tickSecondsRemaining--;
                    setKioskTimerSecondsValue(tickSecondsRemaining);

                    if (tickSecondsRemaining <= 0)
                    {
                        if (commonBillAcceptor != null && BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                        {
                            setKioskTimerSecondsValue(tickSecondsRemaining - 5);
                            KioskStatic.logToFile("Bill acceptor is processing notes. Can not proceed");
                            log.LogVariableState("Bill acceptor is processing notes. Can not proceed", null);
                            log.LogMethodExit();
                            return;
                        }
                        StopKioskTimer();
                        KioskStatic.logToFile("Calling splash");
                        using (frmSplashScreen splashScreen = new frmSplashScreen())
                        {
                            splashScreen.ShowDialog();
                        }
                    }
                }

                if (this.Equals(Form.ActiveForm) || (Form.ActiveForm != null && Form.ActiveForm.Name != null && Form.ActiveForm.Name.Equals("frmSplashScreen")))
                {
                    txtMessage.Text = utilities.MessageUtils.getMessage(462);
                    tickCount++;
                    if (tickCount % 300 == 0) // refresh variable every 5 minutes
                    {
                        kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1694));
                        KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                        KioskStatic.logToFile("Refresh defaults: getParafaitDefaults()");
                        log.LogVariableState("Kiosk app running.., Refresh defaults: getParafaitDefaults()", null);
                        KioskStatic.getParafaitDefaults();
                        tickCount = 0;
                        KioskStatic.RestartPaymentGatewayComponent(machineUserContext);
                    }
                }
                //}
                //else
                //{
                //    int tickSecondsRemaining = GetKioskTimerSecondsValue();
                //    tickSecondsRemaining--;
                //    setKioskTimerSecondsValue(tickSecondsRemaining);
                //    if (tickSecondsRemaining == 10)
                //    {

                //        if (TimeOut.AbortTimeOut(this))
                //        {
                //            ResetKioskTimer();
                //        }
                //        else
                //        {
                //            tickSecondsRemaining = 0;
                //        }
                //    }
                //    if (tickSecondsRemaining <= 0)
                //    {
                //        this.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in KioskTimer_Tick:" + ex.Message);
            }
            log.LogMethodExit();

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            if (keyData == Keys.Enter)
            {
                log.LogMethodExit("keyData == Keys.Enter", "true");
                return true;
            }

            // Call the base class
            bool returnValue = base.ProcessCmdKey(ref msg, keyData);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }

        public override void Form_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //if (singleFunctionMode)
                //{
                if (Application.OpenForms.Count > 1)
                {
                    try
                    {
                        Application.OpenForms[Application.OpenForms.Count - 1].Focus();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    log.LogMethodExit();
                    return;
                }

                if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null
                    || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
                {
                    if (KioskStatic.CurrentTheme.SplashAfterSeconds == 0)
                    {
                        ResetKioskTimer();
                    }
                    else
                    {
                        SetKioskTimerTickValue(KioskStatic.CurrentTheme.SplashAfterSeconds);
                        ResetKioskTimer();
                    }
                }//Playpass1:Ends

                this.TopMost = true;
                this.TopMost = false;

                this.Focus();
                this.Activate();
                StartKioskTimer();
                KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                log.LogVariableState(MessageContainerList.GetMessage(machineUserContext, 1694), null);
                if (Application.OpenForms.Count == 1)
                {
                    if (Audio.soundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        PlayHomeScreenAudio();
                    }
                }
                //}
                //else
                //{
                //    ResetKioskTimer();
                //    StartKioskTimer();
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmHome_Activated: " + ex.Message);
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        private void btnPointsToTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Points To Time button Click");
            ResetKioskTimer();
            try
            {
                DisableCommonBillAcceptor();
                using (frmTapCard ftp = new frmTapCard())
                {
                    ftp.ShowDialog();
                    if (ftp.Card == null)
                    {
                        ftp.Dispose();
                        log.LogMethodExit("ftp.Card == null");
                        return;
                    }
                    else
                    {
                        if (ftp.Card.CardStatus == "NEW")
                        {
                            txtMessage.Text = utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogVariableState(txtMessage.Text, ftp.Card.CardStatus);
                            log.LogMethodExit();
                            return;
                        }
                    }

                    using (Transaction.frmCreditsToTime frm = new Transaction.frmCreditsToTime(ftp.cardNumber))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnPointsToTime_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exist Points To Time Click");
            log.LogMethodExit();
        }

        private void btnPauseTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Pause time button click");
            ResetKioskTimer();
            string message = "";

            try
            {
                DisableCommonBillAcceptor();
                using (frmTapCard ftp = new frmTapCard())
                {
                    ftp.ShowDialog();
                    if (ftp.Card == null)
                    {
                        ftp.Dispose();
                        log.LogMethodExit("ftp.Card == null");
                        return;
                    }
                    else
                    {
                        if (ftp.Card.CardStatus == "NEW")
                        {
                            txtMessage.Text = utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogVariableState(txtMessage.Text, ftp.Card.CardStatus);
                            log.LogMethodExit();
                            return;
                        }

                        Loyalty trxLoyalty = new Loyalty(utilities);
                        bool isTimeRunning = trxLoyalty.IsCreditPlusTimeRunning(ftp.Card.card_id, null);
                        if (!isTimeRunning)
                        {
                            frmOKMsg.ShowUserMessage(utilities.MessageUtils.getMessage(1386));
                            log.LogVariableState("isTimeRunning", isTimeRunning);

                            KioskStatic.logToFile("No time remaining on the card");
                            log.LogMethodExit();
                            return;
                        }

                        TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                        if (!tp.CheckTimePauseLimit(ftp.Card.card_id, ref message))
                        {
                            frmOKMsg.ShowUserMessage(message);
                            log.LogVariableState("CheckTimePauseLimit() failure", message);
                            log.LogMethodExit();
                            return;
                        }
                        if (tp != null)
                            tp = null;
                    }
                    using (Transaction.frmPauseTime frm = new Transaction.frmPauseTime(utilities.MessageUtils.getMessage("Balance Time will be paused"), ftp.cardNumber))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnPauseTime_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exit Pause time click");
            log.LogMethodExit();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Home button click");
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in BtnHome_Click: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            KioskStatic.logToFile("Exit Home button click");
            log.LogMethodExit();
        }
        private void ValidateWaiverSetup()
        {
            log.LogMethodEntry();
            try
            {
                incorrectCustomerSetupForWaiver = true;
                WaiverCustomerUtils.HasValidWaiverSetup(utilities.ExecutionContext);
                incorrectCustomerSetupForWaiver = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                waiverSetupErrorMsg = ex.Message;
                incorrectCustomerSetupForWaiver = true;
                KioskStatic.logToFile("Error in ValidateWaiverSetup: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void btnSignWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Sign waiver button click");
                DisableCommonBillAcceptor();
                if (incorrectCustomerSetupForWaiver == false)
                {
                    using (frmSelectWaiverOptions sws = new frmSelectWaiverOptions())
                    {
                        sws.ShowDialog();
                    }
                }
                else
                {
                    frmOKMsg.ShowUserMessage(waiverSetupErrorMsg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in btnSignWaiver_Click: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            KioskStatic.logToFile("Exit Sign waiver click");
            log.LogMethodExit();
        }
        private void btnPlaygroundEntry_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InvokePlaygroundEntryBtnAction();
            log.LogMethodExit();
        }

        private void InvokePlaygroundEntryBtnAction()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Play ground entry click");
                DisableCommonBillAcceptor();
                string val = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CHECK_IN_OPTIONS_IN_POS-IN");
                log.Debug("CHECK_IN_OPTIONS_IN_POS : " + val);
                if (string.IsNullOrWhiteSpace(val) == false && val != "NO")
                {
                    string errMsg = MessageContainerList.GetMessage(machineUserContext, 4343);
                    //'Setup Error: CHECK_IN_OPTIONS_IN_POS must be set to NO for Kiosk'
                    frmOKMsg.ShowUserMessage(errMsg);
                    KioskStatic.logToFile(errMsg);
                    log.LogMethodExit();
                    return;
                }
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                using (frmTapCard frmtc = new frmTapCard())
                {
                    frmtc.ShowDialog();
                    if (frmtc.Card != null)
                    {
                        KioskStatic.logToFile("Card: " + frmtc.Card.CardNumber);
                        Card card = frmtc.Card;
                        if (card.technician_card.Equals('Y'))
                        {
                            //Technician Card (&1) not allowed for Transaction
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 197, card.CardNumber));
                            log.LogMethodExit();
                            return;
                        }

                        if (card.customer_id != -1)
                        {
                            kioskTransaction = new KioskTransaction(utilities);
                            try
                            {
                                kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                            }
                            catch (ArgumentNullException)
                            {
                                log.LogMethodExit("timeout");
                                return;
                            }
                            PaymentModesContainerDTO userSelectedPaymentModeDTO;
                            try
                            {
                                userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                            }
                            catch (ArgumentNullException)
                            {
                                log.LogMethodExit("timeout");
                                return;
                            }
                            kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
                            using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETCHECKINCHECKOUTTYPE, "CheckInCheckOut", "ALL", card.CardNumber))
                            {
                                DialogResult dr = frm.ShowDialog();
                                kioskTransaction = frm.GetKioskTransaction;
                            }
                        }
                        else
                        {
                            using (frmYesNo frmYN = new frmYesNo(MessageContainerList.GetMessage(machineUserContext, 758),
                                                                  MessageContainerList.GetMessage(machineUserContext, 759)))
                            {   //758 - Would you like to Register? 759-* Register to get 100 FREE Tickets
                                if (frmYN.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                {
                                    Customer customer = new Customer(card.CardNumber);
                                    customer.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnPlaygroundEntry_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ClearTrxObject();
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Play ground entry click");
            }
            log.LogMethodExit();
        }

        private void btnAttractions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InvokeAttractionBtnAction();
            log.LogMethodExit();
        }

        private void InvokeAttractionBtnAction()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Attractions button click");
                DisableCommonBillAcceptor();
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }

                kioskTransaction = new KioskTransaction(utilities);
                try
                {
                    kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                PaymentModesContainerDTO userSelectedPaymentModeDTO;
                try
                {
                    userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETATTRACTIONSTYPE, entitlementType))
                {
                    DialogResult dr = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnAttractions_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ClearTrxObject();
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Attraction button click");
            }
            log.LogMethodExit();
        }

        private bool IsThisCardIsManagerCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            bool returnvalue = false;
            UsersList usersList = new UsersList(machineUserContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>
            {
                new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.CARD_NUMBER, cardNumber),
                new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
            };
            List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameters);
            if (usersDTOList != null && usersDTOList.Count > 0)
            {
                UserRoles userRoles = new UserRoles(machineUserContext, usersDTOList[0].RoleId);
                if (userRoles != null)
                {
                    if (userRoles.getUserRolesDTO.ManagerFlag == "Y")
                    {
                        returnvalue = true;
                    }
                }
            }
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmHome");
            try
            {
                foreach (Control c in flpOptions.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.HomeScreenOptionsTextForeColor;//Buy New card, Add points or Time ,Pause Card, Transfer points,Check Balance button

                    }
                }
                this.btnRegister.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRegisterTextForeColor;//Register button
                this.btnCheckBalance.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnCheckBalanceTextForeColor;//CHeck balance button
                this.btnPurchase.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPurchaseTextForeColor;//Purchase button
                this.btnFNB.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnFoodAndBeveragesTextForeColor;//FNB button
                this.btnNewCard.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnNewCardTextForeColor;//New Card button
                this.btnRecharge.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRechargeTextForeColor;//Recharge button
                this.btnPlaygroundEntry.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPlaygroundEntryTextForeColor;//Kidzoona Entry button
                this.btnTransfer.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnTransferTextForeColor;//CHeck balance button
                this.btnRedeemTokens.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRedeemTokensTextForeColor;//CHeck balance button
                this.btnPauseTime.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPauseTimeTextForeColor;//CHeck balance button
                this.btnPointsToTime.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPointsToTimeTextForeColor;//CHeck balance button
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor;//CHeck balance button
                this.btnSignWaiver.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnSignWaiverTextForeColor;//CHeck balance button
                this.btnLanguage.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnLanguageTextForeColor;//Language
                this.btnFAQ.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnTermsTextForeColor;//Faq(Terms)
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.lblAppVersion.ForeColor = KioskStatic.CurrentTheme.ApplicationVersionForeColor;
                this.btnExecuteOnlineTransaction.ForeColor = KioskStatic.CurrentTheme.FSKCoverPageBtnExecuteOnlineTransactionTextForeColor;
                this.btnAttractions.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnAttractionsTextForeColor;//Attractions button
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmHome: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void CheckApplicationVersion()
        {
            log.LogMethodEntry();
            int result = utilities.VersionCheck();
            if (result == -1)
            {
                string message = utilities.MessageUtils.getMessage(2290); //"You are not using the latest version of this application. Please update to continue using this application."
                KioskStatic.logToFile(message);
                frmOKMsg.ShowUserMessage(message);
                KioskStatic.logToFile("Exiting kiosk App");
                Program.ShowTaskbar();
                Environment.Exit(1);
            }
            else if (result == 1)
            {
                //"The app version is not in sync with the server. Please sync version with the server."
                string message = utilities.MessageUtils.getMessage(2291);
                KioskStatic.logToFile(message);
                frmOKMsg.ShowUserMessage(message);
            }
            else { } //DB Version and App version are same.
            log.LogMethodExit();
        }
        private void DisplayAssemblyVersion()
        {
            log.LogMethodEntry();

            try
            {
                string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                lblAppVersion.Text = "v" + version;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in DisplayAssemblyVersion:" + ex.Message);
            }

            log.LogMethodExit();
        }

        private PaymentModesContainerDTO GetUserPreferredPaymentMode()
        {
            log.LogMethodEntry();
            PaymentModesContainerDTO userSelectedPaymentModeDTO = null;
            try
            {
                //List<PaymentModesContainerDTO> paymentModestWithDisplayGroups = null;
                if (isPaymentModeDrivenSalesInKioskEnabled == true)
                {
                    //paymentModestWithDisplayGroups = KioskHelper.GetPaymentModesWithDisplayGroups();
                    if (paymentModestWithDisplayGroups != null && paymentModestWithDisplayGroups.Any())
                    {
                        if (paymentModestWithDisplayGroups.Count > 1)
                        {
                            using (frmPreSelectPaymentMode frmpm = new frmPreSelectPaymentMode(machineUserContext, paymentModestWithDisplayGroups))
                            {
                                DialogResult dr = frmpm.ShowDialog();
                                if (dr != System.Windows.Forms.DialogResult.OK) // back button pressed
                                {
                                    ArgumentNullException argumentNullException = new ArgumentNullException();
                                    throw argumentNullException;
                                }
                                else
                                {
                                    userSelectedPaymentModeDTO = frmpm.SelectedPaymentModesContainerDTO;
                                    frmpm.Close();
                                }
                            }
                        }
                        else
                        {
                            POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO = KioskStatic.pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeId == paymentModestWithDisplayGroups[0].PaymentModeId).FirstOrDefault();
                            string paymentModeName = String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName) ? pOSPaymentModeInclusionDTO.PaymentModeDTO.PaymentMode
                                : pOSPaymentModeInclusionDTO.FriendlyName;
                            //You are allowed to use only &1 for payment.Do you want to proceed?
                            string msg = MessageContainerList.GetMessage(machineUserContext, 4983, paymentModeName); ;
                            using (frmYesNo frmyn = new frmYesNo(msg))
                            {
                                DialogResult dr = frmyn.ShowDialog();
                                if (dr == System.Windows.Forms.DialogResult.Yes) // back button pressed
                                {
                                    userSelectedPaymentModeDTO = paymentModestWithDisplayGroups[0];
                                    frmyn.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in GetUserPreferredPaymentMode() in Home screen:" + ex.Message);
                throw;
            }
            if (isPaymentModeDrivenSalesInKioskEnabled == true && userSelectedPaymentModeDTO == null)
            {
                logWarningMsg();
            }
            log.LogMethodExit(userSelectedPaymentModeDTO);
            return userSelectedPaymentModeDTO;
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Purchase button click");

            if (!KioskStatic.receipt && !utilities.getParafaitDefaults("IGNORE_PRINTER_ERROR").Equals("Y"))
            {
                using (frmYesNo frmyn = new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(461)))
                {
                    if (frmyn.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                    {
                        frmyn.Dispose();
                        log.LogMethodExit();
                        return;
                    }
                    frmyn.Dispose();
                }
            }

            try
            {
                DisableCommonBillAcceptor();
                List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = null;
                if (enabledProductMenuBtnCount > 1)
                {
                    try
                    {
                        selectedFundsAndDonationsList = GetFundraiserOrDonationProducts(null);
                    }
                    catch (ArgumentNullException)
                    {
                        log.LogMethodExit("timeout");
                        return;
                    }
                }
                PaymentModesContainerDTO userSelectedPaymentModeDTO;
                try
                {
                    userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                if (enabledProductMenuBtnCount == 1)
                {
                    LaunchProductMenu();
                }
                else
                {
                    using (frmPurchase frm = new frmPurchase(machineUserContext, userSelectedPaymentModeDTO, selectedFundsAndDonationsList))
                    {
                        frm.ShowDialog();
                        frm.FormClosed += (s, ea) =>
                        {
                            this.Activate();
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnPurchase_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Purchase button click");
            }
            log.LogMethodExit();
        }
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }
        private bool OkToIgorePrinerError()
        {
            log.LogMethodEntry();
            bool okToIgnore = true;
            if (KioskStatic.receipt == false
                && ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "IGNORE_PRINTER_ERROR").Equals("Y") == false)
            {
                //Cannot print Receipt for this Transaction. Do you want to continue?
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(machineUserContext, 461)))
                {
                    if (frmyn.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                    {
                        okToIgnore = false;
                    }
                    frmyn.Dispose();
                }
            }
            log.LogMethodExit(okToIgnore);
            return okToIgnore;
        }
        private void btnFNB_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InvokeFNBBtnAction();
            log.LogMethodExit();
        }

        private void InvokeFNBBtnAction()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Food and Beverage button click");
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                DisableCommonBillAcceptor();
                kioskTransaction = new KioskTransaction(utilities);
                try
                {
                    kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                PaymentModesContainerDTO userSelectedPaymentModeDTO;
                try
                {
                    userSelectedPaymentModeDTO = GetUserPreferredPaymentMode();
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
                kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETFNBTYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    frm.FormClosed += (s, ea) =>
                    {
                        this.Activate();
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in btnFNB_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ClearTrxObject();
                EnableCommonAcceptor();
                KioskStatic.logToFile("Exit Food and Beverage button click");
            }
            log.LogMethodExit();
        }

        private void HideApplicablePurchaseButtons()
        {
            log.LogMethodEntry();
            if (showCartInKiosk)
            {
                this.btnNewCard.Visible = false;
                this.btnRecharge.Visible = false;
                this.btnPlaygroundEntry.Visible = false;
                this.btnFNB.Visible = false;
                this.btnAttractions.Visible = false;

                flpOptions.Controls.Remove(btnNewCard);
                flpOptions.Controls.Remove(btnRecharge);
                flpOptions.Controls.Remove(btnPlaygroundEntry);
                flpOptions.Controls.Remove(btnFNB);
                flpOptions.Controls.Remove(btnAttractions);
            }
            else
            {
                this.btnPurchase.Visible = false;
                flpOptions.Controls.Remove(btnPurchase);
            }
            log.LogMethodExit();
        }

        private void ArrangeDisplayButtonImg()
        {
            log.LogMethodEntry();
            SetBasicButtonImages();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            Font largeButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsLargeFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font smallButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsSmallFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font mediumButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsMediumFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            int btnCount = flpOptions.Controls.Count;
            if (btnCount == 1)
            {
                Button c = flpOptions.Controls[0] as Button;
                c.BackgroundImage = GetBigBackgroundImage(c);
                c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                c.Margin = c.Margin = new System.Windows.Forms.Padding(650, 20, 3, 3);
                c.Size = c.BackgroundImage.Size;
                c.Font = largeButtonFont;
            }
            else if (btnCount == 2)
            {
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetBigBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                        c.Margin = c.Margin = (i == 0) ? new System.Windows.Forms.Padding(315, 20, 3, 3) : new System.Windows.Forms.Padding(100, 20, 3, 3);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = largeButtonFont;
                    }
                }
            }
            else if (btnCount == 3)
            {
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetBigBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                        c.Margin = new Padding(45, 20, 3, 3);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = largeButtonFont;
                    }
                }
            }
            else if (btnCount == 4)
            {
                this.flpOptions.Size = new System.Drawing.Size(750, 625);
                this.flpOptions.Location = new System.Drawing.Point(535, 232);
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetMediumBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeMediumButtonTextAlignment);   
                        c.Margin = new System.Windows.Forms.Padding(75, 10, 3, 30);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = mediumButtonFont;
                    }
                }
            }
            else if (btnCount == 6 || btnCount == 5)
            {
                this.flpOptions.Size = new System.Drawing.Size(1200, 625);
                this.flpOptions.Location = new System.Drawing.Point(350, 232);
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetMediumBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeMediumButtonTextAlignment);
                        c.Margin = new System.Windows.Forms.Padding(75, 10, 3, 30);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = mediumButtonFont;
                    }
                }
            }
            else if (btnCount == 7 || btnCount == 8)
            {
                this.flpOptions.Size = new System.Drawing.Size(1520, 625);
                this.flpOptions.Location = new System.Drawing.Point(170, 232);
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetMediumBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeMediumButtonTextAlignment);
                        c.Margin = new System.Windows.Forms.Padding(75, 10, 3, 30);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = mediumButtonFont;
                    }
                }
            }
            else if (btnCount == 10 || btnCount == 9)
            {
                this.flpOptions.Size = new System.Drawing.Size(1600, 625);
                this.flpOptions.Location = new System.Drawing.Point(140, 232);
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetSmallBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeSmallButtonTextAlignment);
                        c.Margin = new System.Windows.Forms.Padding(45, 10, 3, 40);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = smallButtonFont;
                    }
                }
            }
            else if (btnCount == 11 || btnCount == 12)
            {
                for (int i = 0; i < flpOptions.Controls.Count; i++)
                {
                    string type = flpOptions.Controls[i].GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        Button c = flpOptions.Controls[i] as Button;
                        c.BackgroundImage = GetSmallBackgroundImage(c);
                        c.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeSmallButtonTextAlignment);
                        c.Margin = new System.Windows.Forms.Padding(35, 10, 3, 40);
                        c.Size = c.BackgroundImage.Size;
                        c.Font = smallButtonFont;
                    }
                }
            }
            //if (btnCount == 13)
            //{
            //    
            //}
            log.LogMethodExit();
        }
        private Image GetBigBackgroundImage(Button b)
        {
            log.LogMethodEntry();
            string name = b.Name;
            Image backgroundImage = null;
            switch (name)
            {
                case "btnRedeemTokens":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonBig;//KioskStatic.CurrentTheme.ExchangeTokensButtonBig;
                    break;
                case "btnTransfer":
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonBig; //KioskStatic.CurrentTheme.TransferPointButtonBig;
                    break;
                case "btnRegister":
                    backgroundImage = ThemeManager.CurrentThemeImages.RegisterPassBig; //KioskStatic.CurrentTheme.RegisterPassBig;
                    break;
                case "btnCheckBalance":
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonBig; //KioskStatic.CurrentTheme.CheckBalanceButtonBig;
                    break;
                case "btnPauseTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonBig;// KioskStatic.CurrentTheme.PauseCardButtonBig;
                    break;
                case "btnPointsToTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonBig; //KioskStatic.CurrentTheme.PointsToTimeButtonBig;
                    break;
                case "btnSignWaiver":
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonBig;
                    break;
                case "btnPurchase":
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonBig;
                    break;
                case "btnNewCard":
                    backgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;// KioskStatic.CurrentTheme.NewPlayPassButtonBig;
                    break;
                case "btnRecharge":
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig; //KioskStatic.CurrentTheme.RechargePlayPassButtonBig;
                    break;
                case "btnPlaygroundEntry":
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntryBig;
                    break;
                case "btnFNB":
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageBig;
                    break;
                case "btnAttractions":
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonBig;
                    break;
                case "btnExecuteOnlineTransaction":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonBig;
                    break;
            }
            log.LogMethodExit();
            return backgroundImage;
        }

        private Image GetMediumBackgroundImage(Button b)
        {
            log.LogMethodEntry();
            string name = b.Name;
            Image backgroundImage = null;
            switch (name)
            {
                case "btnRedeemTokens":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonMedium;//KioskStatic.CurrentTheme.ExchangeTokensButtonBig;
                    break;
                case "btnTransfer":
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonMedium; //KioskStatic.CurrentTheme.TransferPointButtonBig;
                    break;
                case "btnRegister":
                    backgroundImage = ThemeManager.CurrentThemeImages.RegisterPassMedium; //KioskStatic.CurrentTheme.RegisterPassBig;
                    break;
                case "btnCheckBalance":
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonMedium; //KioskStatic.CurrentTheme.CheckBalanceButtonBig;
                    break;
                case "btnPauseTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonMedium;// KioskStatic.CurrentTheme.PauseCardButtonBig;
                    break;
                case "btnPointsToTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonMedium; //KioskStatic.CurrentTheme.PointsToTimeButtonBig;
                    break;
                case "btnSignWaiver":
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonMedium;
                    break;
                case "btnPurchase":
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonMedium;
                    break;
                case "btnNewCard":
                    backgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonMedium;// KioskStatic.CurrentTheme.NewPlayPassButtonBig;
                    break;
                case "btnRecharge":
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonMedium; //KioskStatic.CurrentTheme.RechargePlayPassButtonBig;
                    break;
                case "btnPlaygroundEntry":
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntryMedium;
                    break;
                case "btnFNB":
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageMedium;
                    break;
                case "btnAttractions":
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonMedium;
                    break;
                case "btnExecuteOnlineTransaction":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonMedium;
                    break;
            }
            log.LogMethodExit();
            return backgroundImage;
        }

        private Image GetSmallBackgroundImage(Button b)
        {
            log.LogMethodEntry();
            string name = b.Name;
            Image backgroundImage = null;
            switch (name)
            {
                case "btnRedeemTokens":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonSmall;// KioskStatic.CurrentTheme.ExchangeTokensButtonSmall;
                    break;
                case "btnTransfer":
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonSmall; //KioskStatic.CurrentTheme.TransferPointButtonSmall;
                    break;
                case "btnRegister":
                    backgroundImage = ThemeManager.CurrentThemeImages.RegisterPassSmall; //KioskStatic.CurrentTheme.RegisterPassSmall;
                    break;
                case "btnCheckBalance":
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonSmall; //KioskStatic.CurrentTheme.CheckBalanceButtonSmall;
                    break;
                case "btnPauseTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonSmall;//KioskStatic.CurrentTheme.PauseCardButtonSmall;
                    break;
                case "btnPointsToTime":
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonSmall; //KioskStatic.CurrentTheme.PointsToTimeButtonSmall;
                    break;
                case "btnSignWaiver":
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonSmall;
                    break;
                case "btnPurchase":
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonSmall;
                    break;
                case "btnNewCard":
                    backgroundImage = ThemeManager.CurrentThemeImages.NewCardButtonSmall;// KioskStatic.CurrentTheme.NewPlayPassButtonSmall;
                    break;
                case "btnRecharge":
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonSmall;//KioskStatic.CurrentTheme.RechargePlayPassButtonSmall;
                    break;
                case "btnPlaygroundEntry":
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntrySmall;
                    break;
                case "btnFNB":
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageSmall;
                    break;
                case "btnAttractions":
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonSmall;
                    break;
                case "btnExecuteOnlineTransaction":
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonSmall;
                    break;
            }
            log.LogMethodExit();
            return backgroundImage;
        }
        private void SetButtonMargine(int buttonIndex, Button btnObject)
        {
            log.LogMethodEntry(buttonIndex, btnObject.Name);

            if (buttonIndex % 2 == 0) //Right side of the screen
            {
                btnObject.Margin = new Padding(75, 25, 0, 0);
            }
            else //left side of the screen
            {
                btnObject.Margin = new Padding(40, 25, 0, 0);
            }
            log.LogMethodExit();
        }
        private void SetButtonMargineForMore(int buttonIndex, Button btnObject)
        {
            log.LogMethodEntry(buttonIndex, btnObject.Name);

            if (buttonIndex % 2 == 0) //Right side of the screen
            {
                btnObject.Margin = new Padding(40, 25, 0, 0);
            }
            else //left side of the screen
            {
                btnObject.Margin = new Padding(75, 25, 0, 0);
            }
            log.LogMethodExit();
        }
        private void logWarningMsg()
        {
            log.LogMethodEntry();

            string warningMsg = "Setup Error: Payment Mode Driven Sales is enabled but not set any display group for payment, "
                                + "Proceeding with Non Payment Mode Driven Sales";
            KioskStatic.logToFile(warningMsg);
            log.Debug(warningMsg);

            log.LogMethodExit();
        }
        private void ClearTrxObject()
        {
            log.LogMethodEntry();
            try
            {
                if (kioskTransaction != null)
                {
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    kioskTransaction = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in ClearTrxObject: " + ex.Message + ":" + ex.StackTrace);
            }
            log.LogMethodExit();
        }

        private void RestartRFIDPrinter()
        {
            log.LogMethodEntry();
            try
            {
                log.Info("Calling Restart RFID Printer");
                KioskStatic.logToFile("Calling Restart RFID Printer");
                DeviceContainer.RestartRFIDPrinter(utilities.ExecutionContext, utilities.ParafaitEnv.POSMachineId);
                log.Info("RFID Printer Restarted Successfully");
                KioskStatic.logToFile("RFID Printer Restarted Successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error restarting RFID Printer: " + ex.Message);
                using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + utilities.MessageUtils.getMessage(441)))
                {
                    f.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void BtnExecuteOnlineTransaction_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Execute Online Transaction button click");
            try
            {
                CardDispenserStatusCheck();
                CheckNewCardDependency();
                DisableCommonBillAcceptor();
                using (FSKExecuteOnlineTransaction frm = new FSKExecuteOnlineTransaction(machineUserContext))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                this.Activate();
                KioskStatic.logToFile("Exit Execute Online Transaction button click");
            }
            log.LogMethodExit();
        }

        private KioskTransaction AddFundraiserOrDonationProduct(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = GetFundraiserOrDonationProducts(kioskTransaction);
            kioskTransaction = frmPaymentMode.AddFundRaiserOrDonationProducts(kioskTransaction, selectedFundsAndDonationsList);
            log.LogMethodExit();
            return kioskTransaction;
        }

        private static List<KeyValuePair<string, ProductsDTO>> GetFundraiserOrDonationProducts(KioskTransaction kioskTransaction)
        {
            List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = new List<KeyValuePair<string, ProductsDTO>>();
            selectedFundsAndDonationsList = frmPaymentMode.GetSelectedFundsAndDonationsList(kioskTransaction);
            return selectedFundsAndDonationsList;
        }
        
        public static bool CheckIsThisCardIsManagerCard(ExecutionContext machineUserContext, string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            bool thisIsManagerCard = false;
            try
            {
                UserIdentificationTagsDTO userIdTagDTO = null;
                UserIdentificationTagListBL userIdTagsList = new UserIdentificationTagListBL();
                List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> userIdTagSearchParams = new List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>>();

                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG, "1"));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER, cardNumber));
                userIdTagSearchParams.Add(new KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>(UserIdentificationTagsDTO.SearchByUserIdTagsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                List<UserIdentificationTagsDTO> userIdTagList = userIdTagsList.GetUserIdentificationTagsDTOList(userIdTagSearchParams);

                if (userIdTagList != null && userIdTagList.Count > 0)
                {
                    userIdTagDTO = userIdTagList[0];
                }

                if (userIdTagDTO != null)
                {
                    List<UsersDTO> usersDTOList = new List<UsersDTO>();
                    UsersList usersList = new UsersList(machineUserContext);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                    usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, userIdTagDTO.UserId.ToString()));
                    usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    usersDTOList = usersList.GetAllUsers(usersSearchParams);
                    if (usersDTOList != null && usersDTOList.Count > 0)
                    {
                        UserRolesDTO userRolesDTO = new UserRolesDTO();
                        UserRoles userRolesBL = new UserRoles(machineUserContext, usersDTOList[0].RoleId);
                        userRolesDTO = userRolesBL.getUserRolesDTO;
                        if (userRolesDTO != null)
                        {
                            thisIsManagerCard = userRolesDTO.ManagerFlag == "Y" ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(thisIsManagerCard);
            return thisIsManagerCard;
        }


        private bool InvokeCardDispenserCheckStatus(ref int cardPosition, ref string message)
        {
            log.LogMethodEntry(cardPosition, message);
            bool suc = cardDispenser.checkStatus(ref cardPosition, ref message);
            log.LogMethodExit();
            return suc;
        }

        private void DoDynamicDisplay()
        {
            log.LogMethodEntry();
            SetBasicButtonImages();
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            Font largeButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsLargeFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font smallButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsSmallFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font mediumButtonFont = new System.Drawing.Font(fontFamName, KioskStatic.CurrentTheme.HomeScreenOptionsMediumFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            int btnCount = flpOptions.Controls.Count;
            HomeScreenOptionDisplay homeScreenOptionDisplay = new HomeScreenOptionDisplay(machineUserContext);
            flpOptions.Controls.Clear();
            flpOptions.SuspendLayout();
            //flpOptions.Margin = new Padding(20, 0, 20, 0);
            //flpOptions.Padding = new Padding(0); 
            if (KioskStatic.CurrentTheme.HomeScreenOptionWidth > 0)
            {
                flpOptions.Size = new Size(KioskStatic.CurrentTheme.HomeScreenOptionWidth, flpOptions.Height);
                int locX = (1920 / 2) - (flpOptions.Width / 2);
                flpOptions.Location = new Point(locX, flpOptions.Location.Y);
            }
            //flpOptions.FlowDirection = FlowDirection.LeftToRight;
            //flpOptions.BorderStyle = BorderStyle.FixedSingle;
            try
            {
                foreach (HomeScreenOption item in homeScreenOptionDisplay.GetOptionList)
                {
                    if (item.CanShowTheOption())
                    {
                        try
                        {
                            Button optionButton = GetButton(item.GetOptionButtonName());
                            optionButton.Margin = new Padding(35, 20, 3, 3);
                            optionButton.Padding = new Padding(0);
                            optionButton.BackgroundImage = GetBtnImage(item.GetOptionImageName());
                            optionButton.Size = new Size(optionButton.BackgroundImage.Width, optionButton.BackgroundImage.Height);
                            //optionButton.FlatStyle = FlatStyle.Flat;
                            //optionButton.FlatAppearance.BorderSize = 1;
                            if (item.GetFontSize() == HomeScreenOption.LARGE_FONT)
                            {
                                optionButton.Font = largeButtonFont;
                                optionButton.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                            }
                            if (item.GetFontSize() == HomeScreenOption.MEDIUM_FONT)
                            {
                                optionButton.Font = mediumButtonFont;
                                optionButton.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeMediumButtonTextAlignment);
                            }
                            else
                            {
                                optionButton.Font = smallButtonFont;
                                optionButton.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeSmallButtonTextAlignment);
                            }
                            flpOptions.Controls.Add(optionButton);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile("Error during dynamic display. Unable to add " + item.GetOptionButtonName());
                        }
                    } 
                }
            }
            finally
            {
                flpOptions.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private Image GetBtnImage(string imageName)
        {
            log.LogMethodEntry(imageName);
            Image backgroundImage = null;
            switch (imageName)
            {       
                case HomeScreenOptionImageList.PURCHASEBUTTONBIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonBig;
                    break;
                case HomeScreenOptionImageList.PURCHASEBUTTONMEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonMedium;
                    break;
                case HomeScreenOptionImageList.PURCHASEBUTTONSMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.PurchaseButtonSmall;
                    break;
                case HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonBig;
                    break;
                case HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonMedium;
                    break;
                case HomeScreenOptionImageList.CHECK_BALANCE_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonSmall;
                    break;
                case HomeScreenOptionImageList.REGISTER_PASS_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.RegisterPassBig;
                    break;
                case HomeScreenOptionImageList.REGISTER_PASS_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonMedium;
                    break;
                case HomeScreenOptionImageList.REGISTER_PASS_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.RegisterPassSmall;
                    break;
                case HomeScreenOptionImageList.TRANSFER_POINT:
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonBig;
                    break;
                case HomeScreenOptionImageList.TRANSFER_POINT_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonMedium;
                    break;
                case HomeScreenOptionImageList.TRANSFER_POINT_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonSmall;
                    break;
                case HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonBig;
                    break;
                case HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonMedium;
                    break;
                case HomeScreenOptionImageList.POINTS_TO_TIME_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeButtonSmall;
                    break;
                case HomeScreenOptionImageList.PAUSE_CARD_BUTTON_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonBig;
                    break;
                case HomeScreenOptionImageList.PAUSE_CARD_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonMedium;
                    break;
                case HomeScreenOptionImageList.PAUSE_CARD_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.PauseCardButtonSmall;
                    break;
                case HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonBig;
                    break;
                case HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonMedium;
                    break;
                case HomeScreenOptionImageList.EXECUTE_ONLINE_TRX_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButtonSmall;
                    break;
                case HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonBig;
                    break;
                case HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonMedium;
                    break;
                case HomeScreenOptionImageList.SIGN_WAIVER_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.SignWaiverButtonSmall;
                    break;
                case HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonBig; 
                    break;
                case HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonMedium;
                    break;
                case HomeScreenOptionImageList.EXCHANGE_TOKENS_BUTTON_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonSmall;
                    break;
                case HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;
                    break;
                case HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonMedium;
                    break;
                case HomeScreenOptionImageList.NEW_PLAY_PASS_BUTTON:
                    backgroundImage = ThemeManager.CurrentThemeImages.NewCardButtonSmall;
                    break;
                case HomeScreenOptionImageList.RECHARGE_PLAY_PASS_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig; 
                    break;
                case HomeScreenOptionImageList.RECHARGE_PLAY_PASS_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonMedium;
                    break;
                case HomeScreenOptionImageList.RECHARGE_PLAY_PASS_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonSmall;
                    break;
                case HomeScreenOptionImageList.ATTRACTIONSBUTTONBIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonBig;
                    break;
                case HomeScreenOptionImageList.ATTRACTIONSBUTTONMEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonMedium;
                    break;
                case HomeScreenOptionImageList.ATTRACTIONSBUTTONSMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonSmall;
                    break;
                case HomeScreenOptionImageList.FOODANDBEVERAGEBIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageBig;
                    break;
                case HomeScreenOptionImageList.FOODANDBEVERAGEMEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageMedium;
                    break;
                case HomeScreenOptionImageList.FOODANDBEVERAGESMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageSmall;
                    break;
                case HomeScreenOptionImageList.PLAYGROUND_ENTRY_BIG:
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntryBig;
                    break;
                case HomeScreenOptionImageList.PLAYGROUND_ENTRY_MEDIUM:
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntryMedium;
                    break;
                case HomeScreenOptionImageList.PLAYGROUND_ENTRY_SMALL:
                    backgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntrySmall;
                    break;
            }
            log.LogMethodExit();
            return backgroundImage;
        }

        private void SetBasicButtonImages()
        {
            log.LogMethodEntry();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
            pbClientLogo.Image = ThemeManager.CurrentThemeImages.HomeScreenClientLogo;

            if (pbClientLogo.Image != null)
                pbClientLogo.SizeMode = PictureBoxSizeMode.Zoom;

            btnFAQ.BackgroundImage = ThemeManager.CurrentThemeImages.TermsButton;
            btnLanguage.BackgroundImage = ThemeManager.CurrentThemeImages.LanguageButton;
            if (ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo != null)
            {
                pbSemnox.Image = ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo;
            }
            else
            {
                pbSemnox.Image = Properties.Resources.semnox_logo;
            }
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            log.LogMethodExit();
        }
        private Button GetButton(string btnName)
        {
            log.LogMethodEntry(btnName);
            Button foundButton = FindControlByName<Button>(btnName); 
            log.LogMethodExit(foundButton.Name);
            return foundButton;
        }
        private T FindControlByName<T>(string name) where T : Control
        {
            log.LogMethodEntry(name);
            T fieldObj = null;
            FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(T) && field.Name == name)
                {
                    fieldObj = (T)field.GetValue(this);
                    break;
                }
            }
            if (fieldObj == null)
            {
                string msg = MessageContainerList.GetMessage(machineUserContext, "Unable to find &1", name);
                throw new ValidationException(msg);
            }
            log.LogMethodExit(fieldObj.Name);
            return fieldObj;
        }

        private void LaunchProductMenu()
        {
            log.LogMethodEntry();
            if (enabledProductMenuBtnName == NEW_CARD_MENU)
            {
                InvokeNewCardBtnAction();
            }
            else if (enabledProductMenuBtnName == RECHARGE_CARD_MENU)
            {
                InvokeRechargeCardBtnAction();
            }
            else if (enabledProductMenuBtnName == CHECKIN_MENU)
            {
                InvokePlaygroundEntryBtnAction();
            }
            else if (enabledProductMenuBtnName == FOOD_N_BEV_MENU)
            {
                InvokeFNBBtnAction();
            }
            else if (enabledProductMenuBtnName == ATTRACTION_MENU)
            {
                InvokeAttractionBtnAction();
            }
            log.LogMethodExit();
        }
    }
}
