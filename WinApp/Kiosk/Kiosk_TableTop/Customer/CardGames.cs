/********************************************************************************************
* Project Name - Parafait_Kiosk - CardGames
* Description  - CardGames.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class CardGames : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int cardId;
        public CardGames(int pcardId)
        {
            log.LogMethodEntry(pcardId);
            KioskStatic.Utilities.setLanguage();
            InitializeComponent();
            cardId = pcardId;
            log.LogVariableState("CardId", cardId);
            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.Utilities.setLanguage(this);
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            log.LogMethodExit();
        }

        private void CardGames_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            displayCardGames(cardId);
            SetCustomizedFontColors();
            //timer1.Start();
            log.LogMethodExit();
        }

        void displayCardGames(int cardId)
        {
            log.LogMethodEntry(cardId);
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
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                base.CloseForms();
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
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in Cardgames");
            try
            {
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardGamesPrevTextForeColor;
                this.dgvCardGames.ForeColor = KioskStatic.CurrentTheme.CardGamesdgvCardGamesForeColor;
                this.dgvCardGames.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.CardGamesdgvCardGamesHeaderForeColor;
                this.BackColor =
                    dgvCardGames.BackgroundColor = KioskStatic.CurrentTheme.CardGamesBackColor;

            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in Cardgames: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
