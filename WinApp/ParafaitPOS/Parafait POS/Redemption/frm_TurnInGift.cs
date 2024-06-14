/*  Date          Modification                                          Done by         Version
 *  12-Sep-2018   Redemption Reversal changes                           Guru S A        2.4.0     
 *  12-Feb-2019   Redemption gift search changes                        Archana         2.5.0
 *  11-Apr-2019   Include/Exclude for redeemable products               Archana         2.6.0
 *  10-Sep-2019   Added logger to the methods                           Jinto Thomas    2.8.0
 *  26-Sep-2019   Redemption currency rule enhancement                  Dakshakh        2.8.0
*/
using System;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public partial class frm_redemption : Form
    {
        class clsTurnInProducts
        {
            public object productId;
            public string Code;
            public string Description;
            public int Quantity;
            public int TurnInPriceInTickets;
        }

        List<clsTurnInProducts> turnInList = new List<clsTurnInProducts>();

        private void btnTurnInClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                txtTurnInProdDesc.Text = "";
                txtTurnInProdCode.Text = "";
                txtFeedback.Text = "";
                dgvTurnInProducts.DataSource = null;
                turnInList.Clear();
                refreshSelectedTurnInDGV();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnTurnInProductSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                dgvTurnInProducts.DataSource = getTurnInGifts(txtTurnInProdCode.Text + "%", "%" + txtTurnInProdDesc.Text + "%", 'P');
                dgvTurnInProducts.Columns["ProductId"].Visible = false;
                dgvTurnInProducts.Columns["Price"].DefaultCellStyle =
                dgvTurnInProducts.Columns["Quantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvTurnInProducts.Columns["Price"].HeaderText = POSStatic.TicketTermVariant;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        DataTable getTurnInGifts(string code, string desc, char type)
        {
            log.LogMethodEntry(code, desc, type);
            SqlCommand cmd = Utilities.getCommand();
            //24-Mar-2016
            cmd.CommandText = "select top 100 Code, P.Description, isnull(TurnInPriceInTickets, 0) Price, I.Quantity, P.ProductId " +
                              "from Product P join products ps on p.ManualProductId = ps.product_id  " +
                              "left outer join Inventory I " +
                                "on P.productId = I.ProductId " +
                                "and I.LocationId = (select isnull(pos.InventoryLocationId, p.defaultLocationId) " +
                                                   "from (select 1 a) v left outer join POSMachines pos " +
                                                        "on POSMachineId = @posId) " +
                                @"left outer join (select * 
														 from (
																select *, row_number() over(partition by productid order by productid) as num 
																from productbarcode 
																where BarCode = @bar_code and isactive = 'Y')v 
														 where num = 1) b on p.productid = b.productid 
                              where ((@type = 'B' and b.BarCode = @bar_code) OR (@type = 'P' and Code like @product_code)) 
                                and (P.Description like @product_desc or P.ProductName like @product_code)
                                and P.IsActive = 'Y'
                                 and not exists (select 1 from redemptionCurrency rc where rc.productId = P.productId) 
                                and P.IsRedeemable = 'Y'
                                and not exists (select 1 
											  from ProductsDisplayGroup pd , 
														   ProductDisplayGroupFormat pdgf,
														   POSProductExclusions ppe 
											  where ps.product_id = pd.ProductId 
											  and pd.DisplayGroupId = pdgf.Id 
											  and ppe.ProductDisplayGroupFormatId = pdgf.Id
											  and ppe.POSMachineId = @posId ) 
									and not exists (select 1 
													from ProductsDisplayGroup pd , 
														     ProductDisplayGroupFormat pdgf,
														     UserRoleDisplayGroupExclusions urdge , 
														     users u
													where  ps.product_id = pd.ProductId 
													and pd.DisplayGroupId = pdgf.Id 
													and urdge.ProductDisplayGroupId = pdgf.Id
                                                    and urdge.role_id = u.role_id
                                                    and u.loginId = @loginId )
                                  Order by Code ";
            //24-Mar-2016
            cmd.Parameters.AddWithValue("@posId", ParafaitEnv.POSMachineId);
            cmd.Parameters.AddWithValue("@bar_code", code);
            cmd.Parameters.AddWithValue("@product_code", code);
            cmd.Parameters.AddWithValue("@product_desc", desc);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@loginId", ParafaitEnv.LoginID);

            log.LogVariableState("@posId", ParafaitEnv.POSMachineId);
            log.LogVariableState("@bar_code", code);
            log.LogVariableState("@product_code", code);
            log.LogVariableState("@product_desc", desc);
            log.LogVariableState("@type", type);
            log.LogVariableState("@loginId", ParafaitEnv.LoginID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 1 && type == 'B')
            {
                clsTurnInProducts item = turnInList.Find(delegate (clsTurnInProducts prod) { return ((int)prod.productId == (int)dt.Rows[0]["ProductId"]); });
                if (item != null)
                {
                    item.Quantity++;
                }
                else
                {
                    item = new clsTurnInProducts();
                    item.productId = dt.Rows[0]["ProductId"];
                    item.Code = dt.Rows[0]["Code"].ToString();
                    item.Description = dt.Rows[0]["Description"].ToString();
                    item.Quantity = 1;
                    item.TurnInPriceInTickets = Convert.ToInt32(dt.Rows[0]["Price"]);

                    turnInList.Add(item);

                    populateTurnInLocations(item.productId);
                }
                refreshSelectedTurnInDGV();
            }
            log.LogMethodExit(dt);
            return dt;
        }

        private void btnTurnInSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                btnPrintTurnIn.Tag = null;
                btnPrintTurnIn.Enabled = false;

                if (ParafaitEnv.ALLOW_REDEMPTION_WITHOUT_CARD == "N"
                    && dgvTurnInCard.RowCount < 1)
                {
                    txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,257);
                    log.Info("Ends-btnTurnInSave_Click() as no card was tapped");//Added for logger function on 08-Mar-2016
                    this.ActiveControl = dgvTurnInCard;
                    log.LogMethodExit();
                    return;
                }

                if (turnInList.Count == 0)
                {
                    txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,119);
                    log.Info("Ends-btnTurnInSave_Click() as Please select gift(s) before saving");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }

                //Updated condition to check the combobox value instead of textbox value 29-May-2017
                if (cmbTurninFromLocation.SelectedValue == null)
                {
                    txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,511);
                    log.Info("Ends-btnTurnInSave_Click() as not selected the Turn-In Location to receive gift from");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }

                if (cmbTargetLocation.SelectedValue == null)
                {
                    txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,806);
                    log.Info("Ends-btnTurnInSave_Click() as not selected the target location");//Added for logger function on 08-Mar-2016
                    this.ActiveControl = cmbTargetLocation;
                    log.LogMethodExit();
                    return;
                }

                int redemptionId = -1;
                if (UpdateTurnInsToDB(ref redemptionId))
                {
                    txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,122);
                    log.Info("btnTurnInSave_Click() - Save Successful");//Added for logger function on 08-Mar-2016
                    btnTurnInProductSearch.PerformClick();
                    dgvTurnInCard.Rows.Clear();
                    turnInList.Clear();
                    refreshSelectedTurnInDGV();
                    btnPrintTurnIn.Tag = redemptionId;
                    btnPrintTurnIn.Enabled = true;
                    if (POSStatic.AUTO_PRINT_REDEMPTION_RECEIPT)
                        btnPrintTurnIn.PerformClick();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private bool UpdateTurnInsToDB(ref int redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            FireSetLastActivityTime();
            int totalTickets = 0;
            object turnInlocationId = cmbTurninFromLocation.SelectedValue;
            SqlCommand cmd = Utilities.getCommand(Utilities.createConnection().BeginTransaction());
            SqlTransaction cmd_trx = cmd.Transaction;
            clsRedemption redemption = new clsRedemption(Utilities);

            try
            {
                foreach (clsTurnInProducts item in turnInList)
                {
                    totalTickets += item.TurnInPriceInTickets * item.Quantity;
                }
                try
                {
                    RedemptionReversalTurnInLimitCheck(totalTickets);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, 
                                                MessageContainerList.GetMessage(Utilities.ExecutionContext,"TurnIn Approval") + 
                                                MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                    log.Error(ex);
                    log.LogMethodExit(false);
                    return false;
                }
                foreach (clsTurnInProducts item in turnInList)
                {
                    string message = "";
                    if (InvUtils.ReceiveTurnInGift(item.productId, turnInlocationId, cmbTargetLocation.SelectedValue, item.Quantity, cmd_trx, ref message) == false)
                    {
                        cmd_trx.Rollback();
                        POSUtils.ParafaitMessageBox(message, MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                        log.Warn("UpdateTurnInsToDB() - ReceiveTurnInGift is false error :" + message);//Added for logger function on 08-Mar-2016
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                if (dgvTurnInCard.Rows.Count > 0)
                {
                    cmd.CommandText = "Insert into Redemption (card_id, primary_card_number, manual_tickets, eTickets, redeemed_date, LastUpdatedBy, remarks, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, CreatedBy, CreationDate , customerId, posmachineid) " +
                                       " Values(@card_id, @primary_card_number,@manual_tickets, @eTickets, getdate(), @LastUpdatedBy, 'TURNINREDEMPTION', @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CreatedBy, getdate(), @CustomerId, @PosMachineId); SELECT @@IDENTITY";


                    cmd.Parameters.AddWithValue("@card_id", dgvTurnInCard.Rows[0].Cells["cardIdTICard"].Value);
                    cmd.Parameters.AddWithValue("@primary_card_number", dgvTurnInCard.Rows[0].Cells["cardNumberTICard"].Value.ToString());
                    if (string.IsNullOrEmpty(dgvTurnInCard.Rows[0].Cells["customerIdTICard"].Value.ToString()) == false
                        && Convert.ToInt32(dgvTurnInCard.Rows[0].Cells["customerIdTICard"].Value) > -1)
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", dgvTurnInCard.Rows[0].Cells["customerIdTICard"].Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@manual_tickets", 0);
                    cmd.Parameters.AddWithValue("@eTickets", totalTickets * -1);
                    cmd.Parameters.AddWithValue("@Source", "POS Redemption");
                    cmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                    cmd.Parameters.AddWithValue("@RedemptionOrderNo", redemption.GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                    cmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                    cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                    if (ParafaitEnv.POSMachineId > -1)
                    {
                        cmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PosMachineId", DBNull.Value);
                    }
                    redemptionId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());

                    cmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                    " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                    try
                    {
                        //Insert into redemption cards table
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@card_no", dgvTurnInCard.Rows[0].Cells["cardNumberTICard"].Value.ToString());
                        cmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                        cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@card_id", dgvTurnInCard.Rows[0].Cells["cardIdTICard"].Value);
                        cmd.Parameters.AddWithValue("@ticket_count", totalTickets * -1);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception Ex)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,123, Ex.Message),
                                                  MessageContainerList.GetMessage(Utilities.ExecutionContext, "Save Information")
                                                  + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                        log.Fatal("Ends-UpdateTurnInsToDB() due to exception Error creating redemption cards information. Error: " + Ex.Message);//Added for logger function on 08-Mar-2016
                        cmd_trx.Rollback();
                        log.LogMethodExit(false);
                        return false;
                    }

                    //cmd.CommandText = "update cards set ticket_count = isnull(ticket_count, 0) - @ticket_count where card_id = @card_id";
                    //cmd.ExecuteNonQuery();
                    Loyalty Loyalty = new Loyalty(Utilities);
                    Loyalty.CreateGenericCreditPlusLine(Convert.ToInt32(dgvTurnInCard.Rows[0].Cells["cardIdTICard"].Value), "T", totalTickets, false, 0, "N", loginID, "Redemption turn in Tickets", cmd_trx, DateTime.MinValue, Utilities.getServerTime());


                    foreach (clsTurnInProducts item in turnInList)
                    {
                        //Insert into redemption gifts table 
                        int i = item.Quantity;
                        while (i-- > 0)
                        {
                            cmd.CommandText = "Insert into Redemption_gifts (redemption_id, gift_code, productid, locationId, Tickets, OriginalPriceInTickets, LastUpdatedBy, CreationDate, CreatedBy)  " +
                                               @" select @redemption_id, Code, productid, @inLocationId, TurnInPriceInTickets * -1 , PriceInTickets * -1, @LastUpdatedBy, getdate(), @CreatedBy
                                            from product
                                            where ProductId = @prod_id; SELECT @@IDENTITY";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                            cmd.Parameters.AddWithValue("@inLocationId", turnInlocationId);
                            cmd.Parameters.AddWithValue("@prod_id", item.productId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.Parameters.AddWithValue("@POSMachine", ParafaitEnv.POSMachineId);
                            int redemptionGiftId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());

                            cmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets], [TurnInTickets] ,[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate], [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId]  )
                                              select redemption_id, Redemption_Gifts_Id, null ,null, null, null, null, null, null, null, null, 
                                                     tickets, @CreatedBy, getdate(), @LastUpdatedBy ,getdate(), null, null, null 
                                        from Redemption_gifts
                                        where Redemption_Gifts_Id = @redemptionGiftId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets], [TurnInTickets] ,[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate],  [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId]  )
                                              select redemption_id,null, null ,null, null, null, null, null, null, null, null, 
                                                     tickets*-1, @CreatedBy, getdate(), @LastUpdatedBy ,getdate(), null, null, null
                                        from Redemption_gifts
                                        where Redemption_Gifts_Id = @redemptionGiftId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    cmd.CommandText = "Insert into Redemption (manual_tickets, eTickets, redeemed_date, LastUpdatedBy, remarks, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, customerId, PosMachineId) " +
                                        " Values(@manual_tickets, @eTickets, getdate(), @LastUpdatedBy, 'TURNINREDEMPTION', @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CustomerId, @PosmachineId); SELECT @@IDENTITY";

                    cmd.Parameters.AddWithValue("@manual_tickets", totalTickets * -1);
                    cmd.Parameters.AddWithValue("@eTickets", 0);
                    cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID); //Updated to see that LoginID is considered instead of ParafaitEnv.LoginID 30-May-2017
                    cmd.Parameters.AddWithValue("@RedemptionOrderNo", redemption.GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                    cmd.Parameters.AddWithValue("@Source", "POS Redemption");
                    cmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                    cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    if (ParafaitEnv.POSMachineId > -1)
                    {
                        cmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PosMachineId", DBNull.Value);
                    }
                    redemptionId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());

                    foreach (clsTurnInProducts item in turnInList)
                    {
                        //Insert into redemption gifts table  
                        int i = item.Quantity;
                        while (i-- > 0)
                        {
                            cmd.CommandText = "Insert into Redemption_gifts (redemption_id, gift_code, productid, locationId, Tickets, OriginalPriceInTickets, LastUpdatedBy, LastUpdateDate, CreationDate, CreatedBy )  " +
                                               @" select @redemption_id, Code, productid, @inLocationId, TurnInPriceInTickets * -1 , PriceInTickets * -1, @LastUpdatedBy, getdate(), getdate(), @CreatedBy
                                            from product
                                            where ProductId = @prod_id; SELECT @@IDENTITY";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                            cmd.Parameters.AddWithValue("@inLocationId", turnInlocationId);
                            cmd.Parameters.AddWithValue("@prod_id", item.productId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.Parameters.AddWithValue("@POSMachine", ParafaitEnv.POSMachineId);
                            int redemptionGiftId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());

                            cmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets], [TurnInTickets] ,[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate], [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId] )
                                              select redemption_id,Redemption_Gifts_Id, null ,null, null, null, null, null, null, null, null, 
                                                     tickets, @CreatedBy, getdate(), @LastUpdatedBy ,getdate(), null, null, null
                                        from Redemption_gifts
                                        where Redemption_Gifts_Id = @redemptionGiftId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets], [TurnInTickets] ,[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate],  [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId]  )
                                              select redemption_id,null, null ,null, null, null, null, null, null, null, null, 
                                                     tickets*-1, @CreatedBy, getdate(), @LastUpdatedBy ,getdate(), null, null, null
                                        from Redemption_gifts
                                        where Redemption_Gifts_Id = @redemptionGiftId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //Redemption.clsRedemption redemption = new Redemption.clsRedemption(Utilities);
                    string message = "";
                    if (redemption.printRealTicketReceipt(redemptionId, totalTickets, ref message, cmd_trx) == -1)
                    {
                        txtFeedback.Text = message;
                        log.Error("UpdateTurnInsToDB() - unable to printRealTicketReceipt error " + message);//Added for logger function on 08-Mar-2016                        
                    }
                }

                cmd_trx.Commit();

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception Ex)
            {
                cmd_trx.Rollback();
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,126, Ex.Message),
                                            MessageContainerList.GetMessage(Utilities.ExecutionContext, "Save Information")
                                            + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                log.Fatal("Ends-UpdateTurnInsToDB() Error while updating database. Error due to exception " + Ex.Message);//Added for logger function on 08-Mar-2016
                log.LogMethodExit(false);
                return false;
            }
        }

        void populateTurnInLocations(object productId)
        {
            log.LogMethodEntry(productId);
            //Updated query to see that only store and department locations are shown 2-Mar-2017
            //cmbTurninFromLocation.SelectedValue = Utilities.executeDataTable(@"select Name, LocationId
            //                                            from location l, LocationType lt
            //                                            where IsTurnInLocation = 'Y'
            //			and l.IsActive = 'Y'
            //			and l.LocationTypeID = lt.LocationTypeId
            //			and LocationType in ('Store', 'Department')
            //                                            union all
            //                                            select '<NONE>', null order by 1");

            cmbTargetLocation.SelectedValue = Utilities.executeScalar(@"select isnull(pos.InventoryLocationId, P.DefaultLocationId) 
                                                                       from Product P left outer join posMachines pos on POSMachineId = @POSMachine
                                                                        where P.ProductId = @productId",
                                                            new SqlParameter("@productId", productId),
                                                            new SqlParameter("@POSMachine", POSStatic.ParafaitEnv.POSMachineId));

            log.LogMethodExit();
        }

        private void btnAddTurnInProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                if (dgvTurnInProducts.SelectedRows.Count > 0)
                {
                    clsTurnInProducts item = turnInList.Find(delegate (clsTurnInProducts prod) { return ((int)prod.productId == (int)dgvTurnInProducts.SelectedRows[0].Cells["productId"].Value); });
                    if (item != null)
                    {
                        item.Quantity += (int)nudQuantity.Value;
                    }
                    else
                    {
                        item = new clsTurnInProducts();
                        item.productId = (int)dgvTurnInProducts.SelectedRows[0].Cells["productId"].Value;
                        item.Code = dgvTurnInProducts.SelectedRows[0].Cells["Code"].Value.ToString();
                        item.Description = dgvTurnInProducts.SelectedRows[0].Cells["Description"].Value.ToString();
                        item.Quantity = (int)nudQuantity.Value;
                        item.TurnInPriceInTickets = Convert.ToInt32(dgvTurnInProducts.SelectedRows[0].Cells["Price"].Value);
                        turnInList.Add(item);
                    }
                    refreshSelectedTurnInDGV();
                    //"Turn-in gift " + item.Code + " successfully added."
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2669, item.Code),
                                               MessageContainerList.GetMessage(Utilities.ExecutionContext,"Turn-in product")+ MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                    nudQuantity.Value = 1;
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                log.Fatal("Ends-btnAddTurnInProduct_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            FireSetLastActivityTime();
            log.LogMethodExit();
        }

        void refreshSelectedTurnInDGV()
        {
            log.LogMethodEntry();
            dgvSelectedProducts.Rows.Clear();
            txtTotalTickets.Text = "";
            int totTickets = 0;
            foreach (clsTurnInProducts item in turnInList)
            {
                dgvSelectedProducts.Rows.Add(item.productId, item.Code, item.Description, item.Quantity);
                totTickets += item.TurnInPriceInTickets * item.Quantity;
            }
            txtTotalTickets.Text = totTickets.ToString();
            log.LogMethodExit();
        }

        private void btnPrintTurnIn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                if (btnPrintTurnIn.Tag != null)
                    PrintRedemptionReceipt.Print(Convert.ToInt32(btnPrintTurnIn.Tag), null, true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        //Added methods 29-May-2017
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnExitProductLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                pnlProductLookup.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                pnlProductLookup.Visible = true;
                txtTurnInProdCode.Text = "";
                txtTurnInProdDesc.Text = "";
                btnTurnInProductSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void dgvSelectedProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                if (e.RowIndex >= 0
                         && dgvSelectedProducts["dcSelectedProductId", e.RowIndex].Value == null
                         && dgvSelectedProducts.Columns[e.ColumnIndex].Name.Equals("dcSelecctedProductCode")
                         && dgvSelectedProducts["dcSelecctedProductCode", e.RowIndex].Value.ToString().Trim() != "")
                {
                    DataTable dt = getTurnInGifts(dgvSelectedProducts["dcSelecctedProductCode", e.RowIndex].Value.ToString() + "%", "%%", 'P');
                    if (dt.Rows.Count == 0)
                    {
                        txtFeedback.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,111);
                        log.Info("Ends-dgvSelectedProducts_CellValueChanged() as Product not found ");//Added for logger function on 08-Mar-2016
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        DataGridView dgv = new DataGridView();

                        dgv.LostFocus += (object s, EventArgs ea) =>
                        {
                            dgv.Visible = false;
                            this.Controls.Remove(dgv);
                        };

                        dgv.CellClick += (object se, DataGridViewCellEventArgs eva) =>
                        {
                            FireSetLastActivityTime();
                            if (eva.RowIndex < 0)
                            {
                                log.Info("Ends-dgvSelectedProducts_CellValueChanged() as eva.RowIndex < 0");//Added for logger function on 08-Mar-2016
                                log.LogMethodExit();
                                return;
                            }
                            int ProductID = Convert.ToInt32(dgv.CurrentRow.Cells["ProductId"].Value);
                            dgv.Visible = false;
                            this.Controls.Remove(dgv);

                            clsTurnInProducts item = turnInList.Find(delegate (clsTurnInProducts prod) { return ((int)prod.productId == ProductID); });
                            if (item != null)
                            {
                                item.Quantity++;
                            }
                            else
                            {
                                item = new clsTurnInProducts();
                                item.productId = Convert.ToInt32(dgv.CurrentRow.Cells["ProductId"].Value);
                                item.Code = dgv.CurrentRow.Cells["Code"].Value.ToString();
                                item.Description = dgv.CurrentRow.Cells["Description"].Value.ToString();
                                item.Quantity = 1;
                                item.TurnInPriceInTickets = Convert.ToInt32(dgv.CurrentRow.Cells["Price"].Value);
                                turnInList.Add(item);
                            }
                            refreshSelectedTurnInDGV();
                            log.Info("dgvSelectedProducts_CellValueChanged() as addGift Sucessfull with code " + dgv.CurrentRow.Cells["Code"].Value.ToString() + "");//Added for logger function on 08-Mar-2016
                        };
                        dgv.Scroll += (object s, ScrollEventArgs ea) =>
                        {
                            FireSetLastActivityTime();
                        };
                        dgv.MouseMove += (object s, MouseEventArgs ea) =>
                        {
                            FireSetLastActivityTime();
                        };
                        dgv.DataSource = dt;
                        this.Controls.Add(dgv);
                        dgv.BringToFront();
                        dgv.Focus();

                        dgv.BorderStyle = BorderStyle.None;
                        dgv.AllowUserToAddRows = false;
                        dgv.BackgroundColor = Color.White;
                        dgv.Columns["ProductId"].Visible =
                            dgv.Columns["Quantity"].Visible = false;
                        dgv.Columns["Price"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                        dgv.Font = new Font("Arial", 10, FontStyle.Regular);
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dgv.ReadOnly = true;
                        dgv.RowHeadersVisible = false;
                        dgv.AllowUserToResizeColumns = false;
                        dgv.MultiSelect = false;
                        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                        dgv.AutoSize = true;
                        int wid = dgv.Width;
                        dgv.AutoSize = false;
                        dgv.Size = new Size(wid, (dgv.Rows[0].Cells[0].Size.Height * (dgv.Rows.Count)) + dgv.ColumnHeadersHeight);

                        dgv.Location = new Point(dgvSelectedProducts[0, 0].Size.Width, (dgvSelectedProducts.Location.Y + (dgvSelectedProducts.CurrentRow.Index + 2) * dgvSelectedProducts.CurrentRow.Height));
                    }
                }
            }
            catch (Exception ex)
            {
                txtFeedback.Text = ex.Message;
                log.Fatal("Ends-dgvSelectedProducts_CellValueChanged() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void btnTurnInSave_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnTurnInSave.BackgroundImage = Properties.Resources.CompleteTrxPressed;
            log.LogMethodExit();
        }

        private void btnTurnInSave_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnTurnInSave.BackgroundImage = Properties.Resources.CompleteTrx;
            log.LogMethodExit();
        }

        private void btnTurnInClear_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnTurnInClear.BackgroundImage = Properties.Resources.ClearTrxPressed;
            log.LogMethodExit();
        }

        private void btnTurnInClear_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnTurnInClear.BackgroundImage = Properties.Resources.ClearTrx;
            log.LogMethodExit();
        }

        private void btnPrintTurnIn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnPrintTurnIn.BackgroundImage = Properties.Resources.PrintTrxPressed;
            log.LogMethodExit();
        }

        private void btnPrintTurnIn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnPrintTurnIn.BackgroundImage = Properties.Resources.PrintTrx;
            log.LogMethodExit();
        }

        private void btnProductSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnProductSearch.BackgroundImage = Properties.Resources.Product_Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnProductSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnProductSearch.BackgroundImage = Properties.Resources.Product_Search_Btn_Normal;
            log.LogMethodExit();
        }

        private void btnAddTurnInProduct_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnAddTurnInProduct.BackgroundImage = Properties.Resources.Add_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnAddTurnInProduct_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnAddTurnInProduct.BackgroundImage = Properties.Resources.Add_Btn_Normal;
            log.LogMethodExit();
        }

        private void btnExitProductLookup_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnExitProductLookup.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodExit();
        }

        private void btnExitProductLookup_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnExitProductLookup.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodExit();
        }

        private void btnTurnInProductSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnTurnInProductSearch.BackgroundImage = Properties.Resources.Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnTurnInProductSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnTurnInProductSearch.BackgroundImage = Properties.Resources.Search_Btn_Normal;
            log.LogMethodExit();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                txtTurnInProdCode.Text = "";
                txtTurnInProdDesc.Text = "";
                //dgvTurnInProducts.DataSource = null;
                btnTurnInProductSearch_Click(null, null);
                refreshSelectedTurnInDGV();
                this.ActiveControl = txtTurnInProdCode;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClearSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            FireSetLastActivityTime();
            btnClearSearch.BackgroundImage = Properties.Resources.ClearTrxPressed;
            log.LogMethodExit();
        }

        private void btnClearSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnClearSearch.BackgroundImage = Properties.Resources.ClearTrx;
            log.LogMethodExit();
        }

        private void FireSetLastActivityTime()
        {
            log.LogMethodEntry();
            if (SetLastActivityTime != null)
            {
                SetLastActivityTime();
            }
            log.LogMethodExit();
        }
        //End Added methods 29-May-2017 
    }
}