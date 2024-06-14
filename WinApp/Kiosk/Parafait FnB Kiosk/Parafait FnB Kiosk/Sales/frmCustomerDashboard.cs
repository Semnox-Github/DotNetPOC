/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCustomerdashboard.cs
* Description  - frmCustomerdashboard 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
 * 2.70.3      08-Aug-2020      Guru S A           Fix kiosk crash due to large data in card activity/game play 
********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;

namespace Parafait_FnB_Kiosk
{
    public partial class frmCustomerDashboard : BaseForm
    {
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;
        private int showXRecordsInCustomerDashboard = 1000;

        string CardNumber;
        bool ShowTickets = true;
        public frmCustomerDashboard()
        {
            log.LogMethodEntry();
            InitializeComponent();

            if (ShowTickets)
                lblTicketsCourtesy.Text = "Tickets";

            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void clearFields()
        {
            log.LogMethodEntry();
            lblCredits.Text = 0.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
            lblCard.Text = "";
            lblTimeRemainingValue.Text = 0.ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT) + " " + Common.utils.MessageUtils.getMessage("Minutes");

            dgvPurchases.DataSource = null;
            dgvGamePlay.DataSource = null;
            log.LogMethodExit();
        }

        void DisplayBalance()
        {
            log.LogMethodEntry();
            try
            {
                Card card = new Card(CardNumber, "", Common.utils);
                lblCredits.Text = (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits + card.bonus + card.courtesy).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT);
                lblCard.Text = card.CardNumber;
                lblTimeRemainingValue.Text = (card.time + card.CreditPlusTime).ToString(KioskStatic.KIOSK_CARD_VALUE_FORMAT) + " " + Common.utils.MessageUtils.getMessage("Minutes");

                FetchTrxDetails(card.card_id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }

        private void CustomerDashboard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showXRecordsInCustomerDashboard = KioskHelper.GetShowXRecordsInCustomerDashboard(Utilities.ExecutionContext);
            this.WindowState = FormWindowState.Normal;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            if (string.IsNullOrEmpty(_callingElement.Attribute.ActionScreenTitle1) == false)
                lblScreenTitle.Text = _callingElement.Attribute.ActionScreenTitle1;

            grpActivities.Visible = true;
            dgvPurchases.Columns.Clear();
            dgvGamePlay.Columns.Clear();
            dgvPurchases.AllowUserToAddRows = dgvGamePlay.AllowUserToAddRows = false;

            if (Common.utils.ParafaitEnv.MIFARE_CARD)
            {
                panelPurchase.Top = panelGameplay.Top;
                panelGameplay.Visible = false;
            }

            clearFields();

            this.Opacity = 0;
            ScreenModel.ElementParameter parameter = new ScreenModel.ElementParameter();
            frmTapCard ftc = new frmTapCard(parameter);
            if (ftc.ShowDialog() == DialogResult.OK)
            {
                CardNumber = parameter.CardNumber;
                this.Opacity = 100;
            }
            else
            {
                Close();
                log.LogMethodExit();
                return;
            }

            DisplayBalance();

            Audio.PlayAudio(Audio.GamePlayDetails);
            log.LogMethodExit();
        }

        private void FetchTrxDetails(object card_id)
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
                dgvPurchases.Columns["Courtesy"].Visible =
                dgvPurchases.Columns["Bonus"].Visible =
                dgvPurchases.Columns["Loyalty Points"].Visible =
                dgvPurchases.Columns["Tokens"].Visible =
                dgvPurchases.Columns["Tickets"].Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.logToFile("Error in FetchTrxDetails: " + ex.Message);
            }

            Utilities.setLanguage(dgvPurchases);
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            int[] colWidth = { 193, 162, 190, 200, 192 };
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
                SqlCommand GamePlayCmd = Utilities.getCommand();
                GamePlayCmd.CommandText = "select top (@topRecordCount) gp.play_date Date, " +
                                           @"m.machine_name Game, 
                                             (gp.credits + gp.CPCardBalance + gp.CPCredits) " + "\"Game Price (Credits)\", " +
                                            "gp.bonus + gp.CPBonus \"Game Price (Bonus)\", " +
                                            "gp.ticket_count Tickets, gp.courtesy Courtesy " +
                                            "from gameplay gp, machines m " +
                                            "where gp.card_id = @card_id " +
                                            "and gp.machine_id = m.machine_id " +
                                            "order by gp.play_date desc";

                GamePlayCmd.Parameters.Clear();
                GamePlayCmd.Parameters.AddWithValue("@card_id", card_id);
                GamePlayCmd.Parameters.AddWithValue("@topRecordCount", showXRecordsInCustomerDashboard);
                DataTable GamePlayTbl = new DataTable();
                da = new SqlDataAdapter(GamePlayCmd);
                da.Fill(GamePlayTbl);
                dgvGamePlay.DataSource = GamePlayTbl;
                da.Dispose();

                dgvGamePlay.Columns[2].HeaderText = dgvGamePlay.Columns[2].HeaderText.Replace("Credits", MessageUtils.getMessage("Credits"));
                dgvGamePlay.Columns[2].HeaderText = dgvGamePlay.Columns[2].HeaderText.Replace("Game Price", KioskStatic.Utilities.MessageUtils.getMessage("Game Price"));
                dgvGamePlay.Columns[3].HeaderText = dgvGamePlay.Columns[3].HeaderText.Replace("Game Price", KioskStatic.Utilities.MessageUtils.getMessage("Game Price"));
                dgvGamePlay.Columns[3].HeaderText = dgvGamePlay.Columns[3].HeaderText.Replace("Bonus", KioskStatic.Utilities.MessageUtils.getMessage("Bonus"));

                Utilities.setLanguage(dgvGamePlay);

                dgvGamePlay.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvGamePlay.Columns[2].DefaultCellStyle =
                dgvGamePlay.Columns[3].DefaultCellStyle =
                dgvGamePlay.Columns[5].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvGamePlay.Columns["Tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

                try
                {
                    if (ShowTickets)
                    {
                        dgvGamePlay.Columns["Courtesy"].Visible = false;
                        dgvGamePlay.Columns["Tickets"].Visible = true;
                    }
                    else
                    {
                        dgvGamePlay.Columns["Courtesy"].Visible = true;
                        dgvGamePlay.Columns["Tickets"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Common.logToFile("Error in FetchTrxDetails: " + ex.Message);
                }

                dgvGamePlay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                colWidth = new int[] {163, 150, 234, 198, 188};
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void vScrollBarPur_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (dgvPurchases.Rows.Count > 0)
                dgvPurchases.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }

        private void vScrollBarGp_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (dgvGamePlay.Rows.Count > 0)
                dgvGamePlay.FirstDisplayedScrollingRowIndex = e.NewValue;
            log.LogMethodExit();
        }
    }
}