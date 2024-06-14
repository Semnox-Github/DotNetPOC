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
* 2.80        5-Sep-2019      Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmLoadBonus : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        public Card TechCard;

        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        private readonly TagNumberParser tagNumberParser;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        public frmLoadBonus(Card techCard)
        {
            log.LogMethodEntry(techCard);
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

            TechCard = techCard;
            btnShowKeyPad.Visible = false;
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);

            lblSiteName.Text = KioskStatic.SiteHeading;
            //lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;//Modification on 17-Dec-2015 for introducing new theme

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            txtLoadBonus.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            pbDownArrow.Image = ThemeManager.CurrentThemeImages.CardReaderArrowImage;
            //textBoxMessageLine.ForeColor = lblTapMsg.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;

            //KioskStatic.setFieldLabelForeColor(panelCardDetails);//Ends:Modification on 17-Dec-2015 for introducing new theme

            displayMessageLine(Message, MESSAGE);

            //btnPrev.Top = lblSiteName.Bottom + 20;
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
            double varAmount = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm(KioskStatic.Utilities.MessageUtils.getMessage(745), "-", KioskStatic.Utilities);
            if (varAmount <= 0)
            {
                log.LogMethodExit();
                return;
            }
            else
                txtLoadBonus.Text = varAmount.ToString("N0");
         log.LogMethodExit();
        }

        private void frmLoadBonus_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void frmLoadBonus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Cursor.Show();

            panelCardDetails.Location = new Point(this.Width / 2 - panelCardDetails.Width / 2, 220);
            pbDownArrow.Location = new Point(this.Width / 2 - pbDownArrow.Width / 2, pbDownArrow.Location.Y);

           // btnPrev.Top = this.Top + 10;//playpass1:Starts

            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
            KioskTimerSwitch(false);
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
                log.LogVariableState("CardNumber", CardNumber);
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
            }
            log.LogMethodExit();
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
                        txtLoadBonus.SelectAll();
                        log.LogMethodExit();
                        return;
                    }
                    decimal avltkresult = 0;
                    decimal.TryParse(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"), out avltkresult);

                    if (tsfr > avltkresult)
                    {
                        displayMessageLine(MessageUtils.getMessage(43, avltkresult), WARNING);
                        this.ActiveControl = txtLoadBonus;
                        txtLoadBonus.SelectAll();
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
                                        "Load bonus from kiosk. Staff Card: " + TechCard.CardNumber + (TechCard.customerDTO == null ? "" : " / " + TechCard.customerDTO.FirstName),
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

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
    }
}
