/********************************************************************************************
* Project Name - Parafait_Kiosk -CustomerDashboard
* Description  - CustomerDashboard 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019     Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        4-Sep-2019     Deeksha             Added logger methods.
 * 2.80        23-Mar-2020    Archana             SHOW_GAME_NAME_IN_GAME_MANAGEMENT config check value 
 *                                                 is considered to display game name in kiosk activity grid 
 * 2.70.3      08-Aug-2020    Guru S A            Fix kiosk crash due to large data in card activity/game play 
 *2.150.1      22-Feb-2023    Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class CustomerDashboard : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        private readonly TagNumberParser tagNumberParser;
        private int showXRecordsInCustomerDashboard = 1000;

        Card card;
        //Timer ClearDisplayTimer;
        //double totSecs = 0;
        //double screenTimeout = 20;
        Font savTimeOutFont;
        int gameScrollIndex = -1;//Starts:Modification on 17-Dec-2015 for introducing new theme
        int purchaseScrollIndex = -1;//Ends:Modification on 17-Dec-2015 for introducing new theme
        string cardNumberLabelText;
        bool isCardRealticket = false;//Added on 15-May-2017 for allowing cardticket mode change
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        public CustomerDashboard(Card inCard)
        {
            log.LogMethodEntry(inCard);
            Utilities.setLanguage();
            InitializeComponent();

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            Utilities.setLanguage(this);
            //Starts:Modification on 17-Dec-2015 for introducing new theme
            panelPurchase.BackgroundImage = ThemeManager.CurrentThemeImages.ProductTableImage;
            panelGameplay.BackgroundImage = ThemeManager.CurrentThemeImages.GamePriceTable;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            btnGameScrollDown.BackgroundImage = btnPurchaseScrollDown.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollDownButton;
            btnGameScrollUp.BackgroundImage = btnPurchaseScrollUp.BackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButton;
            lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;

            KioskStatic.setDefaultFont(this);

            if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
            {
                dgvGamePlay.ForeColor = dgvPurchases.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            else
            {
                dgvGamePlay.ForeColor = dgvPurchases.ForeColor = Color.Black;
            }//Ends:Modification on 17-Dec-2015 for introducing new theme

            try
            {
                // screenTimeout = Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT"));
                SetKioskTimerTickValue(Convert.ToInt32(KioskStatic.Utilities.getParafaitDefaults("BALANCE_SCREEN_TIMEOUT")));
            }
            catch
            {
                //screenTimeout = 30;
                SetKioskTimerTickValue();
            }
            //if (screenTimeout <= 10)
            //    screenTimeout = 20;
            if (GetKioskTimerTickValue() <= 10)
                SetKioskTimerTickValue(20);
            ResetKioskTimer();
            //totSecs = screenTimeout;
            //ClearDisplayTimer = new Timer();
            //ClearDisplayTimer.Interval = 1000;
            //ClearDisplayTimer.Tick += new EventHandler(ClearDisplayTimer_Tick);
            //ClearDisplayTimer.Start();


            savTimeOutFont = lblTimeOut.Font;
            lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            //lblTimeOut.Text = screenTimeout.ToString();
            lblTimeOut.Text = GetKioskTimerTickValue().ToString();

            card = inCard;
            cardNumberLabelText = Utilities.MessageUtils.getMessage(lblCardNumber.Text);

            if (card.CardNumber.Length > 8)
            {
                lblCardNumber.Text = "    " + cardNumberLabelText + " " + card.CardNumber;//Starts:Modification on 17-Dec-2015 for introducing new theme
            }
            else
            {
                lblCardNumber.Text = cardNumberLabelText + " " + card.CardNumber;//Starts:Modification on 17-Dec-2015 for introducing new theme
            }

            if (Utilities.getParafaitDefaults("ALLOW_CHANGE_TICKET_MODE").Equals("Y"))
            {
                UpdateRealTicketLink();
                lblTicketMode.Visible = BtnTicketModeChange.Visible = true;

                BtnTicketModeChange.Location = new Point(lblTicketMode.Location.X + 760, lblTicketMode.Location.Y + 19);
            }
            else
            {
                lblTicketMode.Visible = BtnTicketModeChange.Visible = false;
            }

            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//NewTheme102015
            }//Ends:Modification on 17-Dec-2015 for introducing new theme
            catch { }

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            clearFields();
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
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    KioskStatic.logToFile(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardNumber = KioskStatic.ReverseTopupCardNumber(CardNumber);
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch { }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            //totSecs = screenTimeout;
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
                catch { }

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
            lblCardNumber.Text = cardNumberLabelText + card.CardNumber;
            FetchTrxDetails(card.card_id);
            log.LogMethodExit();
        }


        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showXRecordsInCustomerDashboard = KioskHelper.GetShowXRecordsInCustomerDashboard(Utilities.ExecutionContext);
            lblSiteName.Text = KioskStatic.SiteHeading;
            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            //Added on 14-may-2017 for real ticket mode enahancement
            int m = 120;
            int n = lblHeading.Bottom + 40;

            panelCardDetails.Location = new Point(m, n);

            int x = 100, y = panelCardDetails.Bottom + 20;
            //end

            grpActivities.Location = new Point(x, y);
            grpActivities.Size = new System.Drawing.Size(this.Width - x * 2, this.Height - y - 10);

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
            {
                panelPurchase.Top = panelGameplay.Top;
                panelPurchase.Height += panelGameplay.Height;
                panelGameplay.Visible = false; //Added on 29-May-2015 to hide gameplay on mifare rw environment
            }
            else
            {
                panelGameplay.Height = (grpActivities.Height / 2) - 10;
                panelPurchase.Top = panelGameplay.Bottom;
                panelPurchase.Height = (grpActivities.Height / 2) - 10;
            }

            registerUSBDevice();

            this.Invalidate();
            this.Refresh();

            //totSecs = screenTimeout;
            ResetKioskTimer();
            grpActivities.Visible = true;
            grpActivities.BringToFront();

            FetchTrxDetails(card.card_id);
            if (dgvGamePlay.Rows.Count > 10)
            {
                btnGameScrollUp.Visible = btnGameScrollDown.Visible = true;
            }
            if (dgvPurchases.Rows.Count > 10)
            {
                btnPurchaseScrollDown.Visible = btnPurchaseScrollUp.Visible = true;
            }
            //Audio.PlayAudio(Audio.GamePlayDetails);
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

        private void FetchTrxDetails(object card_id)
        {
            log.LogMethodEntry(card_id);
            try
            {
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
                dgvPurchases.Columns["Product"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPurchases.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvPurchases.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvPurchases.Columns["Courtesy"].DefaultCellStyle =
                dgvPurchases.Columns["Bonus"].DefaultCellStyle =
                dgvPurchases.Columns["Time"].DefaultCellStyle =
                dgvPurchases.Columns["Loyalty Points"].DefaultCellStyle =
                dgvPurchases.Columns["Credits"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvPurchases.Columns["Credits"].HeaderText = MessageUtils.getMessage("Credits");

                dgvPurchases.Columns["Tokens"].DefaultCellStyle =
                dgvPurchases.Columns["Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

                int index = dgvPurchases.Columns["Tickets"].Index + 1;
                while (index < dgvPurchases.Columns.Count)
                {
                    dgvPurchases.Columns[index++].Visible = false;
                }

                bool minDetails = KioskStatic.Utilities.getParafaitDefaults("MINIMAL_DETAILS_IN_ACTIVITY_SCREEN").Equals("Y");
                try
                {
                    if (minDetails)
                    {
                        dgvPurchases.Columns["Courtesy"].Visible =
                        dgvPurchases.Columns["Bonus"].Visible =
                        dgvPurchases.Columns["Time"].Visible =
                        dgvPurchases.Columns["Loyalty Points"].Visible =
                        dgvPurchases.Columns["Tokens"].Visible =
                        dgvPurchases.Columns["Tickets"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error in FetchTrxDetails: " + ex.Message);
                }

                Utilities.setLanguage(dgvPurchases);
                dgvPurchases.ColumnHeadersDefaultCellStyle.Font = new Font(dgvPurchases.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, dgvPurchases.ColumnHeadersDefaultCellStyle.Font.Style);
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
                    dgvGamePlay.DataSource = GamePlayTbl;
                    da.Dispose();

                    if (GamePlayTbl != null)
                    {
                        eaterTickets = GamePlayTbl.Compute("Sum([T.Eater Tickets])", "[T.Eater Tickets] > 0");
                    }

                    dgvGamePlay.Columns[2].HeaderText = dgvGamePlay.Columns[2].HeaderText.Replace("Credits", MessageUtils.getMessage("Credits"));
                    dgvGamePlay.Columns[2].HeaderText = dgvGamePlay.Columns[2].HeaderText.Replace("Game Price", KioskStatic.Utilities.MessageUtils.getMessage("Game Price"));
                    dgvGamePlay.Columns[3].HeaderText = dgvGamePlay.Columns[3].HeaderText.Replace("Game Price", KioskStatic.Utilities.MessageUtils.getMessage("Game Price"));
                    dgvGamePlay.Columns[3].HeaderText = dgvGamePlay.Columns[3].HeaderText.Replace("Bonus", KioskStatic.Utilities.MessageUtils.getMessage("Bonus"));
                    Utilities.setLanguage(dgvGamePlay);

                    dgvGamePlay.ColumnHeadersDefaultCellStyle.Font = new Font(dgvGamePlay.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, dgvGamePlay.ColumnHeadersDefaultCellStyle.Font.Style);
                    dgvGamePlay.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                    dgvGamePlay.Columns[2].DefaultCellStyle =
                    dgvGamePlay.Columns[3].DefaultCellStyle =
                    dgvGamePlay.Columns["Courtesy"].DefaultCellStyle =
                    dgvGamePlay.Columns["Time"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                    dgvGamePlay.Columns["Tickets"].DefaultCellStyle =
                    dgvGamePlay.Columns["T.Eater Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

                    try
                    {
                        if (minDetails)
                        {
                            dgvGamePlay.Columns["Courtesy"].Visible =
                            dgvGamePlay.Columns["Time"].Visible =
                            dgvGamePlay.Columns["Tickets"].Visible = false;
                            dgvGamePlay.Columns["T.Eater Tickets"].Visible = false;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(eaterTickets.ToString()) && Convert.ToDouble(eaterTickets) > 0)
                            {
                                dgvGamePlay.Columns["T.Eater Tickets"].Visible = true;
                                dgvGamePlay.Columns["T.Eater Tickets"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgvGamePlay.Columns["T.Eater Tickets"].Width = 100;
                                dgvGamePlay.Columns["Game Price (Credits)"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgvGamePlay.Columns["Game Price (Credits)"].Width = 140;
                                dgvGamePlay.Columns["Game Price (Bonus)"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgvGamePlay.Columns["Game Price (Bonus)"].Width = 140;
                            }
                            else
                            {
                                dgvGamePlay.Columns["T.Eater Tickets"].Visible = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.logToFile("Error in FetchTrxDetails: " + ex.Message);
                    }

                    dgvGamePlay.Refresh();
                    if (dgvGamePlay.SelectedCells.Count > 0)
                        dgvGamePlay.SelectedCells[0].Selected = false;

                    vScrollBarGp.Maximum = dgvGamePlay.Rows.Count + 1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in FetchTrxDetails: " + ex.Message);
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

        private void CustomerDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile(this.Name + "_Closed");
            //ClearDisplayTimer.Stop();

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
            }
            log.LogMethodExit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //ClearDisplayTimer.Stop();
            log.LogMethodEntry();
            StopKioskTimer();

            frmTapCard ftc = new frmTapCard();
            ftc.ShowDialog();
            //totSecs = screenTimeout;
            ResetKioskTimer();
            if (ftc.Card == null)
            {
                //ClearDisplayTimer.Start();
                StartKioskTimer();
                this.Activate();
                log.LogMethodExit();
                return;
            }
            if (ftc.Card != null && ftc.Card.technician_card == 'Y') //Prevent transfer to Technician card
            {
                //ClearDisplayTimer.Start();
                StartKioskTimer();
                this.Activate();
                log.LogMethodExit();
                return;
            }

            string CardNumber = ftc.cardNumber;

            CustomerStatic.ShowCustomerScreen(CardNumber);
            //ClearDisplayTimer.Start();
            StartKioskTimer();
            this.Activate();
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (grpActivities.Visible)
                grpActivities.Visible = false;
            else
                Close();
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.back_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void CustomerDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile(this.Name + "_Closed");
            //ClearDisplayTimer.Stop();

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
            }
            log.LogMethodExit();
        }

        private void vScrollBarPur_Scroll(object sender, ScrollEventArgs e)
        {
            //totSecs = 0;
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 0)
                dgvPurchases.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            //totSecs = 0;
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
            if (dgvGamePlay.Rows.Count > 10)
            {
                if (gameScrollIndex == -1)
                    gameScrollIndex = 0;
                else
                    gameScrollIndex = Math.Max(gameScrollIndex - 1, 0);
                dgvGamePlay.FirstDisplayedScrollingRowIndex = gameScrollIndex;
            }
            log.LogMethodExit();
        }//Starts:Modification on 17-Dec-2015 for introducing new theme

        private void btnGameScrollDown_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvGamePlay.Rows.Count > 10)
            {
                if (gameScrollIndex == -1)
                    gameScrollIndex = 1;
                else
                    gameScrollIndex = Math.Min(gameScrollIndex + 1, dgvGamePlay.RowCount - 10);
                dgvGamePlay.FirstDisplayedScrollingRowIndex = gameScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btnPurchaseScrollDown_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 10)//18 is the visible row count  without scrolling
            {
                if (purchaseScrollIndex == -1)
                    purchaseScrollIndex = 1;
                else
                    purchaseScrollIndex = Math.Min(purchaseScrollIndex + 1, dgvPurchases.RowCount - 10);//18 is the visible row count  without scrolling
                dgvPurchases.FirstDisplayedScrollingRowIndex = purchaseScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btnPurchaseScrollUp_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvPurchases.Rows.Count > 0)//18 is the visible row count  without scrolling
            {
                if (purchaseScrollIndex == -1)
                    purchaseScrollIndex = 0;
                else
                    purchaseScrollIndex = Math.Max(purchaseScrollIndex - 1, 0);
                dgvPurchases.FirstDisplayedScrollingRowIndex = purchaseScrollIndex;
            }
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        #region starts ticket mode code, added on 10-may-2017
        private void BtnRealTicket_Click(object sender, EventArgs e)
        {
            //load checked real ticket image
            log.LogMethodEntry();
            isCardRealticket = true;
            BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_Checked;
            btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_unchecked;
            log.LogMethodExit();
        }

        private void btnEticket_Click(object sender, EventArgs e)
        {
            //load checked E ticket image
            log.LogMethodEntry();
            isCardRealticket = false;
            BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_unchecked;
            btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_Checked;
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelTicketMode.Visible = false;
            BtnTicketModeChange.Enabled = true;
            btnHome.Enabled = true;

            if (card != null && card.CardStatus != "NEW")
            {
                try
                {
                    int res = Utilities.executeNonQuery(@"UPDATE cards 
                                                    SET real_ticket_mode = @ticketStatus 
                                                WHERE card_id = @cardId",
                                                      new SqlParameter("@cardId", card.card_id),
                                                      new SqlParameter("@ticketStatus", isCardRealticket ? 'Y' : 'N'));

                    if (res == 1)
                    {
                        card.real_ticket_mode = isCardRealticket ? 'Y' : 'N';
                        UpdateRealTicketLink();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message + ":" + ex.StackTrace);
                }
            }
            log.LogMethodExit();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelTicketMode.Visible = false;
            btnHome.Enabled = true;
            BtnTicketModeChange.Enabled = true;
            log.LogMethodExit();
        }

        void UpdateRealTicketLink()
        {
            log.LogMethodEntry();
            if (card.real_ticket_mode.Equals('Y'))
            {
                lblTicketMode.Text = " Ticket Mode: Real Ticket";
            }
            else
            {
                lblTicketMode.Text = "Ticket Mode: E-Ticket    ";
            }
            log.LogMethodExit();
        }

        private void BtnTicketModeChange_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            panelTicketMode.Location = new Point(
            this.ClientSize.Width / 2 - panelTicketMode.Size.Width / 2,
            this.ClientSize.Height / 2 - panelTicketMode.Size.Height / 2);
            panelTicketMode.Anchor = AnchorStyles.None;

            if (card.real_ticket_mode.Equals('Y'))
            {
                isCardRealticket = true;
                BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_Checked;
                btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_unchecked;
            }
            else
            {
                isCardRealticket = false;
                BtnRealTicket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_unchecked;
                btnEticket.BackgroundImage = Parafait_Kiosk.Properties.Resources.Radio_Btn_Checked;
            }

            panelTicketMode.Visible = true;
            panelTicketMode.BringToFront();
            btnHome.Enabled = false;
            BtnTicketModeChange.Enabled = false;
            log.LogMethodExit();
        }
        #endregion
    }
}
