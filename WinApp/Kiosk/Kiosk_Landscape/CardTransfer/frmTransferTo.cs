/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmTransferTo.cs 
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
    public partial class frmTransferTo : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber;
        public Card Card;
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
            log.LogVariableState("CardNo", CardNo);
            NoOfTokens = noofTokens;
            log.LogVariableState("NoOfTokens", NoOfTokens);
            log.LogMethodExit();
        }

        void init(string Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.formatMessageLine(textBoxMessageLine);
            KioskStatic.setDefaultFont(this);

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

            lblSiteName.Text = KioskStatic.SiteHeading;
            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            pbSuccess.Image = ThemeManager.CurrentThemeImages.SucessTransfer;
            buttonNext.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;            
            buttonNext.Size = buttonNext.BackgroundImage.Size;
            textBoxMessageLine.BackgroundImage = ThemeManager.CurrentThemeImages.BottomMessageLineImage;
            lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            //lblTapMsg.Text = MessageUtils.getMessage(750);//Ends:Modification on 17-Dec-2015 for introducing new theme

            //textBoxMessageLine.ForeColor = lblTapMsg.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;//Modification on 17-Dec-2015 for introducing new theme

            txtAvlblTokens.Text = txtCardNo.Text = txtFromCard.Text = txtTransferFromCredits.Text = txtTransfrdTokens.Text = "";

            displayMessageLine(Message, MESSAGE);

            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            log.LogMethodExit();
        }

        //int timeOutSecs = 30;
        //int secondsRemaining;//Playpass1:Starts
        //Timer TimeOutTimer;//PlayPass1:Ends
        string errMessage = "";
        private void frmTransferTokenTo_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmTransferTokenTo_Load");
            Cursor.Show();

            panelCardDetails.Location = new Point(this.Width / 2 - panelCardDetails.Width / 2, panelCardDetails.Location.Y);

            Card cardp = new Card(CardNo, "", Utilities);
            ParentCardID = cardp.card_id;

            TimeSpan ts = new TimeSpan(0, 0, GetKioskTimerTickValue());
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString();//PlayPass1:Ends
            
                if (selectedEntitlementType == "Time")
                {
                    lblAvlCredits.Text = MessageUtils.getMessage(1341);
                    lblNewPoints.Text = MessageUtils.getMessage(1344);
                    lblTapMsg.Text = MessageUtils.getMessage(1347);
                    lblHeading.Text = MessageUtils.getMessage("Transfer Time");
                }
                else
                {
                    lblAvlCredits.Text = MessageUtils.getMessage("Available Credits");
                    lblNewPoints.Text = MessageUtils.getMessage("New Credits");
                    lblHeading.Text = MessageUtils.getMessage("Transfer Points");
                }
            bool loop = true;
            while (loop)
            {
                Application.DoEvents();
                frmTapCard ftp;
                if (string.IsNullOrEmpty(errMessage))
                {
                    ftp = new frmTapCard();
                }
                else
                {
                    ftp = new frmTapCard(errMessage);
                    errMessage = "";
                }
                ftp.ShowDialog();
                if (ftp.Card == null)
                {
                    loop = false;
                    Close();
                }
                else
                {
                    cardNumber = ftp.cardNumber;
                    log.LogVariableState("cardNumber", cardNumber);
                    if (handleCardRead())
                        loop = false;
                }
            }
            // TimeOutTimer.Start();
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
                    errMessage = MessageUtils.getMessage(751);
                    log.LogMethodExit(false);
                    return false;
                }
                txtCardNo.Text = cardNumber;
                if (selectedEntitlementType == "Time")
                {
                    txtAvlblTokens.Text = (card.time + card.CreditPlusTime).ToString();
                    txtTransfrdTokens.Text = ((decimal)(card.time + card.CreditPlusTime) + NoOfTokens).ToString();
                }
                else
                {
                    txtAvlblTokens.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT).ToString();
                    txtTransfrdTokens.Text = ((decimal)(card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits) + NoOfTokens).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                }
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
            displayMessageLine("", MESSAGE);
            if (txtCardNo.Text.ToString().Length == 0)
            {
               // msg = "Play Pass details are not available.Please tap the Play Pass";
                displayMessageLine(MessageUtils.getMessage(748), WARNING);
                log.LogMethodExit();
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
                    // secondsRemaining = 10;//playpas1:starts
                    setKioskTimerSecondsValue(10);
                    lblTimeRemaining.Visible = false;
                    textBoxMessageLine.Visible = false;//playpas1:Ends                   
                    buttonNext.Enabled = false;
                    // msg = "Points transferred successfully";
                    displayMessageLine(MessageUtils.getMessage(798), MESSAGE);
                    //lblTapMsg.Text = textBoxMessageLine.Text;
                    
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
                    txtAvlblTokens.Text = txtTransfrdTokens.Text;

                    //panelTransferer.Width = panelCardDetails.Width;//Starts:Modification on 17-Dec-2015 for introducing new theme
                    //panelCardDetails.Height = panelTransferer.Height;
                    buttonNext.Visible = txtTransfrdTokens.Visible = lblNewPoints.Visible = false;

                    lblTapMsg.Top = btnHome.Bottom + 5;
                    lblTapMsg.Visible = true;
                    pbSuccess.Size = pbSuccess.Image.Size;
                    pbSuccess.Location = new Point((this.Width - pbSuccess.Width) / 2, (this.Height - pbSuccess.Height) / 2);
                    pbSuccess.Top = lblTapMsg.Bottom + 5;
                   
                    pbSuccess.Visible = true;
                    if (selectedEntitlementType == "Time")
                        lblTapMsg.Text = NoOfTokens + " " + MessageUtils.getMessage("minutes have been transferred");
                    else
                        lblTapMsg.Text = NoOfTokens + " " + MessageUtils.getMessage("points have been transferred");
                    lblTapMsg.Visible = true;                   
                    panelTransferer.Location = new Point(this.Left+5, (this.Height - panelTransferer.Height) / 2); //new Point((this.Width - panelTransferer.Width) / 2, panelCardDetails.Location.Y + 50);//NewTheme102015:starts
                    panelCardDetails.Location = new Point(this.Width - panelCardDetails.Width - 5, (this.Height - panelCardDetails.Height) / 2);//new Point(panelTransferer.Location.X, panelTransferer.Bottom + 140);

                    panelTransferer.Top = panelCardDetails.Top = pbSuccess.Bottom + lblTransfererDetails.Height + 5;

                    lblTransfererDetails.Location = new Point(panelTransferer.Location.X, panelTransferer.Location.Y - lblTransfererDetails.Height);
                    lblTransfereeDetails.Location = new Point(panelCardDetails.Location.X, panelCardDetails.Location.Y - lblTransfereeDetails.Height);

                    lblHeading.Text = MessageUtils.getMessage("SUCCESS");//Ends:Modification on 17-Dec-2015 for introducing new theme
                    lblTransfererDetails.Text = MessageUtils.getMessage(756);
                    lblTransfereeDetails.Text = MessageUtils.getMessage(757);
                    lblTransfereeDetails.Visible = lblTransfererDetails.Visible = true;
                    
                    panelTransferer.Visible = true;
                    
                    this.ActiveControl = btnPrev;
                }
                else
                {
                    // msg = "Error occured.Please try again.";
                    displayMessageLine(MessageUtils.getMessage(718), ERROR);
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

        private void frmTransferTo_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            //TimeOutTimer.Stop();
            KioskStatic.logToFile("frmTransferTokenTo_FormClosed");
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme
        
    }
}
