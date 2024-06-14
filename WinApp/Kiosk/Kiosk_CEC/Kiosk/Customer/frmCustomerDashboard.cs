/********************************************************************************************
 * Project Name - customer dashboard
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *1.00                                            Created 
 *2.4.0       13-Mar-2019     Raghuveera          Card changed to account and screen design changed[top up and register button is hidden]
 *2.70         1-Jul-2019     Lakshminarayana     Modified to add support for ULC cards 
 *2.70.3      08-Aug-2020     Guru S A            Fix kiosk crash due to large data in card activity/game play 
 *2.80.1      02-Feb-2021     Deeksha             Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021     Dakshakh            Theme changes to support customized Font ForeColor
 *2.150.0.0   13-Oct-2022     Sathyavathi         Mask card number
 ********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using Semnox.Parafait.Game;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System.Data.SqlClient;

namespace Parafait_Kiosk
{
    public partial class CustomerDashboard : BaseFormKiosk
    {
        private Font savTimeOutFont;
        private bool isCardRealticket = false;
        private AccountDTO accountDTO;
        private string selectedEntitlementType = "Credits";
        private readonly TagNumberParser tagNumberParser;
        private int showXRecordsInCustomerDashboard = 1000;

        public CustomerDashboard(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            if (KioskStatic.ShowTickets)
            {
                lblTicketsCourtesy.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Tickets");
            }
            else
            {
                lblTicketsCourtesy.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Courtesy");
            }
            KioskStatic.Utilities.setLanguage(this);
            try
            {
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch (Exception ex)
            {
                SetKioskTimerTickValue();
                log.Error(ex);
            }

            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);

            ResetKioskTimer();
            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();
            this.accountDTO = accountDTO;
            KioskStatic.setDefaultFont(this);
            this.BackgroundImage = KioskStatic.CurrentTheme.BalanceBackgroundImage;

            SetCustomizedFontColors();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (KioskStatic.Utilities.getParafaitDefaults("ALLOW_CHANGE_TICKET_MODE").Equals("Y"))
            {
                btnChangeTicketMode.Visible = true;
            }
            else
            {
                btnChangeTicketMode.Visible = false;
            }
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }



        private void RegisterUSBDevice()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
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
                catch (Exception ex) { log.Error(ex); }
            }
            log.LogMethodExit();
        }

        void HandleCardRead(string inCardNumber, DeviceClass readerDevice)
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
                    log.Debug(ex.ToString());
                }
                log.LogMethodExit();
                return;
            }
            else
                accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, inCardNumber, true, false);
            if (accountBL.AccountDTO != null)
            {
                accountDTO = accountBL.AccountDTO;
            }
            if (accountBL.AccountDTO == null)
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
                log.LogMethodExit("NEW card tapped.Rejected.");
                return;
            }
            DisplayBalance();
            log.LogMethodExit();
        }

        void DisplayBalance()
        {
            log.LogMethodEntry();
            try
            {
                if (accountDTO != null)
                {
                    string membership = (accountDTO.VipCustomer ? (accountDTO.MembershipId == -1 ? KioskStatic.VIPTerm : KioskStatic.VIPTerm + " - " + accountDTO.MembershipName) : (accountDTO.MembershipId == -1 ? "" : accountDTO.MembershipName));
                    if (membership != "")
                        membership = " [" + membership + "]";
                    lblCardNumber.Text = KioskStatic.Utilities.MessageUtils.getMessage("Card Number") + ": ";
                    lblCard.Text = KioskHelper.GetMaskedCardNumber(accountDTO.TagNumber) + membership;
                    TaskProcs tp = new TaskProcs(KioskStatic.Utilities);
                    if (tp != null)
                        tp = null;
                }
                FetchTrxDetails(accountDTO.AccountId);
                if (accountDTO.RealTicketMode)
                {
                    lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("Real Ticket");
                }
                else
                {
                    lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("E-Ticket");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in DisplayBalance: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.SuspendLayout();
                showXRecordsInCustomerDashboard = KioskHelper.GetShowXRecordsInCustomerDashboard(KioskStatic.Utilities.ExecutionContext);
                grpActivities.Visible = true;
                //dgvPurchases.Columns.Clear();
                dgvGamePlay.Columns.Clear();
                dgvPurchases.AllowUserToAddRows = dgvGamePlay.AllowUserToAddRows = false;

                lblSiteName.Text = KioskStatic.SiteHeading;
                if (KioskStatic.DisablePurchase)
                {
                    btnTopUp.Visible = false;
                }

                try
                {
                    if ((KioskStatic.Utilities.getParafaitDefaults("REGISTRATION_ALLOWED").Equals("N")) || (KioskStatic.Utilities.getParafaitDefaults("DISABLE_CUSTOMER_REGISTRATION").Equals("Y")))
                    {
                        btnRegisterNew.Visible = false;
                    }
                }
                catch (Exception ex) { log.Error(ex); }

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                {
                    panelPurchase.Top = panelGameplay.Top;
                    panelGameplay.Visible = false; //Added on 29-May-2015 to hide gameplay on mifare rw environment
                }

                RegisterUSBDevice();

                DisplayBalance();

                this.Invalidate();
            }
            finally
            {
                this.ResumeLayout();
            }
            //this.Refresh(); // Commented to eliminate the flickering issue. Added Suspend and Resume instead.

            Audio.PlayAudio(Audio.GamePlayDetails);
            displaybtnCancel(true);
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                lblTimeOut.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Time Out");
                Close();
            }
            else
            {
                lblTimeOut.Text = tickSecondsRemaining.ToString("#0");
            }
            log.LogMethodExit();
        }

        private SortableBindingList<AccountActivityDTO> GetAccountActivityDTOList(int accountId)
        {
            log.LogMethodEntry(accountId);
            SortableBindingList<AccountActivityDTO> accountActivityDTOSortableBindingList;
            List<AccountActivityDTO> accountActivityDTOList = null;
            AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(KioskStatic.Utilities.ExecutionContext);
            List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
            accountActivityDTOList = accountActivityViewListBL.GetAccountActivityDTOList(searchByParameters, false, null, showXRecordsInCustomerDashboard, 0);
            if (accountActivityDTOList == null)
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>();
            }
            else
            {
                accountActivityDTOSortableBindingList = new SortableBindingList<AccountActivityDTO>(accountActivityDTOList);
            }
            log.LogMethodExit(accountActivityDTOSortableBindingList);
            return accountActivityDTOSortableBindingList;
        }

        private SortableBindingList<GamePlayDTO> GetGamePlayDTOList(int accountId, bool detailed)
        {
            log.LogMethodEntry(accountId, detailed);
            SortableBindingList<GamePlayDTO> gamePlayDTOSortableBindingList;
            List<GamePlayDTO> gamePlayDTOList = null;
            GamePlaySummaryListBL accountGameMetricViewListBL = new GamePlaySummaryListBL(KioskStatic.Utilities.ExecutionContext);
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
            gamePlayDTOList = accountGameMetricViewListBL.GetGamePlayDTOList(searchByParameters, false, detailed, null, showXRecordsInCustomerDashboard, 0);
            if (gamePlayDTOList == null)
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>();
            }
            else
            {
                gamePlayDTOSortableBindingList = new SortableBindingList<GamePlayDTO>(gamePlayDTOList);
            }
            log.LogMethodExit(gamePlayDTOSortableBindingList);
            return gamePlayDTOSortableBindingList;
        }

        private void FetchTrxDetails(int accountId)
        {
            log.LogMethodEntry(accountId);
            if (accountDTO != null)
            {
                SortableBindingList<AccountActivityDTO> accountActivityDTOList = GetAccountActivityDTOList(accountDTO.AccountId);
                if (accountActivityDTOList == null)
                {
                    accountActivityDTOList = new SortableBindingList<AccountActivityDTO>();
                }

                accountActivityDTOBindingSource.DataSource = accountActivityDTOList;
                dgvPurchases.DataSource = accountActivityDTOBindingSource;
                KioskStatic.Utilities.setLanguage(dgvPurchases);

                int[] colWidth = { 220, 200, 169, 200, 152 };//Modification on 21-Dec-2015 for displaying time in purchase 
                int i = 0;
                foreach (DataGridViewColumn dc in dgvPurchases.Columns)
                {
                    if (dc.Visible)
                        dc.Width = colWidth[i++];
                }

                dgvPurchases.Refresh();
                if (dgvPurchases.SelectedCells.Count > 0)
                    dgvPurchases.SelectedCells[0].Selected = false;

                vScrollBarPur.Maximum = dgvPurchases.Rows.Count + 1;

                if (!KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                {
                    object eaterTickets = null;
                    SqlCommand GamePlayCmd = KioskStatic.Utilities.getCommand();
                    if (ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "SHOW_GAME_NAME_IN_GAME_MANAGEMENT").Equals("Y"))
                    {
                        GamePlayCmd.CommandText = @"SELECT top (@topRecordCount) Date, 
                                                       Machine + ' - ' + Game as [Machine], 
                                                       credits as [Game Price(Credits)], 
                                                       bonus as [Game Price(Bonus)], 
                                                       Tickets, [T.Eater Tickets], 
                                                       Courtesy
                                                  FROM GameMetricViewForDisplay
                                                 WHERE card_id = @card_id
                                                 order by date desc";
                    }
                    else
                    {
                        GamePlayCmd.CommandText = @"SELECT top (@topRecordCount) Date, 
                                                       Machine, credits as [Game Price(Credits)], 
                                                       bonus as [Game Price(Bonus)], 
                                                       Tickets, [T.Eater Tickets], 
                                                       Courtesy
                                                  FROM GameMetricViewForDisplay
                                                 WHERE card_id = @card_id
                                                 order by date desc";
                    }

                    GamePlayCmd.Parameters.Clear();
                    GamePlayCmd.Parameters.AddWithValue("@card_id", accountId);
                    GamePlayCmd.Parameters.AddWithValue("@topRecordCount", showXRecordsInCustomerDashboard);
                    DataTable GamePlayTbl = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(GamePlayCmd);
                    da.Fill(GamePlayTbl);

                    if (KioskStatic.ShowTickets && GamePlayTbl != null)
                    {
                        eaterTickets = GamePlayTbl.Compute("Sum([T.Eater Tickets])", "[T.Eater Tickets] > 0");
                    }

                    dgvGamePlay.DataSource = GamePlayTbl;
                    da.Dispose();

                    KioskStatic.Utilities.setLanguage(dgvGamePlay);

                    dgvGamePlay.Columns["Date"].DefaultCellStyle = KioskStatic.Utilities.gridViewDateTimeCellStyle();
                    dgvGamePlay.Columns[2].DefaultCellStyle =
                    dgvGamePlay.Columns[3].DefaultCellStyle =
                    dgvGamePlay.Columns[5].DefaultCellStyle = KioskStatic.Utilities.gridViewAmountCellStyle();
                    dgvGamePlay.Columns["Tickets"].DefaultCellStyle =
                    dgvGamePlay.Columns["T.Eater Tickets"].DefaultCellStyle = KioskStatic.Utilities.gridViewNumericCellStyle();
                    try
                    {
                        if (KioskStatic.ShowTickets)
                        {
                            if (!string.IsNullOrEmpty(eaterTickets.ToString()) && Convert.ToDouble(eaterTickets) > 0)
                            {
                                colWidth = new int[] { 213, 178, 152, 148, 124, 124 };
                                lblEaterTickets.Visible = true;
                                dgvGamePlay.Columns["T.Eater Tickets"].Visible = true;

                                lblTicketsCourtesy.Location = new Point(705, 55);
                                lblEaterTickets.Location = new Point(815, 43);
                                label4.Location = new Point(230, 55); //gamelabel
                                label5.Location = new Point(386, 43); // game price
                                label6.Location = new Point(546, 43); // game points 
                            }
                            else
                            {
                                colWidth = new int[] { 213, 188, 180, 176, 176 }; //New
                                lblEaterTickets.Visible = false;
                                dgvGamePlay.Columns["T.Eater Tickets"].Visible = false;
                            }

                            dgvGamePlay.Columns["Courtesy"].Visible = false;
                            dgvGamePlay.Columns["Tickets"].Visible = true;
                        }
                        else
                        {
                            colWidth = new int[] { 213, 188, 180, 176, 176 };
                            lblEaterTickets.Visible = false;
                            dgvGamePlay.Columns["Courtesy"].Visible = true;
                            dgvGamePlay.Columns["Tickets"].Visible = false;
                            dgvGamePlay.Columns["T.Eater Tickets"].Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }

                    dgvGamePlay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                    i = 0;
                    foreach (DataGridViewColumn dc in dgvGamePlay.Columns)
                    {
                        if (dc.Visible)
                            dc.Width = colWidth[i++];
                    }

                    dgvGamePlay.Refresh();
                    if (dgvGamePlay.SelectedCells.Count > 0)
                        dgvGamePlay.SelectedCells[0].Selected = false;

                    vScrollBarGp.Maximum = dgvGamePlay.Rows.Count + 1;
                }
            }
            log.LogMethodExit();
        }

        private void Dashboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((Keys)e.KeyChar == Keys.Escape)
                this.Close();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("btnClose Click");
            this.Close();
            log.LogMethodExit();
        }

        private void CustomerDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile(this.Name + "_Closed");

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
                log.Info(this.Name + ": TopUp Reader unregistered");
            }
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Back pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            string CardNumber = "";
            if (KioskStatic.AllowRegisterWithoutCard)
            {
                CardNumber = null;
                StartKioskTimer();
            }
            else
            {
                using (frmTapCard ftc = new frmTapCard())
                {
                    ftc.ShowDialog();
                    ResetKioskTimer();
                    if (ftc.Card == null)
                    {
                        StartKioskTimer();
                        this.Activate();
                        log.LogMethodExit("ftc.Card == null");
                        return;
                    }

                    if (ftc.Card != null && ftc.Card.technician_card == 'Y') //Prevent transfer to Technician card
                    {
                        StartKioskTimer();
                        this.Activate();
                        log.LogMethodExit("technician_card == 'Y'");
                        return;
                    }
                    CardNumber = ftc.cardNumber;
                }
            }
            CustomerStatic.ShowCustomerScreen(CardNumber);
            StartKioskTimer();
            this.Activate();
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            ResetKioskTimer();
            try
            {
                DataTable dTable = KioskStatic.GetProductDisplayGroups("R", selectedEntitlementType);
                if (dTable != null && dTable.Rows.Count > 1)
                {
                    using (frmChooseDisplayGroup frmDisp = new frmChooseDisplayGroup("R", selectedEntitlementType))
                    {
                        if (frmDisp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                        {
                            Close();
                        }
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
                                    log.LogMethodExit("Selected Dialog result is other than Okay");
                                    return;
                                }
                                selectedEntitlementType = frmEntitle.selectedEntitlement;
                                frmEntitle.Dispose();
                            }
                        }
                    }
                    using (frmChooseProduct frm = new frmChooseProduct("R", selectedEntitlementType))
                    {
                        if (frm.IsDisposed != true)
                        {
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                Close();
                            }
                            else
                                StartKioskTimer();
                        }
                        else
                        {
                            StartKioskTimer();
                            log.LogMethodExit("frm.IsDisposed == true");
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
            log.LogMethodExit();
        }



        private void lblCardGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (accountDTO != null)
            {
                using (CardGames cg = new CardGames(accountDTO.AccountId))
                {
                    cg.ShowDialog();
                }
            }
            log.LogMethodExit();
        }



        private void vScrollBarPur_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 0)
                dgvPurchases.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (dgvGamePlay.Rows.Count > 0)
                dgvGamePlay.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }



        private void btnChangeTicketMode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            if (accountDTO != null)
            {
                using (frmTicketType frmTicket = new frmTicketType(KioskStatic.Utilities, accountDTO))
                {
                    if (frmTicket.ShowDialog() == DialogResult.OK)
                    {
                        if (accountDTO.RealTicketMode)
                        {
                            lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("Real Ticket");
                        }
                        else
                        {
                            lblRealTicketMode.Text = KioskStatic.Utilities.MessageUtils.getMessage("E-Ticket");
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
                foreach (Control c in panelGameplay.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardHeader1DetailsTextForeColor; //(GamePlay Details Header) -#CustomerDashboardHeader1DetailsForeColor
                }
                foreach (Control c in panelPurchase.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardHeader2DetailsTextForeColor; //(Purchase and Tasks Details Header) -#CustomerDashboardHeader2DetailsForeColor
                }
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardCardNumberTextForeColor;//(Card number header)-#CustomerDashboardCardNumberForeColor
                this.lblCard.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardCardNumberInfoTextForeColor;//(Card Number Info)-#CustomerDashboardCardNumberInfoForeColor
                this.lblTicketMode.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardTicketModeTextForeColor;//(Ticket Mode header) -#CustomerDashboardTicketModeForeColor
                this.lblRealTicketMode.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardTicketModeInfoTextForeColor;//(Ticket Mode info) -#CustomerDashboardTicketModeInfoForeColor
                this.btnChangeTicketMode.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardChangeButtonTextForeColor;//(Change button) -#CustomerDashboardChangeButtonForeColor
                this.label21.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardHeader1TextForeColor;//(GamePlay header) -#CustomerDashboardHeader1ForeColor
                this.label13.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardHeader2TextForeColor;//(Purchase and Tasks) -#CustomerDashboardHeader2ForeColor
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardBackBtnTextForeColor;//#CustomerDashboardBackBtnTextForeColor
                this.btnTopUp.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardTopUpBtnTextForeColor;//#CustomerDashboardTopUpBtnTextForeColor
                this.btnRegisterNew.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardRegisterBtnTextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}
