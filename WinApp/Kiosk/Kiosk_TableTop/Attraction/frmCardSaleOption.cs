/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Attrcation Card Sale Option pop up screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.155.0.0   16-Jun-2023      Suraj              Created for Attraction Sale in Kiosk
 *2.152.0.0   12-Dec-2023      Suraj Pai          Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class frmCardSaleOption : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private CardDispenser cardDispenser;
        private bool isDispenserCardReaderValid;
        private Monitor cardDispenserMonitor;
        private string dispenserMessage = string.Empty;
        private string msg1;
        private string msg2;

        private CardSaleOption selectedOption = CardSaleOption.NONE;

        internal CardSaleOption SelectedOption { get { return selectedOption; } }

        internal enum CardSaleOption
        {
            NONE = 0,
            NEW = 1,
            EXISTING = 2
        };

        public frmCardSaleOption()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("In frmCardSaleOption()");
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            SetCustomImages();
            SetCustomizedFontColors();
            msg1 = MessageContainerList.GetMessage(executionContext, 460);//Problem in Card Dispenser. Cannot issue new card.
            msg2 = MessageContainerList.GetMessage(executionContext, 441);//Please contact our staff
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmCardSaleOption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            this.lblNewCard.Text = MessageContainerList.GetMessage(executionContext, "I have a card");
            this.lblExistingCard.Text = MessageContainerList.GetMessage(executionContext, "Get a new card?");
            if (KioskStatic.DispenserReaderDevice != null || KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
            }
            if (KioskStatic.config.dispport > 0)
            {
                cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                cardDispenserMonitor = new Monitor(Monitor.MonitorAppModule.CARD_DISPENSER);
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            else
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            log.LogMethodExit();
        }

        private void pbCheckBoxExistingCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            selectedOption = CardSaleOption.EXISTING;
            pbCheckBoxExistingCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewTickedCheckBox;
            pbCheckBoxNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewUnTickedCheckBox;
            log.LogMethodExit();
        }

        private void pbCheckBoxNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            selectedOption = CardSaleOption.NEW;
            pbCheckBoxNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewTickedCheckBox;
            pbCheckBoxExistingCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewUnTickedCheckBox;
            log.LogMethodExit();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (SelectedOption != frmCardSaleOption.CardSaleOption.NONE)
            {
                if(SelectedOption == frmCardSaleOption.CardSaleOption.NEW)
                {
                    CardDispenserStatusCheck();
                    if (CheckNewCardDependency() == false)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        Close();
                        log.LogMethodExit();
                        return;
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, "Please select any one of the option to continue"));
            }
            log.LogMethodExit();
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

                log.LogVariableState(dispenserMessage, null);
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
                KioskStatic.logToFile("Card dispenser is null (not initialized)");
                frmOKMsg.ShowUserMessage(msg1 + ". " + msg2);
                log.LogVariableState("Card dispenser is null (not initialized)", cardDispenser);
                log.LogMethodExit(false);
                return false;
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                string errMsg = string.IsNullOrEmpty(dispenserMessage) ? msg1 : dispenserMessage;
                KioskStatic.logToFile(errMsg);
                frmOKMsg.ShowUserMessage(errMsg + ". " + msg2);
                log.LogVariableState(errMsg, cardDispenser);
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

        private void frmCardSaleOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.CardSaleOptionBox);
                panelExistingCard.BackgroundImage =
                    panelNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.PanelCardSaleOption;
                pbExistingcard.BackgroundImage = ThemeManager.CurrentThemeImages.AttractionExistingCard;
                pbNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.AttractionNewCard;
                pbCheckBoxExistingCard.BackgroundImage =
                    pbCheckBoxNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewUnTickedCheckBox;
                btnCancel.BackgroundImage =
                    btnConfirm.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.CardSaleOptionButtons);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Images for frmCardSaleOption", ex);
                KioskStatic.logToFile("Error while setting customized Images for the UI elements of frmCardSaleOption: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblHeader.ForeColor = KioskStatic.CurrentTheme.CardSaleOptionHeaderTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CardSaleOptionCancelBtnTextForeColor;
                this.btnConfirm.ForeColor = KioskStatic.CurrentTheme.CardSaleOptionConfirmBtnTextForeColor;
                this.lblNewCard.ForeColor = KioskStatic.CurrentTheme.CardSaleOptionLblNewCardTextForeColor;
                this.lblExistingCard.ForeColor = KioskStatic.CurrentTheme.CardSaleOptionLblExistingCardTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmCardSaleOption", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmCardSaleOption: " + ex.Message);
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