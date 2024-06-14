/********************************************************************************************
* Project Name - Parafait_Kiosk -frmTapCard
* Description  - frmTapCard 
* 
**************
**Version Log
**************
*Version       Date             Modified By         Remarks          
*********************************************************************************************
 * 2.70        01-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
 * 2.80        05-Sep-2019      Deeksha             Added logger methods.
 * 2.130.0     09-Jul-2021      Dakshak             Theme changes to support customized Font ForeColor
 * 2.140.0     18-Oct-2021      Sathyavathi         Check-In Check-Out feature in Kiosk
 * 2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 * 2.150.7     10-Nov-2023      Sathyavathi         Customer Lookup Enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmTapCard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //int ticks = 0;
        public string cardNumber;
        public Card Card;
        bool _isPurchase = false;
        private readonly TagNumberParser tagNumberParser;
        private KioskTransaction kioskTransaction;
        private bool isRegisteredCustomerOnly;
        private System.Drawing.Size frmTapCardSize;

        /// <summary>
        /// GetKioskTransaction
        /// </summary>
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmTapCard()
        {
            log.LogMethodEntry();
            _isPurchase = false;
            DialogResult = DialogResult.None;
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
            //If Customer Mandatory for Check-In is set to No, user is allowed to skip card tap and proceed to product screen.
            string val = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "CUSTOMER_MANDATORY_FOR_CHECK-IN");
            log.Debug("CUSTOMER_MANDATORY_FOR_CHECK-IN : " + val);
            if (string.IsNullOrWhiteSpace(val) == false && val == "N")
            {
                btnSkip.Visible = true;
            }
            btnCustom.Visible = btnYes.Visible = btnNo.Visible = false;
            btnClose.Visible = KioskStatic.ENABLE_CLOSE_IN_READ_CARD_SCREEN;
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public frmTapCard(KioskTransaction kioskTransaction, bool isRegisteredCustomerOnly, bool isPurchase = false) : this()
        {
            log.LogMethodEntry("kioskTransaction", isRegisteredCustomerOnly, isPurchase);
            this._isPurchase = isPurchase;
            DialogResult = DialogResult.None;
            this.kioskTransaction = kioskTransaction;
            this.isRegisteredCustomerOnly = isRegisteredCustomerOnly;
            log.LogMethodExit();
        }

        public frmTapCard(string Message, string customButtonMessage = null)
        {
            log.LogMethodEntry(Message, customButtonMessage);
            DialogResult = DialogResult.None;
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
                    btnClose.Location = new System.Drawing.Point(btnClose.Location.X + 40, btnClose.Location.Y);
                    btnCustom.Location = new System.Drawing.Point(btnCustom.Location.X + 40, btnCustom.Location.Y);
                }
                btnClose.BackgroundImageLayout = ImageLayout.Stretch;
                btnCustom.BackgroundImageLayout = ImageLayout.Stretch;
            }
            KioskStatic.Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            lblmsg.Text = Message;

            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;   //Starts:Modification on 17-Dec-2015 for introducing new theme         
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
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void frmTapCard_FormClosed(object sender, FormClosedEventArgs e)
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

        private void frmTapCard_Load(object sender, EventArgs e)
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

            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height - 200);

            //timer1.Start();
            frmTapCardSize = this.Size;
            KioskStatic.logToFile("frmTapCard loaded");
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
                        lblmsg.Text = ex.Message;
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        lblmsg.Text = ex.Message;
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        lblmsg.Text = ex.Message;
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
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
                catch (CustomerStatic.TimeoutOccurred ex)
                {
                    KioskStatic.logToFile("Timeout occured");
                    log.Error(ex);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                catch (Exception ex)
                {
                    lblmsg.Text = ex.Message;
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
            lblmsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008);
            //Processing..Please wait...
            Application.DoEvents();
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
                        lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                        log.Info(lblmsg.Text);
                        log.LogMethodExit();
                        return;
                    }
                    log.Error("Error occurred while executing handleCardRead()" + ex.Message);
                }
            }
            else
            {
                lblmsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 91);//
                //"Loading Card data. Please Wait...";
                Application.DoEvents();
                Card = new Card(readerDevice, cardNumber, "External POS", KioskStatic.Utilities);

                string message = "";
                lblmsg.Text = "Refreshing Card from HQ. Please Wait...";
                Application.DoEvents();
                if (!KioskHelper.refreshCardFromHQ(ref Card, ref message))
                {
                    lblmsg.Text = message;
                    Card = null;
                    log.LogMethodExit();
                    return;
                }
            }

            log.LogVariableState("Card.CardStatus", Card.CardStatus);
            if (Card.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(459);

                Application.DoEvents();
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

                if (_isPurchase && Card.technician_card.Equals('Y') == false)
                {
                    bool isLinked = false;
                    try
                    {
                        HideTapCardForm();
                        isLinked = CustomerStatic.LinkCustomerToTheCard(kioskTransaction, isRegisteredCustomerOnly, Card);
                        CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isRegisteredCustomerOnly, isLinked);
                    }
                    catch (CustomerStatic.TimeoutOccurred ex)
                    {
                        log.Error(ex);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                    finally
                    {
                        UnHideTapCard();
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                Close();
            }
            log.LogMethodExit();
        }

        private void UnHideTapCard()
        {
            log.LogMethodEntry();
            for (int i = Application.OpenForms.Count-1; i > 1; i--)
            {
                if (Application.OpenForms[i].Name == "frmTapCard" && (Application.OpenForms[i].Size.Width == 0 && Application.OpenForms[i].Size.Height == 0))
                {
                    Application.OpenForms[i].Size = frmTapCardSize;
                    break;
                }
            }
            log.LogMethodExit();
        }

        private void HideTapCardForm()
        {
            log.LogMethodEntry();
            for (int i = Application.OpenForms.Count - 1; i > 1; i--)
            {
                if (Application.OpenForms[i].Name == "frmTapCard" && (kioskTransaction.HasCustomerRecord() == true))
                {
                    frmTapCardSize = Application.OpenForms[i].Size;
                    Application.OpenForms[i].Size = new System.Drawing.Size(0, 0);
                    break;
                }
            }
            log.LogMethodExit();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            //  (sender as Button).BackgroundImage = Properties.Resources.button_normal;//Modification on 17-Dec-2015 for introducing new theme
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmTapCard");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.TapCardScreenHeaderTextForeColor;//Please Tap your card at the reader
                this.btnCustom.ForeColor = KioskStatic.CurrentTheme.TapCardScreenNewCardTextForeColor;//New Card button
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.TapCardScreenYesBtnTextForeColor;//Yes button
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.TapCardScreenCloseBtnTextForeColor;//Close button
                this.btnNo.ForeColor = KioskStatic.CurrentTheme.TapCardScreenNoBtnTextForeColor;//No Button
                this.button1.ForeColor = KioskStatic.CurrentTheme.TapCardScreenButton1TextForeColor;//No Button
                btnYes.BackgroundImage =
                btnNo.BackgroundImage =
                btnClose.BackgroundImage =
                btnCustom.BackgroundImage =
                btnSkip.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardButtons;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmTapCard: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }
    }
}
