/********************************************************************************************
 * Project Name - Parafait Kiosk- frmLoadBonus 
 * Description  - frmLoadBonus
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmLoadBonus : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        public Card TechCard;

        private Utilities utilities = KioskStatic.Utilities; 
        private readonly TagNumberParser tagNumberParser; 

        public frmLoadBonus(Card techCard)
        {
            log.LogMethodEntry(techCard);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                Init(MessageContainerList.GetMessage(utilities.ExecutionContext,527));
                Audio.PlayAudio(Audio.InsertCardIntoReader);
            }
            else
            {
                Init(MessageContainerList.GetMessage(utilities.ExecutionContext,500));
                Audio.PlayAudio(Audio.TapCardOnReader);
            }
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            TechCard = techCard;
            btnShowKeyPad.Visible = false;
            log.LogMethodExit();
        }

        private void Init(string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            lblSiteName.Visible = KioskStatic.CurrentTheme.ShowSiteHeading;
            lblSiteName.Text = KioskStatic.SiteHeading;


            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.LoadBonusBackgroundImage);//starts:Modification on 17-Dec-2015 for introducing new theme
            btnPrev.BackgroundImage = btnSave.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            panel1.BackgroundImage = panel2.BackgroundImage = panel3.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;
            DisplayMessageLine(message);
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            DisplaybtnHome(false);
            KioskStatic.Utilities.setLanguage(this);
            // btnPrev.Top = lblSiteName.Bottom + 20;
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                ShowKeyPadAfterCardTap();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ShowKeyPadAfterCardTap()
        {
            log.LogMethodEntry();
            double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(utilities.ExecutionContext,745), '-', KioskStatic.Utilities);
            if (varAmount <= 0)
            {
                log.LogMethodExit();
                return;
            }
            else
            {
                string numFormat = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "NUMBER_FORMAT");
                txtLoadBonus.Text = varAmount.ToString(numFormat);
            }
            log.LogMethodExit();
        }

        private void frmLoadBonus_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        
        private void frmLoadBonus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }

            Cursor.Show();

            panelCardDetails.Left = this.Width / 2 - panelCardDetails.Width / 2;
            //KioskTimerSwitch(false);
            KioskTimerSwitch(true);
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    DisplayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardNumber = KioskStatic.ReverseTopupCardNumber(CardNumber);
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            cardNumber = inCardNumber;
            Card card = new Card(readerDevice, cardNumber, "", utilities);

            if (card.CardStatus == "NEW")
            {
                // msg = "Tap an issued card";
                DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 459));
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = cardNumber;
            try
            {
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    txtAvlblCredits.Text = card.credits.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    btnShowKeyPad.Visible = true;
                    txtLoadBonus.Focus();
                    ShowKeyPadAfterCardTap();
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,"Tag No") + ": " + txtCardNo.Text.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing handleCardRead()" + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message.ToString());
            }
            log.LogMethodExit();
        }

        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                btnSave.Enabled = false;
                btnPrev.Enabled = false;
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    if (txtLoadBonus.Text.ToString().Length > 0)
                    {
                        decimal tsfr = Convert.ToDecimal(txtLoadBonus.Text);

                        if (tsfr == 0)
                        {
                            //  msg = "Please enter a value greater than Zero for points";
                            DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,745));
                            this.ActiveControl = txtLoadBonus;
                            log.LogMethodExit();
                            return;
                        }
                        decimal avltkresult = 0;
                        decimal.TryParse(utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"), out avltkresult);

                        if (tsfr > avltkresult)
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,43, avltkresult));
                            this.ActiveControl = txtLoadBonus;
                            log.LogMethodExit();
                            return;
                        }

                        TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                        string message = "";
                        TaskProcs.EntitlementType bonusType;
                        if (utilities.getParafaitDefaults("LOAD_CREDITS_INSTEAD_OF_CARD_BALANCE").Equals("Y"))
                            bonusType = TaskProcs.EntitlementType.Credits;
                        else
                            bonusType = TaskProcs.EntitlementType.Bonus;
                        string msgInfo = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008);
                        //Processing..Please wait...
                        DisplayMessageLine(msgInfo);
                        if (!tp.loadBonus(new Card(cardNumber, KioskStatic.Utilities.ParafaitEnv.LoginID, utilities),
                                            (double)tsfr,
                                            bonusType,
                                            false,
                                            -1,
                                            "Load bonus from kiosk. Staff Card: " + TechCard.CardNumber + (TechCard.customerDTO == null ? "" : " / " + TechCard.customerDTO.FirstName),
                                            ref message))
                        {
                            DisplayMessageLine(message);
                        }
                        else
                        {
                            frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(utilities.ExecutionContext,44));
                            this.Close();
                        }
                    }
                    else
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,745));
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext,748));
                }
            }catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                btnSave.Enabled = true;
                btnPrev.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void txtLoadBonus_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int count;

            if (txtLoadBonus.Text.ToString().Contains("."))
            {
                if (txtLoadBonus.Text.ToString().IndexOf(".") == 0)

                    SendKeys.Send("{BACKSPACE}");
                else
                {
                    count = txtLoadBonus.Text.ToString().Split('.').Length - 1;
                    if (count > 1)
                        SendKeys.Send("{BACKSPACE}");
                }
            }
            log.LogMethodExit();
        }


        private void txtLoadBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                bool val;
                int result;
                val = int.TryParse(txtLoadBonus.Text, out result);

                switch (e.KeyChar)
                {
                    case '.':
                        break;
                    case (char)Keys.Back:
                        break;
                    default:
                        if (Char.IsDigit(e.KeyChar))
                            break;
                        else
                            SendKeys.Send("{BACKSPACE}");
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frmLoadBonus_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }
            KioskStatic.logToFile(this.Name + ": Form closed");
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmLoadBonus");
            try
            {
                this.lblTapMsg.ForeColor = KioskStatic.CurrentTheme.LoadBonusHeaderTextForeColor;//(PLEASE TAP THE CARD TO LOAD BONUS POINTS TO)
                this.label1.ForeColor = KioskStatic.CurrentTheme.LoadBonusCardHeaderTextForeColor;//
                this.label2.ForeColor = KioskStatic.CurrentTheme.LoadBonusCreditsHeaderTextForeColor;//Footer text message
                this.label3.ForeColor = KioskStatic.CurrentTheme.LoadBonusBonusHeaderTextForeColor;
                this.txtCardNo.ForeColor = KioskStatic.CurrentTheme.LoadBonusCardInfoTextForeColor;
                this.txtAvlblCredits.ForeColor = KioskStatic.CurrentTheme.LoadBonusCreditsInfoTextForeColor;
                this.txtLoadBonus.ForeColor = KioskStatic.CurrentTheme.LoadBonusBonusInfoTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.LoadBonusBtnPrevTextForeColor;
                this.btnSave.ForeColor = KioskStatic.CurrentTheme.LoadBonusBtnSaveTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.LoadBonusFooterTextForeColor;
                lblSiteName.Text = KioskStatic.SiteHeading;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }

            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmLoadBonus: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                {
                    this.Close();
                }
            }
            log.LogMethodExit();
        }
    }
}
