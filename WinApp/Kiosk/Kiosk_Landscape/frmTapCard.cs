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
* 2.80        4-Sep-2019      Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk
{
    public partial class frmTapCard : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

            btnYes.Visible = btnNo.Visible = false;
            btnClose.Visible = KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN;

            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        public frmTapCard(string Message, string customButtonMessage = null)
        {
            log.LogMethodEntry(Message, customButtonMessage);
            init(Message);
            btnClose.Visible = KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN;
            if (string.IsNullOrEmpty(customButtonMessage) == false)
            {
                btnYes.Visible = btnNo.Visible = false;
                if (ThemeManager.CurrentThemeImages.CardReaderArrowImage != null
                    && KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN == false)
                {
                    btnCustom.Location = new System.Drawing.Point(btnClose.Location.X, btnYes.Location.Y);
                }
                else
                {
                    btnCustom.Location = btnNo.Location;
                }
                btnClose.Location = new System.Drawing.Point(btnClose.Location.X, btnYes.Location.Y);
                btnCustom.Size = new System.Drawing.Size(btnClose.Width, btnClose.Height);
                btnCustom.Visible = true;
                btnCustom.Text = customButtonMessage;
                if (KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN == true && KioskStatic.AllowRegisterWithoutCard == true)
                {
                    pbDownArrow.Visible = false;
                    btnClose.Location = new System.Drawing.Point(btnClose.Location.X, btnClose.Location.Y);
                    btnCustom.Location = new System.Drawing.Point(btnCustom.Location.X, btnCustom.Location.Y);
                }
                btnClose.BackgroundImageLayout = ImageLayout.Stretch;
                btnCustom.BackgroundImageLayout = ImageLayout.Stretch;
            }
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;//Starts:Modification on 17-Dec-2015 for introducing new theme
            this.Size = this.BackgroundImage.Size;
            btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;

            button1.Text = KioskStatic.Utilities.MessageUtils.getMessage("Card Reader Below");
            if (ThemeManager.CurrentThemeImages.CardReaderArrowImage != null)
            {
                pbDownArrow.Image = ThemeManager.CurrentThemeImages.CardReaderArrowImage;
                pbDownArrow.Visible = true;
            }
            else
            {
                pbDownArrow.Visible = false;
            }

            lblmsg.Text = Message;
            KioskStatic.setDefaultFont(this);
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
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
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
               

            KioskStatic.logToFile("frmTapCard_FormClosing");
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

            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));

            //timer1.Start();

            KioskStatic.logToFile("frmTapCard_Load");
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
                    lblmsg.Text = message;
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
                    lblmsg.Text = ex.Message;
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            //ticks = 0;
            cardNumber = inCardNumber;
            ResetKioskTimer();
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
                        lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            else
            {
                Card = new Card(readerDevice, cardNumber, "External POS", KioskStatic.Utilities);

                string message = "";
                lblmsg.Text = "Refreshing Card from HQ. Please Wait...";
                KioskStatic.logToFile(lblmsg.Text);
                
                Application.DoEvents();
                if (!KioskHelper.refreshCardFromHQ(ref Card, ref message))
                {
                    lblmsg.Text = message;
                    Card = null;
                    log.LogMethodExit();
                    return;
                }
            }

            if (Card.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(459);
                
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
                            && Card.customerDTO!=null 
                            && !Card.customerDTO.PolicyTermsAccepted)
                        ) && KioskStatic.RegisterBeforePurchase)
                {
                    Audio.PlayAudio(Audio.RegisterCardPrompt);
                    //this.Hide(); // hide so that tap card message doesn't show during screen transitions
                    if (new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(758), KioskStatic.Utilities.MessageUtils.getMessage(759)).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        CustomerStatic.ShowCustomerScreen(Card.CardNumber);
                        Card.getCardDetails(Card.card_id); //refresh card object
                    }
                }

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
            //btnClose.BackgroundImage = Properties.Resources.cancel_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            // btnClose.BackgroundImage = Properties.Resources.cancel_btn;//Modification on 17-Dec-2015 for introducing new theme
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
            //btnCustom.BackgroundImage = Properties.Resources.button_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnCustom_MouseUp(object sender, MouseEventArgs e)
        {
            // btnCustom.BackgroundImage = Properties.Resources.button_normal;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnYes_MouseDown(object sender, MouseEventArgs e)
        {
            //(sender as Button).BackgroundImage = Properties.Resources.button_pressed;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnYes_MouseUp(object sender, MouseEventArgs e)
        {
            // (sender as Button).BackgroundImage = Properties.Resources.button_normal;//Starts:Modification on 17-Dec-2015 for introducing new theme
        }
    }
}
