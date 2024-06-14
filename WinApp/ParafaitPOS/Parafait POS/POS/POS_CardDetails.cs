using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Semnox.Parafait.Customer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public partial class POS
    {
        private void displayCardActivity()
        {
            lastTrxActivityTime = DateTime.Now;
            try
            {
                POSUtils.displayCardActivity(CurrentCard, dataGridViewPurchases, dataGridViewGamePlay, false, lnkShowHideExtended.Tag.ToString().Equals("0") ? false : true);

                //Start Modififcation: Added to assign the cardnumber to textbox in activities tab on 14-feb-2017
                if (CurrentCard != null)
                {
                    TxtCardNumber.Text = CurrentCard.CardNumber;
                }
                else
                {
                    TxtCardNumber.Text = string.Empty;
                }
                //End Modififcation: Added to assign the cardnumber to textbox in activities tab on 14-feb-2017
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            lastTrxActivityTime = DateTime.Now;
        }

        void createCardDetailsGrid()
        {
            dataGridViewCardDetails.Columns[1].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dataGridViewCardDetails.RowsDefaultCellStyle = null;
            dataGridViewCardDetails.Columns[1].DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridViewCardDetails.Columns[1].DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridViewCardDetails.AlternatingRowsDefaultCellStyle = dataGridViewCardDetails.RowsDefaultCellStyle;
            dataGridViewCardDetails.Columns[1].DefaultCellStyle.Font = new Font("arial", 12, FontStyle.Bold);
            dataGridViewCardDetails.GridColor = Color.LightSteelBlue;
            dataGridViewCardDetails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCardDetails.BorderStyle = BorderStyle.None;

            dataGridViewCardDetails.Rows.Clear();
            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[0].Cells[0].Value = MessageUtils.getMessage("Issue Date");
            dataGridViewCardDetails.Rows[0].Cells[1].Style.Font = new Font("arial", 11, FontStyle.Bold);

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[1].Cells[0].Value = MessageUtils.getMessage("Card Deposit");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[2].Cells[0].Value = MessageUtils.getMessage("Credits");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[3].Cells[0].Value = MessageUtils.getMessage("Courtesy");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[4].Cells[0].Value = MessageUtils.getMessage("Bonus");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[5].Cells[0].Value = MessageUtils.getMessage("Time");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[6].Cells[0].Value = MessageUtils.getMessage("Games");

            dataGridViewCardDetails.Rows[6].Cells[1].Style = new DataGridViewCellStyle(dataGridViewCardDetails.Columns[1].DefaultCellStyle);
            dataGridViewCardDetails.Rows[6].Cells[1].Style.Font = new System.Drawing.Font(dataGridViewCardDetails.Columns[1].DefaultCellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            dataGridViewCardDetails.Rows[6].Cells[1].Style.ForeColor =
                dataGridViewCardDetails.Rows[6].Cells[1].Style.SelectionForeColor = Color.Navy;

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[7].Cells[0].Value = MessageUtils.getMessage("Credit Plus");

            dataGridViewCardDetails.Rows[7].Cells[1].Style = new DataGridViewCellStyle(dataGridViewCardDetails.Columns[1].DefaultCellStyle);
            dataGridViewCardDetails.Rows[7].Cells[1].Style.Font = new System.Drawing.Font(dataGridViewCardDetails.Columns[1].DefaultCellStyle.Font, FontStyle.Bold | FontStyle.Underline);
            dataGridViewCardDetails.Rows[7].Cells[1].Style.ForeColor =
                dataGridViewCardDetails.Rows[7].Cells[1].Style.SelectionForeColor = Color.Navy;

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[8].Height = 5;
            dataGridViewCardDetails.Rows[8].Cells[0].Style.BackColor =
            dataGridViewCardDetails.Rows[8].Cells[1].Style.BackColor = dataGridViewCardDetails.GridColor;
            
            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[9].Cells[0].Value = MessageUtils.getMessage(POSStatic.TicketTermVariant);

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[10].Cells[0].Value = MessageUtils.getMessage("Loyalty Points");

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[11].Cells[0].Value = MessageUtils.getMessage("Recharged/Spent");
            dataGridViewCardDetails.Rows[11].Cells[1].Style.Font = new Font("arial", 11, FontStyle.Bold);

            if (ParafaitEnv.LanguageCode != null && ParafaitEnv.LanguageCode.StartsWith("en", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                dataGridViewCardDetails.Columns[1].DefaultCellStyle.Font = new Font(dataGridViewCardDetails.Columns[1].DefaultCellStyle.Font, FontStyle.Regular);
            }

            dataGridViewCardDetails.Location = new Point(0, panelCardSwipe.Height - dataGridViewCardDetails.Rows.GetRowsHeight(DataGridViewElementStates.Displayed) - 3);
        }

        void clearCardDetails()
        {
            dataGridViewCardDetails.Rows[0].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[1].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[2].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[3].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[4].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[5].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[6].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[7].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[9].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[10].Cells[1].Value = "";
            dataGridViewCardDetails.Rows[11].Cells[1].Value = "";
            labelCardNo.Text = "";
            labelCardStatus.Text = "";
            textBoxCustomerInfo.Clear();
            txtVIPStatus.Clear();
            TxtCardNumber.Text = ""; // Added to clear the cardnumber textbox in activities on 14-feb-2017

            textBoxCustomerInfo.BackColor = POSBackColor;
            textBoxCustomerInfo.ForeColor = Color.White;

            txtVIPStatus.Width = 265;
            btnParentChild.Visible = false;
            btnCardBalancePrint.Visible = false;

            labelCardStatus.BackColor = labelCardNo.BackColor = POSBackColor;
            POSUtils.clearAlerts();
        }

        int CardNumberColorIndex = 0;
        private void displayCardDetails()
        {
            clearCardDetails();

            if (CurrentCard == null)
            {
                SetCustomerTextBoxInfo();
                return;
            }

            if (CurrentCard.ExpiryDate.Equals(DateTime.MinValue))
            {
                dataGridViewCardDetails.Rows[0].Cells[0].Value = MessageUtils.getMessage("Issue Date");
                dataGridViewCardDetails.Rows[0].Cells[1].Value = CurrentCard.issue_date.ToString(ParafaitEnv.DATETIME_FORMAT.Replace("yyyy", "yy").Replace(":ss", ""));
                dataGridViewCardDetails.Rows[0].Cells[1].ToolTipText = "";
            }
            else
            {
                dataGridViewCardDetails.Rows[0].Cells[0].Value = MessageUtils.getMessage("Expiry Date");
                dataGridViewCardDetails.Rows[0].Cells[1].Value = CurrentCard.ExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT.Replace("yyyy", "yy").Replace(":ss", ""));
                dataGridViewCardDetails.Rows[0].Cells[1].ToolTipText = MessageUtils.getMessage("Issue Date") + ": " + CurrentCard.issue_date.ToString(ParafaitEnv.DATETIME_FORMAT.Replace("yyyy", "yy").Replace(":ss", ""));
            }

            dataGridViewCardDetails.Rows[1].Cells[1].Value = CurrentCard.face_value.ToString(ParafaitEnv.AMOUNT_FORMAT);
            if (POSStatic.ADD_CREDITPLUS_IN_CARD_INFO == "Y")
            {
                dataGridViewCardDetails.Rows[2].Cells[1].Value = (CurrentCard.credits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits + CurrentCard.creditPlusItemPurchase).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[2].Cells[1].ToolTipText = CurrentCard.credits.ToString(ParafaitEnv.AMOUNT_FORMAT) + "+" + (CurrentCard.CreditPlusCredits + CurrentCard.CreditPlusCardBalance + CurrentCard.creditPlusItemPurchase).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[4].Cells[1].Value = (CurrentCard.bonus + CurrentCard.CreditPlusBonus).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[4].Cells[1].ToolTipText = CurrentCard.bonus.ToString(ParafaitEnv.AMOUNT_FORMAT) + "+" + CurrentCard.CreditPlusBonus.ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[5].Cells[1].Value = (CurrentCard.time + CurrentCard.CreditPlusTime).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[7].Cells[1].Value = "[" + (CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits + CurrentCard.creditPlusItemPurchase).ToString(ParafaitEnv.AMOUNT_FORMAT) + "]";
            }
            else
            {
                dataGridViewCardDetails.Rows[2].Cells[1].Value = CurrentCard.credits.ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[4].Cells[1].Value = CurrentCard.bonus.ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[5].Cells[1].Value = (CurrentCard.time).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[7].Cells[1].Value = (CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits + CurrentCard.CreditPlusBonus + CurrentCard.creditPlusItemPurchase).ToString(ParafaitEnv.AMOUNT_FORMAT);
                dataGridViewCardDetails.Rows[2].Cells[1].ToolTipText =
                dataGridViewCardDetails.Rows[4].Cells[1].ToolTipText = "";
            }

            dataGridViewCardDetails.Rows[3].Cells[1].Value = CurrentCard.courtesy.ToString(ParafaitEnv.AMOUNT_FORMAT);
            // dataGridViewCardDetails.Rows[5].Cells[1].Value = (CurrentCard.time + CurrentCard.CreditPlusTime).ToString(ParafaitEnv.AMOUNT_FORMAT);

            int gameCount = getGamesCount();
            dataGridViewCardDetails.Rows[6].Cells[1].Value = gameCount.ToString(ParafaitEnv.NUMBER_FORMAT);

            dataGridViewCardDetails.Rows[7].Cells[1].ToolTipText = CurrentCard.CreditPlusCardBalance.ToString(ParafaitEnv.AMOUNT_FORMAT) + "+" + CurrentCard.CreditPlusCredits.ToString(ParafaitEnv.AMOUNT_FORMAT) + "+" + CurrentCard.CreditPlusBonus.ToString(ParafaitEnv.AMOUNT_FORMAT) + "+" + CurrentCard.creditPlusItemPurchase.ToString(ParafaitEnv.AMOUNT_FORMAT);

            dataGridViewCardDetails.Rows[9].Cells[1].Value = (CurrentCard.ticket_count + CurrentCard.CreditPlusTickets).ToString(ParafaitEnv.NUMBER_FORMAT);
            dataGridViewCardDetails.Rows[10].Cells[1].Value = (CurrentCard.loyalty_points + CurrentCard.TotalCreditPlusLoyaltyPoints).ToString(ParafaitEnv.AMOUNT_FORMAT) + " / " + (CurrentCard.loyalty_points + CurrentCard.RedeemableCreditPlusLoyaltyPoints).ToString(ParafaitEnv.AMOUNT_FORMAT);

            CurrentCard.getTotalRechargeAmount();
            dataGridViewCardDetails.Rows[11].Cells[1].Value = CurrentCard.TotalRechargeAmount.ToString(ParafaitEnv.AMOUNT_FORMAT) + "/" + CurrentCard.credits_played.ToString(ParafaitEnv.AMOUNT_FORMAT);

            labelCardNo.Text = CurrentCard.CardNumber;

            Color c = labelCardNo.ForeColor;
            Color[] colors = { Color.White, Color.Yellow };
            CardNumberColorIndex++;
            if (CardNumberColorIndex == colors.Length)
                CardNumberColorIndex = 0;
            labelCardNo.ForeColor = (Color)colors.GetValue(CardNumberColorIndex);

            labelCardNo.BackColor = labelCardStatus.BackColor = Color.MidnightBlue;

            if (CurrentCard.CardStatus == "NEW")
            {
                labelCardStatus.Text = MessageUtils.getMessage("New Card");
                labelCardStatus.ForeColor = Color.OrangeRed;
                txtVIPStatus.Text = MessageUtils.getMessage("New");
                txtVIPStatus.ForeColor = Color.Black;
                txtVIPStatus.Font = new Font(txtVIPStatus.Font, FontStyle.Regular);
            }
            else
            {
                if (CurrentCard.CardStatus == "ISSUED")
                    labelCardStatus.Text = MessageUtils.getMessage("Issued Card");
                else
                    labelCardStatus.Text = MessageUtils.getMessage("Expired Card");

                labelCardStatus.ForeColor = Color.LightGreen;
                SetTxtVIPStatusTextBox();

                object o = Utilities.executeScalar(@"select top 1 c.card_number
                                                    from ParentChildCards pc, cards c
                                                    where ParentCardId = @cardId 
                                                    and c.card_id = pc.ParentCardId 
                                                    union all
                                                    select top 1 c.card_number
                                                    from ParentChildCards pc, cards c
                                                    where ChildCardId = @cardId 
                                                    and c.card_id = pc.ParentCardId",
                                                     new SqlParameter("@cardId", CurrentCard.card_id));

                //-Following section was added on 02-07-2015
                if (CurrentCard.isMifare)
                {
                    txtVIPStatus.Width = 295;
                    btnParentChild.Visible = false;
                }
                else
                {
                    btnParentChild.Visible = true;
                }
                //changes on 02-07-2015 end

                if (o != null)
                {
                    // btnParentChild.Visible = true; //The line was commented on 02-07-2015
                    btnParentChild.Tag = o;
                    txtVIPStatus.Width -= btnParentChild.Width;
                }
                else
                {
                    btnParentChild.Tag = (object)CurrentCard.CardNumber;
                    txtVIPStatus.Width -= btnParentChild.Width;
                }
            }

            if (CurrentCard.customer_id == -1)
            {
                textBoxCustomerInfo.Text = "";
                SetCustomerTextBoxInfo();
            }
            else
            {
                textBoxCustomerInfo.Text = CurrentCard.customerDTO.FirstName + (string.IsNullOrEmpty(CurrentCard.customerDTO.LastName) ? "" : " " + CurrentCard.customerDTO.LastName);
                if (CurrentCard.customerDTO.AddressDTOList != null &&
                    CurrentCard.customerDTO.AddressDTOList.Count > 0 &&
                    !string.IsNullOrEmpty(CurrentCard.customerDTO.LatestAddressDTO.City))
                    textBoxCustomerInfo.AppendText(", " + CurrentCard.customerDTO.LatestAddressDTO.City);
                textBoxCustomerInfo.AppendText(Environment.NewLine);

                if (CurrentCard.customerDTO.DateOfBirth != null)
                {
                    textBoxCustomerInfo.AppendText("DOB: " + CurrentCard.customerDTO.DateOfBirth.Value.ToString("M"));
                    if (CurrentCard.customerDTO.DateOfBirth.Value.DayOfYear >= DateTime.Now.DayOfYear - 1 &&
                        CurrentCard.customerDTO.DateOfBirth.Value.DayOfYear <= DateTime.Now.DayOfYear + 1)
                    {
                        textBoxCustomerInfo.BackColor = Color.Red;
                        textBoxCustomerInfo.ForeColor = Color.Black;
                    }
                }

                if (CurrentCard.customerDTO.DateOfBirth != null)
                {
                    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, CurrentCard.customerDTO);
                    int customerAge = customerBL.GetAge();
                    // Config check to display customer Age
                    if (Utilities.getParafaitDefaults("SHOW_CUSTOMER_AGE_ONSCREEN").Equals("Y"))
                    {
                        textBoxCustomerInfo.AppendText(Environment.NewLine);
                        textBoxCustomerInfo.AppendText(MessageContainerList.GetMessage(Utilities.ExecutionContext, "AGE") + ":" + customerAge);
                    }
                }
            }

            dataGridViewCardDetails.Refresh();

            //displayCustomerDetails(CurrentCard.customerDTO);

            if (tabControlCardAction.TabPages.Count > 0)
            {
                if (tabControlCardAction.SelectedTab.Name == "tabPageActivities")
                    displayCardActivity();
                //May 23 2016
                //else if (tabControlCardAction.SelectedTab.Name == "tabPageCardCustomer")
                //displayCustomerDetails(CurrentCard.customerDTO);
                else if (tabControlCardAction.SelectedTab.Name == "tabPageCardInfo")
                    displayCardInfo();
                else if (tabControlCardAction.SelectedTab.Name == "tabPageTrx"
                    && Utilities.ParafaitEnv.AUTO_POPUP_CARD_PROMOTIONS_IN_POS == "Y"
                    && CurrentCard.card_id > 0)
                {
                    SqlCommand cmd = Utilities.getCommand();
                    cmd.CommandText = @"select top 1 1 
                                    from CardCreditPlus cp
                                   where card_id = @card_id 
                                    and LoyaltyRuleId is not null
                                    and (CreditPlusBalance > 0
                                        or exists (select 1
                                                    from cardCreditPlusConsumption cpc
                                                    where cpc.CardCreditPlusId = cp.CardCreditPlusId
                                                    and cpc.ConsumptionBalance > 0))";
                    cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
                    if (cmd.ExecuteScalar() != null)
                    {
                        CreditPlusDetails cpd = new CreditPlusDetails(CurrentCard.card_id);
                        cpd.ShowDialog();
                    }
                }
            }
            POSUtils.displayCardAlerts(CurrentCard, lblAlerts);
        }        

        int getGamesCount()
        {
            if (CurrentCard.isMifare)
                return CurrentCard.CardGames;
            else
            {
                int cgCount = 0;
                if (CurrentCard.card_id >= 0)
                {
                    AccountBL accountBl = new AccountBL(POSStatic.Utilities.ExecutionContext, CurrentCard.card_id);
                    if (accountBl.AccountDTO != null)
                    {
                        cgCount = accountBl.AccountDTO.AccountSummaryDTO.AccountGameBalance;
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2336));
                    }
                }
                return cgCount;
            }
        }

        private void displayCardInfo()
        {
            if (CurrentCard == null || CurrentCard.CardStatus == "NEW")
            {
                dgvCardGames.DataSource = null;
                dgvCardDiscounts.DataSource = null;
                dgvCreditPlus.Columns.Clear();
                dgvCardGames.Rows.Clear();
                dgvCardDiscounts.Rows.Clear();
                dgvCreditPlus.Rows.Clear();
                lblTicketMode.Text = "";
                lblTicketAllowed.Text = "";
                lblRoamingCard.Text = "";
                return;
            }

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select isnull(isnull(profile_name, game_name), 'Any Game') \"Profile / Game\", Quantity Total, BalanceGames Balance, " + 
                                "case Frequency when 'N' then 'None' when 'D' then 'Daily' when 'W' then 'Weekly' when 'M' then 'Monthly' when 'Y' then 'Yearly' else 'None' end as Frequency, " + 
                                "ExpiryDate as Expiry " +
                                "from CardGames cg left outer join Games g " +
                                "on cg.game_id = g.game_id " +
                                "left outer join game_profile gp " +
                                "on gp.game_profile_id = cg.game_profile_id " +
                                "where card_id = @card_id " +
                               @"and (BalanceGames > 0 
                                   or (BalanceGames < 0 
                                        and exists (select 1 
                                                    from CardGames cg2 
                                                    where cg2.card_id = cg.card_id 
                                                    and isnull(cg2.trxId, -1) = isnull(cg.TrxId, -1) 
                                                    and cg2.BalanceGames > 0))) " +
                                "order by 1 ";
            cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            dgvCardGames.DataSource = dt1;

            Utilities.setupDataGridProperties(ref dgvCardGames);
            CommonFuncs.setupDataGridProperties(dgvCardGames);

            dgvCardGames.BackgroundColor = POSBackColor;
            dgvCardGames.Columns["Balance"].DefaultCellStyle = dgvCardGames.Columns["Total"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvCardGames.Columns["Expiry"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvCardGames.Columns["Expiry"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvCardGames.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCardGames.Columns[1].AutoSizeMode =
            dgvCardGames.Columns[2].AutoSizeMode =
            dgvCardGames.Columns[3].AutoSizeMode =
            dgvCardGames.Columns[4].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.None;
            dgvCardGames.Columns[1].Width = 35;
            dgvCardGames.Columns[2].Width = 50;
            dgvCardGames.Columns[3].Width =
            dgvCardGames.Columns[4].Width = 65;
            try
            {
                dgvCardGames.RowsDefaultCellStyle.SelectionBackColor = dgvCardGames.DefaultCellStyle.BackColor;
                dgvCardGames.RowsDefaultCellStyle.SelectionForeColor = dgvCardGames.RowsDefaultCellStyle.ForeColor;
            }
            catch { }

            cmd.CommandText = @"select case when isnull(CreditPlus, 0) = 0 then cn.ConsumptionBalance else CreditPlusBalance end CreditPlus,
                                isnull(isnull(isnull(isnull(POSTypeName, product_name), profile_name), game_name), l.Attribute) Availability,
                                cp.PeriodTo Expiry,
                                isnull(convert(varchar, cn.DiscountedPrice), convert(varchar, cn.DiscountPercentage) + '%') Disc, 
                                cp.CardCreditPlusId, isnull(CreditPlus, 0) CreditPlusLoaded 
                                from CardCreditPlus cp left outer join CardCreditPlusConsumption cn 
                                on cp.CardCreditPlusId = cn.CardCreditPlusId 
                                left outer join POSTypes p
                                on p.POSTypeId = cn.POSTypeId
                                left outer join Products pr
                                on pr.Product_Id = cn.ProductId
                                left outer join game_profile gp
                                on gp.game_profile_id = cn.gameProfileid
                                left outer join games g
                                on g.game_id = cn.gameId,
                                LoyaltyAttributes l
                                where cp.card_id = @card_id
                                and l.CreditPlusType = cp.CreditPlusType
                                and (CreditPlusBalance > 0 or cn.ConsumptionBalance > 0)
                                and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                  and (cp.PeriodTo is null or cp.PeriodTo + 1 > getdate())
                                  order by 4 ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            SqlDataAdapter daCp = new SqlDataAdapter(cmd);
            DataTable dtCp = new DataTable();
            daCp.Fill(dtCp);
            dgvCreditPlus.Rows.Clear();
            dgvCreditPlus.Columns.Clear();
            foreach (DataColumn dc in dtCp.Columns)
                dgvCreditPlus.Columns.Add(dc.ColumnName, dc.ColumnName);
            dgvCreditPlus.Columns[dtCp.Columns.Count - 1].Visible = false;
            dgvCreditPlus.Columns[dtCp.Columns.Count - 2].Visible = false;

            if (dtCp.Rows.Count > 0)
            {
                int prevCardCreditPlusId = -1;
                for (int i = 0; i < dtCp.Rows.Count; i++)
                {
                    if (prevCardCreditPlusId == Convert.ToInt64(dtCp.Rows[i][4]) && Convert.ToDecimal(dtCp.Rows[i]["CreditPlusLoaded"]) != 0)
                    {
                        dgvCreditPlus.Rows.Add(new object[] { null, dtCp.Rows[i][1], dtCp.Rows[i][2] });
                        dgvCreditPlus.Rows[i - 1].Cells[0].Style.BackColor = dgvCreditPlus.Rows[i].Cells[0].Style.BackColor = dgvCreditPlus.GridColor;
                        dgvCreditPlus.Rows[i - 1].Cells[0].Style.SelectionBackColor = dgvCreditPlus.Rows[i].Cells[0].Style.SelectionBackColor = dgvCreditPlus.DefaultCellStyle.BackColor;
                        dgvCreditPlus.Rows[i - 1].Cells[0].Style.SelectionForeColor = dgvCreditPlus.Rows[i].Cells[0].Style.SelectionForeColor = dgvCreditPlus.RowsDefaultCellStyle.ForeColor;
                    }
                    else
                        dgvCreditPlus.Rows.Add(new object[] { dtCp.Rows[i][0], dtCp.Rows[i][1], dtCp.Rows[i][2], dtCp.Rows[i][3] });
                    prevCardCreditPlusId = Convert.ToInt32(dtCp.Rows[i][4]);
                }
            }

            Utilities.setupDataGridProperties(ref dgvCreditPlus);
            CommonFuncs.setupDataGridProperties(dgvCreditPlus);

            dgvCreditPlus.BackgroundColor = POSBackColor;
            dgvCreditPlus.Columns[0].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvCreditPlus.Columns["Expiry"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            try
            {
                dgvCreditPlus.RowsDefaultCellStyle.SelectionBackColor = dgvCreditPlus.DefaultCellStyle.BackColor;
                dgvCreditPlus.RowsDefaultCellStyle.SelectionForeColor = dgvCreditPlus.RowsDefaultCellStyle.ForeColor;
            }
            catch { }

            cmd.CommandText = "select discount_name discount, discount_percentage \"%\", expiry_date expiry " +
                                "from CardDiscounts cd, Discounts d " +
                                "where cd.Card_id = @card_id " +
                                "and cd.discount_id = d.discount_id " +
                                " AND ISNULL(cd.IsActive, 'Y') = 'Y' " +
                                "and (cd.expiry_date is null or cd.expiry_date > getdate()) " +
                                "and d.active_flag = 'Y' " +
                                "order by 1 ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            da.SelectCommand = cmd;
            DataTable dtDisc = new DataTable();
            da.Fill(dtDisc);
            dgvCardDiscounts.DataSource = dtDisc;
            Utilities.setupDataGridProperties(ref dgvCardDiscounts);
            CommonFuncs.setupDataGridProperties(dgvCardDiscounts);

            dgvCardDiscounts.BackgroundColor = POSBackColor;
            dgvCardDiscounts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCardDiscounts.Columns[2].DefaultCellStyle = Utilities.gridViewDateCellStyle();
            try
            {
                dgvCardDiscounts.RowsDefaultCellStyle.SelectionBackColor = dgvCardDiscounts.DefaultCellStyle.BackColor;
                dgvCardDiscounts.RowsDefaultCellStyle.SelectionForeColor = dgvCardDiscounts.RowsDefaultCellStyle.ForeColor;
            }
            catch { }

            if (CurrentCard.ticket_allowed == 'Y')
            {
                lblTicketAllowed.Text = MessageUtils.getMessage("Ticket Allowed") + ":";
                lblTicketAllowed.ForeColor = Color.Orange;

                if (CurrentCard.real_ticket_mode == 'Y')
                {
                    lblTicketMode.Text = MessageUtils.getMessage("Physical Ticket Mode");
                }
                else
                {
                    lblTicketMode.Text = MessageUtils.getMessage("e-Ticket Mode");
                }
            }
            else
            {
                lblTicketAllowed.Text = MessageUtils.getMessage("Ticket Not Allowed");
                lblTicketAllowed.ForeColor = Color.Red;
                lblTicketMode.Text = "";
            }

            cmd.CommandText = "select 1 from cards c, site s " +
                                " where (s.site_id = c.site_id or c.site_id is null) " +
                                "and c.card_id = @card_id ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            object o = cmd.ExecuteScalar();
            if (o == null) // not found
            {
                lblRoamingCard.Text = MessageUtils.getMessage("Roaming Card");
                lblRoamingCard.ForeColor = Color.Red;
            }
            else
            {
                lblRoamingCard.Text = MessageUtils.getMessage("Local Card");
                lblRoamingCard.ForeColor = Color.Orange;
            }
            refreshCardInfoChart();

            dgvCardGames.BorderStyle = dgvCreditPlus.BorderStyle = dgvCardDiscounts.BorderStyle = BorderStyle.FixedSingle;
        }

        void refreshCardInfoChart()
        {
            if (CurrentCard != null)
                CardInfoBarChart.CreateGraph(zedGraphCardInfo, CurrentCard.card_id, dtpGraphFrom.Value.Date.AddHours(6), dtpGraphTo.Value.Date.AddDays(1).AddHours(6), rdCount.Checked);
        }

        private void btnRefreshGraph_Click(object sender, EventArgs e)
        {
            refreshCardInfoChart();
        }

        //private void displayCustomerDetails(CustomerDTO customerDTO)
        //{
        //    try
        //    {
        //        if (customerDetailUI != null)
        //        {
        //            if (customerDTO == null)
        //            {
        //                customerDTO = new CustomerDTO();
        //            }
        //            customerDetailUI.CustomerDTO = customerDTO;
        //            CurrentActiveTextBox = customerDetailUI.Controls.Find("txtFirstName", true)[0];
        //            CurrentActiveTextBox.Focus();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        POSUtils.ParafaitMessageBox(ex.Message);
        //    }
        //}

        private void SetTxtVIPStatusTextBox()
        {
            log.LogMethodEntry();
            if (CurrentCard != null)
            {
                if (CurrentCard.vip_customer == 'Y')
                {
                    //txtVIPStatus.Text = "VIP" + (CurrentCard.CardTypeId == -1 ? "" : "-" + CurrentCard.CardType);
                    txtVIPStatus.Text = "VIP" + (CurrentCard.MembershipId == -1 ? "" : "-" + CurrentCard.MembershipName);
                    txtVIPStatus.ForeColor = Color.Red;
                    txtVIPStatus.Font = new Font(txtVIPStatus.Font, FontStyle.Bold);
                }
                else
                {
                    //txtVIPStatus.Text = CurrentCard.CardType;
                    txtVIPStatus.Text = CurrentCard.MembershipName;
                    txtVIPStatus.ForeColor = Color.Black;
                    txtVIPStatus.Font = new Font(txtVIPStatus.Font, FontStyle.Regular);
                }
            }
            else
            {
                if (customerDTO != null)
                {
                    string membershipName = Utilities.MessageUtils.getMessage("Normal");
                    if (customerDTO.MembershipId > -1)
                    {
                        CustomerBL custBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                        membershipName = custBL.GetMembershipName();
                    }
                    txtVIPStatus.Text = membershipName;
                    txtVIPStatus.ForeColor = Color.Black;
                    txtVIPStatus.Font = new Font(txtVIPStatus.Font, FontStyle.Regular);
                }
            }
            log.LogMethodExit();
        }
    }
}
