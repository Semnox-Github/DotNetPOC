/********************************************************************************************
 * Project Name - Parafait_Kiosk  
 * Description  - frmPurchase
 * 
 **************
 **Version Log
 **************
*Version      Date           Modified By           Remarks          
*********************************************************************************************
*2.150.1.0    28-Nov-2022    Guru S A              Created for Cart feature in Kiosk 
*2.155.0      20-Jun-2023    Sathyavathi           Attraction Sale in Kiosk
*2.150.7      10-Nov-2023    Sathyavathi           Customer Lookup Enhancement
********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.logger;
using Semnox.Parafait.Transaction;
using Parafait_Kiosk.Transaction;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk
{
    public partial class frmPurchase : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CardDispenser cardDispenser;
        private bool isDispenserCardReaderValid;
        private Monitor cardDispenserMonitor;
        private string dispenserMessage = string.Empty;
        private string msg1;
        private string msg2;
        private bool showCartInKiosk = false;
        private string NUMBERFORMAT;
        public frmPurchase(ExecutionContext executionContext, PaymentModesContainerDTO userSelectedPaymentModeDTO = null, List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = null)
        {
            log.LogMethodEntry(executionContext, userSelectedPaymentModeDTO, selectedFundsAndDonationsList);
            KioskStatic.logToFile("frmPurchase");
            StopKioskTimer();
            this.executionContext = executionContext;
            InitializeComponent();
            showCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CART_IN_KIOSK", false);
            NUMBERFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 2438)); //Please choose an option
            //this.ShowInTaskbar = false;
            InitPurchaseMenu();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetKioskTimerTickValue();
            kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
            kioskTransaction.PreSelectedPaymentModeDTO = userSelectedPaymentModeDTO;
            kioskTransaction = frmPaymentMode.AddFundRaiserOrDonationProducts(kioskTransaction, selectedFundsAndDonationsList);
            lblGreeting1.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            msg1 = MessageContainerList.GetMessage(executionContext, 460);//Problem in Card Dispenser. Cannot issue new card.
            msg2 = MessageContainerList.GetMessage(executionContext, 441);//Please contact our staff
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmPurchase_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }
        private void InitPurchaseMenu()
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            try
            {
                if (KioskStatic.DisablePurchase)
                {
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "DISABLE_RECHARGE", false))
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
                if (!KioskStatic.EnablePlaygroundEntry)
                {
                    flpOptions.Controls.Remove(btnPlaygroundEntry);
                }
                else
                {
                    btnPlaygroundEntry.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_FNB_PRODUCTS_IN_KIOSK", false) == false)
                {
                    flpOptions.Controls.Remove(btnFNB);
                }
                else
                {
                    btnFNB.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ATTRACTION_PRODUCTS_IN_KIOSK", false) == false)
                {
                    flpOptions.Controls.Remove(btnAttractions);
                }
                else
                {
                    btnAttractions.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            DisplaybtnCancel(true);
            DisplaybtnCart(showCartInKiosk);
            SetCustomImages();
            SetCustomizedFontColors();
            if (KioskStatic.DispenserReaderDevice != null || KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
            }
            if (KioskStatic.config.dispport > 0)
            {
                cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                cardDispenserMonitor = new Monitor(Monitor.MonitorAppModule.CARD_DISPENSER);
            }
        }
        public override void Form_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StartKioskTimer();
            RefreshCartIconText(NUMBERFORMAT);
            log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in frmPurchase_Deactivate : " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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
                    tickSecondsRemaining = 0;
            }
            if (tickSecondsRemaining <= 0)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void frmPurchase_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            try
            {
                if (kioskTransaction != null)
                {
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while clearing transction: " + ex.Message);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        public override void btnCart_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cart button clicked");
            try
            {
                base.LaunchCartForm(NUMBERFORMAT);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("frmPurchase: Setting customized background images");
            try
            {
                this.SuspendLayout();
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.PurchaseMenuBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnCart.SetCartImage(ThemeManager.CurrentThemeImages.KioskCartIcon);
                btnNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewPlayPassButtonBig;
                btnRecharge.BackgroundImage = ThemeManager.CurrentThemeImages.RechargePlayPassButtonBig;
                btnPlaygroundEntry.BackgroundImage = ThemeManager.CurrentThemeImages.PlaygroundEntryBig;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnFNB.BackgroundImage = ThemeManager.CurrentThemeImages.FoodAndBeverageBig;
                btnAttractions.BackgroundImage = ThemeManager.CurrentThemeImages.AttractionsButtonBig;
                btnRecharge.Font = btnNewCard.Font = btnFNB.Font = btnAttractions.Font = btnPlaygroundEntry.Font;
                btnNewCard.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                btnRecharge.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                btnPlaygroundEntry.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                btnFNB.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                btnAttractions.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.HomeBigButtonTextAlignment);
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images", ex);
                KioskStatic.logToFile("frmPurchase: while setting customized background images: " + ex.Message);
            }
            this.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmPurchase");
            try
            {
                this.SuspendLayout();
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuLblSiteNameTextForeColor;
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuGreetingTextForeColor;//Greeting text ForeColor
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuBtnTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuBtnTextForeColor;//Cancel button 
                this.btnNewCard.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnNewCardTextForeColor;
                this.btnRecharge.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnRechargeTextForeColor;
                this.btnFNB.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnFoodAndBeveragesTextForeColor;
                this.btnPlaygroundEntry.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnPlaygroundEntryTextForeColor;
                this.btnAttractions.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnAttractionsTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuFooterTextForeColor; //footer message 
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PurchaseMenuBtnHomeTextForeColor;//Home button 
                this.btnCart.SetFont(this.btnHome.Font);
                this.btnCart.SetForeColor(this.btnHome.ForeColor, KioskStatic.CurrentTheme.KioskCartQuantityTextForeColor);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPurchase: " + ex.Message);
            }
            this.ResumeLayout(true);
            log.LogMethodExit();
        }
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmPurchase: Home button clicked");
            CloseForm("Home");
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmPurchase: Back button pressed");
            CloseForm("Back");
            log.LogMethodExit();
        }
        private void CloseForm(string btnName)
        {
            log.LogMethodEntry(btnName);
            try
            {
                if (kioskTransaction != null && kioskTransaction.HasUnsavedItems())
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 5010);
                    // "Your cart items will be lost. Do you want go back?"
                    using (frmYesNo f = new frmYesNo(msg))
                    {
                        if (f.ShowDialog() == DialogResult.No)
                        {
                            ResetKioskTimer();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("frmPurchase: Error on " + btnName + " button click: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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

                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETNEWCARDTYPE, entitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    this.kioskTransaction = frm.GetKioskTransaction;
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
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                KioskStatic.logToFile("Exit New Card click");
            }
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                KioskStatic.logToFile("exit Top-up click");
            }
            log.LogMethodExit();
        }

        private void btnPlaygroundEntry_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("btnPlaygroundEntry_Click()");
                string val = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHECK_IN_OPTIONS_IN_POS-IN");
                log.Debug("CHECK_IN_OPTIONS_IN_POS : " + val);
                if (string.IsNullOrWhiteSpace(val) == false && val != "NO")
                {
                    string errMsg = MessageContainerList.GetMessage(executionContext, 4343);
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
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 197, card.CardNumber));
                            log.LogMethodExit();
                            return;
                        }

                        if (card.customer_id != -1)
                        {
                            if (kioskTransaction.HasCustomerRecord() == false)
                            {
                                kioskTransaction.SetTransactionCustomer(card.customerDTO);
                            }
                            using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETCHECKINCHECKOUTTYPE, "CheckInCheckOut", "ALL", card.CardNumber))
                            {
                                DialogResult dr = frm.ShowDialog();
                                kioskTransaction = frm.GetKioskTransaction;
                            }
                        }
                        else
                        {
                            using (frmYesNo frmYN = new frmYesNo(MessageContainerList.GetMessage(executionContext, 758),
                                                                  MessageContainerList.GetMessage(executionContext, 759)))
                            {   //758 - Would you like to Register? 759-* Register to get 100 FREE Tickets
                                if (frmYN.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                {
                                    Customer customer = new Customer(card.CardNumber);
                                    DialogResult dr = customer.ShowDialog();
                                    if (dr == DialogResult.Cancel)
                                    {
                                        string msg = MessageContainerList.GetMessage(executionContext, "Timeout");
                                        throw new CustomerStatic.TimeoutOccurred(msg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                KioskStatic.logToFile("btnPlaygroundEntry_Click() - Exit");
            }
            log.LogMethodExit();
        }

        private void btnFNB_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Food and Beverage button click");
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETFNBTYPE, string.Empty))
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
                KioskStatic.logToFile("Exit Food and Beverage button click");
            }
            log.LogMethodExit();
        }

        private void btnAttractions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Attractions button click");
                if (OkToIgorePrinerError() == false)
                {
                    log.LogMethodExit();
                    return;
                }
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETATTRACTIONSTYPE, string.Empty))
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
                KioskStatic.logToFile("Error in btnAttractions_Click: " + ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                KioskStatic.logToFile("Exit Attraction button click");
            }
            log.LogMethodExit();
        }

        private bool OkToIgorePrinerError()
        {
            log.LogMethodEntry();
            bool okToIgnore = true;
            if (KioskStatic.receipt == false
                && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IGNORE_PRINTER_ERROR").Equals("Y") == false)
            {
                //Can't print Receipt for this Transaction. Do you want to continue?
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(executionContext, 461)))
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
                        dispenserMessage = MessageContainerList.GetMessage(executionContext, 1696);
                        //Unable to register Dispenser card reader
                    }
                    else
                    {
                        int cardPosition = -1;
                        this.Cursor = Cursors.WaitCursor;
                        bool suc = BackgroundProcessRunner.Run<bool>(() => {
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
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.LogVariableState(message, cardDispenser.dispenserWorking);
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(mes);
                            dispenserMessage = MessageContainerList.GetMessage(executionContext, 377);//Card Dispenser Problem
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
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 2438));//Please choose an option
                }
                log.LogVariableState(txtMessage.Text, null);
                StartKioskTimer();
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("CardStatusCheck(): " + ex.Message + ": " + ex.StackTrace);
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
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
            bool disblePurchaseOnCardLowLevel = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "DISABLE_PURCHASE_ON_CARD_LOW_LEVEL", false);
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
                string mes = MessageContainerList.GetMessage(executionContext, 378) + ". " + msg2;
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

        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }

        private bool InvokeCardDispenserCheckStatus(ref int cardPosition, ref string message)
        {
            log.LogMethodEntry(cardPosition, message);
            bool suc = cardDispenser.checkStatus(ref cardPosition, ref message);
            log.LogMethodExit();
            return suc;
        }
    }
}
