/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCreditsToTime 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint changes 
*2.80        09-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.0.0   13-Oct-2022      Sathyavathi        Mask card number
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk.Transaction
{
    public partial class frmCreditsToTime : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        Font savTimeOutFont;
        Font TimeOutFont;
        public frmCreditsToTime(string pCardno)
        {
            log.LogMethodEntry(pCardno);
            init(KioskStatic.Utilities.MessageUtils.getMessage(500));
            Audio.PlayAudio(Audio.TransferFromTapCard);
            if (pCardno != null)
            {
                cardNumber = pCardno;
            }
            log.LogMethodExit();
        }

        void init(string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();
            //KioskStatic.formatMessageLine(textBoxMessageLine, 26, Properties.Resources.bottom_bar);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;

            txtAvlblTokens.Text = txtCardNo.Text = txtCreditsToConvert.Text = txtBalanceCredits.Text = txtAvailableTime.Text = txtNewTime.Text = " ";

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;

            lblTapMsg.Text = MessageUtils.getMessage(1382);
            txtMessage.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));

            txtMessage.Text = message;
            KioskStatic.logToFile(message);

            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad numPad = null;
        Panel NumberPadVarPanel;


        private void ShowKeyPadAfterCardTap()
        {
            log.LogMethodEntry();
            if (numPad == null)
            {
                numPad = new Semnox.Core.Utilities.KeyPads.Kiosk.frmNumberPad();
                numPad.Init(KioskStatic.KIOSK_CARD_VALUE_FORMAT, 0);
                NumberPadVarPanel = numPad.NumPadPanel();
                NumberPadVarPanel.Controls["btnClose"].Visible = false;
                NumberPadVarPanel.Controls["btnCloseX"].Visible = false;
                NumberPadVarPanel.Height = 730;
                NumberPadVarPanel.BackgroundImageLayout = ImageLayout.Stretch;


                NumberPadVarPanel.Location = new Point((this.Width - NumberPadVarPanel.Width) / 2, btnPrev.Bottom + 5);
                this.Controls.Add(NumberPadVarPanel);

                numPad.setReceiveAction = EventnumPadOKReceived;
                numPad.setKeyAction = EventnumPadKeyPressReceived;

                this.KeyPreview = true;

                this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            }

            numPad.NewEntry = true;
            NumberPadVarPanel.Visible = true;
            NumberPadVarPanel.BringToFront();
            log.LogMethodExit();
        }
        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            txtCreditsToConvert.Text = n.ToString();

            btnSave.PerformClick();
            log.LogMethodExit();
        }

        void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            txtCreditsToConvert.Text = n.ToString();
            txtNewTime.Text = ((Convert.ToDouble(txtAvailableTime.Text) + (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0 ? (n * KioskStatic.TIME_IN_MINUTES_PER_CREDIT) : n))).ToString();
            txtBalanceCredits.Text = ((Convert.ToDouble(txtAvlblTokens.Text) - n)).ToString();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == (char)Keys.Clear)
            {
                txtMessage.Text = MessageUtils.getMessage(1382);
                txtBalanceCredits.Text = txtAvlblTokens.Text;
            }
            else
            {
                numPad.GetKey(e.KeyChar);
                //secondsRemaining = timeOutSecs;
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void frmCreditsToTime_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TimeSpan ts = new TimeSpan(0, 0, GetKioskTimerTickValue());
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString();
            handleCardInformation();
            log.LogMethodExit();
        }
        void handleCardInformation()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            Card card = new Card(cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                // msg = "Tap an issued Pass for Point Transfer";
                txtMessage.Text = MessageUtils.getMessage(459);
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = KioskHelper.GetMaskedCardNumber(cardNumber);
            try
            {
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    txtAvlblTokens.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString();
                    txtAvailableTime.Text = (card.time + card.CreditPlusTime).ToString();// (KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    txtBalanceCredits.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString();
                    ShowKeyPadAfterCardTap();
                    txtMessage.Text = MessageUtils.getMessage(1382);

                    Audio.PlayAudio(Audio.PointsToBeTransferred);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message.ToString());
            }
            log.LogMethodExit();
        }


        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining <= 60)
            {
                tickSecondsRemaining = tickSecondsRemaining - 1;
                lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                tickSecondsRemaining = tickSecondsRemaining - 1;
                lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 3)
            {
                btnSave.Enabled = false;
            }

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
                KioskStatic.logToFile("frmCreditsToTime Timed out");
                txtMessage.Text = MessageUtils.getMessage(457);
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string message = "";
            bool sv = false;
            try
            {
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    if (txtCreditsToConvert.Text.ToString().Length > 0)
                    {
                        decimal creditsToConvert = 0;
                        if (decimal.TryParse(txtCreditsToConvert.Text, out creditsToConvert) == false)
                        {
                            txtMessage.Text = MessageUtils.getMessage(1382);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }

                        if (creditsToConvert == 0)
                        {
                            //  msg = "Please enter a value greater than Zero for points";
                            txtMessage.Text = MessageUtils.getMessage(1382);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }
                        decimal avltkresult = 0;
                        decimal.TryParse(txtAvlblTokens.Text, out avltkresult);

                        if (creditsToConvert > avltkresult)
                        {
                            txtMessage.Text = MessageUtils.getMessage(1383, avltkresult);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }
                    }
                    Card cardp = new Card(cardNumber, "", Utilities);
                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    sv = tp.ConvertCreditsForTime(cardp, Convert.ToInt32(txtCreditsToConvert.Text),-1,-1,true, "Kiosk Trx: convert Credit to Time", ref message);
                    if (sv == true)
                    {
                        Card updateCard = new Card(cardNumber, "", Utilities);
                        setKioskTimerSecondsValue(10);
                        NumberPadVarPanel.Visible = false;
                        lblTimeRemaining.Visible = false;
                        txtMessage.Visible = false;//playpas1:Ends                   
                        btnSave.Visible = false;
                        txtCardNo2.Text = KioskHelper.GetMaskedCardNumber(cardNumber);
                        lblTapMsg.Text = MessageUtils.getMessage(1384);

                        txtAvlTime2.Text = (updateCard.time + updateCard.CreditPlusTime).ToString();// + (Convert.ToDouble(txtCreditsToConvert.Text) * KioskStatic.TIME_IN_MINUTES_PER_CREDIT)).ToString();
                        txtAavlCredits2.Text = ((updateCard.credits + updateCard.CreditPlusCardBalance + updateCard.CreditPlusCredits)).ToString();// - Convert.ToDouble(txtCreditsToConvert.Text)).ToString();

                        panelAfterSave.Width = panelCardDetails.Width;
                        panelAfterSave.Height = panelCardDetails.Height;

                        panelAfterSave.Visible = true;
                        btnPrev.Location = new Point(btnPrev.Location.X + 120, btnPrev.Location.Y);
                        panelAfterSave.Location = panelCardDetails.Location;
                        panelCardDetails.Visible = false;

                        btnPrev.Top = panelAfterSave.Bottom + 10;
                        this.ActiveControl = btnPrev;

                    }
                    else// msg = "Error occured.Please try again.";
                    {
                        KioskStatic.logToFile("Error msg: " + message);
                        txtMessage.Text = MessageUtils.getMessage(718);
                    }
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.ToString();
                KioskStatic.logToFile("Error in ConvertCreditsForTime");
            }
            log.LogMethodExit();
        }


        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCreditsToTime");
            try
            {
                foreach (Control c in panelCardDetails.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeDetailsHeaderTextForeColor;//Panel card details info 
                    }
                    if (type.Contains("panel"))
                    {
                        foreach (Control lbl in c.Controls)
                        {
                            lbl.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeDetailsInfoTextForeColor;//Panel card details info
                        }
                    }
                }

                foreach (Control c in panelAfterSave.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeAfterSaveHeaderTextForeColor;//After save header
                    }
                    if (type.Contains("panel"))
                    {
                        foreach (Control lbl in c.Controls)
                        {
                            lbl.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeAfterSaveInfoTextForeColor;//After save info 
                        }
                    }
                }

                this.lblTapMsg.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeHeaderTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeBtnPrevTextForeColor;
                this.btnSave.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeBtnSaveTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeFooterTextForeColor;
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeTimeRemainingTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CreditsToTimeBtnHomeTextForeColor;
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                lblSiteName.Text = KioskStatic.SiteHeading;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.TransferBackgroundImage);
                btnPrev.BackgroundImage =
                    btnSave.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCreditsToTime: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
