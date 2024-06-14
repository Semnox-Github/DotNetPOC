/********************************************************************************************
 * Project Name - frmHome
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *1.00                                            Created 
 *2.4.0      13-Mar-2019      Raghuveera          Card changed to account in check balance click and pause card click messages updated
 *2.70       1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 *2.80       20-Sep-2019      Archana             Modified to update FCCclientService config file for freedompay integration
 *2.80       11-Nov-2019      Girish Kundar       Modified : Ticket printer integration 
 *2.80       03-Jan-2020      Jinto Thomas        Modified query for selecting variable recharge product in initKiosk()
 *2.90       15-Jul-2020      Deeksha             Ticket# 649045: Default_Language not picking up from POS level
 *2.80.1     02-Feb-2021      Deeksha             Theme changes to support customized Images/Font
 *2.110      21-Dec-2020      Jinto Thomas        Modified: As part of WristBand printer changes
 *2.130.0    30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
 *2.140.0    22-Oct-2021      Sathyavathi         CEC enhancement - Application Version Changes
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Semnox.Parafait.Device;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Configuration;
using System.Xml;

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
        ExecutionContext machineUserContext;

        CommonBillAcceptor commonBillAcceptor;
        Monitor freewayClientServiceMonitor;
        ServiceController fccClientServiceController;

        public Utilities Utilities;
        public string cardNumber;
        public Card Card;

        public string entitlementType;

        public delegate void IsHomeForm(bool isHome);
        public IsHomeForm isHomeForm;
        private TagNumberParser tagNumberParser;
        //private bool incorrectCustomerSetupForWaiver = true;
        //private string waiverSetupErrorMsg = string.Empty;
        private bool singleFunctionMode = true;

        public frmHome()
        {
            log.LogMethodEntry();
            DoubleBuffered = true;
            Utilities = KioskStatic.Utilities;
            machineUserContext = Utilities.ExecutionContext;
            Utilities.setLanguage();
            InitializeComponent();
            Utilities.setLanguage(this); 

            btnLanguage.Text = Utilities.ParafaitEnv.LanguageName;

            if (Utilities.getParafaitDefaults("ALLOW_KIOSK_LANGUAGE_CHANGE").Equals("N"))
                btnLanguage.Visible = false;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            GetCoverPageConfigs();
            pbSemnox.BackColor = KioskStatic.MessageLineBackColor;
            initKiosk();
            //Utilities.setLanguage();
            //Utilities.setLanguage(this); 
            log.LogMethodExit();
        }

        bool checkNewCardDependency()
        {
            log.LogMethodEntry();
            log.Debug("Disport: " + KioskStatic.config.dispport);
            if (KioskStatic.config.dispport == -1)
            {
                string mes = Utilities.MessageUtils.getMessage(2384);
                KioskStatic.logToFile(mes);
                log.LogMethodExit("Disabled the card dispenser by setting card dispenser port = -1");
                return true;
            }
            if (cardDispenser == null)
            {
                txtMessage.Text = Utilities.MessageUtils.getMessage(460);
                KioskStatic.logToFile("Card dispenser is null (not initialized)");
                using (frmOKMsg f = new frmOKMsg(txtMessage.Text + ". " + Utilities.MessageUtils.getMessage(441)))
                {
                    f.ShowDialog();
                }
                log.LogVariableState("Card dispenser is null (not initialized)", cardDispenser);
                log.LogMethodExit(false);
                return false;
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                txtMessage.Text = string.IsNullOrEmpty(dispenserMessage) ? Utilities.MessageUtils.getMessage(460) : dispenserMessage;
                KioskStatic.logToFile(txtMessage.Text);
                using (frmOKMsg f = new frmOKMsg(txtMessage.Text + ". " + Utilities.MessageUtils.getMessage(441)))
                {
                    f.ShowDialog();
                }
                log.LogVariableState(txtMessage.Text, cardDispenser);
                log.LogMethodExit(false);
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
            KioskStatic.logToFile("New Card click");
            if (KioskHelper.isTimeEnabledStore())
                entitlementType = "Time";
            else
                entitlementType = "Credits";

            log.LogVariableState("entitlementType", entitlementType);
            CardDispenserStatusCheck();

            if (!checkNewCardDependency())
            {
                log.LogMethodExit();
                return;
            }

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
                Form frm = new Form();
                DataTable dTable = KioskStatic.GetProductDisplayGroups("I", entitlementType);
                if (dTable != null && dTable.Rows.Count > 1)
                {
                    using (frm = new frmChooseDisplayGroup("I", entitlementType))
                    {
                        frm.ShowDialog();
                        frm.FormClosed += (s, ea) =>
                        {
                            this.Activate();
                        };
                    }
                }
                else
                {
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (KioskHelper.isTimeEnabledStore() == true)
                        {
                            entitlementType = "Time";
                        }
                        else
                        {
                            using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                            {
                                if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    frmEntitle.Dispose();
                                    log.LogMethodExit();
                                    return;
                                }
                                entitlementType = frmEntitle.selectedEntitlement;
                                frmEntitle.Dispose();
                            }
                        }
                    }
                    string backgroundImageFileName = dTable.Rows[0]["BackgroundImageFileName"].ToString().Trim();
                    using (frm = new frmChooseProduct("I", entitlementType, "ALL", backgroundImageFileName))
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
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Top-up click");
            if (KioskHelper.isTimeEnabledStore())
                entitlementType = "Time";
            else
                entitlementType = "Credits";

            log.LogVariableState("entitlementType", entitlementType);
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
                Form frm;
                DataTable dTable = KioskStatic.GetProductDisplayGroups("R", entitlementType);
                if (dTable != null && dTable.Rows.Count > 1)
                {
                    frm = new frmChooseDisplayGroup("R", entitlementType);
                }
                else
                {
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (KioskHelper.isTimeEnabledStore() == true)
                        {
                            entitlementType = "Time";
                        }
                        else
                        {
                            using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                            {
                                if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    frmEntitle.Dispose();
                                    log.LogMethodExit();
                                    return;
                                }
                                entitlementType = frmEntitle.selectedEntitlement;
                                frmEntitle.Dispose();
                            }
                        }
                    }
                    string backgroundImageFileName = dTable.Rows[0]["BackgroundImageFileName"].ToString().Trim();
                    frm = new frmChooseProduct("R", entitlementType, "ALL", backgroundImageFileName);
                }

                frm.FormClosed += (s, ea) =>
                {
                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
                    }

                    this.Activate();
                };

                frm.ShowDialog();
                frm.Dispose();
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
                KioskStatic.logToFile("exit Top-up click");
            }
            log.LogMethodExit();
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
                KioskStatic.logToFile("playHomeScreenAudio: " + ex.Message);
            }
            log.LogMethodExit();
        }

        void initKiosk()
        {
            log.LogMethodEntry();
            StopKioskTimer();

            CheckApplicationVersion();

            lblDebug.Text = "";
            if (singleFunctionMode)
            {
                playHomeScreenAudio();
            }

            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            btnFAQ.Visible = KioskStatic.EnableFAQ;
            KioskStatic.setDefaultFont(this);

            this.BackgroundImage = KioskStatic.CurrentTheme.HomeScreenBackgroundImage;
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            KioskStatic.logToFile("App started");
            log.Info("App started");

            try
            {
                if (KioskStatic.DisablePurchase)
                {
                    flpOptions.Controls.Remove(btnNewCard);
                    flpOptions.Controls.Remove(btnRecharge);
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }


            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "DISABLE_RECHARGE", false))
                {
                    flpOptions.Controls.Remove(btnRecharge);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            try
            {
                if (!KioskStatic.RegistrationAllowed || KioskStatic.DisableCustomerRegistration)
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
                    //flpOptions.Controls.Remove(btnSignWaiver);
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
                    //flpOptions.Controls.Remove(btnPlaygroundEntry);
                }
                else
                {
                    //btnPlaygroundEntry.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            this.BackgroundImage = KioskStatic.CurrentTheme.HomeScreenBackgroundImage;
            btnFAQ.BackgroundImage = KioskStatic.CurrentTheme.FAQButton;
            btnLanguage.BackgroundImage = KioskStatic.CurrentTheme.LanguageButton;
            btnNewCard.BackgroundImage = KioskStatic.CurrentTheme.NewPlayPassButtonSmall;
            btnRecharge.BackgroundImage = KioskStatic.CurrentTheme.RechargePlayPassButtonSmall;
            btnPauseTime.BackgroundImage = KioskStatic.CurrentTheme.PauseCardButtonBig;
            btnPointsToTime.BackgroundImage = KioskStatic.CurrentTheme.PointsToTimeButtonBig;
            btnTransfer.BackgroundImage = KioskStatic.CurrentTheme.TransferPointButtonBig;
            btnRedeemTokens.BackgroundImage = KioskStatic.CurrentTheme.ExchangeTokensButtonBig;
            btnRegister.BackgroundImage = KioskStatic.CurrentTheme.RegisterPassSmall;
            btnCheckBalance.BackgroundImage = KioskStatic.CurrentTheme.CheckBalanceButtonSmall;

            if (KioskStatic.DisablePurchase)
            {
                foreach (Control c in flpOptions.Controls)
                {
                    Button b = c as Button;
                    b.BackgroundImage = btnRedeemTokens.BackgroundImage;
                    b.Size = btnRedeemTokens.Size;
                    b.ForeColor = btnRedeemTokens.ForeColor;
                }

                if (flpOptions.Controls.Contains(btnRegister))
                {
                    flpOptions.Controls.SetChildIndex(btnRegister, 0);
                }
            }
            else if (flpOptions.Controls.Count < 6)
            {
                btnRegister.Size = btnCheckBalance.Size = btnRedeemTokens.Size;
                //btnNewCard.Size = btnRecharge.Size = btnRegister.Size;
                btnRegister.ForeColor = btnCheckBalance.ForeColor = btnRedeemTokens.ForeColor;
                btnCheckBalance.BackgroundImage = KioskStatic.CurrentTheme.CheckBalanceButtonBig;
                //btnNewCard.BackgroundImage = KioskStatic.CurrentTheme.NewPlayPassButtonBig;
                //btnRecharge.BackgroundImage = KioskStatic.CurrentTheme.RechargePlayPassButtonBig;
                btnPauseTime.BackgroundImage = KioskStatic.CurrentTheme.PauseCardButtonBig;
                btnPointsToTime.BackgroundImage = KioskStatic.CurrentTheme.PointsToTimeButtonBig;
                btnTransfer.BackgroundImage = KioskStatic.CurrentTheme.TransferPointButtonBig;
                btnRedeemTokens.BackgroundImage = KioskStatic.CurrentTheme.ExchangeTokensButtonBig;
                btnRegister.BackgroundImage = KioskStatic.CurrentTheme.RegisterPassBig;
            }

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(TopUpCardScanCompleteEventHandle);
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            SetCustomizedFontColors();
            if (KioskStatic.DispenserReaderDevice != null || KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
            }

            lblSiteName.Text = KioskStatic.SiteHeading;


            if (KioskStatic.config.coinAcceptorport != 0 && KioskStatic.spCoinAcceptor != null)
            {
                KioskStatic.spCoinAcceptor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spCoinAcceptor_DataReceived);
            }

            if (KioskStatic.config.dispport > 0)
            {
                cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                if (singleFunctionMode)
                {
                    cardDispenserMonitor = new Monitor(Monitor.MonitorAppModule.CARD_DISPENSER);
                }
            }
            if (CheckIsKioskPaymentGatewayIsFreedomPay())
            {
                fccClientServiceController = new ServiceController("FCCClientSvc");
                freewayClientServiceMonitor = new Monitor(Monitor.MonitorAppModule.CREDIT_CARD_PAYMENT);
                UpdateServersConfig();
            }

            StartKioskTimer();

            this.Activate();
            if (KioskStatic.debugMode == false && singleFunctionMode)
            {
                Cursor.Hide();
            }

            getLanguages();
            if (KioskStatic.config.baport > 0)
            {
                try
                {
                    if (!KioskStatic.DisablePurchase)
                    {
                        if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_KIOSK_DIRECT_CASH") != "N")
                        {
                            commonBillAcceptor = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;
                            commonBillAcceptor.setReceiveAction = handleBillAcceptorEvent;
                            enableCommonAcceptor();
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
            log.LogMethodExit();
        }
        private void UpdateServersConfig()
        {
            log.LogMethodEntry();
            try
            {
                SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
                connBuilder.ConnectionString = ConfigurationManager.ConnectionStrings["Parafait_Kiosk.Properties.Settings.ParafaitConnectionString"].ToString();

                string server = connBuilder.DataSource;
                if (server.Contains("\\"))
                {
                    server = server.Substring(0, (server.IndexOf("\\")));
                }
                string serverIpAddress = GetIpAddressFromMachineName(server);

                string serverXmlFile = @"C:\Program Files (x86)\FreedomPay\FreewayCommerceConnect\servers.xml";
                if (serverXmlFile != string.Empty)
                {

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(serverXmlFile);

                    XmlNode node = xmlDocument.SelectSingleNode("/SusannaConfiguration/FigaroServers/FigaroServer");
                    if (serverIpAddress != node.Attributes["server"].Value)
                    {
                        node.Attributes["server"].Value = serverIpAddress;
                        xmlDocument.Save(serverXmlFile);
                        if (fccClientServiceController != null && (fccClientServiceController.Status == ServiceControllerStatus.Running || fccClientServiceController.Status == ServiceControllerStatus.Stopped))
                        {
                            freewayClientServiceMonitor.Post(Monitor.MonitorLogStatus.WARNING, "FCCClientService restart required");
                            log.Info("FCCClientService restart required");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Errow while updating servers xml file" + ex);
            }
            log.LogMethodExit();
        }
        private string GetIpAddressFromMachineName(string serverMachineNameName)
        {
            log.LogMethodEntry(serverMachineNameName);
            string serverIpAddress = string.Empty;
            try
            {
                System.Net.IPAddress ip = System.Net.Dns.GetHostEntry(serverMachineNameName).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                serverIpAddress = ip.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(serverIpAddress);
            return serverIpAddress;
        }
        private bool CheckIsKioskPaymentGatewayIsFreedomPay()
        {
            log.LogMethodEntry();
            bool freedomPayPaymentGateway = false;
            try
            {
                string kioskCreditCardPaymenetGateway = Utilities.getParafaitDefaults("KIOSK_CREDITCARD_PAYMENT_MODE");
                if (!string.IsNullOrEmpty(kioskCreditCardPaymenetGateway))
                {
                    PaymentModeList paymentModeList = new PaymentModeList(machineUserContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> paymentModeSearchParams = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>()
                    {
                        new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, kioskCreditCardPaymenetGateway)
                    };
                    List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
                    paymentModeDTOList = paymentModeList.GetPaymentModeList(paymentModeSearchParams);
                    if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
                    {
                        if (paymentModeDTOList[0].PaymentGateway.LookupValue.Equals("FreedomPay"))
                        {
                            freedomPayPaymentGateway = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(freedomPayPaymentGateway);
            return freedomPayPaymentGateway;
        }

        void handleBillAcceptorEvent(KioskStatic.acceptance ac)
        {
            log.LogMethodEntry(ac);
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
                using (frmCashInsert fci = new frmCashInsert(entitlementType))
                {
                    fci.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message);
            }
            finally
            {
                enableCommonAcceptor();
            }
            log.LogMethodExit();
        }

        void enableCommonAcceptor()
        {
            log.LogMethodEntry();
            log.Debug("commonBillAcceptor :" + commonBillAcceptor);
            if (commonBillAcceptor != null)
            {
                commonBillAcceptor.setReceiveAction = handleBillAcceptorEvent;
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


        private void btnCheckBalance_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Check Balance click");

                disableCommonBillAcceptor();
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
                    using (frmCheckBalance frmCheckBalance = new frmCheckBalance(accountBL.AccountDTO))
                    {
                        frmCheckBalance.ShowDialog();
                        frmCheckBalance.Dispose();
                    }

                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
                    }
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
            }
            log.LogMethodExit();
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

                    CardNumber = null;
                }
                else
                {
                    using (frmTapCard ftc = new frmTapCard())
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
                        log.LogMethodExit(txtMessage.Text);
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
            KioskStatic.logToFile("Starts:HandleTopUpCardRead()");
            try
            {
                KioskStatic.logToFile("HomeScreen Card Tap: " + inCardNumber);
                log.LogVariableState("HomeScreen Card Tap: ", inCardNumber);
                if (Application.OpenForms.Count > 2)
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
                    card = new Card(readerDevice, inCardNumber, "", Utilities);
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
                    bool isManagerCard = FSKCoverPage.CheckIsThisCardIsManagerCard(machineUserContext, inCardNumber);
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
                                CloseApplication();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                log.Error(ex);
            }
            KioskStatic.logToFile("Ends:HandleTopUpCardRead()");
            log.LogMethodExit();
        }
        
        void CloseApplication()
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
                                log.Info("Card at read positon. Ejecting.");
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.LogVariableState(message, cardDispenser.dispenserWorking);
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

                log.LogVariableState(txtMessage.Text, null);
                StartKioskTimer();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("CardStatusCheck(): " + ex.Message + ": " + ex.StackTrace);
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void frmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //try
            //{
            //    try
            //    {
            //        KioskStatic.spCoinAcceptor.Close();
            //    }
            //    catch { }

            //    try
            //    {
            //        if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y")
            //            KioskStatic.CloseFiscalPrinter();
            //    }
            //    catch
            //    {

            //    }

            //    if (Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
            //    {
            //        KioskStatic.TopUpReaderDevice.Dispose();
            //        KioskStatic.cardAcceptor.EjectCardFront();
            //        KioskStatic.cardAcceptor.BlockAllCards();
            //    }


            //    if (KioskStatic.DispenserReaderDevice != null)
            //        KioskStatic.DispenserReaderDevice.Dispose();

            //    if (KioskStatic.TopUpReaderDevice != null)
            //    {
            //        KioskStatic.TopUpReaderDevice.Dispose();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    KioskStatic.logToFile(ex.Message + Environment.NewLine + ex.StackTrace);
            //    log.Error(ex);
            //}
            //try
            //{ disableCommonBillAcceptor(); }
            //catch { }

            //KioskStatic.logToFile("App exiting");
            //log.LogMethodExit("App exiting");
            try
            {
                disableCommonBillAcceptor();
                commonBillAcceptor.setReceiveAction = null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            if (singleFunctionMode)
            {
                isHomeForm(true);
                KioskStatic.logToFile("App exiting");
            }
            else
            {
                log.Info(this.Name + ": Form closed");
            }
            log.LogMethodExit();
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
                    HandleDispenserCardRead(lclCardNumber, sender as DeviceClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Cover page dispenser Card read error: " + ex.Message);
                KioskStatic.logToFile("Remove any card that is present at dispenser read or eject position(s)");
            }
            log.LogMethodExit();
        }

        void HandleDispenserCardRead(string cardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(cardNumber, readerDevice);
            try
            {
                KioskStatic.logToFile("HomeScreen:Dispenser Card: " + cardNumber);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmHome_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (singleFunctionMode)
            {
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
            }
            log.LogMethodExit();
        }


        private void spCoinAcceptor_DataReceived(System.Object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Threading.Thread.Sleep(20);
            KioskStatic.spCoinAcceptor.Read(KioskStatic.coinAcceptor_rec, 0, KioskStatic.spCoinAcceptor.BytesToRead);
            KioskStatic.coinAcceptorDatareceived = true;

            if (KioskStatic.caReceiveAction != null)
                KioskStatic.caReceiveAction.Invoke();

            log.LogMethodExit();
        }

        private void btnNewCard_MouseDown(object sender, MouseEventArgs e)
        {
            //  btnNewCard.BackgroundImage = Properties.Resources.new_card_pressed;
        }

        private void btnNewCard_MouseUp(object sender, MouseEventArgs e)
        {
            //  btnNewCard.BackgroundImage = Properties.Resources.New_Card_New;
        }

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
            lstLang.MinimumSize = new System.Drawing.Size(lstLang.Width, 100);
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
            ResetKioskTimer();
            if (lstLang.Items.Count == 2)
            {
                DataTable dt = lstLang.DataSource as DataTable;
                if (dt.Rows[0]["LanguageName"].ToString().Equals(btnLanguage.Text))
                    restartOnLanguageChange((int)dt.Rows[0]["LanguageId"], dt.Rows[0]["LanguageName"].ToString());
                else
                    restartOnLanguageChange((int)dt.Rows[1]["LanguageId"], dt.Rows[1]["LanguageName"].ToString());

                log.LogMethodExit("lstLang.Items.Count == 2");
                return;
            }

            lstLang.Visible = !lstLang.Visible;
            if (lstLang.Visible)
                Audio.PlayAudio(Audio.SelectLanguage);

            log.LogMethodExit();
        }

        void lstLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            //this.btnPlaygroundEntry.Text = "Playground Entry";
            this.btnCheckBalance.Text = "Check Balance / Activity";
            this.btnRegister.Text = "Register";
            this.btnTransfer.Text = "Transfer";
            this.btnRedeemTokens.Text = "Redeem Tokens";
            this.btnFAQ.Text = "FAQ";
            //this.btnSignWaiver.Text = "Sign Waiver";
            this.btnPauseTime.Text = "Pause Card";
            this.btnPointsToTime.Text = "Convert Points To Time";

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
                            log.LogMethodExit(txtMessage.Text);
                            return;
                        }
                    }

                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_TRANSFER").Equals("BOTH"))
                        {
                            if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0 && ftp.Card.credits != 0)
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
                            else if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0)
                            {
                                entitlementType = "Time";
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
                                log.Info("ENTITLEMENT_TYPE is Time");
                            }
                            else
                            {
                                entitlementType = "Credits";
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
                                log.Info("ENTITLEMENT_TYPE is Credits");
                            }
                        }
                        else if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_TRANSFER").Equals("TIME"))
                        {
                            if ((ftp.Card.CreditPlusTime + ftp.Card.time) != 0)//&& ftp.Card.credits == 0
                            {
                                entitlementType = "Time";
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
                                log.Info("ENTITLEMENT_TYPE is Time");
                            }
                            else
                            {
                                using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(1842, Utilities.MessageUtils.getMessage("Time"))))
                                {
                                    f.ShowDialog();
                                }

                                KioskStatic.logToFile("Transfer is allowed only for Time. Card does not have any Time to transfer.");
                                log.Info("Transfer is allowed only for Time. Card does not have any Time to transfer.");
                                return;
                            }
                        }
                        else if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_TRANSFER").Equals("POINT"))
                        {
                            if (ftp.Card.credits != 0)//&& (ftp.Card.CreditPlusTime + ftp.Card.time) == 0
                            {
                                entitlementType = "Credits";
                                KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
                                log.Info("ENTITLEMENT_TYPE is Credits");
                            }
                            else
                            {
                                using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(1842, Utilities.MessageUtils.getMessage("Points"))))
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
                        tfFrom.Dispose();
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
            // btnRedeemTokens.BackgroundImage = Properties.Resources.Redeem_Tokens_New;
        }

        private void btnFAQ_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("FAQ Click");

                if (commonBillAcceptor != null && BillCollectorIsInWIPMode(commonBillAcceptor.GetBillAcceptor()))
                {
                    KioskStatic.logToFile("Bill acceptor is processing notes. Can not proceed");
                    return;
                }
                Audio.Stop();
                using (frmFAQ faq = new frmFAQ())
                {
                    faq.ShowDialog();
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
            log.LogMethodExit();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayAssemblyVersion();

            if (KioskHelper.isTimeEnabledStore())
                entitlementType = "Time";
            else
                entitlementType = "Credits";

            log.LogVariableState("entitlementType", entitlementType);
            txtMessage.Text = (Utilities.MessageUtils.getMessage(462));

            if (singleFunctionMode)
            {
                if (KioskStatic.CurrentTheme.SplashScreenImage != null)//Playpass1:Starts
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
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (singleFunctionMode)
                {
                    if (KioskStatic.CurrentTheme.SplashScreenImage != null)
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
                                return;
                            }
                            StopKioskTimer();
                            KioskStatic.logToFile("Calling splash");
                            log.Info("Calling splash");
                            using (frmSplashScreen splashScreen = new frmSplashScreen())
                            {
                                splashScreen.ShowDialog();
                            }
                        }
                    }

                    if (this.Equals(Form.ActiveForm) || (Form.ActiveForm != null && Form.ActiveForm.Name != null && Form.ActiveForm.Name.Equals("frmSplashScreen")))
                    {
                        txtMessage.Text = Utilities.MessageUtils.getMessage(462);
                        tickCount++;
                        if (tickCount % 300 == 0) // refresh variable every 5 minutes
                        {
                            kioskMonitor.Post(Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1694));
                            KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                            FCCClientServiceSatus();
                            KioskStatic.logToFile("Refresh defaults: getParafaitDefaults()");
                            log.LogVariableState("Kiosk app running.., Refresh defaults: getParafaitDefaults()", null);
                            KioskStatic.getParafaitDefaults();
                            tickCount = 0;
                            KioskStatic.RestartPaymentGatewayComponent();
                        }
                    }
                }
                else
                {
                    int tickSecondsRemaining = GetKioskTimerSecondsValue();
                    tickSecondsRemaining--;
                    setKioskTimerSecondsValue(tickSecondsRemaining);
                    if (tickSecondsRemaining == 10)
                    {

                        if (TimeOut.AbortTimeOut(this))
                        {
                            ResetKioskTimer();
                        }
                        else
                        {
                            tickSecondsRemaining = 0;
                        }
                    }
                    if (tickSecondsRemaining <= 0)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void FCCClientServiceSatus()
        {
            log.LogMethodEntry();
            if (fccClientServiceController != null && (fccClientServiceController.Status == ServiceControllerStatus.Running))
            {
                freewayClientServiceMonitor.Post(Monitor.MonitorLogStatus.INFO, "FCCClientService is running");
                log.Info("FCCClientService restart required");
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

        private void frmHome_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void frmHome_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (singleFunctionMode)
                {
                    if (Application.OpenForms.Count > 1)
                    {
                        try
                        {
                            Application.OpenForms[Application.OpenForms.Count - 1].Focus();
                        }
                        catch { }
                        log.LogMethodExit();
                        return;
                    }

                    if (KioskStatic.CurrentTheme.SplashScreenImage != null)//Playpass1:Starts
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
                    log.LogVariableState(MessageContainerList.GetMessage(machineUserContext, 1694), null);
                    if (Application.OpenForms.Count == 2)
                    {
                        if (Audio.soundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                            playHomeScreenAudio();
                    }
                }
                else
                {
                    ResetKioskTimer();
                    StartKioskTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("frmHome_Activated: " + ex.Message);
                StartKioskTimer();
            }
            log.LogMethodExit();
        }


        private void btnPointsToTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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

                    using (frmCreditsToTime frm = new frmCreditsToTime(ftp.cardNumber))
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
                        AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, ftp.Card.card_id, true, false);
                        if (!accountBL.CardHasCreditPlus(CreditPlusType.TIME))
                        {
                            using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(1841)))
                            {
                                f.ShowDialog();
                            }
                            KioskStatic.logToFile("Fresh card is not loaded with credit plus so card is not active.");
                            log.LogMethodExit("Fresh card is not loaded with credit plus so card is not active.");
                            return;
                        }
                        Loyalty trxLoyalty = new Loyalty(Utilities);
                        if ((ftp.Card.time + ftp.Card.CreditPlusTime) > 0)
                        {
                            bool isTimeRunning = trxLoyalty.IsCreditPlusTimeRunning(ftp.Card.card_id, null);
                            if (!isTimeRunning)
                            {
                                using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(2065) + "\n" + Utilities.MessageUtils.getMessage(2066)))
                                {
                                    f.ShowDialog();
                                }
                                log.LogVariableState("isTimeRunning", isTimeRunning);
                                log.LogMethodExit();
                                return;
                            }
                        }
                        else
                        {
                            using (frmOKMsg f = new frmOKMsg(Utilities.MessageUtils.getMessage(1838)))
                            {
                                f.ShowDialog();
                            }
                            KioskStatic.logToFile("No time remaining on the card");
                            log.LogMethodExit();
                            return;
                        }
                        TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                        if (!tp.CheckTimePauseLimit(ftp.Card.card_id, ref message))
                        {
                            using (frmOKMsg f = new frmOKMsg(message))
                            {
                                f.ShowDialog();
                            }
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

        private void BtnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                for (int i = Application.OpenForms.Count - 1; ((singleFunctionMode) ? i > 1 : i > 0); i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("In Home form. Cannot go back to coverpage page due to " + ex.Message);
                using (frmOKMsg f = new frmOKMsg(ex.Message))
                {
                    f.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        private void ValidateWaiverSetup()
        {
            log.LogMethodEntry();
            //try
            //{
            //    incorrectCustomerSetupForWaiver = true;
            //    WaiverCustomerUtils.HasValidWaiverSetup(Utilities.ExecutionContext);
            //    incorrectCustomerSetupForWaiver = false;
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //    waiverSetupErrorMsg = ex.Message;
            //    incorrectCustomerSetupForWaiver = true;
            //}
            log.LogMethodExit();
        }
        private void btnSignWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (incorrectCustomerSetupForWaiver == false)
            //{
            //    using (frmSelectWaiverOptions sws = new frmSelectWaiverOptions())
            //    {
            //        sws.ShowDialog();
            //    }
            //}
            //else
            //{
            //    using (frmOKMsg frm = new frmOKMsg(waiverSetupErrorMsg))
            //    {
            //        frm.ShowDialog();
            //    }
            //}
            log.LogMethodExit();
        }
        private void btnPlaygroundEntry_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //KioskStatic.logToFile("btnPlaygroundEntry_Click()");

            //if (!KioskStatic.receipt && !(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "IGNORE_PRINTER_ERROR").Equals("Y")))
            //{
            //    using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 461)))
            //    {
            //        if (frmyn.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
            //        {
            //            frmyn.Dispose();
            //            log.LogMethodExit();
            //            return;
            //        }
            //        frmyn.Dispose();
            //    }
            //}

            //try
            //{
            //    using (frmTapCard frmtc = new frmTapCard())
            //    {
            //        frmtc.ShowDialog();

            //        if (frmtc.Card != null)
            //        {
            //            KioskStatic.logToFile("Card: " + frmtc.Card.CardNumber);
            //            Card card = frmtc.Card;
            //            if (card.technician_card.Equals('Y'))
            //            {
            //                txtMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 197, card.CardNumber);
            //                log.LogMethodExit();
            //                return;
            //            }
            //            if (card.customer_id != -1)
            //            {
            //                using (frmChooseProduct frm = new frmChooseProduct("C", "CheckInCheckOut", "ALL", card, card.customerDTO))
            //                {
            //                    DialogResult dr = frm.ShowDialog();
            //                    if (dr == DialogResult.OK)
            //                        return;
            //                    if (dr != DialogResult.OK)
            //                    {
            //                        frm.Dispose();
            //                        log.LogMethodExit();
            //                        return;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                using (frmYesNo frmYN = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 758), KioskStatic.Utilities.MessageUtils.getMessage(759)))
            //                {
            //                    if (frmYN.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            //                    {
            //                        Customer customer = new Customer(card.CardNumber);
            //                        customer.ShowDialog();
            //                        log.LogMethodExit();
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            //}
            //finally
            //{
            //    KioskStatic.logToFile("btnPlaygroundEntry_Click() - Exit");
            //}
            log.LogMethodExit();
        }
        private void GetCoverPageConfigs()
        {
            log.LogMethodEntry();
            bool fskSalesEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_SALES_OPTION") == "Y");
            bool fskExecuteOnlineTrxEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_EXECUTE_TRANSACTION_OPTION") == "Y");
            singleFunctionMode = !(fskSalesEnabled && fskExecuteOnlineTrxEnabled);
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
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in flpOptions.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.HomeScreenOptionsTextForeColor;//Buy New card, Add points or Time ,Pause Card, Transfer points,Check Balance button
                    }
                    if (type.Contains("button") && c.Name.Equals("btnRegister"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRegisterTextForeColor;//Register button
                    }
                    if (type.Contains("button") && c.Name.Equals("btnCheckBalance"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnCheckBalanceTextForeColor;//CHeck balance button
                    }
                }
                this.btnRegister.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRegisterTextForeColor;//Register button
                this.btnCheckBalance.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnCheckBalanceTextForeColor;//CHeck balance button
                this.btnNewCard.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnNewCardTextForeColor;//CHeck balance button
                this.btnRecharge.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRechargeTextForeColor;//CHeck balance button
                //this.btnPlaygroundEntry.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPlaygroundEntryTextForeColor;//Kidzoona Entry button
                this.btnTransfer.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnTransferTextForeColor;//CHeck balance button
                this.btnRedeemTokens.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRedeemTokensTextForeColor;//CHeck balance button
                this.btnPauseTime.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPauseTimeTextForeColor;//CHeck balance button
                this.btnPointsToTime.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPointsToTimeTextForeColor;//CHeck balance button
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor;//CHeck balance button
                //this.btnSignWaiver.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnSignWaiverTextForeColor;//CHeck balance button
                this.btnLanguage.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnLanguageTextForeColor;//Language
                this.btnFAQ.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnTermsTextForeColor;//Faq(Terms)
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.lblAppVersion.ForeColor = KioskStatic.CurrentTheme.ApplicationVersionForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void CheckApplicationVersion()
        {
            log.LogMethodEntry();
            int result = Utilities.VersionCheck();
            if (result == -1)
            {
                string message = Utilities.MessageUtils.getMessage(2290); //"You are not using the latest version of this application. Please update to continue using this application."
                KioskStatic.logToFile(message);
                using (frmOKMsg f = new frmOKMsg(message))
                {
                    f.ShowDialog();
                }
                Environment.Exit(1);
            }
            else if (result == 1)
            {
                string message = Utilities.MessageUtils.getMessage(2291); //"The app version is not in sync with the server. Please sync version with the server."
                KioskStatic.logToFile(message);
                using (frmOKMsg f = new frmOKMsg(message))
                {
                    f.ShowDialog();
                }
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
    }
}
