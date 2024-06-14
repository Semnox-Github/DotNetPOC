/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business Logic to create and save transaction
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.60.2      28-May-2019      Mathew Ninan   Redeem Loyalty Points changed to load entitlements
 *                                            as credit plus
 *2.70.2.0    04-Oct-2019      Jinto Thomas   Modified insert trx_header query. Added createdby,
 *                                                                          creationdate fields
 *2.80.0      19-Mar-2020      Mathew NInan   Use new field ValidityStatus to track
 *                                            status of entitlements     
 *                                            Modified RedeemloyaltyPoints(), Added  createTransactionLine()
 *                                                part of redeemloayltypoints enhancement
 *2.80.0      07-May-2020      Jinto Thomas  Modified LoyaltyOnGamePlay(),RedeemLoyaltyPoints, added
 *                                              GetLoyltyProductOrderTypegroupId() for updating trx_header
 *                                              with trx_no
 *2.130.0     19-July-2021      Girish Kundar             Modified : VirtualPoints column added part of Arcade changes       
 *2.140.0    22-Oct-2021        Prajwal S      Modified : Virtual Arcade DbSynch log issues.
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System.Diagnostics;
using System.Windows;
using Semnox.Parafait.Product;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.Transaction
{
    public class Loyalty
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        public Loyalty(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;

            log.LogMethodExit(null);
        }
       
        void CreateLoyaltyPromotionReward(DataRow dtRulesRow, int TrxId, long gamePlayId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(dtRulesRow, TrxId, gamePlayId, SQLTrx);
            SqlCommand attribCmd = Utilities.getCommand(SQLTrx);
            attribCmd.CommandText = @"select * from LoyaltyBonusAttributes lba, LoyaltyAttributes la 
                                       where lba.LoyaltyAttributeId = la.LoyaltyAttributeId and LoyaltyRuleId = @RuleId";
            attribCmd.Parameters.AddWithValue("@RuleId", dtRulesRow["LoyaltyRuleId"]);

            log.LogVariableState("@RuleId", dtRulesRow["LoyaltyRuleId"]);

            SqlDataAdapter daAttrib = new SqlDataAdapter(attribCmd);
            DataTable dtAttrib = new DataTable();

            dtAttrib.Rows.Clear();
            daAttrib.Fill(dtAttrib);
            if (dtAttrib.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return;
            }

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "select isnull(max(lineId), 0) from trx_lines where trxId = @TrxId";
            cmd.Parameters.AddWithValue("@TrxId", TrxId);
            int numLines = Convert.ToInt32(cmd.ExecuteScalar());

            cmd.CommandText = "select trxDate from trx_header where trxId = @TrxId";
            DateTime trxDateTime = Convert.ToDateTime(cmd.ExecuteScalar());
            DateTime trxDate = trxDateTime.Date.AddHours(6);

            cmd.CommandText = "select top 1 product_id " +
                                "from products " +
                                "where product_type_id = (select product_type_id " +
                                                        "from product_type " +
                                                        "where product_type = 'LOYALTY' " +
                                                        "  and (site_id = @siteId or @siteId = -1) )";
            if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
            {
                cmd.Parameters.AddWithValue("@SiteId", -1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SiteId", Utilities.ParafaitEnv.SiteId);
            }

            object o = cmd.ExecuteScalar();
            int loyaltyProductId = -1;
            if (o != null)
            {
                loyaltyProductId = Convert.ToInt32(o);
            }

            string trxLineCmd = @"insert into trx_lines (TrxId, LineId, price, amount, quantity, card_id, card_number, product_id, gameplayId, createdBy, CreationDate, LastUpdatedBy, LastUpdateDate, site_id)
                                   values (@TrxId, @LineId, 0, 0, 1, @card_id, @card_number, @product_id, @gameplayId, @user, GETDATE(), @user, GETDATE(), @siteId)";

            string trxLineGamePlayCmd = @"insert into TransactionLineGamePlayMapping(TrxId, LineId, GamePlayId, IsActive,
                                                       guid, site_id, creationDate, createdBy, LastUpdateDate, LastUpdatedBy) 
                                          values (@TrxId, @LineId, @gameplayId, 1, newid(), @siteId, getdate(), @user, getdate(), @user)";

            string creditPlusCmd = "insert into CardCreditPlus " +
                                       "([CreditPlus] " +
                                       ",[CreditPlusType] " +
                                       ",[Refundable] " +
                                       ",[Remarks] " +
                                       ",[Card_id] " +
                                       ",[TrxId] " +
                                       ",[LineId] " +
                                       ",[CreditPlusBalance] " +
                                       ",[PeriodFrom] " +
                                       ",[PeriodTo] " +
                                       ",[ExtendOnReload] " +
                                       ",[TimeFrom] " +
                                       ",[TimeTo] " +
                                       ",[NumberOfDays] " +
                                       ",[Monday] " +
                                       ",[Tuesday] " +
                                       ",[Wednesday] " +
                                       ",[Thursday] " +
                                       ",[Friday] " +
                                       ",[Saturday] " +
                                       ",[Sunday] " +
                                       ",[TicketAllowed] " +
                                       ",[MinimumSaleAmount] " +
                                       ",[LoyaltyRuleId] " +
                                       ",[CreationDate] " +
                                       ",[LastupdatedDate] " +
                                       ",[LastUpdatedBy] " +
                                       ",[ForMembershipOnly] " +
                                       ",[site_id] ) " +
                                 "select " +
                                       "@CreditPlus " +
                                       ",@CreditPlusType " +
                                       ",'N' " +
                                       ",@PromotionName " +
                                       ",@Card_id " +
                                       ",@TrxId " +
                                       ",@LineId " +
                                       ",@CreditPlus " +
                                       ",isnull(dateadd(hh, 6, PeriodFrom), case isnull(ValidAfterDays, 0) when 0 then @TrxDateTime else @TrxDate + ValidAfterDays end) " +
                                       ",isnull(dateadd(hh, 30, PeriodTo), case isnull(NumberOfDays, 0) when 0 then NULL else (isnull(dateadd(hh, 6, PeriodFrom), case isnull(ValidAfterDays, 0) when 0 then @TrxDateTime else @TrxDate + ValidAfterDays end)) + NumberOfDays end) " +
                                       ",ExtendOnReload " +
                                       ",TimeFrom " +
                                       ",TimeTo " +
                                       ",NumberOfDays " +
                                       ",Monday " +
                                       ",Tuesday " +
                                       ",Wednesday " +
                                       ",Thursday " +
                                       ",Friday " +
                                       ",Saturday " +
                                       ",Sunday " +
                                       ",1 " +
                                       ",MinimumSaleAmount " +
                                       ",LoyaltyRuleId " +
                                       ",getdate() " +
                                       ",getdate() " +
                                       ",@user " +
                                       ",ForMembershipOnly " +
                                       ",@siteId " +
                                       "from LoyaltyBonusAttributes " +
                                           "where LoyaltyBonusId = @LoyaltyBonusId;select @@identity";

            string creditPluspurchaseCritCmd = "INSERT INTO [CardCreditPlusPurchaseCriteria] " +
                               "([CardCreditPlusId] " +
                               ",[POSTypeId] " +
                               ",[ProductId] " +
                               ",[LastupdatedDate] " +
                               ",[LastUpdatedBy] " +
                               ",[site_id]) " +
                            "select " +
                               "@CardCreditPlusId " +
                               ",POSTypeId " +
                               ",ProductId " +
                               ",getdate() " +
                               ",@user " +
                               ",@siteId " +
                               "from LoyaltyBonusPurchaseCriteria " +
                            "where LoyaltyBonusId = @LoyaltyBonusId";

            string creditPlusConsumptionCmd = "INSERT INTO [CardCreditPlusConsumption] " +
                               "([CardCreditPlusId] " +
                               ",[POSTypeId] " +
                               ",[ExpiryDate] " +
                               ",[ProductId] " +
                               ",[CategoryId] " +
                               ",[GameProfileId] " +
                               ",[GameId] " +
                               ",[DiscountPercentage] " +
                               ",[DiscountedPrice] " +
                               ",[DiscountAmount] " +
                               ",[ConsumptionBalance] " +
                               ",[LastupdatedDate] " +
                               ",[LastUpdatedBy] " +
                               ",[site_id]) " +
                            "select  " +
                               "@CardCreditPlusId " +
                               ",POSTypeId " +
                               ",NULL " +
                               ",ProductId " +
                               ",CategoryId " +
                               ",GameProfileId " +
                               ",GameId " +
                               ",DiscountPercentage " +
                               ",DiscountedPrice " +
                               ",DiscountAmount " +
                               ",1 " +
                               ",getdate() " +
                               ",@user " +
                               ",@siteId " +
                   "from LoyaltyBonusRewardCriteria " +
                            "where LoyaltyBonusId = @LoyaltyBonusId";

            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);

            for (int j = 0; j < dtAttrib.Rows.Count; j++)
            {
                string CreditPlusType = dtAttrib.Rows[j]["CreditPlusType"].ToString();
                if (CreditPlusType == "T")
                {
                    // skip if credit plus type is Tickets and ticket is not allowed for card
                    if (Utilities.executeScalar("select ticket_allowed from cards where card_id = @cardId",
                                                SQLTrx,
                                                new SqlParameter("@cardId", dtRulesRow["card_id"])).ToString() == "N")
                    {
                        continue;
                    }
                }


                double genValue = 0;
                if (dtAttrib.Rows[j]["BonusPercentage"] != DBNull.Value && Convert.ToDouble(dtAttrib.Rows[j]["BonusPercentage"]) != 0)
                {
                    if (dtAttrib.Rows[j]["ApplicableElement"].ToString() == "A")
                    {
                        genValue = Convert.ToDouble(dtAttrib.Rows[j]["BonusPercentage"]) * Convert.ToDouble(dtRulesRow["Amount"]) / 100.0;
                    }
                    else  //ApplicableElement = T 
                    {
                        genValue = Convert.ToDouble(dtAttrib.Rows[j]["BonusPercentage"]) * Convert.ToDouble(dtRulesRow["Tickets"]) / 100.0;
                    }
                }
                else
                {
                    if (dtAttrib.Rows[j]["BonusValue"] != DBNull.Value)
                    {
                        genValue = Convert.ToDouble(dtAttrib.Rows[j]["BonusValue"]);
                    }
                }

                if (gamePlayId > -1 && genValue == 0)
                {
                    log.LogVariableState("Skip as zero loyalty point for Game play Id ", gamePlayId);
                    log.LogVariableState("CreditPlusType ", CreditPlusType);
                    continue;
                }

                trxCmd.CommandText = trxLineCmd;
                trxCmd.Parameters.Clear();

                trxCmd.Parameters.AddWithValue("@TrxId", TrxId);
                trxCmd.Parameters.AddWithValue("@TrxDate", trxDate);
                trxCmd.Parameters.AddWithValue("@TrxDateTime", trxDateTime);
                trxCmd.Parameters.AddWithValue("@LineId", ++numLines);
                trxCmd.Parameters.AddWithValue("@card_id", dtRulesRow["card_id"]);
                trxCmd.Parameters.AddWithValue("@card_number", dtRulesRow["card_number"]);
                trxCmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                object site_id = DBNull.Value;
                if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
                    site_id = DBNull.Value;
                else
                    site_id = Utilities.ParafaitEnv.SiteId;
                trxCmd.Parameters.AddWithValue("@siteId", site_id);
                log.LogVariableState("@TrxId", TrxId);
                log.LogVariableState("@TrxDate", trxDate);
                log.LogVariableState("@TrxDateTime", trxDateTime);
                log.LogVariableState("@LineId", numLines);
                log.LogVariableState("@card_id", dtRulesRow["card_id"]);
                log.LogVariableState("@card_number", dtRulesRow["card_number"]);
                log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@siteId", Utilities.ParafaitEnv.SiteId);

                if (loyaltyProductId == -1)
                {
                    trxCmd.Parameters.AddWithValue("@product_id", DBNull.Value);
                    log.LogVariableState("@product_id", DBNull.Value);
                }
                else
                {
                    trxCmd.Parameters.AddWithValue("@product_id", loyaltyProductId);
                    log.LogVariableState("@product_id", loyaltyProductId);
                }

                //if (gamePlayId == -1)
                //{
                trxCmd.Parameters.AddWithValue("@gameplayId", DBNull.Value);
                log.LogVariableState("@gameplayId", DBNull.Value);
                //}
                //else
                //{
                //    trxCmd.Parameters.AddWithValue("@gameplayId", gamePlayId);
                //    log.LogVariableState("@gameplayId", gamePlayId);
                //}

                trxCmd.ExecuteNonQuery();

                trxCmd.CommandText = creditPlusCmd;
                trxCmd.Parameters.AddWithValue("@PromotionName", dtRulesRow["Name"]);
                trxCmd.Parameters.AddWithValue("@LoyaltyBonusId", dtAttrib.Rows[j]["LoyaltyBonusId"]);
                trxCmd.Parameters.AddWithValue("@CreditPlus", genValue);

                log.LogVariableState("@PromotionName", dtRulesRow["Name"]);
                log.LogVariableState("@LoyaltyBonusId", dtAttrib.Rows[j]["LoyaltyBonusId"]);
                log.LogVariableState("@CreditPlus", genValue);

                if (CreditPlusType == "")
                {
                    CreditPlusType = "A";
                }

                trxCmd.Parameters.AddWithValue("@CreditPlusType", CreditPlusType);

                object CardCreditPlusId = trxCmd.ExecuteScalar();

                log.LogVariableState("@CreditPlusType", CreditPlusType);

                trxCmd.CommandText = creditPluspurchaseCritCmd;
                trxCmd.Parameters.AddWithValue("@CardCreditPlusId", CardCreditPlusId);
                trxCmd.ExecuteNonQuery();

                log.LogVariableState("@CardCreditPlusId", CardCreditPlusId);

                trxCmd.CommandText = creditPlusConsumptionCmd;
                trxCmd.ExecuteNonQuery();

                ExtendOnReload(Convert.ToInt32(dtRulesRow["card_id"]), CreditPlusType, SQLTrx);
                if (gamePlayId != -1)
                {
                    trxCmd.CommandText = trxLineGamePlayCmd;
                    trxCmd.Parameters.Clear();
                    trxCmd.Parameters.AddWithValue("@TrxId", TrxId);
                    log.LogVariableState("@TrxId", TrxId);

                    trxCmd.Parameters.AddWithValue("@LineId", numLines);
                    log.LogVariableState("@LineId", numLines);

                    trxCmd.Parameters.AddWithValue("@gameplayId", gamePlayId);
                    log.LogVariableState("@gameplayId", gamePlayId);

                    trxCmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                    if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
                        site_id = DBNull.Value;
                    else
                        site_id = Utilities.ParafaitEnv.SiteId;
                    trxCmd.Parameters.AddWithValue("@siteId", site_id);
                    trxCmd.ExecuteNonQuery();
                }
            }

            log.LogVariableState("@cardId", dtRulesRow["card_id"]);
            log.LogMethodExit(null);
        }

        public void ExtendOnReload(int cardId, string CreditPlusType, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(cardId, CreditPlusType, SQLTrx);

            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);

            string creditPlusList = "";
            if (CreditPlusType == "G" || CreditPlusType == "B")
                creditPlusList = "('G', 'B')";
            else
                creditPlusList = "('" + CreditPlusType + "')";

            trxCmd.CommandText = @"update CardCreditPlus 
                                        set PeriodTo = (select max(PeriodTo) 
                                                        from CardCreditPlus c 
                                                       where c.Card_Id = @card_Id 
                                                        and ExtendOnReload = 'Y'
                                                        and CreditPlusType in " + creditPlusList + "), " +
                                        @"LastupdatedDate = getdate()
                                        where Card_Id = @card_Id
                                          and ExtendOnReload = 'Y'
                                          and CreditPlusBalance != 0
                                          and PeriodTo >= getdate()
                                          and CreditPlusType in " + creditPlusList;

            trxCmd.Parameters.AddWithValue("@card_id", cardId);
            trxCmd.Parameters.AddWithValue("@CreditPlusType", CreditPlusType);
            trxCmd.ExecuteNonQuery();

            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@CreditPlusType", CreditPlusType);


            Card updateCard = new Card(cardId, "", Utilities, SQLTrx);
            updateCard.updateCardTime(SQLTrx);
            log.LogVariableState("Card Updated: ", updateCard.card_id);

            log.LogMethodExit(null);
        }

        public int CreateGenericCreditPlusLine(int CardId, string CreditPlusType, double Amount, bool Refundable, int ExpiryDays, string AutoExtend, string LoginId, string Remarks, SqlTransaction SQLTrx, DateTime? periodFrom = null, int trxId = -1, int lineId = -1, bool ticketAllowed = false)
        {
            log.LogMethodEntry(CardId, CreditPlusType, Amount, Refundable, ExpiryDays, AutoExtend, LoginId, Remarks, SQLTrx, periodFrom, trxId, lineId, ticketAllowed);

            int creditplusId = CreateGenericCreditPlusLine(CardId, CreditPlusType, Amount, Refundable, ExpiryDays, AutoExtend, LoginId, Remarks, SQLTrx, DateTime.MinValue, periodFrom, trxId, lineId, -1, ticketAllowed);
            log.LogMethodExit(creditplusId);
            return creditplusId;
        }

        public int CreateGenericCreditPlusLine(int CardId, string CreditPlusType, double Amount, bool Refundable, int ExpiryDays, string AutoExtend, string LoginId, string Remarks, SqlTransaction SQLTrx, DateTime playStartTime, DateTime? periodFrom = null, int trxId = -1, int lineId = -1, int sourceCpId = -1, bool ticketAllowed = false)
        {
            log.LogMethodEntry(CardId, CreditPlusType, Amount, Refundable, ExpiryDays, AutoExtend, LoginId, Remarks, SQLTrx, playStartTime, periodFrom, trxId, lineId, sourceCpId, ticketAllowed);
            int creditplusId = -1;
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"insert into CardCreditPlus  
                                       ([CreditPlus]  
                                       ,[CreditPlusType]  
                                       ,[Refundable]  
                                       ,[Remarks] 
                                       ,[Card_id] 
                                       ,[CreditPlusBalance]  
                                       ,[PeriodFrom]  
                                       ,[PeriodTo] 
                                       ,[ExtendOnReload]  
                                       ,[PlayStartTime]  
                                       ,[CreationDate]  
                                       ,[LastupdatedDate]  
                                       ,[LastUpdatedBy]  
                                       ,[TrxId]  
                                       ,[LineId]
                                       ,[SourceCreditPlusId]
                                       ,[ValidityStatus]
                                       ,[TicketAllowed]
                                       ,[site_Id])
                                       values (
                                        @CreditPlus,
                                        @CreditPlusType,
                                        @Refundable,
                                        @Remarks,
                                        @card_id,
                                        @CreditPlus,
                                        CASE @PeriodFrom WHEN  NULL THEN NULL ELSE dateadd(HH, 6, convert(datetime, convert(varchar, @PeriodFrom, 101), 101)) end,
                                        case @ExpiryDays when 0 then NULL else dateadd(HH, 6, convert(datetime, convert(varchar, getdate(), 101), 101) + @ExpiryDays) end,
                                        @ExtendOnReload,
                                        @PlayStartTime,
                                        getdate(),
                                        getdate(),
                                        @LastUpdatedBy,
                                        @trxId, 
                                        @lineId,
                                        @sourceCpId,
                                        @ValidityStatus,
                                        @TicketAllowed,
                                        @siteId) select @@identity";

            cmd.Parameters.AddWithValue("@ExpiryDays", ExpiryDays);
            cmd.Parameters.AddWithValue("@ExtendOnReload", AutoExtend);
            cmd.Parameters.AddWithValue("@CreditPlusType", CreditPlusType);
            cmd.Parameters.AddWithValue("@Refundable", Refundable ? "Y" : "N");

            cmd.Parameters.Add("@TicketAllowed", SqlDbType.Bit);
            if (ticketAllowed)
                cmd.Parameters["@TicketAllowed"].Value = 1;
            else
                cmd.Parameters["@TicketAllowed"].Value = DBNull.Value;

            cmd.Parameters.AddWithValue("@card_id", CardId);
            cmd.Parameters.AddWithValue("@CreditPlus", Amount);
            cmd.Parameters.AddWithValue("@LastUpdatedBy", LoginId);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.Add("@PlayStartTime", SqlDbType.DateTime);
            if (CreditPlusType.Equals("M") && playStartTime != DateTime.MinValue)
                cmd.Parameters["@PlayStartTime"].Value = playStartTime;
            else
                cmd.Parameters["@PlayStartTime"].Value = DBNull.Value;

            if (periodFrom == null)
            {
                cmd.Parameters.AddWithValue("@PeriodFrom", DBNull.Value);
            }
            else
            {
                if (((DateTime)periodFrom).Hour < 6)
                {
                    periodFrom = ((DateTime)periodFrom).AddDays(-1);
                }
                cmd.Parameters.AddWithValue("@PeriodFrom", periodFrom);
            }

            if (trxId == -1)
            {
                cmd.Parameters.AddWithValue("@trxId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trxId", trxId);
            }
            if (lineId == -1)
            {
                cmd.Parameters.AddWithValue("@lineId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@lineId", lineId);
            }
            if (sourceCpId == -1)
            {
                cmd.Parameters.AddWithValue("@sourceCpId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sourceCpId", sourceCpId);
            }
            object site_id = DBNull.Value;
            if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
            {
                site_id = DBNull.Value;
            }
            else
            {
                site_id = Utilities.ParafaitEnv.SiteId;
            }
            cmd.Parameters.AddWithValue("@siteId", site_id);

            if (playStartTime == DateTime.MinValue && trxId != -1
                && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE").Equals("Y")
               )
            {
                cmd.Parameters.AddWithValue("@ValidityStatus", 'H');
            }
            else
            {
                cmd.Parameters.AddWithValue("@ValidityStatus", DBNull.Value);
            }
            object newCardCreditPlusId = cmd.ExecuteScalar();
            Card updateCard = new Card((int)CardId, LoginId, Utilities, SQLTrx);
            updateCard.updateCardTime(SQLTrx);
            log.LogVariableState("@ExpiryDays", ExpiryDays);
            log.LogVariableState("@ExtendOnReload", AutoExtend);
            log.LogVariableState("@CreditPlusType", CreditPlusType);
            log.LogVariableState("@Refundable", Refundable ? "Y" : "N");
            log.LogVariableState("@card_id", CardId);
            log.LogVariableState("@CreditPlus", Amount);
            log.LogVariableState("@LastUpdatedBy", LoginId);
            log.LogVariableState("@Remarks", Remarks);
            log.LogVariableState("@PeriodFrom", periodFrom);
            log.LogVariableState("@PlayStartTime", SqlDbType.DateTime);
            log.LogVariableState("@trxId", trxId);
            log.LogVariableState("@lineId", lineId);
            log.LogVariableState("@sourceCpId", sourceCpId);
            log.LogVariableState("@ticketAllowed", ticketAllowed);
            log.LogVariableState("newCardCreditPlusId", newCardCreditPlusId);
            creditplusId = Convert.ToInt32(newCardCreditPlusId);
            log.LogMethodExit(creditplusId);
            return creditplusId;
        }


        public void LoyaltyOnPurchase(int TrxId, string applyImmediate, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(TrxId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            cmd.CommandText = "exec LoyaltyPromotionsOnCardPurchase @TrxId, @ApplyImmediate";

            cmd.Parameters.AddWithValue("@TrxId", TrxId);
            if (!String.IsNullOrEmpty(applyImmediate) && applyImmediate == "Y")
                cmd.Parameters.AddWithValue("@ApplyImmediate", "Y");
            else
                cmd.Parameters.AddWithValue("@ApplyImmediate", "N");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtRules = new DataTable();
            da.Fill(dtRules);
            foreach (DataRow dr in dtRules.Rows)
            {
                CreateLoyaltyPromotionReward(dr, TrxId, -1, SQLTrx);              
            }
            log.LogVariableState("@TrxId", TrxId);
            log.LogMethodExit(null);
        }

        public void LoyaltyOnProductConsumption(int TrxId, SqlTransaction SQLTrx, string POSMachine, string applyImmediate)
        {
            log.LogMethodEntry(TrxId, SQLTrx, POSMachine);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            cmd.CommandText = "exec LoyaltyPromotionsOnProductPurchase @TrxId, @ApplyImmediate";

            cmd.Parameters.AddWithValue("@TrxId", TrxId);
            if (!String.IsNullOrEmpty(applyImmediate) && applyImmediate == "Y")
                cmd.Parameters.AddWithValue("@ApplyImmediate", "Y");
            else
                cmd.Parameters.AddWithValue("@ApplyImmediate", "N");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtRules = new DataTable();
            da.Fill(dtRules);
            foreach (DataRow dr in dtRules.Rows)
            {
                CreateLoyaltyPromotionReward(dr, TrxId, -1, SQLTrx);
            }

            log.LogVariableState("@TrxId", TrxId);
            log.LogMethodExit(null);
        }

        public void LoyaltyOnGamePlay(long GamePlayId, string applyImmediate, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(GamePlayId, SQLTrx);

            int orderTypeGroupId = -1;

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            cmd.CommandText = "exec LoyaltyPromotionsOnGamePlay @GamePlayId, @ApplyImmediate";

            cmd.Parameters.AddWithValue("@GamePlayId", GamePlayId);
            if (!String.IsNullOrEmpty(applyImmediate) && applyImmediate == "Y")
                cmd.Parameters.AddWithValue("@ApplyImmediate", "Y");
            else
                cmd.Parameters.AddWithValue("@ApplyImmediate", "N");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtRules = new DataTable();
            da.Fill(dtRules);

            log.LogVariableState("@GamePlayId", GamePlayId);
            log.LogVariableState("@Utilities.ParafaitEnv.User_Id", Utilities.ParafaitEnv.User_Id);
            log.LogVariableState("@Utilities.ParafaitEnv.LoginID", Utilities.ParafaitEnv.LoginID);


            if (dtRules.Rows.Count == 0)
            {
                log.LogMethodExit(null);
                return;
            }

            orderTypeGroupId = GetLoyltyProductOrderTypegroupId(SQLTrx);

            cmd.CommandText = " insert into trx_header (TrxDate, TrxAmount, TrxDiscountPercentage, " +
                                                        "TaxAmount, TrxNetAmount, pos_machine, " +
                                                        "user_id, payment_mode, cashAmount, " +
                                                        "creditCardAmount, gameCardAmount, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateTime, OrderTypeGroupId) " +
                                                "values (getdate(), 0, 0, " +
                                                        "0, 0, @POSName, " +
                                                        "@user_id, 1, 0, " +
                                                        "0, 0, @user_id, getdate(), @loginId, getdate(), @OrderTypeGroupId); select @@identity";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@POSName", Environment.MachineName);
            if (Utilities.ParafaitEnv.User_Id > -1)
            {
                cmd.Parameters.AddWithValue("@user_id", Utilities.ParafaitEnv.User_Id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@user_id", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
            if (orderTypeGroupId < 0)
            {
                cmd.Parameters.AddWithValue("@OrderTypeGroupId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OrderTypeGroupId", orderTypeGroupId);
            }

            int localTrxId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Parameters.Clear();

            CommonFuncs CommonFuncs = new CommonFuncs(Utilities);
            string trxNo = "";

            trxNo = CommonFuncs.getNextTrxNo(Utilities.ParafaitEnv.POSMachineId, orderTypeGroupId, cmd.Transaction);
            log.LogVariableState("Trx_No with getNextTrxNo", trxNo);

            cmd.CommandText = "update trx_header set trx_no = @Trx_no, LastUpdatedBy = @loginId , LastUpdateTime = getdate() where trxId = @TrxId";
            cmd.Parameters.AddWithValue("@TrxId", localTrxId);
            cmd.Parameters.AddWithValue("@Trx_no", trxNo);
            cmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
            cmd.ExecuteNonQuery();

            log.LogVariableState("@trxId", localTrxId);
            log.LogVariableState("@trx_no", trxNo);
            log.LogVariableState("@loginId", Utilities.ParafaitEnv.LoginID);

            foreach (DataRow dr in dtRules.Rows)
            {
                CreateLoyaltyPromotionReward(dr, localTrxId, GamePlayId, SQLTrx);
            }

            log.LogVariableState("@POSName", Environment.MachineName);
            log.LogVariableState("@user_id", DBNull.Value);

            log.LogMethodExit(null);
        }

        public bool IsCreditPlusTimeRunning(int cardId, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry(cardId, inSQLTrx);

            SqlCommand cmd = Utilities.getCommand(inSQLTrx);
            bool isRunning = true;
            cmd.CommandText = @"SELECT PlayStartTime, CASE WHEN PlayStartTime is null 
                                        THEN CreditPlusBalance 
	                                    ELSE CASE WHEN TimeTo is null 
		                                        THEN datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))
		                                        ELSE datediff(MI, getdate(), 
									                (select min(endTime) 
										                from (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
												                union all 
												                select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)) 
		                                      END
	                                    END TimeBalance, CreditPlusBalance
                                from CardCreditPlus cp
                                where 1=1 --
		                        and cp.card_id = @cardId 
                                and CreditPlusType in ('M')
                                and (case when PlayStartTime is null then getdate() else dateadd(MI, CreditPlusBalance, PlayStartTime) end) >= getdate()
                                and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                and (cp.PeriodTo is null or cp.PeriodTo > getdate())
		                        ORDER BY PlayStartTime desc";
            cmd.Parameters.AddWithValue("@cardId", cardId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtIsRunning = new DataTable();
            da.Fill(dtIsRunning);

            if (dtIsRunning.Rows.Count == 0 || (dtIsRunning.Rows.Count > 0 && dtIsRunning.Rows[0][0] == DBNull.Value))
                isRunning = false;
            else
                isRunning = true;

            log.LogVariableState("@cardId", cardId);
            log.LogMethodExit(isRunning);
            return isRunning;
        }

        /// <summary>
        /// RedeemLoyaltyPoints
        /// </summary>
        /// <param name="card_id"></param>
        /// <param name="card_number"></param>
        /// <param name="LoyaltyPoints"></param>
        /// <param name="AttributeColumn"></param>
        /// <param name="RedeemValue"></param>
        /// <param name="POSMachine"></param>
        /// <param name="userId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int RedeemLoyaltyPoints(int card_id, string card_number, double LoyaltyPoints, string AttributeColumn, double RedeemValue, string POSMachine, int userId, SqlTransaction SQLTrx, bool ticketAllowed = false)
        {
            log.LogMethodEntry(card_id, card_number, LoyaltyPoints, AttributeColumn, RedeemValue, POSMachine, userId, SQLTrx, ticketAllowed);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            double amount;
            if (AttributeColumn.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
            {
                amount = RedeemValue * -1;
            }
            else
            {
                amount = 0;
            }
            int localTrxId = CreateLoyaltyTransaction(card_id, AttributeColumn, RedeemValue, POSMachine, userId, amount, SQLTrx);
            CreditPlus cp = new CreditPlus(Utilities);
            double multiplicationFactor = RedeemValue / LoyaltyPoints;
            List<Tuple<int, double, double>> cpList = new List<Tuple<int, double, double>>();
            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card_id, true, true, SQLTrx);
            double balanceLoyaltyPoints = LoyaltyPoints;
            double totalRedeemValue = 0;
            double usedCPBalance = 0;
            int count = 0;
            double cpRedeemValue = 0;
            DataTable dtSourceCreditPlus = cp.getCreditPlusDetails(card_id, "L", SQLTrx);

            if (dtSourceCreditPlus.Rows.Count > 0)
            {
                foreach (DataRow sourceCreditPlusRow in dtSourceCreditPlus.Rows)
                {
                    usedCPBalance = Convert.ToDouble(sourceCreditPlusRow["CreditPlusBalance"]);
                    double deductableValue = 0;
                    int cpId = Convert.ToInt32(sourceCreditPlusRow["CardCreditPlusId"]);
                    while (usedCPBalance > 0 && balanceLoyaltyPoints > 0)
                    {
                        if (balanceLoyaltyPoints < usedCPBalance)
                        {
                            deductableValue = balanceLoyaltyPoints;
                            balanceLoyaltyPoints = 0;
                            usedCPBalance = usedCPBalance - deductableValue;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        else if (balanceLoyaltyPoints >= usedCPBalance)
                        {
                            deductableValue = usedCPBalance;
                            balanceLoyaltyPoints = balanceLoyaltyPoints - deductableValue;
                            usedCPBalance = 0;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        cpList.Add(new Tuple<int, double, double>(cpId, deductableValue, cpRedeemValue));
                        totalRedeemValue = totalRedeemValue + cpRedeemValue;
                        count++;
                    }
                }
            }
            if (balanceLoyaltyPoints > 0)
            {
                if (accountBL.AccountDTO.LoyaltyPoints >= Convert.ToInt32(balanceLoyaltyPoints))
                {
                    cpRedeemValue = Math.Round(balanceLoyaltyPoints * multiplicationFactor, 2);
                    cpList.Add(new Tuple<int, double, double>(-1, balanceLoyaltyPoints, cpRedeemValue));
                    totalRedeemValue = totalRedeemValue + cpRedeemValue;
                    count++;
                }
                else
                {
                    log.Error(Utilities.MessageUtils.getMessage(2493));
                    throw new Exception(Utilities.MessageUtils.getMessage(2493));
                }
            }
            if (totalRedeemValue != RedeemValue)
            {
                double balanceRedeemValue = RedeemValue - totalRedeemValue;
                double newCpRedeemValue = Math.Round(cpList[count - 1].Item3 + balanceRedeemValue, 2);
                cpList[count - 1] = Tuple.Create(cpList[count - 1].Item1, cpList[count - 1].Item2, newCpRedeemValue);
            }
            int loyaltyProductId = GetLoyaltyProductId(SQLTrx);
            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);
            for (int i = 0; i < count; i++)
            {
                if (cpList[i].Item1 != -1)
                {
                    CreateTransactionLine(AttributeColumn, localTrxId, i + 1, amount, card_id, card_number, loyaltyProductId, cpList[i].Item2, cpList[i].Item3, userId, cpList[i].Item1, SQLTrx, false, ticketAllowed);
                    cp.DeductFromCreditPlusRecord(cpList[i].Item1, cpList[i].Item2, SQLTrx);
                }
                else
                {
                    if (accountBL.AccountDTO.LoyaltyPoints >= Convert.ToInt32(balanceLoyaltyPoints))
                    {
                        CreateTransactionLine(AttributeColumn, localTrxId, i + 1, amount, card_id, card_number, loyaltyProductId, cpList[i].Item2, cpList[i].Item3, userId, cpList[i].Item1, SQLTrx);
                        accountBL = new AccountBL(Utilities.ExecutionContext, card_id, true, true, SQLTrx);
                        accountBL.AccountDTO.LoyaltyPoints = accountBL.AccountDTO.LoyaltyPoints - Convert.ToInt32(balanceLoyaltyPoints);
                    }
                    else
                    {
                        MessageBox.Show(Utilities.MessageUtils.getMessage(2493));
                        throw new Exception(Utilities.MessageUtils.getMessage(2493));
                    }
                    accountBL.Save(SQLTrx);
                }
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE").Equals("Y"))
            {
                LoadEntitlements(localTrxId, SQLTrx);
            }
            log.LogMethodExit(localTrxId);
            return localTrxId;
        }

        /// <summary>
        /// RedeemVirtualLoyaltyPoints method to redeem vortual loyalty points to credit plus types
        /// </summary>
        /// <param name="card_id"></param>
        /// <param name="card_number"></param>
        /// <param name="virtualLoyaltyPoints"></param>
        /// <param name="AttributeColumn"></param>
        /// <param name="RedeemValue"></param>
        /// <param name="POSMachine"></param>
        /// <param name="userId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int RedeemVirtualPoints(int card_id, string card_number, double virtualLoyaltyPoints, string AttributeColumn, double RedeemValue, string POSMachine, int userId, SqlTransaction SQLTrx, bool ticketAllowed = false)
        {
            log.LogMethodEntry(card_id, card_number, virtualLoyaltyPoints, AttributeColumn, RedeemValue, POSMachine, userId, SQLTrx);
            double amount = 0;
            if (AttributeColumn.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
            {
                amount = RedeemValue * -1;
            }
            else
            {
                amount = 0;
            }
            int localTrxId = CreateLoyaltyTransaction(card_id, AttributeColumn, RedeemValue, POSMachine, userId, amount, SQLTrx);
            CreditPlus cp = new CreditPlus(Utilities);
            double multiplicationFactor = RedeemValue / virtualLoyaltyPoints;
            List<Tuple<int, double, double>> cpList = new List<Tuple<int, double, double>>();
            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card_id, true, true, SQLTrx);
            double balanceVirtualLoyaltyPoints = virtualLoyaltyPoints;
            double totalRedeemValue = 0;
            double usedCPBalance = 0;
            int count = 0;
            double cpRedeemValue = 0;
            DataTable dtSourceCreditPlus = cp.getCreditPlusDetails(card_id, "V", SQLTrx);
            if (dtSourceCreditPlus.Rows.Count > 0)
            {
                foreach (DataRow sourceCreditPlusRow in dtSourceCreditPlus.Rows)
                {
                    usedCPBalance = Convert.ToDouble(sourceCreditPlusRow["CreditPlusBalance"]);
                    double deductableValue = 0;
                    int cpId = Convert.ToInt32(sourceCreditPlusRow["CardCreditPlusId"]);
                    while (usedCPBalance > 0 && balanceVirtualLoyaltyPoints > 0)
                    {
                        if (balanceVirtualLoyaltyPoints < usedCPBalance)
                        {
                            deductableValue = balanceVirtualLoyaltyPoints;
                            balanceVirtualLoyaltyPoints = 0;
                            usedCPBalance = usedCPBalance - deductableValue;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        else if (balanceVirtualLoyaltyPoints >= usedCPBalance)
                        {
                            deductableValue = usedCPBalance;
                            balanceVirtualLoyaltyPoints = balanceVirtualLoyaltyPoints - deductableValue;
                            usedCPBalance = 0;
                            cpRedeemValue = Math.Round(deductableValue * multiplicationFactor, 2);
                        }
                        cpList.Add(new Tuple<int, double, double>(cpId, deductableValue, cpRedeemValue));
                        totalRedeemValue = totalRedeemValue + cpRedeemValue;
                        count++;
                    }
                }
            }
            if (balanceVirtualLoyaltyPoints > 0)
            {
                throw new Exception(Utilities.MessageUtils.getMessage(2493));
            }
            if (totalRedeemValue != RedeemValue)
            {
                double balanceRedeemValue = RedeemValue - totalRedeemValue;
                double newCpRedeemValue = Math.Round(cpList[count - 1].Item3 + balanceRedeemValue, 2);
                cpList[count - 1] = Tuple.Create(cpList[count - 1].Item1, cpList[count - 1].Item2, newCpRedeemValue);
            }
            int loyaltyProductId = GetLoyaltyProductId(SQLTrx);
            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);
            for (int i = 0; i < count; i++)
            {
                if (cpList[i].Item1 != -1)
                {
                    CreateTransactionLine(AttributeColumn, localTrxId, i + 1, amount, card_id, card_number, loyaltyProductId, cpList[i].Item2, cpList[i].Item3, userId, cpList[i].Item1, SQLTrx, true, ticketAllowed);
                    cp.DeductFromCreditPlusRecord(cpList[i].Item1, cpList[i].Item2, SQLTrx);
                }
                else
                {
                    log.Error(Utilities.MessageUtils.getMessage(2493));
                    throw new Exception(Utilities.MessageUtils.getMessage(2493));
                }
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE").Equals("Y"))
            {
                LoadEntitlements(localTrxId, SQLTrx);
            }
            log.LogMethodExit(localTrxId);
            return localTrxId;
        }

        private void LoadEntitlements(int localTrxId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(localTrxId, SQLTrx);
            TransactionUtils trxUtils = new TransactionUtils(Utilities);
            Transaction Trx = trxUtils.CreateTransactionFromDB(localTrxId, Utilities, false, false, SQLTrx);
            string message = "";
            if (Trx.CompleteTransaction(SQLTrx, ref message) == false)
            {
                log.Error(message);
                throw new Exception(message);
            }
            log.LogMethodExit();
        }

        private int GetLoyaltyProductId(SqlTransaction SQLtrx)
        {
            log.LogMethodEntry(SQLtrx);
            int loyaltyProductId = -1;
            SqlCommand cmd = Utilities.getCommand(SQLtrx);
            cmd.CommandText = "select top 1 product_id " +
                                         "from products " +
                                         "where product_type_id = (select product_type_id " +
                                                                 "from product_type " +
                                                                 "where product_type = 'LOYALTY')";
            object o = cmd.ExecuteScalar();
            if (o != null)
            {
                loyaltyProductId = Convert.ToInt32(o);
            }
            log.LogMethodExit(loyaltyProductId);
            return loyaltyProductId;
        }

        private int CreateLoyaltyTransaction(int card_id, string AttributeColumn, double RedeemValue, string POSMachine, int userId, double amount, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(card_id, AttributeColumn, RedeemValue, POSMachine, userId, amount, SQLTrx);
            int transactionId = -1;
            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            int orderTypeGroupId = -1;
            orderTypeGroupId = GetLoyltyProductOrderTypegroupId(SQLTrx);
            cmd.CommandText = " insert into trx_header (TrxDate, TrxAmount, TrxDiscountPercentage, " +
                                                        "TaxAmount, TrxNetAmount, pos_machine, " +
                                                        "user_id, payment_mode, cashAmount, " +
                                                        "creditCardAmount, gameCardAmount, OtherPaymentModeAmount, PrimaryCardId, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateTime, Status, OrderTypeGroupId) " +
                                                "values (getdate(), @amount, 0, " +
                                                        "0, @amount, @POSName, " +
                                                        "@user_id, 1, @amount, " +
                                                        "0, 0, 0, @cardId, @user_id, getdate(), @loginId, getdate(),'CLOSED', @OrderTypeGroupId); select @@identity";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@POSName", POSMachine);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
            cmd.Parameters.AddWithValue("@cardId", card_id);
            if (orderTypeGroupId < 0)
            {
                cmd.Parameters.AddWithValue("@OrderTypeGroupId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@OrderTypeGroupId", orderTypeGroupId);
            }
            transactionId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Parameters.Clear();
            CommonFuncs CommonFuncs = new CommonFuncs(Utilities);
            string trxNo = "";
            trxNo = CommonFuncs.getNextTrxNo(Utilities.ParafaitEnv.POSMachineId, orderTypeGroupId, cmd.Transaction);
            log.LogVariableState("Trx_No with getNextTrxNo", trxNo);
            cmd.CommandText = "update trx_header set trx_no = @Trx_no, LastUpdatedBy = @loginId , LastUpdateTime = getdate() where trxId = @TrxId";
            cmd.Parameters.AddWithValue("@TrxId", transactionId);
            cmd.Parameters.AddWithValue("@Trx_no", trxNo);
            cmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
            cmd.ExecuteNonQuery();

            log.LogVariableState("@trxId", transactionId);
            log.LogVariableState("@trx_no", trxNo);
            log.LogVariableState("@loginId", Utilities.ParafaitEnv.LoginID);

            log.LogMethodExit(transactionId);
            return transactionId;
        }

        /// <summary>
        /// CreateTransactionLine
        /// </summary>
        /// <param name="AttributeColumn"></param>
        /// <param name="trxId"></param>
        /// <param name="lineId"></param>
        /// <param name="amount"></param>
        /// <param name="cardId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="loyaltyProductId"></param>
        /// <param name="redeemPoint"></param>
        /// <param name="redeemValue"></param>
        /// <param name="userId"></param>
        /// <param name="sourceCpId"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="createVirtualPointLine"></param>
        /// <param name="ticketAllowed"></param>
        public void CreateTransactionLine(string AttributeColumn, int trxId, int lineId, double amount, int cardId, string cardNumber, int loyaltyProductId, double redeemPoint,
                                        double redeemValue, int userId, int sourceCpId, SqlTransaction SQLTrx, bool createVirtualPointLine = false, bool ticketAllowed = false)
        {
            log.LogMethodEntry(AttributeColumn, trxId, lineId, amount, cardId, cardNumber, loyaltyProductId, redeemPoint,
                                        redeemValue, userId, sourceCpId, SQLTrx, createVirtualPointLine);
            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);
            string creditPlusRemarks = string.Empty;
            string insert = "insert into trx_lines (TrxId, LineId, price, amount, quantity, card_id, card_number, product_id, loyalty_points, Createdby, CreationDate, LastUpdatedBy, LastUpdateDate,Remarks ";
            if (string.IsNullOrWhiteSpace(AttributeColumn) || AttributeColumn.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
            {
                insert += ") values (@TrxId, @LineId, @amount, @amount, 1, @card_id, @card_number, @product_id, @redeemPoints, @loginId, getdate(), @loginId, getdate(),@Remarks)";
            }
            else
            {
                insert += ", " + AttributeColumn;
                insert += ") values (@TrxId, @LineId, @amount, @amount, 1, @card_id, @card_number, @product_id, @redeemPoints, @loginId, getdate(), @loginId, getdate(),@Remarks, @redeemValue)";
            }
            trxCmd.CommandText = insert;
            trxCmd.Parameters.Clear();

            trxCmd.Parameters.AddWithValue("@TrxId", trxId);
            trxCmd.Parameters.AddWithValue("@LineId", lineId);
            trxCmd.Parameters.AddWithValue("@card_id", cardId);
            trxCmd.Parameters.AddWithValue("@amount", amount);
            trxCmd.Parameters.AddWithValue("@card_number", cardNumber);
            if (loyaltyProductId == -1)
                trxCmd.Parameters.AddWithValue("@product_id", DBNull.Value);
            else
                trxCmd.Parameters.AddWithValue("@product_id", loyaltyProductId);
            if (createVirtualPointLine)
            {
                trxCmd.Parameters.AddWithValue("@redeemPoints", 0);
                trxCmd.Parameters.AddWithValue("@Remarks", Utilities.MessageUtils.getMessage("Redeem : Redeeming " + redeemPoint + " loyalty points"));
                creditPlusRemarks = Utilities.MessageUtils.getMessage("Redeem VirtualPoint Credit Plus");
            }
            else
            {
                trxCmd.Parameters.AddWithValue("@redeemPoints", redeemPoint * -1);
                trxCmd.Parameters.AddWithValue("@Remarks", Utilities.MessageUtils.getMessage("Redeem : Redeeming " + redeemPoint + " loyalty points"));
                creditPlusRemarks = Utilities.MessageUtils.getMessage("Redeem Loyalty Credit Plus");
            }
            trxCmd.Parameters.AddWithValue("@redeemValue", DBNull.Value);//storing null for trxline since redeemed value are stored in creditplus
            trxCmd.Parameters.AddWithValue("@userId", userId);
            trxCmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);


            trxCmd.ExecuteNonQuery();

            if (!AttributeColumn.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
            {
                string creditPlusType;
                switch (AttributeColumn)
                {
                    case "credits":
                        {
                            creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE);
                            break;
                        }
                    case "Bonus":
                    case "bonus":
                        {
                            creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS);
                            break;
                        }
                    case "time":
                        {
                            creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.TIME);
                            break;
                        }
                    case "tickets":
                        {
                            creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.TICKET);
                            break;
                        }
                    default:
                        {
                            creditPlusType = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE);
                            break;
                        }
                }

                CreateGenericCreditPlusLine(cardId, creditPlusType, redeemValue, false, 0, "N", Utilities.ParafaitEnv.LoginID, creditPlusRemarks, SQLTrx, DateTime.MinValue, Utilities.getServerTime(), trxId, lineId, sourceCpId, ticketAllowed);
            }
        }

        private int GetLoyltyProductOrderTypegroupId(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            int orderTypeGroupId = -1;
            int orderTypeId = -1;

            cmd.CommandText = @"select top 1 IsNull(p.OrderTypeId, pt.OrderTypeId) as OrderTypeId
                                  from Products p, product_type pt
                                 where product_type = 'Loyalty Product'
                                   and p.product_type_id = pt.product_type_id
                                   and (p.site_id = @siteId or @siteId = -1)";
            cmd.Parameters.AddWithValue("@siteId", Utilities.ExecutionContext.GetSiteId());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtLotaltProductOrderTypeId = new DataTable();
            da.Fill(dtLotaltProductOrderTypeId);

            if (dtLotaltProductOrderTypeId.Rows.Count > 0)
            {
                orderTypeId = dtLotaltProductOrderTypeId.Rows[0]["OrderTypeId"] != DBNull.Value ? Convert.ToInt32(dtLotaltProductOrderTypeId.Rows[0]["OrderTypeId"]) : -1;
                if (orderTypeId >= 0)
                {
                    OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
                    List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParameters);
                    OrderTypeGroupDTO orderTypeGroupDTO = null;
                    if (orderTypeGroupDTOList != null)
                    {
                        foreach (var ordTypGrpDTO in orderTypeGroupDTOList)
                        {
                            OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(Utilities.ExecutionContext, ordTypGrpDTO);
                            if (orderTypeGroupBL.Match(new HashSet<int>() { orderTypeId }))
                            {
                                if (orderTypeGroupDTO == null || orderTypeGroupDTO.Precedence < orderTypeGroupBL.OrderTypeGroupDTO.Precedence)
                                {
                                    orderTypeGroupDTO = orderTypeGroupBL.OrderTypeGroupDTO;
                                }
                            }
                        }
                    }
                    if (orderTypeGroupDTO != null)
                    {
                        orderTypeGroupId = orderTypeGroupDTO.Id;
                    }
                }
            }
            log.LogMethodExit(orderTypeGroupId);
            return orderTypeGroupId;
        }
    }
}


