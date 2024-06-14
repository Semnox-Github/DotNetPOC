/********************************************************************************************
* Project Name - Parafait_Kiosk - FSKCoverPage
* Description  - FSKCoverPage 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A            Created for Online Transaction in Kiosk changes
 * 2.70      1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 * 2.70.1    03-Jan-2020      Jinto Thomas        Modified query for selecting variable recharge product in
 *                                                SetKioskVariableTopUpProduct()
 *2.90       15-Jul-2020      Deeksha             Ticket# 649045: Default_Language not picking up from POS level
 *2.100      01-jan-2021      Guru S A            Allow wristband print when card dispener is not enabled
 **2.110     21-Dec-2020      Jinto Thomas        Modified: As part of WristBand printer changes
*2.130.0     09-Jul-2021      Dakshak             Theme changes to support customized Font ForeColor
*2.130.9     16-Jun-2022      Guru S A            Execute online transaction changes in Kiosk
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.logger;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Device;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.Languages;
using System.Reflection;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;

namespace Parafait_Kiosk
{
    public partial class FSKCoverPage : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CardDispenser cardDispenser;
        string dispenserMessage = "";
        bool isDispenserCardReaderValid = false;
        Monitor kioskMonitor;
        Monitor cardDispenserMonitor;
        Monitor billAcceptorMonitor;
        internal CommonBillAcceptor commonBillAcceptor;
        ExecutionContext machineUserContext;
        Utilities utilities;
        public string cardNumber;
        public Card Card;
        int tickCount = 0;
        int homeAudioInterval = 0;
        bool coverPageIsNotHome = false;
        private TagNumberParser tagNumberParser;
        public string entitlementType;
        private bool fskSalesEnabled;
        private bool fskExecuteOnlineTrxEnabled;
        private bool singleFunctionMode;

        public FSKCoverPage()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In FSKCoverPage");
            InitiateUtils();
            InitializeComponent();
            SetStyles();
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            InitKiosk();
            GetCoverPageConfigs();
            utilities.setLanguage();
            utilities.setLanguage(this);
            //tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            ProductsContainerList.GetActiveProductsContainerDTOList(machineUserContext.SiteId);
            SetCustomizedFontColors();
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
                Environment.Exit(0);
            }

            log.LogMethodExit();
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
        private void SetStyles()
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            log.LogMethodExit();
        }

        private void InitKiosk()
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
            KioskStatic.get_config(machineUserContext);
            SetTheme();
            SetClientLogo();
            KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1693));
            SetupDevices();
            StartKioskTimer();
            this.Activate();
            if (KioskStatic.debugMode == false)
                Cursor.Hide();
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
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                log.Error("Please define External POS user id in users table", ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1699));
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

        private void SetTheme()
        {
            log.LogMethodEntry();
            ThemeManager.setThemeImages(KioskStatic.CurrentTheme.ThemeId.ToString());
            KioskStatic.formatMessageLine(txtMessage, 26, KioskStatic.CurrentTheme.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);
            this.BackgroundImage = ThemeManager.CurrentThemeImages.FSKCoverPageBackgroundImage;
            pbClientLogo.Image = ThemeManager.CurrentThemeImages.HomeScreenClientLogo;
            btnFSKSales.BackgroundImage = ThemeManager.CurrentThemeImages.FSKSalesButton;
            btnExecuteOnlineTransaction.BackgroundImage = ThemeManager.CurrentThemeImages.ExecuteOnlineTrxButton;
            flpOptions.Location = new Point(31, 420);
            btnFSKSales.Size = btnFSKSales.BackgroundImage.Size;
            btnExecuteOnlineTransaction.Size = btnExecuteOnlineTransaction.BackgroundImage.Size;
            btnFSKSales.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.FSKSalesTextAlignment);
            btnExecuteOnlineTransaction.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.FSKSalesTextAlignment);
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
            DeviceContainer.InitializeSerialPorts(spCoinAcceptor_DataReceived);


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
                            DisableCommonBillAcceptor();
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
                    KioskStatic.DispenserReaderDevice = DeviceContainer.RegisterDispenserCardReader(KioskStatic.Utilities.ExecutionContext, this, DispenserCardScanCompleteEventHandle, KioskStatic.CardDispenserModel);
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
            if (KioskStatic.DispenserReaderDevice != null)
            {
                KioskStatic.DispenserReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": Dispenser Reader unregistered");
            }
            log.LogMethodExit();
        }

        private void FrmFSKCoverPage_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DisplayAssemblyVersion();
                if (singleFunctionMode)
                {
                    this.flpOptions.Visible = false;
                    Cursor.Show();
                    StopKioskTimer();
                    if (fskExecuteOnlineTrxEnabled)
                    {
                        DisableCommonBillAcceptor();
                        KioskStatic.logToFile("Launching FSKExecuteOnlineTransaction");
                        using (FSKExecuteOnlineTransaction executeOnlineScreen = new FSKExecuteOnlineTransaction(machineUserContext))
                        {
                            executeOnlineScreen.isHomeForm += new FSKExecuteOnlineTransaction.IsHomeForm(SetCoverPageIsNotHome);
                            executeOnlineScreen.ShowDialog();
                        }
                        log.LogMethodExit(fskExecuteOnlineTrxEnabled, "fskExecuteOnlineTrxEnabled");
                        return;
                    }
                    else if (fskSalesEnabled)
                    {
                        KioskStatic.logToFile("Launching frmHome");
                        using (frmHome salesHomeScreen = new frmHome())
                        {
                            salesHomeScreen.isHomeForm += new frmHome.IsHomeForm(SetCoverPageIsNotHome);
                            salesHomeScreen.ShowDialog();
                        }
                        log.LogMethodExit(fskSalesEnabled, "fskSalesEnabled");
                        return;
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 1700);
                        //Enable Sales option and/or Retrieve my purchase option to proceed
                        KioskStatic.logToFile(msg);
                        frmOKMsg.ShowUserMessage(msg);
                        KioskStatic.logToFile("Exiting Kiosk App");
                        Environment.Exit(1);
                    }
                }

                if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
                {
                    if (KioskStatic.CurrentTheme.SplashAfterSeconds == 0)//Playpass1:Starts
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
                    ResetKioskTimer();

                if (!KioskStatic.questStatus)
                {
                    txtMessage.Text = MessageContainerList.GetMessage(machineUserContext, 1692);
                }

                kioskMonitor = new Monitor();
                kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1693));
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_KIOSK_DIRECT_CASH", false) == true)
                {
                    EnableCommonAcceptor();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in FrmFSKCoverPage_Load: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCoverPageIsNotHome(bool closeForm)
        {
            log.LogMethodEntry(closeForm);
            coverPageIsNotHome = closeForm;
            log.LogMethodExit();
        }
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || KioskStatic.CurrentTheme.SplashScreenImage != null)
                {
                    int tickSecondsRemaining = GetKioskTimerSecondsValue();
                    tickSecondsRemaining--;
                    setKioskTimerSecondsValue(tickSecondsRemaining);

                    if (tickSecondsRemaining <= 0)
                    {
                        if (commonBillAcceptor != null && BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                        {
                            setKioskTimerSecondsValue(tickSecondsRemaining - 5);
                            KioskStatic.logToFile("Bill acceptor is processing notes. Cannot proceed");
                            log.LogVariableState("Bill acceptor is processing notes. Cannot proceed", null);
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in KioskTimer_Tick: " + ex.Message);
            }

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
                if (!coverPageIsNotHome)
                {
                    if (Application.OpenForms.Count > 1)
                    {
                        try
                        {
                            Application.OpenForms[Application.OpenForms.Count - 1].Focus();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
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
                    }

                    this.TopMost = true;
                    this.TopMost = false;

                    this.Focus();
                    this.Activate();
                    StartKioskTimer();
                    KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                    log.LogVariableState("Kiosk app running...", null);
                    if (Application.OpenForms.Count == 1)
                    {
                        if (Audio.soundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                            PlayHomeScreenAudio();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in FrmFSKCoverPage_Activated: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmFSKCoverPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if ((int)e.KeyChar == 3)
                Cursor.Show();
            else if ((int)e.KeyChar == 8)
                Cursor.Hide();
            else
                e.Handled = true;

            log.LogMethodExit();
        }
        private void BtnFSKSales_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("FSK Sales button click");
            try
            {
                DisableCommonBillAcceptor();

                using (frmHome frm = new frmHome())
                {
                    frm.isHomeForm += new frmHome.IsHomeForm(SetCoverPageIsNotHome);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in BtnFSKSales_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                this.Activate();
                KioskStatic.logToFile("Exit FSK Sales button click");
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
                CheckCardDispenserDependency();
                DisableCommonBillAcceptor();
                using (FSKExecuteOnlineTransaction frm = new FSKExecuteOnlineTransaction(machineUserContext))
                {
                    frm.isHomeForm += new FSKExecuteOnlineTransaction.IsHomeForm(SetCoverPageIsNotHome);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in BtnExecuteOnlineTransaction_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                EnableCommonAcceptor();
                this.Activate();
                KioskStatic.logToFile("Exit Execute Online Transaction button click");
            }
            log.LogMethodExit();
        }
        private void CardDispenserStatusCheck()
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                if (cardDispenser != null)
                {
                    string mes = "";

                    if (isDispenserCardReaderValid == false)
                    {
                        cardDispenser.dispenserWorking = false;
                        dispenserMessage = MessageContainerList.GetMessage(machineUserContext, 1696);
                    }
                    else
                    {
                        int cardPosition = -1;
                        bool suc = cardDispenser.checkStatus(ref cardPosition, ref mes);
                        dispenserMessage = mes;
                        if (suc)
                        {
                            if (cardPosition == 3)
                            {
                                cardDispenser.dispenserWorking = false;
                                KioskStatic.logToFile("Card at mouth positon. Please remove card.");
                                log.LogVariableState("Card at mouth positon. Please remove card.", cardDispenser.dispenserWorking);
                            }
                            else if (cardPosition == 2)
                            {
                                cardDispenser.dispenserWorking = false;
                                string message = "";
                                KioskStatic.logToFile("Card at read positon. Ejecting.");
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.LogVariableState(message, cardDispenser.dispenserWorking);
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(mes);
                            dispenserMessage = utilities.MessageUtils.getMessage(377);
                        }
                    }
                    cardDispenserMonitor.Post((cardDispenser.dispenserWorking ? Monitor.MonitorLogStatus.INFO : Monitor.MonitorLogStatus.ERROR), string.IsNullOrEmpty(dispenserMessage) ? "Dispenser working" : dispenserMessage + ": " + mes);
                }

                if (dispenserMessage != "")
                    txtMessage.Text = dispenserMessage;
                else
                    txtMessage.Text = utilities.MessageUtils.getMessage(462);

                log.LogVariableState(txtMessage.Text, null);
                StartKioskTimer();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in CardStatusCheck: " + ex.Message + ": " + ex.StackTrace);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CheckCardDispenserDependency()
        {
            log.LogMethodEntry();
            string errorMessage = "";
            log.Debug("Disport: " + KioskStatic.config.dispport);
            if (KioskStatic.config.dispport == -1)
            {
                string mes = MessageContainerList.GetMessage(utilities.ExecutionContext, 2384);
                KioskStatic.logToFile(mes);
                log.LogMethodExit("Disabled the card dispenser by setting card dispenser port = -1");
                return;
            }
            if (cardDispenser == null)
            {
                txtMessage.Text = errorMessage = utilities.MessageUtils.getMessage(460);
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                txtMessage.Text = errorMessage = string.IsNullOrEmpty(dispenserMessage) ? utilities.MessageUtils.getMessage(460) : dispenserMessage;
            }
            else if (KioskStatic.DISABLE_PURCHASE_ON_CARD_LOW_LEVEL && cardDispenser.cardLowlevel)
            {
                txtMessage.Text = errorMessage = utilities.MessageUtils.getMessage(378) + ". " + utilities.MessageUtils.getMessage(441);
            }
            if (!String.IsNullOrEmpty(errorMessage))
            {
                log.LogVariableState("errorMessage:", errorMessage);
                KioskStatic.logToFile(errorMessage);
                throw new Exception(errorMessage);
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
                    Audio.PlayAudio(Audio.ChooseOption);

                homeAudioInterval++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in playHomeScreenAudio: " + ex.Message);
            }
            log.LogMethodExit();
        } 
        private void EnableCommonAcceptor()
        {
            log.LogMethodEntry();
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
                    KioskStatic.logToFile("Error in EnableCommonAcceptor: " + ex.Message);
                }
            }
            log.LogMethodExit();
        }
        void DisableCommonBillAcceptor()
        {
            log.LogMethodEntry();
            if (commonBillAcceptor != null)
            {
                if (BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                {
                    log.Error("Money is inserted into Bill Acceptor. Please wait till processing is over");
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1701));
                }
                try
                {
                    commonBillAcceptor.Stop();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in DisableCommonBillAcceptor: " + ex.Message);
                }
            }
            log.LogMethodExit();
        } 
        private void UpdateDisplayMsg(string message)
        {
            log.LogMethodEntry(message);
            this.txtMessage.Text = message;
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
                KioskStatic.logToFile("Error in HandleBillAcceptorEvent: " + ex.Message);
            }
            finally
            {
                EnableCommonAcceptor();
            }
            log.LogMethodExit();
        }
        private void TopUpCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                            KioskStatic.logToFile(ex.Message);
                            return;
                        }
                        try
                        {
                            scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                        }
                        catch (ValidationException ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile(ex.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile(ex.Message);
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
                    if ((sender as DeviceClass).GetType().ToString().Contains("KeyboardWedge"))
                    {
                        lclCardNumber = KioskStatic.ReverseTopupCardNumber(lclCardNumber);
                    }
                    HandleTopUpCardRead(lclCardNumber, sender as DeviceClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Cover page Card Tap error: " + ex.Message);
            }
            log.LogMethodExit();
        } 
        void HandleTopUpCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            try
            {
                KioskStatic.logToFile("Cover page Card Tap: " + inCardNumber);
                log.LogVariableState("Cover page Card Tap: ", inCardNumber);
                if (Application.OpenForms.Count > 1)
                {
                    KioskStatic.logToFile("Not in Cover page. Ignored.");
                    log.LogVariableState("Not in Cover page. Ignored.", Application.OpenForms.Count);
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
                        using (frmAdmin adminForm = new frmAdmin(machineUserContext, isManagerCard))
                        {
                            if (adminForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                KioskStatic.logToFile("Exit from Admin screen");
                                log.LogVariableState("Exit from Admin screen", card);
                                Close();
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

        private void FrmFSKCoverPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                try
                {
                    KioskStatic.spCoinAcceptor.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                try
                {
                    if (utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y")
                        KioskStatic.CloseFiscalPrinter();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                if (utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.TopUpReaderDevice.Dispose();
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                    KioskStatic.logToFile(this.Name + ": Dispose topup card reader device");
                    log.Info(this.Name + ": Dispose topup card reader device");
                }

                if (KioskStatic.DispenserReaderDevice != null)
                    KioskStatic.DispenserReaderDevice.Dispose();

                if (KioskStatic.TopUpReaderDevice != null)
                {
                    KioskStatic.TopUpReaderDevice.Dispose();
                    KioskStatic.logToFile(this.Name + ": Dispose topup card reader device");
                    log.Info(this.Name + ": Dispose topup card reader device");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in FrmFSKCoverPage_FormClosing: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            try
            {
                DisableCommonBillAcceptor();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in FrmFSKCoverPage_FormClosing: "+ ex.Message + Environment.NewLine + ex.StackTrace);
            } 
            KioskStatic.logToFile("App exiting");
            log.LogMethodExit("App exiting");
        }


        private void DispenserCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    TagNumber tagNumber = null;
                    if (tagNumberParser != null && tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(checkScannedEvent.Message);
                        KioskStatic.logToFile(message);
                        log.LogMethodExit(null, "Invalid Tag Number. " + message);
                        return;
                    }

                    string lclCardNumber = (tagNumber != null ? tagNumber.Value : "");
                    log.LogVariableState("lclCardNumber", lclCardNumber);
                    HandleDispenserCardRead(lclCardNumber, sender as DeviceClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in DispenserCardScanCompleteEventHandle: " + ex.Message);
                KioskStatic.logToFile("Remove any card that is present at dispenser read or eject position(s)");
            }
            log.LogMethodExit();
        }

       private  void HandleDispenserCardRead(string cardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(cardNumber, readerDevice);
            try
            {
                KioskStatic.logToFile("Cover page:Dispenser Card: " + cardNumber);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in HandleDispenserCardRead: " + ex.Message);
            }
            log.LogMethodExit();
        }




        private void spCoinAcceptor_DataReceived(System.Object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Threading.Thread.Sleep(40);
                KioskStatic.coinAcceptor_rec = new byte[22];
                KioskStatic.spCoinAcceptor.Read(KioskStatic.coinAcceptor_rec, 0, KioskStatic.spCoinAcceptor.BytesToRead);
                KioskStatic.coinAcceptorDatareceived = true;

                if (KioskStatic.caReceiveAction != null)
                    KioskStatic.caReceiveAction.Invoke();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in spCoinAcceptor_DataReceived: " + ex.Message);
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
            bool ret = base.ProcessCmdKey(ref msg, keyData);
            log.LogMethodExit(ret);
            return ret;
        }

        private void GetCoverPageConfigs()
        {
            log.LogMethodEntry();
            fskSalesEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_SALES_OPTION") == "Y");
            fskExecuteOnlineTrxEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_EXECUTE_TRANSACTION_OPTION") == "Y");
            singleFunctionMode = !(fskSalesEnabled && fskExecuteOnlineTrxEnabled);
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
                frmOKMsg.ShowUserMessage(ex.Message + ". " + utilities.MessageUtils.getMessage(441));
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
                Environment.Exit(1);
            }
            else if (result == 1)
            {
                string message = utilities.MessageUtils.getMessage(2291); //"The app version is not in sync with the server. Please sync version with the server."
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
                KioskStatic.logToFile("Error while getting App Version:" + ex.Message);
            }

            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in FSKCoverPage");
            try
            {
                this.btnFSKSales.Text = MessageContainerList.GetMessage(machineUserContext, "New Purchase/Topups");
                this.btnExecuteOnlineTransaction.Text = MessageContainerList.GetMessage(machineUserContext, "Retrieve My Purchase");
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.FSKCoverPageLblSiteNameTextForeColor;//Please Tap your card at the reader
                this.btnFSKSales.ForeColor = KioskStatic.CurrentTheme.FSKCoverPageBtnFSKSalesTextForeColor;//New Card button
                this.btnExecuteOnlineTransaction.ForeColor = KioskStatic.CurrentTheme.FSKCoverPageBtnExecuteOnlineTransactionTextForeColor;//Yes button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FSKCoverPageTxtMessageTextForeColor;//Close button
                this.lblAppVersion.ForeColor = KioskStatic.CurrentTheme.ApplicationVersionForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in FSKCoverPage: " + ex.Message);
            }
            log.LogMethodExit();
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
    }
}
