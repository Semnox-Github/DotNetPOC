/********************************************************************************************
* Project Name - Parafait_Kiosk -frmAttractionTapCard
* Description  - Card tap for Attraction 
* 
**************
**Version Log
**************
*Version       Date             Modified By         Remarks          
*********************************************************************************************
 *2.155.0.0   16-Jun-2023       Suraj               Created for Attraction Sale in Kiosk
 *2.150.7     10-Nov-2023       Sathyavathi         Customer Lookup Enhancement
 *2.152.0.0   12-Dec-2023       Suraj Pai           Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;

namespace Parafait_Kiosk
{
    public partial class frmAttractionTapCard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        public Card Card;
        bool _isPurchase = false;
        private readonly TagNumberParser tagNumberParser;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmAttractionTapCard(string message, bool enableNote)
        {
            log.LogMethodEntry(message);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(527));
                Audio.PlayAudio(Audio.InsertCardIntoReader);
            }
            else
            {
                init(message);
                Audio.PlayAudio(Audio.TapCardOnReader);
            }
            btnClose.Visible = KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN;
            lblNote.Visible = enableNote;
            lblMessage.Text = string.Empty;
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public frmAttractionTapCard(KioskTransaction kioskTransaction, string message, bool enableNote) : this(message, enableNote)
        {
            log.LogMethodEntry("kioskTransaction", message, enableNote);
            this.kioskTransaction = kioskTransaction;
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            lblGreeting.Text = Message;
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;   //Starts:Modification on 17-Dec-2015 for introducing new theme         
            btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardButtons;
            button1.Text = KioskStatic.Utilities.MessageUtils.getMessage("Card Reader Below");
            lblNote.Text = KioskStatic.Utilities.MessageUtils.getMessage("Note: To load entitlements onto the same card, please tap it again");
            if (ThemeManager.CurrentThemeImages.CardReaderArrowImage != null)
            {
                pbDownArrow.Image = ThemeManager.CurrentThemeImages.CardReaderArrowImage;
                pbDownArrow.Visible = true;
            }
            else
            {
                pbDownArrow.Visible = false;
            }

            KioskStatic.setDefaultFont(this);//Ends:Modification on 17-Dec-2015 for introducing new theme
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
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void frmAttractionTapCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Audio.Stop();

            if (_isPurchase == false && KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }
            }

            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }
            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
            KioskStatic.logToFile(this.Name + ":Form Closed");
            log.LogMethodExit();
        }

        private void frmAttractionTapCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                    KioskStatic.cardAcceptor.AllowAllCards();
            }
            log.LogVariableState("KioskStatic.TopUpReaderDevice", KioskStatic.TopUpReaderDevice);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            KioskStatic.logToFile("frmAttractionTapCard loaded");
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
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
                        lblMessage.Text = ex.Message;
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        lblMessage.Text = ex.Message;
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        lblMessage.Text = ex.Message;
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    lblMessage.Text = message;
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
                    lblMessage.Text = ex.Message;
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            cardNumber = inCardNumber;
            ResetKioskTimer();
            KioskStatic.logToFile("Tapped card " + cardNumber);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                try
                {
                    Card = new MifareCard(KioskStatic.TopUpReaderDevice, cardNumber, "External POS", KioskStatic.Utilities);
                }
                catch (Exception ex)
                {
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                        log.Info(lblGreeting.Text);
                        log.LogMethodExit();
                        return;
                    }
                    log.Error("Error occurred while executing handleCardRead()" + ex.Message);
                }
            }
            else
            {
                lblMessage.Text = "Loading Card data. Please Wait...";
                Card = new Card(readerDevice, cardNumber, "External POS", KioskStatic.Utilities);

                string message = "";
                lblMessage.Text = "Validating the Card. Please Wait...";
                Application.DoEvents();
                if (!KioskHelper.refreshCardFromHQ(ref Card, ref message))
                {
                    lblMessage.Text = message;
                    Card = null;
                    log.LogMethodExit();
                    return;
                }
            }

            log.LogVariableState("Card.CardStatus", Card.CardStatus);
            if (Card.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(459);

                Card = null;
                cardNumber = "";
                //ticks = 0;
                ResetKioskTimer();

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
                log.Info("NEW card tapped. Rejected.");
            }
            else if (Card.technician_card.Equals('Y'))
            {
                lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(5188); //"Staff Card is not allowed. Please tap Customer Card..."
                Card = null;
                cardNumber = "";
                ResetKioskTimer();
                KioskStatic.logToFile("Staff Card tapped. Rejected.");
                log.Info("Staff Card tapped. Rejected.");
            }
            else
            {
                if (Card.vip_customer == 'N')
                {
                    Card.getTotalRechargeAmount();
                    if ((Card.credits_played >= KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                         || (Card.TotalRechargeAmount >= KioskStatic.Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && KioskStatic.Utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                    {
                        using (frmOKMsg fok = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(451, KioskStatic.VIPTerm)))
                        {
                            fok.ShowDialog();
                        }
                    }
                }
                Close();
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements in frmAttractionTapCard");
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.TapCardScreenHeaderTextForeColor; //Please tap your card for &1, 
                this.lblMessage.ForeColor = KioskStatic.CurrentTheme.TapCardScreenLblMessageTextForeColor; //Please tap your card for &1, 
                this.lblNote.ForeColor = KioskStatic.CurrentTheme.TapCardScreenNoteTextForeColor; //Note: You can tap the same card to book multiple attractions
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.TapCardScreenCloseBtnTextForeColor; //Close button
                this.button1.ForeColor = KioskStatic.CurrentTheme.TapCardScreenButton1TextForeColor; //Card reader below
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmAttractionTapCard: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}