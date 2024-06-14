/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCheckBalance
* Description  - frmCheckBalance 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using System.Linq;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmCheckBalance : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        private readonly TagNumberParser tagNumberParser;

        private AccountDTO card;
        private Font savTimeOutFont;
        private string selectedEntitlementType;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;

        public frmCheckBalance(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            InitializeComponent();
            Utilities.setLanguage();
            KioskStatic.setDefaultFont(this);///Modification on 17-Dec-2015 for introducing new theme

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            try
            {
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmCheckBalance()", ex);
                SetKioskTimerTickValue();
            }
            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);

            ResetKioskTimer();

            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();

            card = accountDTO;
            SetButtonVisibility();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetBackGroundImages();
            DisplaybtnPrev(true);
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void SetButtonVisibility()
        {
            log.LogMethodEntry();
            bool hideRechargeButton = (KioskStatic.DisablePurchase || ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "DISABLE_RECHARGE", false));
            if (hideRechargeButton == true || KioskStatic.EnableWaiverSignInKiosk == false)
            {
                this.btnPrev.Size = new System.Drawing.Size(250, 125);
                this.btnTopUp.Size = new System.Drawing.Size(250, 125);
                this.btnActivities.Size = new System.Drawing.Size(250, 125);
                this.btnViewSignedWaivers.Size = new System.Drawing.Size(250, 125);
                this.btnPrev.Font = new System.Drawing.Font(this.btnPrev.Font.FontFamily, 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnTopUp.Font = new System.Drawing.Font(this.btnTopUp.Font.FontFamily, 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnActivities.Font = new System.Drawing.Font(this.btnActivities.Font.FontFamily, 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnViewSignedWaivers.Font = new System.Drawing.Font(this.btnViewSignedWaivers.Font.FontFamily, 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            if (hideRechargeButton && KioskStatic.EnableWaiverSignInKiosk == false)
            {
                int initialButtonLocationX = 670;
                int seperator = 50;
                this.btnViewSignedWaivers.Visible = false;
                this.btnTopUp.Visible = false;
                this.btnPrev.Font = new System.Drawing.Font(this.btnPrev.Font.FontFamily, 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnPrev.Location = new Point(initialButtonLocationX, this.btnPrev.Location.Y);
                this.btnActivities.Font = new System.Drawing.Font(this.btnActivities.Font.FontFamily, 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnActivities.Location = new Point(this.btnPrev.Location.X + this.btnPrev.Width + seperator, this.btnActivities.Location.Y);
            }
            else if (KioskStatic.EnableWaiverSignInKiosk == false)
            {
                int seperator = 50;
                int initialButtonLocationX = 520;
                this.btnViewSignedWaivers.Visible = false;
                this.btnPrev.Location = new System.Drawing.Point(initialButtonLocationX, this.btnPrev.Location.Y);
                this.btnTopUp.Location = new System.Drawing.Point(this.btnPrev.Location.X + this.btnPrev.Width + seperator, this.btnTopUp.Location.Y);
                this.btnActivities.Location = new System.Drawing.Point(this.btnTopUp.Location.X + this.btnTopUp.Width + seperator, this.btnActivities.Location.Y);
            }
            else if (hideRechargeButton)
            {
                int seperator = 50;
                int initialButtonLocationX = 520;
                this.btnTopUp.Visible = false;
                this.btnPrev.Location = new Point(initialButtonLocationX, this.btnPrev.Location.Y);
                this.btnActivities.Location = new Point(this.btnPrev.Location.X + this.btnPrev.Width + seperator, this.btnActivities.Location.Y);
                this.btnViewSignedWaivers.Location = new Point(this.btnActivities.Location.X + this.btnActivities.Width + seperator, this.btnViewSignedWaivers.Location.Y);
            }
            log.LogMethodExit();
        }

        private void SetBackGroundImages()
        {
            log.LogMethodEntry();
            try
            {
                btnTopUp.BackgroundImage = btnActivities.BackgroundImage = btnPrev.BackgroundImage = btnViewSignedWaivers.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnChangeTicketMode.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.ChangeButtonImage);
                btnChangeTicketMode.Size = (btnChangeTicketMode.Size.Height > 61) ? this.btnChangeTicketMode.Size : btnChangeTicketMode.BackgroundImage.Size;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                flowLayoutpanel.BackgroundImage = ThemeManager.CurrentThemeImages.CheckBalanceBox;
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;//Ends:Modification on 17-Dec-2015 for introducing new theme
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.BalanceBackgroundImage);
                if (ThemeManager.CurrentThemeImages.BalanceAnimationImage != null)
                {
                    this.pBxbackGroundPicureBox.Image = ThemeManager.CurrentThemeImages.BalanceAnimationImage;
                    this.pBxbackGroundPicureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    //no animation
                    this.pBxbackGroundPicureBox.Visible = false;
                    this.pnlbackGroundPanel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmCheckBalance()", ex);
            }
            log.LogMethodExit();
        }
        private void registerUSBDevice()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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
                        log.LogVariableState("Decrypted Tag Number result: ", ex.Message);
                        log.LogMethodExit(null, "Invalid Tag Number. " + ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        log.LogMethodExit(null, "Invalid Tag Number. " + ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        log.LogMethodExit(null, "Invalid Tag Number. " + ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
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
                    log.Error("Error occurred while executing handleCardRead()", ex);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            ResetKioskTimer();
            AccountBL accountBL;
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
                    log.Error("Error occurred while executing handleCardRead()", ex);
                }

                log.LogMethodExit();
                return;
            }
            else
            {
                accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, inCardNumber, true, false);
                log.LogVariableState("AccountDTO", accountBL.AccountDTO);
            }
            if (accountBL == null || accountBL.AccountDTO == null)
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    message = KioskStatic.Utilities.MessageUtils.getMessage(528);
                else
                    message = KioskStatic.Utilities.MessageUtils.getMessage(459);

                txtMessage.Text = message;
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
                log.LogMethodExit("NEW card tapped. Rejected.");
                return;
            }
            if (accountBL != null && accountBL.AccountDTO != null)
            {
                card = accountBL.AccountDTO;
            }
            DisplayBalance();
            log.LogMethodExit();
        }

        void DisplayBalance()
        {
            log.LogMethodEntry();
            if (card != null)
            {
                lblCredits.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalGamePlayCreditsBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalGamePlayCreditsBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblBonus.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalBonusBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalBonusBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblCourtesy.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalCourtesyBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalCourtesyBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblTickets.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalTicketsBalance == null) ? 0 :
                    (int)card.AccountSummaryDTO.TotalTicketsBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT);
                lblTime.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalTimeBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalTimeBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT)
                    + (((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalTimeBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalTimeBalance) == 0 ? "" : " ");
                lblCardGames.Text = ((card.AccountSummaryDTO != null && card.AccountSummaryDTO.TotalGamesBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.AccountSummaryDTO.TotalGamesBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT);

                string membership = (card.VipCustomer ? (card.MembershipId == -1 ? KioskStatic.VIPTerm : KioskStatic.VIPTerm + " - " + card.MembershipName) : (card.MembershipId == -1 ? "" : card.MembershipName));
                if (membership != "")
                    membership = " [" + membership + "]";
                lblCardNumber.Text = MessageContainerList.GetMessage(executionContext, "Card Number") + ": " + KioskHelper.GetMaskedCardNumber(card.TagNumber) + membership;
                KioskStatic.logToFile("TicketMode is " + card.RealTicketMode.ToString());
                log.LogVariableState("TicketMode", card.RealTicketMode);
                if (card.RealTicketMode)
                {
                    lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Real Ticket");
                }
                else
                {
                    lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "E-Ticket");
                }

                string availableLoyaltyPoints = ((card.AccountSummaryDTO != null && card.LoyaltyPoints == null) ? Convert.ToDecimal(0.0) :
                    (decimal)card.LoyaltyPoints).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);

                string redeemableCreditPlusLoyaltyPoints = (((card.AccountSummaryDTO != null && card.LoyaltyPoints == null) ? Convert.ToDecimal(0.0) :
                    (decimal)card.LoyaltyPoints) + (decimal)card.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints)
                    .ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);

                lblLoyaltyPoints.Text = availableLoyaltyPoints + "/" + redeemableCreditPlusLoyaltyPoints;

                lblVirtualPoints.Text = ((card.AccountSummaryDTO != null && card.TotalVirtualPointBalance == null) ? Convert.ToDecimal(0.0) : (decimal)card.TotalVirtualPointBalance).ToString(ParafaitEnv.AMOUNT_FORMAT);
                DisplayMore();
            }
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void DisplayMore()
        {
            log.LogMethodEntry();
            string vipMessage = "";
            txtMessage.Text = vipMessage;

            if (card != null && card.CustomerId != -1 && card.MembershipId != -1)
            {
                int nextLevelMembershipId = MembershipMasterList.GetNextLevelMembership(KioskStatic.Utilities.ExecutionContext, card.MembershipId);
                if (nextLevelMembershipId != -1)
                {
                    DateTime runForDate = ServerDateTime.Now;
                    List<DateTime?> dateRange = MembershipMasterList.GetMembershipQualificationRange(KioskStatic.Utilities.ExecutionContext, nextLevelMembershipId, runForDate);
                    if (dateRange != null)
                    {
                        CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, card.CustomerId, true);
                        double loyaltyPoints = customerBL.GetLoyaltyPoints(dateRange[0], dateRange[1]);
                        vipMessage = MembershipMasterList.GetQualificationMessage(KioskStatic.Utilities.ExecutionContext, nextLevelMembershipId, loyaltyPoints, runForDate);
                    }
                }
            }
            if (vipMessage == "" && card != null && card.VipCustomer)
            {

                double VIPSpendDiff = (double)KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS - (double)card.CreditsPlayed;
                if (VIPSpendDiff > 0)
                {
                    vipMessage = KioskStatic.Utilities.MessageUtils.getMessage(300, VIPSpendDiff.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                }
                else if (KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                {
                    if (card.CustomerId == -1)
                    {
                        vipMessage = KioskStatic.Utilities.MessageUtils.getMessage(298) + "; " + KioskStatic.Utilities.MessageUtils.getMessage(299);
                    }
                    else
                    {
                        vipMessage = KioskStatic.Utilities.MessageUtils.getMessage(298);
                    }
                }
            }
            txtMessage.Text = vipMessage;
            log.LogMethodExit();
        }

        private void frmCheckBalance_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtMessage.Text = "";
            SetLabelText();
            ShowHidePanelsButtonsAndLabels();
            DisplayBalance();
            registerUSBDevice();

            this.Invalidate();
            this.Refresh();
            Audio.PlayAudio(Audio.GamePlayDetails);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        private void SetLabelText()
        {
            log.LogMethodEntry();
            lblPlayValueLabel.Text = MessageContainerList.GetMessage(executionContext, "Credits") + ":";
            lblGamesLabel.Text = MessageContainerList.GetMessage(executionContext, "Games") + ":";
            lblCourtesyLabel.Text = MessageContainerList.GetMessage(executionContext, "Courtesy") + ":";
            lblBonusLabel.Text = MessageContainerList.GetMessage(executionContext, "Bonus") + ":";
            lblTimeLabel.Text = MessageContainerList.GetMessage(executionContext, "Time") + ":";
            lblLoyaltyPointsLabel.Text = MessageContainerList.GetMessage(executionContext, "Loyalty Points") + ":";
            lbleTicketsLabel.Text = MessageContainerList.GetMessage(executionContext, "e-Tickets") + ":";
            lblVirtualPointsLabel.Text = MessageContainerList.GetMessage(executionContext, "Virtual Points") + ":";
            log.LogMethodExit();
        }

        private void ShowHidePanelsButtonsAndLabels()
        {
            log.LogMethodEntry();
            bool showTicketModeInActivity = false;
            panelBonus.Visible = KioskStatic.ShowBonus;
            panelCourtesy.Visible = KioskStatic.ShowCourtesy;
            panelGames.Visible = KioskStatic.ShowGames;
            panelTickets.Visible = KioskStatic.ShowTickets;
            panelLoyaltyPoints.Visible = KioskStatic.ShowLoyaltyPoints;
            panelTime.Visible = KioskStatic.ShowTime;
            panelVirtualPoints.Visible = KioskStatic.ShowVirtualPoints;
            try
            {
                this.flowLayoutpanel.SuspendLayout();
                panelTicket.SuspendLayout();
                showTicketModeInActivity = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_TICKET_MODE_IN_CARD_ACTIVITY", false);
                panelTicket.Visible = showTicketModeInActivity;
            }
            finally
            {
                this.flowLayoutpanel.ResumeLayout();
                panelTicket.ResumeLayout(true);
            }

            //if (KioskStatic.DisablePurchase || ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "DISABLE_RECHARGE", false))
            //{
            //    btnTopUp.Visible = false;
            //}
            if (showTicketModeInActivity)
            {
                if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_CHANGE_TICKET_MODE").Equals("Y"))
                {
                    btnChangeTicketMode.Visible = true;
                    lblRealTicketMode.Visible = true;
                    lblTicketMode.Visible = true;
                    this.panelTicket.Location = new System.Drawing.Point(this.panelTicket.Location.X - 31, this.panelTicket.Location.Y);
                    this.lblRealTicketMode.Size = new Size(this.lblRealTicketMode.Width - 190, this.lblRealTicketMode.Height);
                    this.lblRealTicketMode.Location = new Point(this.lblRealTicketMode.Location.X - 7, this.lblRealTicketMode.Location.Y);
                    this.btnChangeTicketMode.Location = new Point(this.btnChangeTicketMode.Location.X - 205, this.btnChangeTicketMode.Location.Y);
                    this.lblCardNumber.Location = new Point(this.lblCardNumber.Location.X - 20, this.lblCardNumber.Location.Y);
                }
                else
                {
                    btnChangeTicketMode.Visible = false;
                }
            }
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
            { this.Close(); }
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
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
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
                try
                {
                    kioskTransaction = AddFundraiserOrDonationProduct(kioskTransaction);
                }
                catch (ArgumentNullException)
                {
                    log.LogMethodExit("timeout");
                    return;
                }
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
                ReloadData();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnRecharge_Click()", ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                ClearTransaction();
                StartKioskTimer();
                this.Activate();
            }
            log.LogMethodExit();
        }

        private KioskTransaction AddFundraiserOrDonationProduct(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = GetFundraiserOrDonationProducts(kioskTransaction);
            kioskTransaction = frmPaymentMode.AddFundRaiserOrDonationProducts(kioskTransaction, selectedFundsAndDonationsList);
            log.LogMethodExit();
            return kioskTransaction;
        }

        private static List<KeyValuePair<string, ProductsDTO>> GetFundraiserOrDonationProducts(KioskTransaction kioskTransaction)
        {
            List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = new List<KeyValuePair<string, ProductsDTO>>();
            selectedFundsAndDonationsList = frmPaymentMode.GetSelectedFundsAndDonationsList(kioskTransaction);
            return selectedFundsAndDonationsList;
        }

        private void btnActivities_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Card cardObj = new Card(card.AccountId, "External POS", KioskStatic.Utilities);
                using (CustomerDashboard fCustomerDashboard = new CustomerDashboard(cardObj))
                {
                    StopKioskTimer();
                    ResetKioskTimer();
                    fCustomerDashboard.ShowDialog();
                }
                ReloadData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void ReloadData()
        {
            log.LogMethodEntry();
            AccountBL acctBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, card.TagNumber, true, false);
            card = acctBL.AccountDTO;
            DisplayBalance();
            log.LogMethodExit();
        }

        private void lblCardGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                CardGames cg = new CardGames(card.AccountId);
                cg.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
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
            //btnTopUp.BackgroundImage = Properties.Resources.top_up__activities_btn;//Modification on 17-Dec-2015 for introducing new theme
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
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }

            StopKioskTimer();
            KioskStatic.logToFile(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void ShowSignedWaivers()
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                if (card != null && card.CustomerId > -1)
                {
                    CustomerListBL customerBL = new CustomerListBL(KioskStatic.Utilities.ExecutionContext);
                    List<int> idList = new List<int>() { card.CustomerId };
                    List<CustomerDTO> custDTOList = customerBL.GetCustomerDTOList(idList, true, true, true, null, false, this.Utilities);
                    if (custDTOList != null && custDTOList.Any())
                    {
                        txtMessage.Text = string.Empty;
                        using (frmCustomerSignedWaivers frmCSW = new frmCustomerSignedWaivers(custDTOList[0]))
                        {
                            frmCSW.ShowDialog();
                        }
                    }
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2157);
                    KioskStatic.logToFile(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2157));
                }
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        private void btnViewSignedWaivers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 1008);
                //Processing..Please wait...
                KioskStatic.logToFile("View signed waiver button is clicked");
                ShowSignedWaivers();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                KioskStatic.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frm check balance");
            try
            {
                this.button1.ForeColor = KioskStatic.CurrentTheme.CheckBalanceHeader1TextForeColor;//(Header1)#CheckBalanceHeader1ForeColor
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.CheckBalanceCardNumerHeaderTextForeColor;//(Card number header) -#CheckBalanceCardNumerHeaderForeColor
                this.lblPlayValueLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalancePlayValueHeaderTextForeColor;//(Play value header) -#CheckBalancePlayValueHeaderForeColor
                this.lblCredits.ForeColor = KioskStatic.CurrentTheme.CheckBalancePlayValueInfoTextForeColor;//(Play value info) -#CheckBalancePlayValueInfoForeColor
                this.lblBonusLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBonusHeaderTextForeColor;//(Bonus Label Header) -#CheckBalanceBonusHeaderForeColor
                this.lblBonus.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBonusInfoTextForeColor;//(Bonus info) -#CheckBalanceBonusInfoForeColor
                this.lblCourtesyLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceCourtesyHeaderTextForeColor;//(Courtesy Header) -#CheckBalanceCourtesyHeaderForeColor
                this.lblCourtesy.ForeColor = KioskStatic.CurrentTheme.CheckBalanceCourtesyInfoTextForeColor;//(Courtesy info) -#CheckBalanceCourtesyInfoForeColor
                this.lblTimeLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTimeHeaderTextForeColor;//(Time header)-#CheckBalanceTimeHeaderForeColor
                this.lblTime.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTimeInfoTextForeColor;//(Time info) -#CheckBalanceTimeInfoForeColor
                this.lblLoyaltyPointsLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceLPHeaderTextForeColor;//(Loyalty points header) -#CheckBalanceLPHeaderForeColor
                this.lblLoyaltyPoints.ForeColor = KioskStatic.CurrentTheme.CheckBalanceLPInfoTextForeColor;//lblLoyaltyPoints(Loyaltry p info) -#CheckBalanceLPInfoForeColor
                this.lblGamesLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceGameHeaderTextForeColor; //(Game header) -#CheckBalanceGameHeaderForeColor
                this.lblCardGames.ForeColor = KioskStatic.CurrentTheme.CheckBalanceGameInfoTextForeColor; //(Game info) -#CheckBalanceGameInfoForeColor
                this.lbleTicketsLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceETicketHeaderTextForeColor; //(E ticket header) -#CheckBalanceETicketHeaderForeColor
                this.lblTickets.ForeColor = KioskStatic.CurrentTheme.CheckBalanceETicketInfoTextForeColor; //(ticket info) -#CheckBalanceETicketInfoForeColor
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBackButtonTextForeColor; //(back button) -#CheckBalanceBackButtonForeColor
                this.btnTopUp.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTopUpButtonTextForeColor; //(Topup button) -#CheckBalanceTopUpButtonForeColor
                this.btnActivities.ForeColor = KioskStatic.CurrentTheme.CheckBalanceActivityButtonTextForeColor; //(Activitiy button) -#CheckBalanceActivityButtonForeColor
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CheckBalanceFooterTextForeColor; //(Footer message) -#CheckBalanceFooterForeColor
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTimeOutTextForeColor; //(Time out) -#CheckBalanceTimeOutTextForeColor
                this.btnViewSignedWaivers.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBtnViewSignedWaiversTextForeColor; //(Time out) -#CheckBalanceTimeOutTextForeColor
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBtnHomeTextForeColor; //(Time out) -#CheckBalanceTimeOutTextForeColor
                                                                                                    // Virtual Point changes  - Girish
                this.lblVirtualPoints.ForeColor = KioskStatic.CurrentTheme.CheckBalanceVirtualPointInfoTextForeColor;
                this.lblVirtualPointsLabel.ForeColor = KioskStatic.CurrentTheme.CheckBalanceVirtualPointHeaderTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frm check balance: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }
        private void ClearTransaction()
        {
            log.LogMethodEntry();
            try
            {
                if (kioskTransaction != null)
                {
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    kioskTransaction = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnChangeTicketMode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Change Ticket mode pressed");
                StopKioskTimer();
                if (card != null)
                {
                    using (frmTicketType frmTicket = new frmTicketType(KioskStatic.Utilities, card))
                    {
                        if (frmTicket.ShowDialog() == DialogResult.OK)
                        {
                            if (card.RealTicketMode)
                            {
                                lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Real Ticket");
                            }
                            else
                            {
                                lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "E-Ticket");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
    }
}
