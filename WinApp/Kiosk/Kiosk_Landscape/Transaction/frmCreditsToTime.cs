/********************************************************************************************
* Project Name - Parafait_Kiosk.Transaction  - frmCreditsToTime
* Description  - frmCreditsToTime 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint changes 
*2.80        5-Sep-2019       Deeksha            Added logger methods.
********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;

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
                cardNumber = pCardno;
            log.LogMethodExit();
        }

        void init(string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            //KioskStatic.formatMessageLine(textBoxMessageLine, 26, Properties.Resources.bottom_bar);
            // KioskStatic.setDefaultFont(this);

           
            txtAvlblTokens.Text = txtCardNo.Text = txtCreditsToConvert.Text = txtBalanceCredits.Text = txtAvailableTime.Text = txtNewTime.Text = " ";

            try
            {
                TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(KioskStatic.Utilities.ExecutionContext);
                if (tagNumberLengthList.MaximumValidTagNumberLength > 10)
                {
                    txtCardNo.Font = new Font(txtCardNo.Font.FontFamily, txtCardNo.Font.Size - 8, txtCardNo.Font.Style);
                    txtCardNo2.Font = new Font(txtCardNo2.Font.FontFamily, txtCardNo2.Font.Size - 8, txtCardNo2.Font.Style);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while changing the font size of tag number", ex);
            }

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;

            lblMessage.Text = MessageUtils.getMessage(1382);
            textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            textBoxMessageLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));

            textBoxMessageLine.Text = message;
            KioskStatic.logToFile(message);

            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
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
                //numPad.Init(KioskStatic.KIOSK_CARD_VALUE_FORMAT, 0);
                NumberPadVarPanel = numPad.NumPadPanel();
                NumberPadVarPanel.Location = new Point(panelCardDetails.Right - 145, panelCardDetails.Top);
                //NumberPadVarPanel.Location = new Point((this.Width - NumberPadVarPanel.Width) / 2, btnBack.Bottom + 5);
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
                textBoxMessageLine.Text = MessageUtils.getMessage(1382);
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
                textBoxMessageLine.Text = MessageUtils.getMessage(459);
                log.LogMethodExit();
                return;
            }
            txtCardNo.Text = cardNumber;
            try
            {
                if (txtCardNo.Text.ToString().Length > 0)
                {
                    txtAvlblTokens.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString();
                    txtAvailableTime.Text = (card.time + card.CreditPlusTime).ToString();// (KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    txtBalanceCredits.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString();
                    ShowKeyPadAfterCardTap();
                    textBoxMessageLine.Text = MessageUtils.getMessage(1382);

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
                textBoxMessageLine.Text = MessageUtils.getMessage(457);
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
                        //Convert.ToDecimal(txtCreditsToConvert.Text);

                        if(decimal.TryParse(txtCreditsToConvert.Text, out creditsToConvert) == false)
                        {
                            textBoxMessageLine.Text = MessageUtils.getMessage(1382);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }

                        if (creditsToConvert == 0)
                        {
                            //  msg = "Please enter a value greater than Zero for points";
                            textBoxMessageLine.Text = MessageUtils.getMessage(1382);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }
                        decimal avltkresult = 0;
                        decimal.TryParse(txtAvlblTokens.Text, out avltkresult);

                        if (creditsToConvert > avltkresult)
                        {
                            textBoxMessageLine.Text = MessageUtils.getMessage(1383, avltkresult);
                            ShowKeyPadAfterCardTap();
                            log.LogMethodExit();
                            return;
                        }
                    } 
                    Card cardp = new Card(cardNumber, "", Utilities);
                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    sv = tp.ConvertCreditsForTime(cardp, Convert.ToInt32(txtCreditsToConvert.Text),-1, -1,true, "Kiosk Trx: convert Credit to Time", ref message);
                    if (sv == true)
                    {
                        Card updateCard = new Card(cardNumber, "", Utilities);
                        setKioskTimerSecondsValue(10);
                        NumberPadVarPanel.Visible = false;
                        lblTimeRemaining.Visible = false;
                        textBoxMessageLine.Visible = false;//playpas1:Ends                   
                        btnSave.Visible = false;
                        btnBack.Location = new Point(btnBack.Location.X + 40, btnBack.Location.Y - 20);
                        txtCardNo2.Text = cardNumber;
                        lblMessage.Text = MessageUtils.getMessage(1384);

                        txtAvlTime2.Text = (updateCard.time + updateCard.CreditPlusTime).ToString();// + (Convert.ToDouble(txtCreditsToConvert.Text) * KioskStatic.TIME_IN_MINUTES_PER_CREDIT)).ToString();
                        txtAavlCredits2.Text = ((updateCard.credits + updateCard.CreditPlusCardBalance + updateCard.CreditPlusCredits)).ToString();// - Convert.ToDouble(txtCreditsToConvert.Text)).ToString();

                        panelAfterSave.Width = panelCardDetails.Width;
                        panelAfterSave.Height = panelCardDetails.Height;

                        panelAfterSave.Visible = true;

                        panelAfterSave.Location = panelCardDetails.Location;
                        panelCardDetails.Visible = false;

                        btnBack.Top = panelAfterSave.Bottom + 10;
                        this.ActiveControl = btnBack;

                    }
                    else// msg = "Error occured.Please try again.";
                    {
                        textBoxMessageLine.Text = MessageUtils.getMessage(718);
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxMessageLine.Text = ex.ToString();
                KioskStatic.logToFile("Error in ConvertCreditsForTime");
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
