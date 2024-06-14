/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - CardGames.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha             Theme changes to support customized Images/Font
 *2.130.0    30-Jun-2021      Dakshak             Theme changes to support customized Font ForeColor
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class CardGames : BaseForm  
    {
        int cardId;
        public CardGames(int pcardId)
        {
            log.LogMethodEntry(pcardId);
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            cardId = pcardId;
            try
            {
                this.BackgroundImage = KioskStatic.CurrentTheme.ActivityGamesBackGroundImage;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        private void CardGames_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            displayCardGames(cardId);
            //timer1.Start();
            log.LogMethodExit();
        }

        void displayCardGames(int cardId)
        {
            log.LogMethodExit(cardId);
            SqlCommand cmd = KioskStatic.Utilities.getCommand();
            cmd.CommandText = "select isnull(isnull(profile_name, game_name), 'Any Game') \"Profile / Game\", Quantity Total, BalanceGames Balance, " +
                                "case Frequency when 'N' then 'None' when 'D' then 'Daily' when 'W' then 'Weekly' when 'M' then 'Monthly' when 'Y' then 'Yearly' else 'None' end as Frequency, " +
                                "ExpiryDate as Expiry " +
                                "from CardGames cg left outer join Games g " +
                                "on cg.game_id = g.game_id " +
                                "left outer join game_profile gp " +
                                "on gp.game_profile_id = cg.game_profile_id " +
                                "where card_id = @card_id " +
                                "and quantity != 0 " +
                                "order by 1 ";
            cmd.Parameters.AddWithValue("@card_id", cardId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            dgvCardGames.DataSource = dt1;

            KioskStatic.Utilities.setupDataGridProperties(ref dgvCardGames);

            dgvCardGames.Columns["Balance"].DefaultCellStyle = dgvCardGames.Columns["Total"].DefaultCellStyle = KioskStatic.Utilities.gridViewNumericCellStyle();
            dgvCardGames.Columns["Expiry"].DefaultCellStyle = KioskStatic.Utilities.gridViewDateTimeCellStyle();
            dgvCardGames.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            KioskStatic.Utilities.setLanguage(dgvCardGames);
            dgvCardGames.BackgroundColor = this.BackColor;
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();           
            if(tickSecondsRemaining<20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
           // btnPrev.BackgroundImage = Properties.Resources.alert_close_btn;
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.alert_close_btn_pressed;
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardGamesPrevTextForeColor;
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
