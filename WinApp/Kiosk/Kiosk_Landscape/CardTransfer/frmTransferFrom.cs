/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmTransferFrom.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk.CardTransfer
{
    public partial class frmTransferFrom : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        public Card Card;
        public string selectedEntitlementTpe;


        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        Font savTimeOutFont;
        Font TimeOutFont;

        public frmTransferFrom(string pCardno, string entitlementType)
        {
            log.LogMethodEntry(pCardno, entitlementType);
            selectedEntitlementTpe = entitlementType;
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(527));
                Audio.PlayAudio(Audio.InsertCardIntoReader);
            }
            else
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(500));
                Audio.PlayAudio(Audio.TransferFromTapCard);
            }

            if (pCardno != null) cardNumber = pCardno;
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            //KioskStatic.formatMessageLine(textBoxMessageLine);//Starts:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(textBoxMessageLine, 23, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            lblSiteName.Text = KioskStatic.SiteHeading;
            //lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;//Ends:Modification on 17-Dec-2015 for introducing new theme

            txtAvlblTokens.Text = txtCardNo.Text =  "";
            txtTransfer.Text = "0";

            try
            {
                TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(KioskStatic.Utilities.ExecutionContext);
                if (tagNumberLengthList.MaximumValidTagNumberLength > 10)
                {
                    txtCardNo.Font = new Font(txtCardNo.Font.FontFamily, txtCardNo.Font.Size - 9, txtCardNo.Font.Style);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing the font size of tag number", ex);
            }

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            buttonNext.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;
            buttonNext.Size = buttonNext.BackgroundImage.Size;
            lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            //lblTapMsg.Text = MessageUtils.getMessage(749);
            //textBoxMessageLine.ForeColor = lblTapMsg.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;//Ends:Modification on 17-Dec-2015 for introducing new theme

            displayMessageLine(Message, MESSAGE);

            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            savTimeOutFont = lblTimeRemaining.Font;//Starts:Modification on 17-Dec-2015 for introducing new theme
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);//Ends:Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        NumberPad numPad = null;
        Panel NumberPadVarPanel;
        private void ShowKeyPadAfterCardTap()
        {
            log.LogMethodEntry();
            if (numPad == null)
            {
                numPad = new NumberPad(KioskStatic.KIOSK_CARD_VALUE_FORMAT, 0);
                NumberPadVarPanel = numPad.NumPadPanel();
                NumberPadVarPanel.Location = new Point(panelCardDetails.Right + 5, panelCardDetails.Top);
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

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == (char)Keys.Escape)
                NumberPadVarPanel.Visible = false;
            else
            {
                numPad.GetKey(e.KeyChar);
                //secondsRemaining = timeOutSecs;
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            if (selectedEntitlementTpe == "Time")
                txtTransfer.Text = n.ToString();
            else
                txtTransfer.Text = n.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
            buttonNext.PerformClick();
            log.LogMethodExit();
        }

        void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry();
            double n = numPad.ReturnNumber;
            if (selectedEntitlementTpe == "Time")
                txtTransfer.Text = n.ToString();
            else
                txtTransfer.Text = n.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
            // secondsRemaining = timeOutSecs;
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void frmTransferTokenFrom_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Cursor.Show();
            TimeSpan ts = new TimeSpan(0, 0, GetKioskTimerTickValue());
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString();

            if (selectedEntitlementTpe != string.Empty)
            {
                if (selectedEntitlementTpe == "Time")
                {
                    lblAvlCredits.Text = MessageUtils.getMessage(1341);
                    lblCreditsToTransfer.Text = MessageUtils.getMessage(1342);
                    lblTapMsg.Text = MessageUtils.getMessage(1346);
                    lblHeading.Text = MessageUtils.getMessage("Transfer Time");
                }
                else
                {
                    lblAvlCredits.Text = MessageUtils.getMessage("Available Credits");
                    lblCreditsToTransfer.Text = MessageUtils.getMessage("Credits To Transfer");
                    lblHeading.Text = MessageUtils.getMessage("Transfer Points");
                }
            }
            handleCardRead();
            //TimeOutTimer.Start();
            StartKioskTimer();//Modification on 01-Jan-2016 for introducing new theme
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
                buttonNext.Enabled = false;
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
                    setKioskTimerSecondsValue(tickSecondsRemaining);
                }
            }

            if (tickSecondsRemaining <= 0)
            {
                KioskStatic.logToFile("frmTransferFrom Timed out");
                displayMessageLine(MessageUtils.getMessage(457), WARNING);
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        void handleCardRead()
        {
            log.LogMethodEntry();
            //secondsRemaining = timeOutSecs;
            ResetKioskTimer();
            Card card = new Card(cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                // msg = "Tap an issued Pass for Point Transfer";
                displayMessageLine(MessageUtils.getMessage(459), MESSAGE);
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = cardNumber;
            try
            {
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    if (selectedEntitlementTpe == "Time")
                        txtAvlblTokens.Text = (card.time + card.CreditPlusTime).ToString();// (KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    else
                        txtAvlblTokens.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    ShowKeyPadAfterCardTap();
                    displayMessageLine(MessageUtils.getMessage("Play Pass No") + ": " + txtCardNo.Text.ToString(), MESSAGE);
                    Audio.PlayAudio(Audio.PointsToBeTransferred);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message.ToString());
            }
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            textBoxMessageLine.Text = message;
            KioskStatic.logToFile(message);
            log.LogMethodExit();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtCardNo.Text.ToString().Length > 0)
            {
                if (txtTransfer.Text.ToString().Length > 0)
                {
                    decimal tsfr = Convert.ToDecimal(txtTransfer.Text);

                    if (tsfr == 0)
                    {
                        //  msg = "Please enter a value greater than Zero for points";
                        displayMessageLine(MessageUtils.getMessage(745), WARNING);
                        ShowKeyPadAfterCardTap();                        
                        log.LogMethodExit();
                        return;
                    }
                    bool val;
                    decimal avltkresult = 0;
                    val = decimal.TryParse(txtAvlblTokens.Text, out avltkresult);

                    if (tsfr > avltkresult)
                    {
                        // msg = "Can't transfer more than available";
                        displayMessageLine(MessageUtils.getMessage(746), WARNING);
                        ShowKeyPadAfterCardTap();
                        log.LogMethodExit();
                        return;
                    }
                    //TimeOutTimer.Stop();
                    StopKioskTimer();
                     frmTransferTo ftt = new frmTransferTo(txtCardNo.Text, tsfr, selectedEntitlementTpe);
                    ftt.ShowDialog();
                    Close();
                }
                else
                {
                    // msg = "Please enter a value to transfer";
                    displayMessageLine(MessageUtils.getMessage(747), WARNING);
                    log.LogMethodExit();
                    return;
                }
            }
            else
            {
                // msg = "Play Pass details are not available.Please tap the Play Pass ";
                displayMessageLine(MessageUtils.getMessage(748), WARNING);
            }
            log.LogMethodExit();
        }

        private void txtTransfer_TextChanged(object sender, EventArgs e)
        {
            //int count;

            //if (txtTransfer.Text.ToString().Contains("."))
            //{
            //    if (txtTransfer.Text.ToString().IndexOf(".") == 0)
            //        SendKeys.Send("{BACKSPACE}");
            //    else
            //    {
            //        count = txtTransfer.Text.ToString().Split('.').Length - 1;
            //        if (count > 1)
            //            SendKeys.Send("{BACKSPACE}");
            //    }
            //}
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void txtTransfer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ShowKeyPadAfterCardTap();
            log.LogMethodExit();
        }

        private void frmTransferFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TimeOutTimer.Stop();
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme
    }
}
