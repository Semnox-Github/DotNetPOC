/********************************************************************************************
* Project Name - Check balance screen
* Description  - Balance checking screen
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.60.0       13-Mar-2018      Raghuveera        Created this class to display balances.
*2.70         1-Jul-2019       Lakshminarayana   Modified to add support for ULC cards 
*2.70.3       3-Sep-2019       Deeksha           Added logger methods.
*2.70.3       30-Jan-2020      Archana           Modified to append word 'Points' in front of the credits value
 *2.80.1     02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
*2.80.1       13-Nov-2020      Deeksha           Modified to support animation and button image changes
*2.90.0       16-Aug-2020      Girish Kundar     Modified : Fix for UI displaying default 0000 before showing actual entitlements  amounts
*2.130.0      30-Jun-2021      Dakshakh          Theme changes to support customized Font ForeColor
*2.150.0.0    13-Oct-2022      Sathyavathi       Mask card number
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmCheckBalance : BaseFormKiosk
    {
        private AccountDTO accountDTO;
        private Font savTimeOutFont;
        private string selectedEntitlementType;
        private readonly TagNumberParser tagNumberParser;

        public frmCheckBalance(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            this.accountDTO = accountDTO;
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);

            try
            {
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetKioskTimerTickValue();
            }
            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);

            ResetKioskTimer();

            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();
            if (accountDTO.RealTicketMode)
            {
                lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("Real Ticket");
            }
            else
            {
                lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("E-Ticket");
            }

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);
            KioskStatic.logToFile("Loading Check Balance form for "+ this.accountDTO.TagNumber);
            SetBackGroundImages();
            SetLabelText();
            ShowHidePanelsButtonsAndLabels();
            AdjustPanelLocation(); 
            log.LogMethodExit();
        }

        private void SetBackGroundImages()
        {
            log.LogMethodEntry();
            this.BackgroundImage = KioskStatic.CurrentTheme.BalanceBackgroundImage;
            if (KioskStatic.CurrentTheme.BalanceAnimationImage != null)
            {
                this.pBxbackGroundPicureBox.Image = KioskStatic.CurrentTheme.BalanceAnimationImage;
                this.pBxbackGroundPicureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                //no animation
                this.pBxbackGroundPicureBox.Visible = false;
                this.pnlbackGroundPanel.Visible = false;
            }
            if (KioskStatic.CurrentTheme.BalanceButtonImage != null)
            {
                this.btnPrev.BackgroundImage = KioskStatic.CurrentTheme.BalanceButtonImage;
                this.btnTopUp.BackgroundImage = KioskStatic.CurrentTheme.BalanceButtonImage;
                this.btnActivities.BackgroundImage = KioskStatic.CurrentTheme.BalanceButtonImage;
            }
            log.LogMethodExit();
        }

        private void SetLabelText()
        {
            log.LogMethodEntry();
            lblPlayValueLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Credits") + ":";
            lblGamesLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Games") + ":";
            lblCourtesyLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Courtesy") + ":";
            lblBonusLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bonus") + ":";
            lblTimeLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Time") + ":";
            lblLoyaltyPointsLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Loyalty Points") + ":";
            lbleTicketsLabel.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "e-Tickets") + ":";
            log.LogMethodExit();
        }
        private void ShowHidePanelsButtonsAndLabels()
        {
            log.LogMethodEntry();
            panelBonus.Visible = KioskStatic.ShowBonus;
            panelCourtesy.Visible = KioskStatic.ShowCourtesy;
            panelTime.Visible = KioskStatic.ShowTime;
            panelLoyaltyPoints.Visible = KioskStatic.ShowLoyaltyPoints;
            panelGames.Visible = KioskStatic.ShowGames;
            panelTickets.Visible = KioskStatic.ShowTickets;
            if (KioskStatic.DisablePurchase)
            {
                btnTopUp.Visible = false;
            }
            if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_CHANGE_TICKET_MODE").Equals("Y"))
            {
                btnChangeTicketMode.Visible = true;
                lblRealTicketMode.Visible = true;
                lblTicketMode.Visible = true;
            }
            else
            {
                btnChangeTicketMode.Visible = false;
                lblRealTicketMode.Visible = false;
                lblTicketMode.Visible = false;
            }
            log.LogMethodExit();
        }


        private void AdjustPanelLocation()
        {
            log.LogMethodEntry();
            int y = panelBonus.Location.Y;
            if (KioskStatic.ShowBonus)
            {
                y = y + panelBonus.Height + 2;
            }
            panelCourtesy.Location = new Point(panelCourtesy.Location.X, y);

            if (KioskStatic.ShowCourtesy)
            {
                y = y + panelCourtesy.Height + 2;
            }
            panelTime.Location = new Point(panelTime.Location.X, y);

            if (KioskStatic.ShowTime)
            {
                y = y + panelTime.Height + 2;
            }
            panelLoyaltyPoints.Location = new Point(panelLoyaltyPoints.Location.X, y);
            if (KioskStatic.ShowLoyaltyPoints)
            {
                y = y + panelLoyaltyPoints.Height + 2;
            }
            panelGames.Location = new Point(panelGames.Location.X, y);

            if (KioskStatic.ShowGames)
            {
                y = y + panelGames.Height + 2;
            }
            panelTickets.Location = new Point(panelTickets.Location.X, y);
            log.LogMethodExit();
        }

        private void RegisterUSBDevice()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardNumber = KioskStatic.ReverseTopupCardNumber(CardNumber);
                try
                {
                    HandleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }
            }
            log.LogMethodExit();
        }

        void HandleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            AccountBL accountBL;
            ResetKioskTimer();
            txtMessage.Text = "";
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
                catch (Exception ex) { log.Error(ex.Message, ex); }
                log.LogMethodExit();
                return;
            }
            else
                accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, inCardNumber, true, false);

            if (accountBL != null && accountBL.AccountDTO != null)
            {
                accountDTO = accountBL.AccountDTO;
            }
            if (accountBL.AccountDTO == null)
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
            DisplayBalance();
        }

        void DisplayBalance()
        {
            log.LogMethodEntry();
            try
            {
                if (accountDTO != null)
                {
                    lblCredits.Text = ((accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                    lblBonus.Text = ((accountDTO.AccountSummaryDTO.TotalBonusBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalBonusBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                    lblCourtesy.Text = ((accountDTO.AccountSummaryDTO.TotalCourtesyBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalCourtesyBalance).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                    lblTickets.Text = ((accountDTO.AccountSummaryDTO.TotalTicketsBalance == null) ? 0 : 
                        (int)accountDTO.AccountSummaryDTO.TotalTicketsBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT);
                    lblTime.Text = ((accountDTO.AccountSummaryDTO.TotalTimeBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalTimeBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT)
                        + (((accountDTO.AccountSummaryDTO.TotalTimeBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalTimeBalance) == 0 ? "" : " " );
                        //+ KioskStatic.Utilities.MessageUtils.getMessage("minutes"));
                    lblCardGames.Text = ((accountDTO.AccountSummaryDTO.TotalGamesBalance == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.AccountSummaryDTO.TotalGamesBalance).ToString(KioskStatic.Utilities.ParafaitEnv.NUMBER_FORMAT);

                    string membership = (accountDTO.VipCustomer ? (accountDTO.MembershipId == -1 ? KioskStatic.VIPTerm : KioskStatic.VIPTerm + " - " + accountDTO.MembershipName) : (accountDTO.MembershipId == -1 ? "" : accountDTO.MembershipName));
                    if (membership != "")
                        membership = " [" + membership + "]";
                    lblCardNumber.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Card Number") + ":";
                    lblCard.Text = KioskHelper.GetMaskedCardNumber(accountDTO.TagNumber) + membership;
                    KioskStatic.logToFile("TicketMode is " + accountDTO.RealTicketMode.ToString());
                    log.LogVariableState("TicketMode", accountDTO.RealTicketMode);
                    if (accountDTO.RealTicketMode)
                    {
                        lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Real Ticket");
                    }
                    else
                    {
                        lblRealTicketMode.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "E-Ticket");
                    }
                    lblLoyaltyPoints.Text = ((accountDTO.LoyaltyPoints == null) ? Convert.ToDecimal(0.0) : (decimal)accountDTO.LoyaltyPoints).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + "/" + ((decimal)accountDTO.LoyaltyPoints + (decimal)accountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT); 
                    DisplayMore();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            StartKioskTimer();
            log.LogMethodExit();
        }

        void DisplayMore()
        {
            log.LogMethodEntry();
            string vipMessage = "";
            txtMessage.Text = vipMessage;
            if (accountDTO != null && accountDTO.CustomerId != -1 && accountDTO.MembershipId != -1)
            {
                int nextLevelMembershipId = MembershipMasterList.GetNextLevelMembership(KioskStatic.Utilities.ExecutionContext, accountDTO.MembershipId);
                if (nextLevelMembershipId != -1)
                {
                    DateTime runForDate = ServerDateTime.Now;
                    List<DateTime?> dateRange = MembershipMasterList.GetMembershipQualificationRange(KioskStatic.Utilities.ExecutionContext, nextLevelMembershipId, runForDate);
                    if (dateRange != null)
                    {
                        CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, accountDTO.CustomerId, true);
                        double loyaltyPoints = customerBL.GetLoyaltyPoints(dateRange[0], dateRange[1]);
                        vipMessage = MembershipMasterList.GetQualificationMessage(KioskStatic.Utilities.ExecutionContext, nextLevelMembershipId, loyaltyPoints, runForDate);
                    }
                }
            }
            if (vipMessage == "" && accountDTO != null && accountDTO.VipCustomer)
            {

                double VIPSpendDiff = (double)KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS - (double)accountDTO.CreditsPlayed;
                if (VIPSpendDiff > 0)
                    vipMessage = KioskStatic.Utilities.MessageUtils.getMessage(300, VIPSpendDiff.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT));
                else if (KioskStatic.Utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                    vipMessage = KioskStatic.Utilities.MessageUtils.getMessage(298) + "; " + KioskStatic.Utilities.MessageUtils.getMessage(299);

            }
            txtMessage.Text = vipMessage;
            log.LogMethodExit();
        }

        private void frmCheckBalance_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Text = string.Empty;
            lblSiteName.Text = KioskStatic.SiteHeading;
            KioskStatic.formatMessageLine(txtMessage, 26, Properties.Resources.bottom_bar);

            //txtMessage.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));


            DisplayBalance();
            RegisterUSBDevice();

            this.Invalidate();
            this.Refresh();
            Audio.PlayAudio(Audio.GamePlayDetails);
            SetCustomizedFontColors();

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

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            ResetKioskTimer();
            KioskStatic.logToFile("Top-up click");
            if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)
                selectedEntitlementType = "Time";
            else
                selectedEntitlementType = "Credits";

            if (!KioskStatic.receipt && !KioskStatic.Utilities.getParafaitDefaults("IGNORE_PRINTER_ERROR").Equals("Y"))
            {
                using (frmYesNo frmyn = new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(461)))
                {
                    if (frmyn.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                    {
                        frmyn.Dispose();
                        log.LogMethodExit();
                        return;
                    }
                    frmyn.Dispose();
                }
            }
            try
            {
                DataTable dTable = KioskStatic.GetProductDisplayGroups("R", selectedEntitlementType);
                if (dTable != null && dTable.Rows.Count > 1)
                {
                    using (frmChooseDisplayGroup frmDisp = new frmChooseDisplayGroup("R", selectedEntitlementType))
                    {
                        if (frmDisp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            Close();
                        else
                            StartKioskTimer();
                    }

                }
                else
                {
                    if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                    {
                        if (KioskHelper.isTimeEnabledStore() == true)
                        {
                            selectedEntitlementType = "Time";
                        }
                        else
                        {
                            using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                            {
                                if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    frmEntitle.Dispose();
                                    log.LogMethodExit();
                                    return;
                                }
                                selectedEntitlementType = frmEntitle.selectedEntitlement;
                                frmEntitle.Dispose();
                            }
                        }
                    }
                    string backgroundImageFileName = dTable.Rows[0]["BackgroundImageFileName"].ToString().Trim();
                    using (frmChooseProduct frm = new frmChooseProduct("R", selectedEntitlementType, "ALL", backgroundImageFileName)) 
                    {
                        if (frm.IsDisposed != true)
                        {
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                                Close();
                            else
                                StartKioskTimer();
                        }
                        else
                        {
                            StartKioskTimer();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }
        private void btnActivities_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Activities pressed");
            if (accountDTO != null)
            {
                StopKioskTimer();
                ResetKioskTimer();
                using (CustomerDashboard fCustomerDashboard = new CustomerDashboard(accountDTO))
                {                    
                    fCustomerDashboard.ShowDialog();
                }
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void lblCardGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Card Games link pressed");
            using (CardGames cg = new CardGames(accountDTO.AccountId))
            {
                cg.ShowDialog();
            }
            log.LogMethodExit();
        }
            

        private void frmCheckBalance_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.UnRegister();
            StopKioskTimer();
            log.LogMethodExit();
        }

        private void btnChangeTicketMode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Change Ticket mode pressed");
            StopKioskTimer();
            if (accountDTO != null)
            {
                using (frmTicketType frmTicket = new frmTicketType(KioskStatic.Utilities, accountDTO))
                {
                    if (frmTicket.ShowDialog() == DialogResult.OK)
                    {
                        if (accountDTO.RealTicketMode)
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
            StartKioskTimer();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.button1.ForeColor = KioskStatic.CurrentTheme.CheckBalanceHeader1TextForeColor;//(Header1)#CheckBalanceHeader1ForeColor
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.CheckBalanceCardNumerHeaderTextForeColor;//(Card number header) -#CheckBalanceCardNumerHeaderForeColor
                this.lblCard.ForeColor = KioskStatic.CurrentTheme.CheckBalanceCardNumberInfoTextForeColor;//(Card Number Info) -#CheckBalanceCardNumberInfoForeColor
                this.lblTicketMode.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTicketModeHeaderTextForeColor;//(Ticket Mode header) -#CheckBalanceTicketModeHeaderForeColor
                this.lblRealTicketMode.ForeColor = KioskStatic.CurrentTheme.CheckBalanceTicketModeInfoTextForeColor;//(Ticker mode info) -#CheckBalanceTicketModeInfoForeColor
                this.btnChangeTicketMode.ForeColor = KioskStatic.CurrentTheme.CheckBalanceBtnChangeTextForeColor;//(Change ticket mode Button) -#CheckBalanceBtnChangeForeColor
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
