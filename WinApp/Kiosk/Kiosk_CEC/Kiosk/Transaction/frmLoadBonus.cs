/********************************************************************************************
* Project Name - Parafait_Kiosk -frmLoadBonus
* Description  - frmLoadBonus 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
********************************************************************************************/
using System;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmLoadBonus : BaseFormKiosk
    {
        public string cardNumber;
        public Card TechCard;

        private readonly TagNumberParser tagNumberParser;

        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        public frmLoadBonus(Card techCard)
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(527));
                Audio.PlayAudio(Audio.InsertCardIntoReader);
            }
            else
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(500));
                Audio.PlayAudio(Audio.TapCardOnReader);
            }
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            TechCard = techCard;
            btnShowKeyPad.Visible = false;
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
           // KioskStatic.setDefaultFont(this);

            lblSiteName.Text = KioskStatic.SiteHeading;
            this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;
            KioskStatic.setFieldLabelForeColor(panelCardDetails);
            displayMessageLine(Message, MESSAGE);
            SetCustomizedFontColors();

            // btnPrev.Top = lblSiteName.Bottom + 20;
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ShowKeyPadAfterCardTap();
            log.LogMethodExit();
        }

        private void ShowKeyPadAfterCardTap()
        {
            //double varAmount = NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(745), '-', KioskStatic.Utilities);
            log.LogMethodEntry();
            double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(745), "0.00", KioskStatic.Utilities);
            if (varAmount <= 0)
                return;
            else
                txtLoadBonus.Text = varAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            log.LogMethodExit();
        }

        private void frmLoadBonus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Cursor.Show();

            panelCardDetails.Left = this.Width / 2 - panelCardDetails.Width / 2;

            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));

            KioskTimerSwitch(false);
            displaybtnCancel(false);
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
                    displayMessageLine(message, ERROR);
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
                    displayMessageLine(ex.Message, ERROR);
                }
            }
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            cardNumber = inCardNumber;
            Card card = new Card(readerDevice, cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                // msg = "Tap an issued card";
                displayMessageLine(MessageUtils.getMessage(459), MESSAGE);
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
                    displayMessageLine(MessageUtils.getMessage("Play Pass No") + ": " + txtCardNo.Text.ToString(), MESSAGE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            //switch (msgType)
            //{
            //    case "WARNING": textBoxMessageLine.BackColor = Color.Yellow; textBoxMessageLine.ForeColor = Color.Black; break;
            //    case "ERROR": textBoxMessageLine.BackColor = Color.Red; textBoxMessageLine.ForeColor = Color.White; break;
            //    case "MESSAGE": textBoxMessageLine.BackColor = Color.Transparent; textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor; break;
            //    default: textBoxMessageLine.ForeColor = Color.Black; break;
            //}
            log.LogMethodEntry(message, msgType);
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtCardNo.Text.ToString().Length > 0)
            {
                if (txtLoadBonus.Text.ToString().Length > 0)
                {
                    decimal tsfr = Convert.ToDecimal(txtLoadBonus.Text);

                    if (tsfr == 0)
                    {
                        //  msg = "Please enter a value greater than Zero for points";
                        displayMessageLine(MessageUtils.getMessage(745), WARNING);
                        this.ActiveControl = txtLoadBonus;
                        log.LogMethodExit();
                        return;
                    }
                    decimal avltkresult = 0;
                    decimal.TryParse(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"), out avltkresult);

                    if (tsfr > avltkresult)
                    {
                        displayMessageLine(MessageUtils.getMessage(43, avltkresult), WARNING);
                        this.ActiveControl = txtLoadBonus;
                        log.LogMethodExit();
                        return;
                    }

                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    string message = "";
                    TaskProcs.EntitlementType bonusType;
                    if (Utilities.getParafaitDefaults("LOAD_CREDITS_INSTEAD_OF_CARD_BALANCE").Equals("Y"))
                        bonusType = TaskProcs.EntitlementType.Credits;
                    else
                        bonusType = TaskProcs.EntitlementType.Bonus;

                    if (!tp.loadBonus(new Card(cardNumber, KioskStatic.Utilities.ParafaitEnv.LoginID, Utilities), 
                                        (double)tsfr, 
                                        bonusType, 
                                        false, 
                                        -1, 
                                        "Load bonus from kiosk. Staff Card: " + TechCard.CardNumber + (TechCard.customerDTO == null ? "" :  " / " + TechCard.customerDTO.FirstName),
                                        ref message))
                    {
                        displayMessageLine(message, WARNING);
                    }
                    else
                    {
                        frmOKMsg fok = new frmOKMsg(MessageUtils.getMessage(44));
                        fok.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(745), WARNING);
                    log.LogMethodExit();
                    return;
                }
            }
            else
            {
                displayMessageLine(MessageUtils.getMessage(748), WARNING);
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

        //private void btnPrev_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        private void txtLoadBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
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
            log.LogMethodExit();
        }

        private void frmLoadBonus_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
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
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.LoadBonusFooterTextForeColor;
                lblSiteName.Text = KioskStatic.SiteHeading;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;
            }

            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

