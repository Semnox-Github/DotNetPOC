/********************************************************************************************
* Project Name - Parafait_Kiosk -CustomerDashboard
* Description  - CustomerDashboard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019     Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        06-Sep-2019    Deeksha              Added logger methods.
 * 2.80        23-Mar-2020    Archana              SHOW_GAME_NAME_IN_GAME_MANAGEMENT config check value 
 *                                                  is considered to display game name in kiosk activity grid
*2.130.0     09-Jul-2021      Dakshak             Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat        Back and Cancel button changes
*2.150.0.0   23-Sep-2022      Sathyavathi         Check-In feature Phase-2
*2.150.0.0   13-Oct-2022      Sathyavathi         Mask card number
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 *******************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_Kiosk
{
    public partial class CustomerDashboard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        private readonly TagNumberParser tagNumberParser;
        private int showXRecordsInCustomerDashboard = 1000;
        Card card;
        Font savTimeOutFont;
        int gameScrollIndex = -1;//Starts:Modification on 17-Dec-2015 for introducing new theme
        int purchaseScrollIndex = -1;///Ends:Modification on 17-Dec-2015 for introducing new theme
        bool isCardRealticket = false; //Starts:Modification on 11-May-2017 for adding ticket mode change
        private ToolTip purchaseToolTip = new ToolTip();
        private ToolTip gamePlayToolTip = new ToolTip();

        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        public CustomerDashboard(Card inCard)
        {
            log.LogMethodEntry(inCard);
            InitializeComponent();
            string btnEticketTextValue = MessageContainerList.GetMessage(Utilities.ExecutionContext, "E-Ticket");
            string btnRealticketTextValue = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Real Ticket");
            if (KioskStatic.ShowTickets)
                lblTicketsCourtesy.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Tickets");
            panelPurchase.BackgroundImage = ThemeManager.CurrentThemeImages.ProductTableImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            panelGameplay.BackgroundImage = ThemeManager.CurrentThemeImages.GamePriceTable;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            KioskStatic.setDefaultFont(this);
            bool showTicketModeInActivity = false;


            try
            {
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                SetKioskTimerTickValue();
            }

            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);

            ResetKioskTimer();

            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();

            card = inCard;

            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CustomerDashboardBackgroundImage);//Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            showTicketModeInActivity = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_TICKET_MODE_IN_CARD_ACTIVITY", false);
            panelTicket.Visible = showTicketModeInActivity;
            if (showTicketModeInActivity)
            {
                //Added on May -10-2017 for adding ticket mode
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ALLOW_CHANGE_TICKET_MODE"))
                {
                    UpdateRealTicketLink();
                    btnChangeTicketMode.Visible = true;
                }
                else
                {
                    btnChangeTicketMode.Visible = false;
                }//end
            }

            clearFields();

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void clearFields()
        {
            log.LogMethodEntry();
            dgvPurchases.DataSource = null;
            dgvGamePlay.DataSource = null;
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
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    KioskStatic.logToFile(message);
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
                    log.Error(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            //totSecs = screenTimeout;
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
                    log.Error(ex.Message);
                }

                log.LogMethodExit();
                return;
            }
            else
            {
                lclCard = new Card(readerDevice, inCardNumber, "External POS", KioskStatic.Utilities);
                log.LogVariableState("lclCard", lclCard);
            }
            if (lclCard.CardStatus == "NEW")
            {
                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 528);
                else
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 459);

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
            log.LogVariableState("card", card);
            displayBalance();
            log.LogMethodExit();
        }

        void displayBalance()
        {
            log.LogMethodEntry();
            try
            {
                if (card.CardStatus != "NEW")
                {
                    lblCredits.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits + card.bonus + card.courtesy).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                    lblCard.Text = KioskHelper.GetMaskedCardNumber(card.CardNumber);
                }

                fetchTrxDetails(card.card_id);
                if (panelTicket.Visible == true)
                {
                    UpdateRealTicketLink();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.SuspendLayout();
                showXRecordsInCustomerDashboard = KioskHelper.GetShowXRecordsInCustomerDashboard(Utilities.ExecutionContext);
                grpActivities.Visible = true;
                dgvPurchases.Columns.Clear();
                dgvGamePlay.Columns.Clear();
                dgvPurchases.AllowUserToAddRows = dgvGamePlay.AllowUserToAddRows = false;
                lblSiteName.Text = KioskStatic.SiteHeading;

                if (KioskStatic.DisablePurchase)
                {
                    btnTopUp.Visible = false;
                }

                try
                {
                    if ((!KioskStatic.RegistrationAllowed) || (Utilities.getParafaitDefaults("DISABLE_CUSTOMER_REGISTRATION").Equals("Y")))
                    {
                        btnRegisterNew.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                {
                    panelPurchase.Top = panelGameplay.Top;
                    //   panelPurchase.Height += panelGameplay.Height;
                    panelGameplay.Visible = false; //Added on 29-May-2015 to hide gameplay on mifare rw environment
                }

                registerUSBDevice();
                displayBalance();
                this.Invalidate();
            }
            finally
            {
                this.ResumeLayout();
            }
            //this.Refresh(); // Commented to eliminate the flickering issue. Added Suspend and Resume instead.
            Audio.PlayAudio(Audio.GamePlayDetails);
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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

        private void fetchTrxDetails(object card_id)
        {
            log.LogMethodEntry(card_id);
            SqlCommand PurchaseCmd = Utilities.getCommand();
            PurchaseCmd.CommandText = @"select top (@topRecordCount) * 
                                          from CardActivityView  
                                         where card_id = @card_id  
                                         order by date desc, 3 desc";
            PurchaseCmd.Parameters.AddWithValue("@card_id", card_id);
            PurchaseCmd.Parameters.AddWithValue("@topRecordCount", showXRecordsInCustomerDashboard);
            DataTable PurchaseTbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(PurchaseCmd);
            da.Fill(PurchaseTbl);
            dgvPurchases.DataSource = PurchaseTbl;

            dgvPurchases.Columns["card_id"].Visible = false;
            dgvPurchases.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvPurchases.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvPurchases.Columns["Courtesy"].DefaultCellStyle =
            dgvPurchases.Columns["Bonus"].DefaultCellStyle =
            dgvPurchases.Columns["Time"].DefaultCellStyle =
            dgvPurchases.Columns["Loyalty Points"].DefaultCellStyle =
            dgvPurchases.Columns["Virtual Points"].DefaultCellStyle =
            dgvPurchases.Columns["Credits"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvPurchases.Columns["Credits"].HeaderText = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credits");

            dgvPurchases.Columns["Tokens"].DefaultCellStyle =
            dgvPurchases.Columns["Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

            int index = dgvPurchases.Columns["Tickets"].Index + 1;
            while (index < dgvPurchases.Columns.Count)
            {
                dgvPurchases.Columns[index++].Visible = false;
            }

            bool minDetails = KioskStatic.Utilities.getParafaitDefaults("MINIMAL_DETAILS_IN_ACTIVITY_SCREEN").Equals("Y");
            log.LogVariableState("minDetails", minDetails);
            try
            {
                dgvPurchases.Columns["Courtesy"].Visible =
                dgvPurchases.Columns["Bonus"].Visible =
                dgvPurchases.Columns["Time"].Visible =
                dgvPurchases.Columns["Loyalty Points"].Visible =
                dgvPurchases.Columns["Tokens"].Visible =
                dgvPurchases.Columns["Virtual Points"].Visible =
                dgvPurchases.Columns["Tickets"].Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            Utilities.setLanguage(dgvPurchases);
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            int[] colWidth = { 225, 310, 194, 209 };
            int i = 0;
            foreach (DataGridViewColumn dc in dgvPurchases.Columns)
            {
                if (dc.Visible)
                    dc.Width = colWidth[i++];
            }

            dgvPurchases.Refresh();
            da.Dispose();

            if (dgvPurchases.SelectedCells.Count > 0)
                dgvPurchases.SelectedCells[0].Selected = false;

            vScrollBarPur.Maximum = dgvPurchases.Rows.Count + 1;

            if (!KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                object eaterTickets = null;
                SqlCommand GamePlayCmd = Utilities.getCommand();
                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "SHOW_GAME_NAME_IN_GAME_MANAGEMENT").Equals("Y"))
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
                GamePlayCmd.Parameters.AddWithValue("@card_id", card_id);
                GamePlayCmd.Parameters.AddWithValue("@topRecordCount", showXRecordsInCustomerDashboard);
                DataTable GamePlayTbl = new DataTable();
                da = new SqlDataAdapter(GamePlayCmd);
                da.Fill(GamePlayTbl);

                if (KioskStatic.ShowTickets && GamePlayTbl != null)
                {
                    eaterTickets = GamePlayTbl.Compute("Sum([T.Eater Tickets])", "[T.Eater Tickets] > 0");
                }

                dgvGamePlay.DataSource = GamePlayTbl;
                da.Dispose();

                Utilities.setLanguage(dgvGamePlay);

                dgvGamePlay.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvGamePlay.Columns[2].DefaultCellStyle =
                dgvGamePlay.Columns[3].DefaultCellStyle =
                dgvGamePlay.Columns[5].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvGamePlay.Columns["Tickets"].DefaultCellStyle =
                dgvGamePlay.Columns["T.Eater Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
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
            log.LogMethodExit();
        }

        private void Dashboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if ((Keys)e.KeyChar == Keys.Escape)
                this.Close();
            log.LogMethodExit();
        }

        private void CustomerDashboard_FormClosed(object sender, FormClosedEventArgs e)
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
            log.LogMethodExit();
            KioskStatic.logToFile(this.Name + ":Form Closed");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                string CardNumber = string.Empty;
                using (frmTapCard ftc = new frmTapCard())
                {
                    ftc.ShowDialog();
                    ResetKioskTimer();
                    if (ftc.Card == null)
                    {
                        StartKioskTimer();
                        this.Activate();
                        log.LogMethodExit();
                        return;
                    }

                    if (ftc.Card != null && ftc.Card.technician_card == 'Y') //Prevent transfer to Technician card
                    {
                        StartKioskTimer();
                        this.Activate();
                        log.LogMethodExit();
                        return;
                    }

                    CardNumber = ftc.cardNumber;
                }

                CustomerStatic.ShowCustomerScreen(CardNumber);
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                log.Error(ex);
                PerformTimeoutAbortAction(null, null);
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            StartKioskTimer();
            this.Activate();
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            ResetKioskTimer();
            try
            {
                kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
                using (frmChooseProduct frm = new frmChooseProduct(kioskTransaction, KioskTransaction.GETRECHAREGETYPE, selectedEntitlementType))
                {
                    if (frm.isClosed == true)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        kioskTransaction = frm.GetKioskTransaction;
                        Close();
                    }
                    else
                    {
                        kioskTransaction = frm.GetKioskTransaction;
                        StartKioskTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void lblCardGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                using (CardGames cg = new CardGames(card.card_id))
                {
                    cg.ShowDialog();
                }
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
            //  btnTopUp.BackgroundImage = Properties.Resources.top_up_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnRegisterNew_MouseDown(object sender, MouseEventArgs e)
        {
            // btnRegisterNew.BackgroundImage = Properties.Resources.top_up_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnTopUp_MouseUp(object sender, MouseEventArgs e)
        {
            // btnTopUp.BackgroundImage = Properties.Resources.top_up__activities_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnRegisterNew_MouseUp(object sender, MouseEventArgs e)
        {
            // btnRegisterNew.BackgroundImage = Properties.Resources.top_up__activities_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void vScrollBarPur_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 0)
                dgvPurchases.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvGamePlay.Rows.Count > 0)
                dgvGamePlay.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void btnGameScrollUp_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvGamePlay.Rows.Count > 0)
            {
                if (gameScrollIndex == -1)
                    gameScrollIndex = 0;
                else
                    gameScrollIndex = Math.Max(gameScrollIndex - 1, 0);
                dgvGamePlay.FirstDisplayedScrollingRowIndex = gameScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btnGameScrollDown_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvGamePlay.Rows.Count > 0)
            {
                if (gameScrollIndex == -1)
                    gameScrollIndex = 1;
                else
                    gameScrollIndex = Math.Min(gameScrollIndex + 1, dgvGamePlay.RowCount - 1);
                dgvGamePlay.FirstDisplayedScrollingRowIndex = gameScrollIndex;
            }
            log.LogMethodExit();
        }//Starts:Modification on 17-Dec-2015 for introducing new theme

        private void btnPurchaseScrollDown_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 18)//18 is the visible row count  without scrolling
            {
                if (purchaseScrollIndex == -1)
                    purchaseScrollIndex = 1;
                else
                    purchaseScrollIndex = Math.Min(purchaseScrollIndex + 1, dgvPurchases.RowCount - 18);//18 is the visible row count  without scrolling
                dgvPurchases.FirstDisplayedScrollingRowIndex = purchaseScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btnPurchaseScrollUp_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 18)//18 is the visible row count  without scrolling
            {
                if (purchaseScrollIndex == -1)
                    purchaseScrollIndex = 0;
                else
                    purchaseScrollIndex = Math.Max(purchaseScrollIndex - 1, 0);
                dgvPurchases.FirstDisplayedScrollingRowIndex = purchaseScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        #region starts ticket mode code, added on 11-may-2017
        void UpdateRealTicketLink()
        {
            log.LogMethodEntry();
            if (card.real_ticket_mode.Equals('Y'))
            {
                lblRealTicketMode.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Real Ticket");
            }
            else
            {
                lblRealTicketMode.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "E-Ticket");
            }
            log.LogMethodExit();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnHome.Enabled = true;
            btnChangeTicketMode.Enabled = true;
            log.LogMethodExit();
        }

        private void btnChangeTicketMode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Change Ticket mode pressed");
            StopKioskTimer();
            if (card != null)
            {
                AccountBL accountBL = new AccountBL(KioskStatic.Utilities.ExecutionContext, card.card_id, false, false);
                AccountDTO accountDTO = accountBL.AccountDTO;
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
                    card = new Card(card.card_id, "External POS", KioskStatic.Utilities);
                }
            }
            StartKioskTimer();
            log.LogMethodExit();
        }
        #endregion
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerDashboard");
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
                this.button1.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardButton1TextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                this.lblPlayValueLabel.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardLblPlayValueLabelTextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                this.lblCredits.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardLblCreditsTextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardLblTimeOutTextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardBtnHomeTextForeColor;//-#CustomerDashboardRegisterBtnForeColor
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.dgvGamePlay.ForeColor = dgvPurchases.ForeColor = KioskStatic.CurrentTheme.CustomerDashboardInfoTextForeColor;
                this.bigVerticalScrollGamePlay.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigVerticalScrollPurchases.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);

                this.panelGameplay.Controls.Add(this.lblEaterTickets);
                this.panelGameplay.Controls.Add(this.lblTicketsCourtesy);
                this.panelGameplay.Controls.Add(this.label6);
                this.panelGameplay.Controls.Add(this.label5);
                this.panelGameplay.Controls.Add(this.label4);
                this.panelGameplay.Controls.Add(this.label3);
                this.panelGameplay.Controls.Add(this.label21);
                btnChangeTicketMode.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.ChangeButtonImage);
                btnChangeTicketMode.Size = (btnChangeTicketMode.Size.Height > 61) ? this.btnChangeTicketMode.Size: btnChangeTicketMode.BackgroundImage.Size;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerDashboard: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvPurchases_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    ResetKioskTimer();
                    DataGridViewCell cell = dgvPurchases.Rows[e.RowIndex].Cells["Product"];
                    string cellContent = cell.Value.ToString();

                    if (!string.IsNullOrEmpty(cellContent))
                    {
                        Point cursorPosition = Cursor.Position;
                        purchaseToolTip.Show(cellContent, this, ((this.Width / 2) - cellContent.Length / 2), cursorPosition.Y, 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void dgvPurchases_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                purchaseToolTip.Hide(this);
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvGamePlay_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    ResetKioskTimer();
                    DataGridViewCell cell = dgvGamePlay.Rows[e.RowIndex].Cells["Machine"];
                    string cellContent = cell.Value.ToString();

                    if (!string.IsNullOrEmpty(cellContent))
                    {
                        Point cursorPosition = Cursor.Position;
                        gamePlayToolTip.Show(cellContent, this, ((this.Width / 2) - cellContent.Length / 2), cursorPosition.Y, 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvGamePlay_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                gamePlayToolTip.Hide(this);
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
