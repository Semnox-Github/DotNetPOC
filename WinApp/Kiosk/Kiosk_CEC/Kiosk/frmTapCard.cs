/********************************************************************************************
* Project Name - Parafait_Kiosk -frmTapCard
* Description  - frmTapCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak              Theme changes to support customized Font ForeColor
********************************************************************************************/

using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk
{
    public partial class frmTapCard : BaseForm  
    {
        //int ticks = 0;
        public string cardNumber;
        public Card Card;
        bool _isPurchase = false;
        private readonly TagNumberParser tagNumberParser;

        public frmTapCard(bool isPurchase = false)
        {
            log.LogMethodEntry(isPurchase);
            _isPurchase = isPurchase;
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
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            this.BackgroundImage = KioskStatic.CurrentTheme.TapCardBackgroundImage;
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();

            btnCustom.Visible = btnYes.Visible = btnNo.Visible = false;
            btnClose.Visible = KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN;
            log.LogMethodExit();
        }

        public frmTapCard(string Message, string customButtonMessage = null)
        {
            log.LogMethodEntry(Message, customButtonMessage);
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            init(Message);
            if (string.IsNullOrEmpty(customButtonMessage))
            {
                btnClose.Visible = false;
                btnYes.Visible = btnNo.Visible = true;
            }
            else
            {
                btnYes.Visible = btnNo.Visible = false;

                btnClose.Location = btnYes.Location;
                btnCustom.Location = btnNo.Location;
                btnClose.Visible = true;
                btnCustom.Visible = true;
                btnCustom.Text = customButtonMessage;
            }
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
           // this.Size = this.BackgroundImage.Size;
            displayMessage(Message);
            log.LogMethodExit();
        }


        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            else 
                setKioskTimerSecondsValue(tickSecondsRemaining-1);
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void frmTapCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
            log.LogMethodEntry();
            Audio.Stop();

            if (_isPurchase == false && KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }
            }

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
            }

            KioskStatic.logToFile("frmTapCard closed");
            log.LogMethodExit();
        }

        private void frmTapCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                    KioskStatic.cardAcceptor.AllowAllCards();
            }
            //log.Fatal("Start Register topup");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));


            //log.Fatal("End Register topup");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));

            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height - 200);

            //timer1.Start();
            KioskStatic.logToFile("frmTapCard loaded");
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //log.Fatal("Start CardScanCompleteEventHandle");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    displayMessage(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string lclCardNumber = tagNumber.Value;
                lclCardNumber = KioskStatic.ReverseTopupCardNumber(lclCardNumber);
                try
                {
                    handleCardRead(lclCardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    displayMessage(ex.Message);
                }
            }
            //log.Fatal("End CardScanCompleteEventHandle");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            //log.Fatal("Start handleCardRead");
            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
            //ticks = 0;
            ResetKioskTimer();
            cardNumber = inCardNumber;
            KioskStatic.logToFile("Tapped card " + cardNumber);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                try
                {
                    Card = new MifareCard(KioskStatic.TopUpReaderDevice, cardNumber, "External POS", KioskStatic.Utilities);
                }
                catch
                {
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        displayMessage(KioskStatic.Utilities.MessageUtils.getMessage(528));
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            else
            {
                Card = new Card(readerDevice, cardNumber, "External POS", KioskStatic.Utilities);

                string message = "";
                displayMessage(KioskStatic.Utilities.MessageUtils.getMessage("Refreshing Card from HQ. Please Wait..."));
                Application.DoEvents();
                if (!KioskHelper.refreshCardFromHQ(ref Card, ref message))
                {
                    KioskStatic.logToFile(message);
                    displayMessage(message);//KioskStatic.Utilities.MessageUtils.getMessage(441) + Environment.NewLine +
                    Card = null;
                    log.LogMethodExit();
                    return;
                }
            }

            if (Card.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    displayMessage(KioskStatic.Utilities.MessageUtils.getMessage(528));
                else
                    displayMessage(KioskStatic.Utilities.MessageUtils.getMessage(459));
                
                Card = null;
                cardNumber = "";
                //ticks = 0;
                ResetKioskTimer();

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
            }
            else
            {
                if (Card.vip_customer == 'N')
                {
                    Card.getTotalRechargeAmount();
                    if ((Card.credits_played >= KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                         || (Card.TotalRechargeAmount >= KioskStatic.Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && KioskStatic.Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                    {
                        frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(451, KioskStatic.VIPTerm));
                        fok.ShowDialog();
                    }
                }

                if (_isPurchase 
                    && Card.technician_card.Equals('Y') == false
                    && (Card.customer_id == -1 
                        || (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") 
                             && Card.customerDTO != null && !Card.customerDTO.PolicyTermsAccepted)
                        ) 
                    && KioskStatic.RegisterBeforePurchase)
                {
                    Audio.PlayAudio(Audio.RegisterCardPrompt);
                    this.Hide(); // hide so that tap card message doesn't show during screen transitions
                    if (new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(758), KioskStatic.Utilities.MessageUtils.getMessage(759)).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        CustomerStatic.ShowCustomerScreen(Card.CardNumber);
                        Card.getCardDetails(Card.card_id); //refresh card object
                    }
                }

                //log.Fatal("End handleCardRead");
                //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                Close();
            }
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
            log.LogMethodExit();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
           // btnClose.BackgroundImage = Properties.Resources.cancel_btn_pressed;
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
          //  btnClose.BackgroundImage = Properties.Resources.cancel_btn;
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Card = null;
            cardNumber = null;
            Close();
            log.LogMethodExit();
        }

        private void btnCustom_MouseDown(object sender, MouseEventArgs e)
        {
          //  btnCustom.BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnCustom_MouseUp(object sender, MouseEventArgs e)
        {
          //  btnCustom.BackgroundImage = Properties.Resources.button_normal;
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
           // (sender as Button).BackgroundImage = Properties.Resources.button_pressed;
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
          //  (sender as Button).BackgroundImage = Properties.Resources.button_normal;
        }

        void displayMessage(string text)
        {
            log.LogMethodEntry();
            lblmsg.Text = text;
            KioskStatic.logToFile(text);
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.TapCardScreenHeaderTextForeColor;//Please Tap your card at the reader
                this.btnCustom.ForeColor = KioskStatic.CurrentTheme.TapCardScreenNewCardTextForeColor;//New Card button
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.TapCardScreenYesBtnTextForeColor;//Yes button
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.TapCardScreenCloseBtnTextForeColor;//Close button
                this.btnNo.ForeColor = KioskStatic.CurrentTheme.TapCardScreenNoBtnTextForeColor;//No Button
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
