/****************************************************************************************************************
 * Project Name - Parafait_POS - CreditPlusDetails
 * Description  - CreditPlusDetails 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 ****************************************************************************************************************
 *2.4.0       28-Sep-2018      Guru S A           Modified Pause Allowed changes 
 *2.80.0      20-Aug-2019      Girish Kundar      Modified :  Added logger methods and Removed unused namespace's
 *                                                Add logic to show ValidityStatus field for CCP and Card Games
 *2.155.0     06-Jul-2023      Mathew Ninan       Skip cell level formatting when number of CCP records are high. 
 *                                                Use threshold count from config to decide.
 ****************************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public partial class CreditPlusDetails : Form
    {
        private int CardId;
        private int _type;
        private Utilities Utilities = POSStatic.Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CreditPlusDetails(int pCardId, int type = 0)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(pCardId, type);
            POSStatic.Utilities.setLanguage();
            InitializeComponent();
            CardId = pCardId;
            _type = type;
            log.LogMethodExit();
        }

        private void CreditPlusDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvPurchaseCriteria.BackgroundColor = panelPanel.BackColor;
            dgvConsumption.BackgroundColor = panelPanel.BackColor;
            POSStatic.CommonFuncs.setupDataGridProperties(dgvCardGames);
            POSStatic.CommonFuncs.setupDataGridProperties(dgvCardGameExtended);

            if (_type == 0)
            {
                getDetails();
                tcGamesAndCreditPlus.SelectedTab = tpCreditPlus;
            }
            else
            {
                getGames();
                tcGamesAndCreditPlus.SelectedTab = tpGames;
            }

            POSStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        void getGames()
        {
            log.LogMethodEntry();
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = @"select card_game_id, isnull(isnull(profile_name, game_name), 'Any Game') [Game Profile / Game], Quantity Total, BalanceGames Balance,  
                                case Frequency when 'N' then 'None' when 'D' then 'Daily' when 'W' then 'Weekly' when 'M' then 'Monthly' when 'Y' then 'Yearly' when 'B' then 'Birthday' when 'A' then 'Anniversary' else 'None' end as Frequency, 
                                case isnull([Monday], 'Y') when 'Y' then 'Yes' else 'No' end mon,
                                case isnull([Tuesday], 'Y') when 'Y' then 'Yes' else 'No' end tue, 
                                case isnull([Wednesday], 'Y') when 'Y' then 'Yes' else 'No' end wed, 
                                case isnull([Thursday], 'Y') when 'Y' then 'Yes' else 'No' end thu, 
                                case isnull([Friday], 'Y') when 'Y' then 'Yes' else 'No' end fri, 
                                case isnull([Saturday], 'Y') when 'Y' then 'Yes' else 'No' end sat, 
                                case isnull([Sunday], 'Y') when 'Y' then 'Yes' else 'No' end sun, 
                                Fromdate as FromTime, 
                                ExpiryDate as Expiry, LastPlayedTime, TicketAllowed, EntitlementType, 
                                isnull(cg.ValidityStatus, 'Y') Validity 
                                from CardGames cg left outer join Games g 
                                on cg.game_id = g.game_id 
                                left outer join game_profile gp 
                                on gp.game_profile_id = cg.game_profile_id 
                                where card_id = @card_id 
                               and (BalanceGames > 0 
                                   or (BalanceGames < 0 
                                        and exists (select 1 
                                                    from CardGames cg2 
                                                    where cg2.card_id = cg.card_id 
                                                    and isnull(cg2.trxId, -1) = isnull(cg.TrxId, -1) 
                                                    and cg2.BalanceGames > 0))) 
                                order by 2";
            cmd.Parameters.AddWithValue("@card_id", CardId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            dgvCardGames.DataSource = dt1;
            dgvCardGames.Columns[0].Visible = false;

            Utilities.setupDataGridProperties(ref dgvCardGames);

            dgvCardGames.Columns["Game Profile / Game"].MinimumWidth = 200;
            dgvCardGames.BackgroundColor = panelPanel.BackColor;
            dgvCardGames.Columns["Balance"].DefaultCellStyle =
                dgvCardGames.Columns["Total"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

            dgvCardGames.Columns["FromTime"].DefaultCellStyle =
            dgvCardGames.Columns["LastPlayedTime"].DefaultCellStyle =
                dgvCardGames.Columns["Expiry"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();

            dgvCardGames.BorderStyle = BorderStyle.FixedSingle;
            log.LogMethodExit();
        }

        void getDetails()
        {
            log.LogMethodEntry();
            int thresholdCount = 5000;
            int ccpCount = 0;
            string strThresholdCount = ConfigurationManager.AppSettings["CREDITPLUS_THRESHOLD_COUNT"];
            if (!String.IsNullOrEmpty(strThresholdCount))
                thresholdCount = Convert.ToInt32(strThresholdCount);
            object ObjCCPCount = POSStatic.Utilities.executeScalar(@"select count(1) FROM [CardCreditPlus] cp 
                            where card_id = @CardId
                            and (@all = 'Y' or ((CreditPlusBalance != 0
                                                or exists (select 1
                                                            from cardCreditPlusConsumption cpc
                                                            where cpc.CardCreditPlusId = cp.CardCreditPlusId
                                                            and cpc.ConsumptionBalance > 0))
                                                and (PeriodTo is null or PeriodTo + 1 >= getdate())))",
                                                new SqlParameter("@CardId",CardId),
                                                new SqlParameter("@All", (rbAll.Checked ? "Y" : "N")));
            if (ObjCCPCount != null && ObjCCPCount != DBNull.Value)
                ccpCount = Convert.ToInt32(ObjCCPCount);

            SqlCommand cmd = POSStatic.Utilities.getCommand();
                cmd.CommandText = @"SELECT [CardCreditPlusId]
                                ,[Remarks] Name
                                ,isnull(l.Attribute, 'Other') Type
                                ,[CreditPlus] Amount
                                ,case [Refundable] when 'N' then 'No' else 'Yes' end refundable
                                ,[CreditPlusBalance] Balance
                                ,[MinimumSaleAmount] min_Purchase
                                ,[PeriodFrom] Period_from
                                ,[PeriodTo] Period_to
                                ,case [ExtendOnReload] when 'N' then 'No' else 'Yes' end extend_on_reload
                                ,[PlayStartTime] StartTime
                                ,[TimeFrom] Time_from
                                ,[TimeTo] Time_to
                                ,[NumberOfDays] valid_days
                                ,case isnull([Monday], 'Y') when 'Y' then 'Yes' else 'No' end mon
                                ,case isnull([Tuesday], 'Y') when 'Y' then 'Yes' else 'No' end tue
                                ,case isnull([Wednesday], 'Y') when 'Y' then 'Yes' else 'No' end wed
                                ,case isnull([Thursday], 'Y') when 'Y' then 'Yes' else 'No' end thu
                                ,case isnull([Friday], 'Y') when 'Y' then 'Yes' else 'No' end fri
                                ,case isnull([Saturday], 'Y') when 'Y' then 'Yes' else 'No' end sat
                                ,case isnull([Sunday], 'Y') when 'Y' then 'Yes' else 'No' end sun
                                ,TicketAllowed Ticket_Allowed
                                ,ISNULL(PauseAllowed,1) Pause_Allowed
                                ,cp.CreationDate Created_on
                                ,isnull(cp.ValidityStatus, 'Y') Validity
                            FROM [CardCreditPlus] cp left outer join LoyaltyAttributes l on cp.CreditPlusType = l.CreditPlusType
                            where card_id = @CardId
                            and (@all = 'Y' or ((CreditPlusBalance != 0
                                                or exists (select 1
                                                            from cardCreditPlusConsumption cpc
                                                            where cpc.CardCreditPlusId = cp.CardCreditPlusId
                                                            and cpc.ConsumptionBalance > 0))
                                                and (PeriodTo is null or PeriodTo + 1 >= getdate())))
                            order by cp.CreationDate desc";
            //cmd.Parameters.AddWithValue("@thresholdCount", thresholdCount);
            cmd.Parameters.AddWithValue("@CardId", CardId);
            cmd.Parameters.AddWithValue("@All", (rbAll.Checked ? "Y" : "N"));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCardCreditPlus.DataSource = dt;

            if (ccpCount > thresholdCount)
                POSStatic.Utilities.setupDataGridProperties(ref dgvCardCreditPlus, ignoreCellStyling: true);
            else
                POSStatic.Utilities.setupDataGridProperties(ref dgvCardCreditPlus, ignoreCellStyling: false);
            dgvCardCreditPlus.Columns[0].Visible = false;

            dgvCardCreditPlus.Columns["Amount"].DefaultCellStyle =
            dgvCardCreditPlus.Columns["Balance"].DefaultCellStyle =
            dgvCardCreditPlus.Columns["min_purchase"].DefaultCellStyle = POSStatic.Utilities.gridViewAmountCellStyle();

            dgvCardCreditPlus.Columns["StartTime"].DefaultCellStyle =
            dgvCardCreditPlus.Columns["Period_From"].DefaultCellStyle =
            dgvCardCreditPlus.Columns["Period_To"].DefaultCellStyle = POSStatic.Utilities.gridViewDateTimeCellStyle();
            dgvCardCreditPlus.BackgroundColor = panelPanel.BackColor;
            log.LogMethodExit();
        }

        void getChildRecords(object CardCreditPlusId)
        {
            log.LogMethodEntry(CardCreditPlusId);
            SqlCommand cmd = POSStatic.Utilities.getCommand();
            cmd.CommandText = @"SELECT Product_name from products p, CardCreditPlusPurchaseCriteria cp
                            where CardCreditPlusId = @CardCreditPlusId
                            and product_id = productId";
            cmd.Parameters.AddWithValue("@CardCreditPlusId", CardCreditPlusId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvPurchaseCriteria.DataSource = dt;

            POSStatic.Utilities.setupDataGridProperties(ref dgvPurchaseCriteria);
            dgvPurchaseCriteria.BackgroundColor = panelPanel.BackColor;
            dgvPurchaseCriteria.BorderStyle = BorderStyle.FixedSingle;

            cmd.CommandText = @"SELECT 
                                [POSTypeName] Counter
                                ,c.Name Category
                                ,[Product_name]
                                ,o.Name [Order Type]
                                ,[profile_name] Game_Profile
                                ,[Game_name]
                                ,[DiscountPercentage] disc_Percent
                                ,[DiscountAmount] Disc_Amount
                                ,[DiscountedPrice] Disc_Price
                                ,[ConsumptionBalance] Balance
                                ,cn.ExpiryDate expiry_date
                                ,cn.QuantityLimit Qty_Limit
                                FROM [CardCreditPlusConsumption] cn
                                left outer join POSTypes p
                                on p.POSTypeId = cn.POSTypeId 
                                left outer join Products pr
                                on pr.Product_Id = cn.ProductId
                                left outer join game_profile gp
                                on gp.game_profile_id = cn.gameProfileid 
                                left outer join games g 
                                on g.game_id = cn.gameId
                                left outer join Category c 
                                on c.categoryId = cn.CategoryId
                                left outer join OrderType o
                                on cn.OrderTypeId = o.Id
                                where CardCreditPlusId = @CardCreditPlusId";
            da.SelectCommand = cmd;
            DataTable dtCn = new DataTable();
            da.Fill(dtCn);

            dgvConsumption.DataSource = dtCn;

            POSStatic.Utilities.setupDataGridProperties(ref dgvConsumption);
            dgvConsumption.BackgroundColor = panelPanel.BackColor;
            dgvConsumption.BorderStyle = BorderStyle.FixedSingle;

            dgvConsumption.Columns["disc_Percent"].DefaultCellStyle =
            dgvConsumption.Columns["Disc_Amount"].DefaultCellStyle =
            dgvConsumption.Columns["Disc_Price"].DefaultCellStyle =
            dgvConsumption.Columns["Balance"].DefaultCellStyle =
            dgvConsumption.Columns["Qty_Limit"].DefaultCellStyle =
                POSStatic.Utilities.gridViewAmountCellStyle();

            switch (dgvCardCreditPlus.CurrentRow.Cells["Type"].Value.ToString())
            {
                default:
                case "Card Balance":
                    {
                        log.Info("getChildRecords(" + CardCreditPlusId + ") - Card Balance");//Added for logger function on 08-Mar-2016
                        dgvConsumption.Columns["Counter"].Visible
                                        = dgvConsumption.Columns["Category"].Visible
                                        = dgvConsumption.Columns["Product_name"].Visible
                                        = dgvConsumption.Columns["Game_Profile"].Visible
                                        = dgvConsumption.Columns["Game_name"].Visible = true;
                        break;
                    }
                case "Game Play":
                    {
                        log.Info("getChildRecords(" + CardCreditPlusId + ") - Game Play");//Added for logger function on 08-Mar-2016
                        dgvConsumption.Columns["Counter"].Visible =
                        dgvConsumption.Columns["Category"].Visible =
                        dgvConsumption.Columns["Product_name"].Visible =
                                    dgvConsumption.Columns["OrderType"].Visible = false;
                        dgvConsumption.Columns["Game_Profile"].Visible
                        = dgvConsumption.Columns["Game_name"].Visible = true;
                        break;
                    }
                case "Counter Items":
                    {
                        log.Info("getChildRecords(" + CardCreditPlusId + ") - Counter Items");//Added for logger function on 08-Mar-2016
                        dgvConsumption.Columns["Counter"].Visible =
                        dgvConsumption.Columns["Category"].Visible =
                        dgvConsumption.Columns["Product_name"].Visible =
                            dgvConsumption.Columns["OrderType"].Visible = true;
                        dgvConsumption.Columns["Game_Profile"].Visible
                        = dgvConsumption.Columns["Game_name"].Visible = false;
                        break;
                    }
            }
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void dgvCardCreditPlus_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                getChildRecords(dgvCardCreditPlus.SelectedRows[0].Cells[0].Value);
            }
            catch
            {
                log.Fatal("Ends-dgvCardCreditPlus_SelectionChanged() due to exception unable to fetch getChildRecords");//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void rbNonZeroBalance_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            getDetails();
            log.LogMethodExit();
        }

        private void btnCloseGames_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void tcGamesAndCreditPlus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (_type == 0 && tcGamesAndCreditPlus.SelectedTab.Equals(tpGames) && dgvCardGames.ColumnCount == 0)
                getGames();
            else if (_type == 1 && tcGamesAndCreditPlus.SelectedTab.Equals(tpCreditPlus) && dgvCardCreditPlus.ColumnCount == 0)
                getDetails();

            log.LogMethodExit();
        }

        private void dgvCardGames_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            dgvCardGameExtended.DataSource = Utilities.executeDataTable(@"select profile_name, game_name, exclude [Excluded?] 
                                                                            from cardGameExtended cge
                                                                            left outer join games g
                                                                            on g.game_id = cge.gameId
                                                                            left outer join game_profile gp
                                                                            on gp.game_profile_id = cge.gameProfileId
                                                                            where cardGameId = @cardGameId",
                                                                        new SqlParameter("cardGameId", dgvCardGames["card_game_id", e.RowIndex].Value));
            Utilities.setupDataGridProperties(ref dgvCardGameExtended);
            dgvCardGameExtended.BackgroundColor = panelPanel.BackColor;
            log.LogMethodExit();
        }
    }
}
