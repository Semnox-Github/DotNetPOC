/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmTransferTo.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1       02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
 *2.130.0      30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.150.0.0    10-Oct-2022      Sathyavathi        Mask card number
********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.CardTransfer
{
    public partial class frmTransferTo : BaseFormKiosk
    {
        public string cardNumber;
        public Card Card;
        //public string entitlement;
        public string selectedEntitlementType;

        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        String CardNo;
        int ParentCardID;
        int TokReceivedCardID;
        decimal NoOfTokens;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        
        Font savTimeOutFont;
        Font TimeOutFont;

        public frmTransferTo(string cardNo, decimal noofTokens, string entitlementType)
        {
            log.LogMethodEntry(cardNo, noofTokens, entitlementType);
            selectedEntitlementType = entitlementType;
            Utilities = new Utilities();
            ParafaitEnv = Utilities.ParafaitEnv;
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(527));
                Audio.PlayAudio(Audio.InsertCardIntoReader);
            }
            else
            {
                init(KioskStatic.Utilities.MessageUtils.getMessage(500));
                Audio.PlayAudio(Audio.TransferToCardTap);
            }
            CardNo = cardNo;
            NoOfTokens = noofTokens;
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry();
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.formatMessageLine(textBoxMessageLine, 26, Properties.Resources.bottom_bar);
            KioskStatic.setDefaultFont(this);

            lblSiteName.Text = KioskStatic.SiteHeading;

            this.BackgroundImage = KioskStatic.CurrentTheme.TransferBackgroundImage;

            lblTapMsg.Text = MessageUtils.getMessage(750);
            //textBoxMessageLine.ForeColor = lblTapMsg.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //textBoxMessageLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            txtAvlblTokens.Text = txtCardNo.Text = txtFromCard.Text = txtTransferFromCredits.Text = txtTransfrdTokens.Text = "";

            displayMessageLine(Message, MESSAGE);

            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        //int timeOutSecs = 30;
        //int secondsRemaining;//Playpass1:Starts
        //Timer TimeOutTimer;//PlayPass1:Ends
        private void frmTransferTokenTo_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmTransferTokenTo_Load");
            Cursor.Show();

            panelCardDetails.Left = this.Width / 2 - panelCardDetails.Width / 2;

            Card cardp = new Card(CardNo, "", Utilities);
            ParentCardID = cardp.card_id;

            TimeSpan ts = new TimeSpan(0, 0, GetKioskTimerTickValue());
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString();//PlayPass1:Ends

            if (selectedEntitlementType != string.Empty)
            {
                if (selectedEntitlementType == "Time")
                {
                    lblAvlCredits.Text = MessageUtils.getMessage(1341);
                    lblNewPoints.Text = MessageUtils.getMessage(1344);
                    lblTapMsg.Text = MessageUtils.getMessage(1347);
                }
                else
                {
                    lblAvlCredits.Text = MessageUtils.getMessage("Available Credits");
                    lblNewPoints.Text = MessageUtils.getMessage("New Credits");
                }
            }

            bool loop = true;
            while (loop)
            {
                Application.DoEvents();
                frmTapCard ftp = new frmTapCard();
                ftp.ShowDialog();
                if (ftp.Card == null)
                {
                    loop = false;
                    Close();
                }
                else
                {
                    cardNumber = ftp.cardNumber;
                    if (handleCardRead())
                        loop = false;
                }
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
                buttonNext.Enabled = false;
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                KioskStatic.logToFile("frmTransferTokenTo Timed out"); 
                displayMessageLine(MessageUtils.getMessage(457), WARNING);
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmTransferTokenTo_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            //TimeOutTimer.Stop();
            KioskStatic.logToFile("frmTransferTokenTo_FormClosing");
            log.LogMethodExit();
        }

        bool handleCardRead()
        {
            log.LogMethodEntry();
            Card card = new Card(cardNumber, "", Utilities);

            if (card.CardStatus == "NEW")
            {
                // msg = "Tap an issued Play Pass for Point Transfer";
                displayMessageLine(MessageUtils.getMessage(459), WARNING);
                log.LogMethodExit(false);
                return false;
            }

            if (card != null && card.technician_card == 'Y') //Prevent transfer to Technician card
            {
                displayMessageLine(MessageUtils.getMessage(197, card.CardNumber), WARNING);
                log.LogMethodExit(false);
                return false;
            }

            try
            {
                TokReceivedCardID = card.card_id;
                if (ParentCardID == TokReceivedCardID)
                {
                    //  msg = "Transfer of points to the same pass is not possible !Please tap a different pass";
                    displayMessageLine(MessageUtils.getMessage(751), WARNING);
                    log.LogMethodExit(false);
                    return false;
                }
                txtCardNo.Text = KioskHelper.GetMaskedCardNumber(cardNumber);
                if (selectedEntitlementType == "Time")
                    txtAvlblTokens.Text = (card.time + card.CreditPlusTime ).ToString(); //(KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();card.creditPlusTime
                else
                    txtAvlblTokens.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                //txtAvlblTokens.Text = card.credits.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                if (selectedEntitlementType == "Time")
                    txtTransfrdTokens.Text = ((decimal)(card.time + card.CreditPlusTime) + NoOfTokens).ToString();
                else
                    txtTransfrdTokens.Text = ((decimal)(card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits) + NoOfTokens).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                buttonNext.Focus();
                displayMessageLine(MessageUtils.getMessage("Play Pass No") + ": " + txtCardNo.Text.ToString(), MESSAGE);

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                log.LogMethodExit(false);
                return false;
            }
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
            try
            {
                displayMessageLine("", MESSAGE);
                if (txtCardNo.Text.ToString().Length == 0)
                {
                    // msg = "Play Pass details are not available.Please tap the Play Pass";
                    displayMessageLine(MessageUtils.getMessage(748), WARNING);
                    return;
                }

                bool sv = false;
                TaskProcs tp = new TaskProcs(Utilities);
                string message = "";

                if (ParentCardID > 0 && TokReceivedCardID > 0)
                {
                    Card card = new Card(cardNumber, "", Utilities);

                    if (card.CardStatus == "NEW")
                    {
                        // msg = "Tap an issued Play Pass for Point Transfer";
                        displayMessageLine(MessageUtils.getMessage(459), WARNING);
                        log.LogMethodExit();
                        return;
                    }

                    if (ParentCardID == TokReceivedCardID)
                    {
                        // msg = "Transfer of points to the same pass is not possible !Please tap a different pass";
                        displayMessageLine(MessageUtils.getMessage(751), WARNING);
                        log.LogMethodExit();
                        return;
                    }
                    if (selectedEntitlementType == "Time")
                        sv = tp.BalanceTransferTime(ParentCardID, TokReceivedCardID, Convert.ToDouble(NoOfTokens), "Kiosk Transfer", ref message);
                    else
                        sv = tp.BalanceTransfer(ParentCardID, TokReceivedCardID, NoOfTokens, 0, 0, 0, "Kiosk Transfer", ref message);
                    if (sv == true)
                    {
                        //secondsRemaining = 10;//playpas1:starts
                        setKioskTimerSecondsValue(10);
                        lblTimeRemaining.Visible = false;
                        textBoxMessageLine.Visible = false;//playpas1:Ends                   
                        buttonNext.Enabled = false;
                        // msg = "Points transferred successfully";
                        displayMessageLine(MessageUtils.getMessage(798), MESSAGE);
                        lblTapMsg.Text = textBoxMessageLine.Text;

                        txtFromCard.Text = CardNo;
                        Card cardp = new Card(CardNo, "", Utilities);
                        if (selectedEntitlementType == "Time")
                        {
                            lblTapMsg.Text = MessageUtils.getMessage(1340);
                            lblAvlCredits2.Text = MessageUtils.getMessage(1341);
                            txtTransferFromCredits.Text = (cardp.time + cardp.CreditPlusTime).ToString();

                        }
                        else
                        {
                            lblTapMsg.Text = textBoxMessageLine.Text;
                            txtTransferFromCredits.Text = (cardp.credits + cardp.CreditPlusCardBalance + cardp.CreditPlusCredits).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                        }

                        //txtTransferFromCredits.Text = cardp.credits.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                        txtAvlblTokens.Text = txtTransfrdTokens.Text;

                        panelTransferer.Width = panelCardDetails.Width;
                        panelCardDetails.Height = panelTransferer.Height;
                        buttonNext.Visible = txtTransfrdTokens.Visible = lblNewPoints.Visible = panelTrnsferredTokens.Visible = false;

                        panelTransferer.Location = new Point((this.Width - panelTransferer.Width) / 2, panelCardDetails.Location.Y + 50);
                        panelCardDetails.Location = new Point(panelTransferer.Location.X, panelTransferer.Bottom + 140);

                        lblTransfererDetails.Location = new Point((this.Width - lblTransfererDetails.Width) / 2, panelTransferer.Location.Y - lblTransfererDetails.Height);
                        lblTransfereeDetails.Location = new Point((this.Width - lblTransfereeDetails.Width) / 2, panelCardDetails.Location.Y - lblTransfereeDetails.Height);

                        lblTransfererDetails.Text = MessageUtils.getMessage(756);
                        lblTransfereeDetails.Text = MessageUtils.getMessage(757);
                        lblTransfereeDetails.Visible = lblTransfererDetails.Visible = true;

                        panelTransferer.Visible = true;

                        btnPrev.Top = panelCardDetails.Bottom + 10;

                        this.ActiveControl = btnPrev;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            displayMessageLine(message, WARNING);
                        }
                        else// msg = "Error occured.Please try again.";
                        {
                            displayMessageLine(MessageUtils.getMessage(718), ERROR);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Set Customized Font Colors
        /// </summary>
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblTapMsg.ForeColor = KioskStatic.CurrentTheme.TransferToScreenHeaderTextForeColor;//(PLEASE TAP THE CARD THAT CREDITS WILL BE ADDED TO)
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.TransferToScreenTimeRemainingTextForeColor;//
                this.label1.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCardsHeaderTextForeColor;//(Cards Header)
                this.lblAvlCredits.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCreditsHeaderTextForeColor;//(Available Credits Header)
                this.lblNewPoints.ForeColor = KioskStatic.CurrentTheme.TransferToScreenPointsHeaderTextForeColor;//(New points Header)
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.TransferToScreenBtnPrevTextForeColor;//
                this.txtCardNo.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCardsInfoTextForeColor;//(Cards info)
                this.txtAvlblTokens.ForeColor = KioskStatic.CurrentTheme.TransferToScreenAvlTokensInfoTextForeColor;//Available Credits info
                this.txtTransfrdTokens.ForeColor = KioskStatic.CurrentTheme.TransferToScreenTransTokensInfoTextForeColor;//(New points info)
                this.buttonNext.ForeColor = KioskStatic.CurrentTheme.TransferToScreenBtnNextTextForeColor;//
                this.lblTransfererDetails.ForeColor = KioskStatic.CurrentTheme.TransferToScreenTransfererHeaderTextForeColor;//
                this.lblTransfereeDetails.ForeColor = KioskStatic.CurrentTheme.TransferToScreenTransfereeHeaderTextForeColor;//
                this.label6.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCardsHeader2TextForeColor;//Card # header
                this.lblAvlCredits2.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCreditsHeader2TextForeColor;//(Available credits header)
                this.txtFromCard.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCardsInfo2TextForeColor;//(Available credits header)
                this.txtTransferFromCredits.ForeColor = KioskStatic.CurrentTheme.TransferToScreenCreditsInfo2TextForeColor;//(Available credits header)
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.TransferToScreenFooterTextForeColor;//(Available credits header)
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
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
