/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCheckBalance
* Description  - frmCheckBalance 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        4-Sep-2019      Deeksha             Added logger methods.
 *2.90.0       16-Aug-2020     Girish Kundar       Modified : Fix for UI displaying default 0000 before showing actual entitlements  amounts
 *2.150.1      22-Feb-2023     Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmCheckBalance : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        private readonly TagNumberParser tagNumberParser;
        Card card;
        //Timer ClearDisplayTimer;
        //double totSecs = 0;
        //double screenTimeout = 20;
        Font savTimeOutFont;
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        public frmCheckBalance(Card inCard)
        {
            log.LogMethodEntry(inCard);
            Utilities.setLanguage(this);
            InitializeComponent();

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme

            try
            {
                //screenTimeout = Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT"));
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing frmCheckBalance" + ex.Message);
                SetKioskTimerTickValue();
            }
            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);

            //totSecs = screenTimeout;
            //ClearDisplayTimer = new Timer();
            //ClearDisplayTimer.Interval = 1000;
            //ClearDisplayTimer.Tick += new EventHandler(ClearDisplayTimer_Tick);
            ResetKioskTimer();

            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();

            card = inCard;

            try//Starts:Modification on 17-Dec-2015 for introducing new theme
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                btnTopUp.BackgroundImage=btnActivities.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnHome.Size = btnHome.BackgroundImage.Size;
                flowLayoutpanel.BackgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceBox;
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
                KioskStatic.formatMessageLine(txtMessage, 23, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while introducing new theme" + ex.Message);
            }//Ends:Modification on 17-Dec-2015 for introducing new theme

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true); 
            log.LogMethodExit();
        }

        private void registerUSBDevice()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
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
                    log.LogMethodExit();
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
                    log.Error("Error occurred while executing CardScanCompleteEventHandle()" + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            // totSecs = 0;
            log.LogMethodEntry(inCardNumber, readerDevice);
            ResetKioskTimer();
            Card lclCard = null;
            string message;
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                try
                {
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing handleCardRead()" + ex.Message);
                }
                log.LogMethodExit();
                return;
            }
            else
                lclCard = new Card(readerDevice, inCardNumber, "External POS", KioskStatic.Utilities);

            if (lclCard.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    message = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    message = KioskStatic.Utilities.MessageUtils.getMessage(459);

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
                log.LogMethodExit();
                return;
            }

            card = lclCard;
            displayBalance();
            log.LogMethodExit();
        }

        void displayBalance()
        {
            log.LogMethodEntry();
            if (card.CardStatus != "NEW")
            {
                lblCredits.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblBonus.Text = (card.bonus + card.CreditPlusBonus).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblCourtesy.Text = card.courtesy.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblTickets.Text = (card.ticket_count + card.CreditPlusTickets).ToString(ParafaitEnv.NUMBER_FORMAT);
                lblTime.Text = (card.CreditPlusTime + card.time).ToString(ParafaitEnv.NUMBER_FORMAT);
                lblLoyaltyPoints.Text = (card.loyalty_points + card.TotalCreditPlusLoyaltyPoints).ToString(ParafaitEnv.AMOUNT_FORMAT) +"/"+ (card.loyalty_points + card.RedeemableCreditPlusLoyaltyPoints).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblCardGames.Text = Convert.ToInt32(Utilities.executeScalar("select isnull(sum(BalanceGames), 0) play_count " +
                                "from CardGames cg " +
                                "where card_id = @card_id " +
                                "and (expiryDate is null or expiryDate >= getdate()) " +
                                "and BalanceGames > 0",
                                new SqlParameter[] { new SqlParameter("@card_id", card.card_id) })).ToString(ParafaitEnv.NUMBER_FORMAT);

                string membership = (card.vip_customer == 'Y' ? (card.MembershipId == -1 ? KioskStatic.VIPTerm : KioskStatic.VIPTerm + " - " + card.MembershipName) : (card.MembershipId == -1 ? "" : card.MembershipName));
                if (membership != "")
                    membership = " [" + membership + "]";
                lblCardNumber.Text = MessageUtils.getMessage("Card Number") + ": " + card.CardNumber + membership;
                displayMore();
            }

            // ClearDisplayTimer.Start();
            StartKioskTimer();
            log.LogMethodExit();
        }

        void displayMore()
        {
            log.LogMethodEntry();
            string vipMessage = "";
            txtMessage.Text = "";
            if (card.customer_id != -1 && card.MembershipId != -1)
            {
                int nextLevelMembershipId = MembershipMasterList.GetNextLevelMembership(Utilities.ExecutionContext, card.MembershipId);
                if (nextLevelMembershipId != -1)
                {
                    DateTime runForDate = ServerDateTime.Now;
                    List<DateTime?> dateRange = MembershipMasterList.GetMembershipQualificationRange(Utilities.ExecutionContext, nextLevelMembershipId,runForDate);
                    if (dateRange != null)
                    {
                        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, card.customerDTO, true);
                        double loyaltyPoints = customerBL.GetLoyaltyPoints(dateRange[0], dateRange[1]);
                        vipMessage = MembershipMasterList.GetQualificationMessage(Utilities.ExecutionContext, nextLevelMembershipId, loyaltyPoints, runForDate);
                    }
                }
            }

            if (vipMessage == "" && card.vip_customer == 'N')
            {
                //if (card.credits_played >= ParafaitEnv.VIP_POS_ALERT_SPEND_THRESHOLD)
                {
                    double VIPSpendDiff = (double)ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS - card.credits_played;
                    if (VIPSpendDiff > 0)
                        vipMessage = MessageUtils.getMessage(300, VIPSpendDiff.ToString(ParafaitEnv.AMOUNT_FORMAT));
                    else if (ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                        vipMessage = MessageUtils.getMessage(298) + "; " + MessageUtils.getMessage(299);
               }
            }
            txtMessage.Text = vipMessage;
            log.LogMethodExit();
        }

        private void frmCheckBalance_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Utilities.setLanguage(this);
            lblSiteName.Text = KioskStatic.SiteHeading;
            txtMessage.Text = "";
            //    lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;//Modification on 17-Dec-2015 for introducing new theme

            lblPlayValueLabel.Text = MessageUtils.getMessage("Credits") + ":";
            lblGamesLabel.Text = MessageUtils.getMessage("Games") + ":";
            lblCourtesyLabel.Text = MessageUtils.getMessage("Courtesy") + ":";
            lblBonusLabel.Text = MessageUtils.getMessage("Bonus") + ":";
            lblTimeLabel.Text = MessageUtils.getMessage("Time") + ":";
            lblLoyaltyPointsLabel.Text = MessageUtils.getMessage("Loyalty Points") + ":";
            lbleTicketsLabel.Text = MessageUtils.getMessage("e-Tickets") + ":";


            panelBonus.Visible = KioskStatic.ShowBonus;
            panelCourtesy.Visible = KioskStatic.ShowCourtesy;
            panelGames.Visible = KioskStatic.ShowGames;
            panelTickets.Visible = KioskStatic.ShowTickets;
            panelLoyaltyPoints.Visible = KioskStatic.ShowLoyaltyPoints;
            panelTime.Visible = KioskStatic.ShowTime; 

            if (KioskStatic.DisablePurchase)
            {
                btnTopUp.Visible = false;
            }
            displayBalance();
            registerUSBDevice();

            

            this.Invalidate();
            this.Refresh();

            Audio.PlayAudio(Audio.GamePlayDetails);
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
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
                lblTimeOut.Font = savTimeOutFont;
                lblTimeOut.Text = "Time Out";
                Close();
            }
            else
            {
                lblTimeOut.Text = tickSecondsRemaining.ToString("#0");
            }
            log.LogMethodExit();
        }

        private void Dashboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if ((Keys)e.KeyChar == Keys.Escape)
                this.Close();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void frmCheckBalance_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CardReader.RequiredByOthers = false;            
            //ClearDisplayTimer.Stop();
        }       

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            //ClearDisplayTimer.Stop();
            //totSecs = screenTimeout;
            log.LogMethodEntry();
            StopKioskTimer();
            ResetKioskTimer();
            try
            {
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)
                    selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                else
                    selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
                kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETRECHAREGETYPE, selectedEntitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                //ClearDisplayTimer.Start();
                StartKioskTimer();
                this.Activate();
            }
            log.LogMethodExit();
        }

        private void btnActivities_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CustomerDashboard fCustomerDashboard = new CustomerDashboard(card);
            //ClearDisplayTimer.Stop();
            //totSecs = screenTimeout;
            StopKioskTimer();
            ResetKioskTimer();
            fCustomerDashboard.ShowDialog();
            //ClearDisplayTimer.Start();
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void lblCardGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            CardGames cg = new CardGames(card.card_id);
            cg.ShowDialog();                
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnTopUp_MouseDown(object sender, MouseEventArgs e)
        {
            // btnTopUp.BackgroundImage = Properties.Resources.top_up_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnActivities_MouseDown(object sender, MouseEventArgs e)
        {
            // btnActivities.BackgroundImage = Properties.Resources.top_up_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnRegisterNew_MouseDown(object sender, MouseEventArgs e)
        {
            // btnRegisterNew.BackgroundImage = Properties.Resources.top_up_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnTopUp_MouseUp(object sender, MouseEventArgs e)
        {
            //btnTopUp.BackgroundImage = Properties.Resources.top_up__activities_btn;//:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnActivities_MouseUp(object sender, MouseEventArgs e)
        {
            //btnActivities.BackgroundImage = Properties.Resources.top_up__activities_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnRegisterNew_MouseUp(object sender, MouseEventArgs e)
        {
            // btnRegisterNew.BackgroundImage = Properties.Resources.top_up__activities_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void frmCheckBalance_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.UnRegister();

            // ClearDisplayTimer.Stop();
            StopKioskTimer();
            log.LogMethodExit();
        }
    }
}
