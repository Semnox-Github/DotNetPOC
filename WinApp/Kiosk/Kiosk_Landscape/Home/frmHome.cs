/********************************************************************************************
* Project Name - Parafait_Kiosk -frmHome
* Description  - frmHome 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.70        1-Jul-2019      Lakshminarayana      Modified to add support for ULC cards
* 2.80        4-Sep-2019      Deeksha              Added logger methods.
* 2.80        8-Nov-2019      Girish Kundar        Ticket printer integration
* 2.80       03-Jan-2020      Jinto Thomas         Modified query for selecting variable recharge product in initKiosk()
* 2.90       15-Jul-2020      Deeksha              Ticket# 649045: Default_Language not picking up from POS level
* 2.110      21-Dec-2020      Jinto Thomas         Modified: As part of WristBand printer changes
* 2.150.1    22-Feb-2023      Guru S A             Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.logger;
using Semnox.Parafait.Transaction;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Linq;

namespace Parafait_Kiosk
{
    public partial class frmHome : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Timer ClearDisplayTimer;
        CardDispenser cardDispenser;
        string dispenserMessage = "";
        string printer_msg = "";
        bool isDispenserCardReaderValid = false;
        Monitor kioskMonitor;
        Monitor cardDispenserMonitor;
        Monitor billAcceptorMonitor;
        public string cardNumber;
        public Card Card;

        public string entitlementType;

        int printer_count = 0;
        public CommonBillAcceptor commonBillAcceptor = null;
        private readonly TagNumberParser tagNumberParser;
        public Utilities Utilities;
        public frmHome()
        {
            log.LogMethodEntry();
            DoubleBuffered = true;
            // check connectivity
            try
            {
                Utilities = KioskStatic.Utilities = new Utilities();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("DB Connection Error" + ex.Message);
                Program.ShowTaskbar();
                Environment.Exit(0);
            }
            InitializeComponent();
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            btnLanguage.Text = Utilities.ParafaitEnv.LanguageName;

            if (Utilities.getParafaitDefaults("ALLOW_KIOSK_LANGUAGE_CHANGE").Equals("N"))
                btnLanguage.Visible = false;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            foreach (Control c in flpOptions.Controls)
                c.Text += Environment.NewLine + Environment.NewLine;

            initKiosk();
            Utilities.setLanguage();
            Utilities.setLanguage(this);
            if (ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo != null)
            {
                pbSemnox.Image = ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo;
            }
            //else
            //{
            //    pbSemnox.Image = Properties.Resources.Semnox;
            //}
            log.LogMethodExit();
        }

        bool checkNewCardDependency()
        {
            log.LogMethodEntry();

            if (KioskStatic.config.dispport == -1)
            {
                KioskStatic.logToFile("card dispenser is Disabled by setting card dispenser port = -1");
                log.LogMethodExit("card dispenser is Disabled  by setting card dispenser port = -1");
                return true;
            }

            if (cardDispenser == null)
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(460);
                KioskStatic.logToFile("Card dispenser is null (not initialized)");
                using (frmOKMsg f = new frmOKMsg(txtMessage.Text + ". " + Utilities.MessageUtils.getMessage(441)))
                { f.ShowDialog(); }
                log.LogMethodExit("Card dispenser is null(not initialized)");
                return false;
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                txtMessage.Text = string.IsNullOrEmpty(dispenserMessage) ? Utilities.MessageUtils.getMessage(460) : dispenserMessage;
                log.Info(txtMessage.Text);
                KioskStatic.logToFile(txtMessage.Text);
                using (frmOKMsg f = new frmOKMsg(txtMessage.Text + ". " + Utilities.MessageUtils.getMessage(441)))
                {
                    f.ShowDialog();
                }
                log.LogMethodExit("Card dispenser is not working");
                return false;
            }
            else if (KioskStatic.DISABLE_PURCHASE_ON_CARD_LOW_LEVEL && cardDispenser.cardLowlevel)
            {
                string mes = Utilities.MessageUtils.getMessage(378) + ". " + Utilities.MessageUtils.getMessage(441);
                KioskStatic.logToFile(mes);
                using (frmOKMsg f = new frmOKMsg(mes))
                {
                    f.ShowDialog();
                }
                log.Info(mes);
                log.LogMethodExit("Card dispenser is low on cards");
                return false;
            }
            log.LogMethodExit("true");
            return true;
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("New Card click");
            if (KioskHelper.isTimeEnabledStore())
                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
            else
                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
            CardDispenserStatusCheck();
            if (!checkNewCardDependency())
            {
                log.LogMethodExit();
                return;
            }

            if (!KioskStatic.receipt && !Utilities.getParafaitDefaults("IGNORE_PRINTER_ERROR").Equals("Y"))
            {
                frmYesNo frmyn = new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(461));
                if (frmyn.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                {
                    frmyn.Dispose();
                    log.LogMethodExit();
                    return;
                }
                frmyn.Dispose();
            }

            try
            {
                disableCommonBillAcceptor();//:Modification on 17-Dec-2015 for introducing new theme

                kioskTransaction = new KioskTransaction(Utilities);
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETNEWCARDTYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    kioskTransaction = null;

                    frm.FormClosed += (s, ea) =>
                    {
                        this.Activate();
                    };
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
                enableCommonAcceptor();
                KioskStatic.logToFile("Exit New Card click");
                log.LogMethodExit();
            }
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Top-up click");
            if (KioskHelper.isTimeEnabledStore())
                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
            else
                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;

            if (!KioskStatic.receipt && !Utilities.getParafaitDefaults("IGNORE_PRINTER_ERROR").Equals("Y"))
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
                disableCommonBillAcceptor();
                kioskTransaction = new KioskTransaction(Utilities);
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETRECHAREGETYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    kioskTransaction = null;
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
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                enableCommonAcceptor();
                KioskStatic.logToFile("Exit New Card click");
            }
        }

        int homeAudioInterval = 0;
        void playHomeScreenAudio()
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
                    //Audio.PlayAudio(Audio.ChooseOption, Audio.BuyNewCard, Audio.TopUpCard, Audio.CheckBalance_Activity, Audio.Register);
                    Audio.PlayAudio(Audio.ChooseOption);
                else
                    Audio.PlayAudio(Audio.ChooseOption);

                homeAudioInterval++;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("playHomeScreenAudio: " + ex.Message);
            }
            log.LogMethodExit();
        }

        void initKiosk()
        {
            log.LogMethodEntry(); 
            StopKioskTimer();
            string keyMessage = "";
            lblDebug.Text = "";

            KeyManagement km = new KeyManagement(KioskStatic.Utilities.DBUtilities, KioskStatic.Utilities.ParafaitEnv);
            if (!km.validateLicense(ref keyMessage))
            {
                MessageBox.Show(keyMessage, "Validate License Key");
                flpOptions.Enabled = false;
                txtMessage.Text = keyMessage;
            }
            else
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(462);
                playHomeScreenAudio();
            }

            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch (Exception ex){ log.Error(ex); }

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select user_id from users where loginId = 'External POS'";
            try
            {
                KioskStatic.Utilities.ParafaitEnv.User_Id = Convert.ToInt32(cmd.ExecuteScalar());
                KioskStatic.Utilities.ParafaitEnv.LoginID = "External POS";
                if (Utilities.ParafaitEnv.User_Id <= 0)
                {
                    MessageBox.Show("Please define External POS user id in users table");
                    log.Error("Please define External POS user id in users table");
                    Program.ShowTaskbar();
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                log.Error("Please define External POS user id in users table", ex);
                MessageBox.Show("Please define External POS user id in users table");
                Program.ShowTaskbar();
                Environment.Exit(1);
            }

            Utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            Utilities.ParafaitEnv.Initialize();
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Utilities.ParafaitEnv.LoginID);
            machineUserContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
            
            object o = Utilities.getParafaitDefaults("KIOSK_VARIABLE_TOPUP_PRODUCT");
            try
            {
                KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = Convert.ToInt32(o);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId = -1;
            }

            if (KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId < 0)
            {
               o = Utilities.executeScalar(@"select top 1 p.product_id 
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

            KioskStatic.get_config(KioskStatic.Utilities.ExecutionContext);
            btnFAQ.Visible = KioskStatic.EnableFAQ;
            ThemeManager.setThemeImages(KioskStatic.CurrentTheme.ThemeId.ToString());
            KioskStatic.formatMessageLine(txtMessage, 23, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);

            KioskStatic.logToFile("App started");

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage; 

            btnNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;
            if (btnNewCard.BackgroundImage != null)
                btnNewCard.Size = btnNewCard.BackgroundImage.Size;
            btnRecharge.BackgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig;
            if (btnRecharge.BackgroundImage != null)
                btnRecharge.Size = btnRecharge.BackgroundImage.Size;
            btnRedeemTokens.BackgroundImage = ThemeManager.CurrentThemeImages.ExchangeTokensButtonSmall;
            if (btnRedeemTokens.BackgroundImage != null)
                btnRedeemTokens.Size = btnRedeemTokens.BackgroundImage.Size;
            btnCheckBalance.BackgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceButtonSmall;
            if (btnCheckBalance.BackgroundImage != null)
                btnCheckBalance.Size = btnCheckBalance.BackgroundImage.Size;
            btnRegister.BackgroundImage = ThemeManager.CurrentThemeImages.RegisterPassSmall;
            if (btnRegister.BackgroundImage != null)
                btnRegister.Size = btnRegister.BackgroundImage.Size;
            btnTransfer.BackgroundImage = ThemeManager.CurrentThemeImages.TransferPointButtonSmall;
            if (btnTransfer.BackgroundImage != null)
                btnTransfer.Size = btnTransfer.BackgroundImage.Size;
            btnFAQ.BackgroundImage = btnLanguage.BackgroundImage = ThemeManager.CurrentThemeImages.TermsButton;
            
            try
            {
                if (KioskStatic.DisablePurchase)
                {
                    flpBigButtons.Controls.Remove(btnNewCard);
                    flpBigButtons.Controls.Remove(btnRecharge);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (KioskStatic.DisableNewCard)
                {
                    flpBigButtons.Controls.Remove(btnNewCard);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "DISABLE_RECHARGE", false))
                {
                    flpBigButtons.Controls.Remove(btnRecharge);
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
                    flpSmallButtons.Controls.Remove(btnRegister);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (!KioskStatic.EnableTransfer)
                {
                    flpSmallButtons.Controls.Remove(btnTransfer);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (!KioskStatic.EnableRedeemTokens)
                {
                    flpSmallButtons.Controls.Remove(btnRedeemTokens);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (!KioskStatic.AllowPointsToTimeConversion)
                {
                    flpBigButtons.Controls.Remove(btnPointsToTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            try
            {
                if (!KioskStatic.EnablePauseCard)
                {
                    flpBigButtons.Controls.Remove(btnPauseTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (flpBigButtons.Controls.Count == 0)
                flpOptions.Controls.Remove(flpBigButtons);
           

            if (flpOptions.Controls.Count == 2)
            {
                if (flpBigButtons.Controls.Count >= 3)
                {
                    btnNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonSmall;
                    btnRecharge.BackgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonSmall;
                    btnPauseTime.BackgroundImage = ThemeManager.CurrentThemeImages.PauseCardImage;
                    btnPointsToTime.BackgroundImage = ThemeManager.CurrentThemeImages.PointsToTimeConvertionImage;
                    flpBigButtons.Height = btnPauseTime.Height + 10;
                    btnNewCard.Size = btnRecharge.Size = btnPauseTime.Size;
                    btnNewCard.Font = btnRecharge.Font = btnPauseTime.Font;
                    btnNewCard.Margin = btnRecharge.Margin = new Padding(10, 10, 10, 10);
                    flpBigButtons.Location = new Point(0, 0);
                    flpOptions.Location = new Point(flpOptions.Location.X, flpOptions.Location.Y + 50);
                    if (flpBigButtons.Controls.Count == 3)
                    {
                        flpBigButtons.Controls[0].Margin = new Padding(20 + (((btnPauseTime.Width + btnPauseTime.Margin.Left) * (4 - flpBigButtons.Controls.Count)) / 2), 10, 10, 0);
                        flpBigButtons.Margin = new Padding(0, 0, 0, 20);
                    }
                }
                else if (flpBigButtons.Controls.Count == 2)
                {
                    btnNewCard.Margin = btnRecharge.Margin = new Padding(10, 10, 10, 10);
                    flpBigButtons.Height = btnNewCard.Height + 10;
                }
            }
            else if (flpOptions.Controls.Count == 1)
            {
                if (flpSmallButtons.Controls.Count == 4)
                {
                    flpSmallButtons.Margin = new Padding(((flpOptions.Width - flpSmallButtons.Width) / 2), 0, 0, 0);
                    flpSmallButtons.Size = new Size((btnCheckBalance.Width * 2) + 50, (btnCheckBalance.Height * 2) + 50);
                    flpSmallButtons.Margin = btnRecharge.Margin = new Padding((flpOptions.Width - flpSmallButtons.Width) / 2, (flpOptions.Height - flpSmallButtons.Height) / 2, 0, 0);
                }
                else
                {
                    flpSmallButtons.Margin = new Padding(0, (flpOptions.Height - flpSmallButtons.Height) / 2, 0, 0);
                }
            }
            if (flpSmallButtons.Controls.Count < 4 && flpSmallButtons.Controls.Count > 0)
            {
                flpSmallButtons.Controls[0].Margin = new Padding(20 + (((btnCheckBalance.Width + btnCheckBalance.Margin.Left) * (4 - flpSmallButtons.Controls.Count)) / 2), 10, 10, 0);
            }
            else if (flpSmallButtons.Controls.Count == 0)
            {
                flpOptions.Controls.Remove(flpSmallButtons);
                btnNewCard.Margin = btnRecharge.Margin = new Padding(10, (flpOptions.Height - btnNewCard.Height) / 2, 0, 0);
            }
            
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                if (KioskStatic.config.cardAcceptorport > 0)
                {
                    KioskStatic.cardAcceptor = KioskStatic.getCardAcceptor(KioskStatic.config.cardAcceptorport.ToString());
                    string message = "";
                     
                    if (KioskStatic.cardAcceptor.Initialize(ref message) == false)
                    {
                        MessageBox.Show("Card Acceptor: " + message);
                        KioskStatic.cardAcceptor = null;
                    }

                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.TopUpReaderDevice = new SK310UR04((int)KioskStatic.cardAcceptor.deviceHandle, "COM" + KioskStatic.config.cardAcceptorport.ToString());
                        EventHandler CardScanCompleteEvent = new EventHandler(TopUpCardScanCompleteEventHandle);
                        KioskStatic.TopUpReaderDevice.Register(CardScanCompleteEvent);
                    }

                    Utilities.getMifareCustomerKey();
                }
            }
            else
            {
                 
                try
                {
                    log.Info("Registering top up reader");
                    KioskStatic.TopUpReaderDevice = DeviceContainer.RegisterTopupCardReader(Utilities.ExecutionContext, this, TopUpCardScanCompleteEventHandle);
                    log.Info("Top up reader is registered");
                }
                catch (Exception ex)
                { 
                    log.Error("Error registering top up reader", ex);
                    using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + Utilities.MessageUtils.getMessage(441)))
                    {
                        f.ShowDialog();
                    }
                }
            }

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
                    using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + Utilities.MessageUtils.getMessage(441)))
                    {
                        f.ShowDialog();
                    }
                }
            } 

            lblSiteName.Text = KioskStatic.SiteHeading; 
            KioskStatic.get_receipt();

            DeviceContainer.returnMessageToUI += new DeviceContainer.ReturnMessageToUI(UpdateDisplayMsg);
            DeviceContainer.InitializeSerialPorts();

            RestartRFIDPrinter();
            try
            {
                if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y")
                    KioskStatic.InitializeFiscalPrinter();
            }
            catch (Exception ex){ log.Error(ex); }

            if (KioskStatic.config.dispport > 0)
            {
                cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                cardDispenserMonitor = new Monitor(Monitor.MonitorAppModule.CARD_DISPENSER);
            }
             
            StartKioskTimer();

            this.Activate();
            if (KioskStatic.debugMode == false)
                Cursor.Hide();

            getLanguages();
            if (KioskStatic.config.baport > 0)  //&& KioskStatic.ccPaymentModeDetails != null)
            {
                try
                {
                    if (!KioskStatic.DisablePurchase)
                    {
                        billAcceptorMonitor = new Monitor(Monitor.MonitorAppModule.BILL_ACCEPTOR);
                        if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_KIOSK_DIRECT_CASH") == "Y")
                        {
                            commonBillAcceptor = new CommonBillAcceptor(KioskStatic.Utilities.ExecutionContext, billAcceptorMonitor, KioskStatic.config.baport.ToString());
                            enableCommonAcceptor();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void enableCommonAcceptor()
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
                    KioskStatic.logToFile(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void disableCommonBillAcceptor()
        {
            log.LogMethodEntry();
            if (commonBillAcceptor != null)
            {
                if (BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                {
                    log.Error("Money is inserted into Bill Acceptor. Please wait till processing is over");
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1701));
                }
                try
                {
                    commonBillAcceptor.Stop();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile(ex.Message);
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
                    log.Error(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1701));
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1701));
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
        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Check Balance click");
                disableCommonBillAcceptor();
                using (frmTapCard ftc = new frmTapCard())
                {
                    ftc.ShowDialog();
                    if (ftc.Card == null)
                    {
                        ftc.Dispose();
                        log.LogMethodExit();
                        return;
                    }

                    Card card = ftc.Card;
                    ftc.Dispose();

                    using (frmCheckBalance frmchkBal = new frmCheckBalance(card))
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
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                enableCommonAcceptor();
                KioskStatic.logToFile("exit CheckBalance click");
                log.LogMethodExit();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Register click");
            try
            {
                disableCommonBillAcceptor();
                string CardNumber = "";
                if (KioskStatic.AllowRegisterWithoutCard)
                {
                    using (frmTapCard ftc = new frmTapCard(Utilities.MessageUtils.getMessage(496), Utilities.MessageUtils.getMessage("Yes")))
                    {
                        DialogResult dr = ftc.ShowDialog();
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            CardNumber = null;
                            log.LogVariableState("CardNumber", CardNumber);
                        }
                        else if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit("ftc.Card == null");
                            return;
                        }
                        else
                        {
                            CardNumber = ftc.cardNumber;
                            log.LogVariableState("CardNumber", CardNumber);
                        }

                        ftc.Dispose();
                    }
                }
                else
                {
                    using (frmTapCard ftc = new frmTapCard(Utilities.MessageUtils.getMessage(500)))
                    {
                        ftc.ShowDialog();
                        if (ftc.Card == null)
                        {
                            ftc.Dispose();
                            log.LogMethodExit("ftc.Card == null");
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
                        log.LogVariableState("txtMessage.Text: ", txtMessage.Text);
                        log.LogMethodExit("custRegisterCard.technician_card.Equals('Y')");
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
                enableCommonAcceptor();
                KioskStatic.logToFile("exit Register_Click()");
                log.LogMethodExit();
            } 
        }
        
        private void TopUpCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
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
            log.LogMethodExit();
        }

        void HandleTopUpCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            try
            {
                KioskStatic.logToFile("HomeScreen Card Tap: " + inCardNumber);
                if (Application.OpenForms.Count > 1)
                {
                    KioskStatic.logToFile("Not in home screen. Ignored.");
                    log.LogMethodExit("Not in home screen. Ignored.");
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
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
                else
                    card = new Card(readerDevice, inCardNumber, "", Utilities);

                if (card != null)
                {
                    if (card.CardStatus == "NEW")
                    {
                        KioskStatic.logToFile("NEW Card tapped. Ignore");
                        log.LogMethodExit("NEW Card tapped. Ignore");
                        return;
                    }

                    if (card.customerDTO != null)
                    { 
                        StopKioskTimer();
                        txtMessage.Text = Utilities.MessageUtils.getMessage(463, card.customerDTO.FirstName);


                    }
                    else
                        txtMessage.Text = Utilities.MessageUtils.getMessage(462);

                    KioskStatic.logToFile(txtMessage.Text);

                    if (card.vip_customer == 'N')
                    {
                        card.getTotalRechargeAmount();
                        if ((card.credits_played >= Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                             || (card.TotalRechargeAmount >= Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                        {
                            using (frmOKMsg fok = new frmOKMsg(Utilities.MessageUtils.getMessage(451, KioskStatic.VIPTerm)))
                            {
                                fok.ShowDialog();
                                fok.Dispose();
                            }
                        }
                    }

                    if (card.technician_card == 'Y')
                    {
                        KioskStatic.logToFile("Tech card");
                        log.Info("Tech card");
                        frmAdmin adminForm = new frmAdmin();
                        if (adminForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            KioskStatic.logToFile("Exit from Admin screen");
                            log.Info("Exit from Admin screen");
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }

        int semnoxClickCount = 0;  
        int tickCount = 0; 
        void CardDispenserStatusCheck()
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
                        dispenserMessage = "Unable to register Dispenser card reader";
                        log.Error(dispenserMessage);
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
                                log.Info("Card at mouth positon. Please remove card.");
                            }
                            else if (cardPosition == 2)
                            {
                                cardDispenser.dispenserWorking = false;
                                string message = "";
                                KioskStatic.logToFile("Card at read positon. Ejecting.");
                                log.Info("Card at read positon. Ejecting.");
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.Info(message); 
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(mes);
                            log.Info(mes);
                            dispenserMessage = Utilities.MessageUtils.getMessage(377);
                        }
                    }
                    cardDispenserMonitor.Post((cardDispenser.dispenserWorking ? Monitor.MonitorLogStatus.INFO : Monitor.MonitorLogStatus.ERROR), string.IsNullOrEmpty(dispenserMessage) ? "Dispenser working" : dispenserMessage + ": " + mes);
                }

                if (dispenserMessage != "")
                    txtMessage.Text = dispenserMessage;
                else
                    txtMessage.Text = Utilities.MessageUtils.getMessage(462);

               StartKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("CardStatusCheck(): " + ex.Message + ": " + ex.StackTrace);
            }
            log.LogMethodExit();
        }

        private void frmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                
                //try
                //{
                //    KioskStatic.spCoinAcceptor.Close();
                //}
                //catch(Exception ex)
                //{
                //    log.Error(ex.Message);
                //}

                try
                {
                    if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y")
                        KioskStatic.CloseFiscalPrinter();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                if (Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.TopUpReaderDevice.Dispose();
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards(); 
                    }
                 

                    if (KioskStatic.DispenserReaderDevice != null)
                    KioskStatic.DispenserReaderDevice.Dispose();

                if (KioskStatic.TopUpReaderDevice != null)
                    KioskStatic.TopUpReaderDevice.Dispose();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            try
            {
                disableCommonBillAcceptor();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            KioskStatic.logToFile("App exiting");
            string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5148); //Exiting Kiosk
            KioskStatic.UpdateKioskActivityLog(Utilities.ExecutionContext, KioskTransaction.EXITKIOSKMSG, message);
            log.LogMethodExit("App exiting");
        } 

        private void frmHome_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnNewCard_MouseDown(object sender, MouseEventArgs e)
        {
            //btnNewCard.BackgroundImage = Properties.Resources.new_card_pressed;
        }

        private void btnNewCard_MouseUp(object sender, MouseEventArgs e)
        {
            // btnNewCard.BackgroundImage = Properties.Resources.New_Card_New;
        }

        private void btnRegister_MouseDown(object sender, MouseEventArgs e)
        {
            //btnRegister.BackgroundImage = Properties.Resources.regsiter_btn_pressed;
        }

        private void btnRegister_MouseUp(object sender, MouseEventArgs e)
        {
            //btnRegister.BackgroundImage = Properties.Resources.Register_New;
        }

        private void btnRecharge_MouseDown(object sender, MouseEventArgs e)
        {
            // btnRecharge.BackgroundImage = Properties.Resources.top_up_btn_pressed;
        }

        private void btnRecharge_MouseUp(object sender, MouseEventArgs e)
        {
            // btnRecharge.BackgroundImage = Properties.Resources.Add_Points_New;
        }

        private void btnCheckBalance_MouseDown(object sender, MouseEventArgs e)
        {
            //btnCheckBalance.BackgroundImage = Properties.Resources.check_balance_btn_pressed;
        }

        private void btnCheckBalance_MouseUp(object sender, MouseEventArgs e)
        {
            //btnCheckBalance.BackgroundImage = Properties.Resources.Check_Balance_New;
        }

        ListBox lstLang;

        void getLanguages()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable(@"select LanguageCode, LanguageName, LanguageId 
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
            lstLang.Font = new System.Drawing.Font(lstLang.Font.FontFamily, 24);
            this.Controls.Add(lstLang);
            lstLang.BringToFront();
            lstLang.Height = (int)(lstLang.CreateGraphics().MeasureString("English", lstLang.Font).Height * dt.Rows.Count);
            lstLang.MinimumSize = new System.Drawing.Size(lstLang.Width, 50);
            lstLang.Width = (int)(lstLang.CreateGraphics().MeasureString("Chinese (Taiwan)", lstLang.Font).Width);
            int verticalPoint = btnLanguage.Location.Y - lstLang.Height;
            if (verticalPoint < 0)
                verticalPoint = 10;
            lstLang.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - lstLang.Width, verticalPoint);
            lstLang.Visible = false;

            lstLang.SelectedValue = Utilities.ParafaitEnv.LanguageCode;

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
                if (lstLang.Items.Count == 2)
                {
                    DataTable dt = lstLang.DataSource as DataTable;
                    if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                        restartOnLanguageChange((int)dt.Rows[0]["LanguageId"], dt.Rows[0]["LanguageName"].ToString());
                    else
                        restartOnLanguageChange((int)dt.Rows[1]["LanguageId"], dt.Rows[1]["LanguageName"].ToString());
                    log.LogMethodExit();
                    return;
                }

                lstLang.Visible = !lstLang.Visible;

                if (lstLang.Visible)
                    Audio.PlayAudio(Audio.SelectLanguage);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnLanguage_Click() in Home screen: " + ex.Message);
            }
            finally
            {
                btnLanguage.Enabled = true;
            }
            KioskStatic.logToFile("Exit Language button click");
            log.LogMethodExit();
        }

        void lstLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            restartOnLanguageChange((int)(lstLang.SelectedItem as DataRowView).Row["LanguageId"], (lstLang.SelectedItem as DataRowView).Row["LanguageName"].ToString());
            log.LogMethodExit();
        }

        void restartOnLanguageChange(int languageId, string languageName)
        {
            log.LogMethodEntry(languageId, languageName);
            lstLang.Visible = false;

            KioskStatic.logToFile("Language change to " + languageName);
            log.Info("Language change to " + languageName);

            KioskStatic.Utilities.setLanguage(languageId);
            this.btnNewCard.Text = "Buy A New Card";
            this.btnRecharge.Text = "Top Up";
            this.btnCheckBalance.Text = "Check Balance / Activity";
            this.btnRegister.Text = "Register";
            this.btnTransfer.Text = "Transfer";
            this.btnRedeemTokens.Text = "Redeem Tokens";
            this.btnFAQ.Text = "FAQ";

            KioskStatic.Utilities.setLanguage(this);

            foreach (Control c in flpOptions.Controls)
                c.Text += Environment.NewLine + Environment.NewLine;

            if (lstLang.Items.Count == 2)
            {
                DataTable dt = lstLang.DataSource as DataTable;
                if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                    btnLanguage.Text = dt.Rows[1]["LanguageName"].ToString();
                else
                    btnLanguage.Text = dt.Rows[0]["LanguageName"].ToString();
            }
            else
            { btnLanguage.Text = languageName; }

            log.LogMethodExit();
        }
         

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Transfer balance click");
                disableCommonBillAcceptor();
                ResetKioskTimer();
                using (frmTapCard ftp = new frmTapCard())
                {
                    ftp.ShowDialog();
                    if (ftp.Card == null)
                    {
                        ftp.Dispose();
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                       if (ftp.Card.CardStatus == "NEW")
                        {

                            txtMessage.Text = Utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogMethodExit(txtMessage.Text);
                            return;
                        }
                    }

                    double totalCredits = ftp.Card.credits + ftp.Card.CreditPlusCardBalance + ftp.Card.CreditPlusCredits;
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
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
                                log.LogVariableState("entitlementType", entitlementType);
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
                    using (CardTransfer.frmTransferFrom tfFrom = new CardTransfer.frmTransferFrom(ftp.Card.CardNumber, entitlementType))
                    {
                        tfFrom.ShowDialog();
                        tfFrom.Dispose();
                    }
                    Audio.Stop();
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
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                enableCommonAcceptor();
            }
            log.LogMethodExit();
        }

        private void btnTransfer_MouseDown(object sender, MouseEventArgs e)
        {
            //btnTransfer.BackgroundImage = Properties.Resources.transfer_points_pressed;
        }

        private void btnTransfer_MouseUp(object sender, MouseEventArgs e)
        {
            // btnTransfer.BackgroundImage = Properties.Resources.Transfer_Points_New;
        }

        private void btnRedeemTokens_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Redeem tokens click");           

            try
            {
                disableCommonBillAcceptor();
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
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                enableCommonAcceptor(); 
            }
            log.LogMethodExit();
        }

        private void btnRedeemTokens_MouseDown(object sender, MouseEventArgs e)
        {
            // btnRedeemTokens.BackgroundImage = Properties.Resources.redeem_tokens_pressed;
        }

        private void btnRedeemTokens_MouseUp(object sender, MouseEventArgs e)
        {
            //btnRedeemTokens.BackgroundImage = Properties.Resources.Redeem_Tokens_New;
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
                    log.LogMethodExit();
                    return;
                }
                Audio.Stop();
                using (frmFAQ faq = new frmFAQ())
                { faq.ShowDialog(); }
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
            log.LogMethodExit();
        }

       
        private void pbSemnox_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                Cursor.Show();

            log.LogMethodExit();
        }
         
        private void frmHome_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (KioskHelper.isTimeEnabledStore())
                entitlementType = KioskTransaction.TIME_ENTITLEMENT;
            else
                entitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
            log.LogVariableState("entitlementType", entitlementType);
            if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)//Playpass1:Starts
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
                txtMessage.Text = "Quest not initialized.";
            } 

            kioskMonitor = new Monitor();
            kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, "Kiosk app started");
            string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5147); //Launching Kiosk
            KioskStatic.UpdateKioskActivityLog(Utilities.ExecutionContext, KioskTransaction.LAUNCHKIOSKMSG, message);
            log.LogMethodExit();
        }

         

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (ThemeManager.CurrentThemeImages.SplashScreenImage != null)
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
                    log.Info("Calling splash");
                    using (frmSplashScreen splashScreen = new frmSplashScreen())
                    {
                        splashScreen.ShowDialog();
                        ResetKioskTimer();
                    }
                }
            }

            if (this.Equals(Form.ActiveForm) || (Form.ActiveForm != null && Form.ActiveForm.Name != null && Form.ActiveForm.Name.Equals("frmSplashScreen")))
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(462);
                tickCount++;
                if (tickCount % 300 == 0) // refresh variable every 5 minutes
                {
                    kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, "Kiosk app running");
                    KioskStatic.logToFile("Kiosk app running...");
                    log.Info("Kiosk app running...");
                    KioskStatic.logToFile("Refresh defaults: getParafaitDefaults()");
                    log.Info("Refresh defaults: getParafaitDefaults()");
                    KioskStatic.getParafaitDefaults();
                    tickCount = 0;
                }
            }

            log.LogMethodExit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            if (keyData == Keys.Enter)
            {
                log.LogMethodExit("keyData == Keys.Enter");
                return true;
            }
            bool ret = base.ProcessCmdKey(ref msg, keyData);
            log.LogMethodExit(ret);
            return ret;
        }

        private void frmHome_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
            {
                //if (TimeOutTimer != null)
                //{
                //    TimeOutTimer.Stop();//playpass1:starts,ends//This line is used to stop the splash screen timer when activity is going on
                //}
            } 
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void frmHome_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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

            if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
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
            KioskStatic.logToFile("Kiosk app running...");
            log.Info("Kiosk app running...");
            if (Application.OpenForms.Count == 1)
            {
                if (Audio.soundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                    playHomeScreenAudio();
            }
            log.LogMethodExit();
        }

        private void btnPauseTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("btnPointsToTime_Click");
            ResetKioskTimer();
            string message = "";           
            try
            {
                disableCommonBillAcceptor();
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
                            txtMessage.Text = Utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogVariableState(txtMessage.Text, ftp.Card.CardStatus);
                            log.LogMethodExit();
                            return;
                        } 
                        Loyalty trxLoyalty = new Loyalty(Utilities);
                        bool isTimeRunning = trxLoyalty.IsCreditPlusTimeRunning(ftp.Card.card_id, null);
                        if (!isTimeRunning)
                        {
                            using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(1386)))
                            { f.ShowDialog(); }
                            log.LogVariableState("isTimeRunning", isTimeRunning);
                            log.LogMethodExit();
                            return;
                        }
                        TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                        if (!tp.CheckTimePauseLimit(ftp.Card.card_id, ref message))
                        {
                            using (frmOKMsg f = new frmOKMsg(message))
                            { f.ShowDialog(); }
                            log.LogVariableState("CheckTimePauseLimit() failure", message);
                            log.LogMethodExit();
                            return;
                        }
                        if (tp != null)
                            tp = null;
                    }

                    using (Transaction.frmPauseTime frm = new Transaction.frmPauseTime(Utilities.MessageUtils.getMessage("Balance Time will be paused"), ftp.cardNumber))
                    {
                        frm.ShowDialog();
                    }
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
                enableCommonAcceptor();
            }
            log.LogMethodExit();
        }

        private void btnPointsToTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnPointsToTime_Click");
            ResetKioskTimer();

            
            try
            {
                disableCommonBillAcceptor();
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
                            txtMessage.Text = Utilities.MessageUtils.getMessage(459);
                            KioskStatic.logToFile(txtMessage.Text);
                            log.LogVariableState(txtMessage.Text, ftp.Card.CardStatus);
                            log.LogMethodExit();
                            return;
                        }
                    }


                    using (Transaction.frmCreditsToTime frm = new Transaction.frmCreditsToTime(ftp.cardNumber))
                    { frm.ShowDialog(); }
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
                enableCommonAcceptor();
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
                DeviceContainer.RestartRFIDPrinter(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                log.Info("RFID Printer Restarted Successfully");
                KioskStatic.logToFile("RFID Printer Restarted Successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error restarting RFID Printer: " + ex.Message);
                using (frmOKMsg f = new frmOKMsg(ex.Message + ". " + Utilities.MessageUtils.getMessage(441)))
                {
                    f.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }
    }
}
