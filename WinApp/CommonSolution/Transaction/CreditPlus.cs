/***************************************************************************************************************************
 * Project Name - CreditPlus
 * Description  - Business Logic to create and save Credit Plus records for a card
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ***************************************************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.50.0      18-Dec-2018      Mathew Ninan   Remove staticDataExchange from calls as Staticdataexchange
 *                                            is deprecated. TransactionPaymentsDTO is added
 *2.60.0      21-Apr-2019      Mathew Ninan   Handling Credit Plus consumption for Check-in/Out products
 *                                            where multiple quantities exist for single transaction line
 *2.60.0      30-May-2019      Mathew Ninan   Price should be adjusted based on number of check-ins. In case
 *                                            of discounted price or discount amount, price should reflect
 *                                            number of check-ins
 *2.70.0      28-Jun-2019      Mathew Ninan   Changed logic of number of check-ins as each checkin is created
 *                                            as individual lines
 *2.70.2.0    28-Nov-2019      Mathew NInan   Added Card Credit Plus id in deductCreditPlusConsumption.
 *                                            This will help in better tracking and reconciliation
 *2.80.0      19-Mar-2020       Jinto Thomas    Added method DeductFromCreditPlusRecord() part of redeemloyalty
 *                                              enhancement
 *2.80.0      19-Mar-2020   Mathew NInan           Use new field ValidityStatus to track
 *                                                 status of entitlements
 *2.110.0     24-Nov-2020     Mathew Ninan    Ticket Allowed for CreditPlusForGamePlay should ignore ticketallowed
 *                                            status of 0 creditplusBalance records
 *2.130.4     02-Feb-2022      Mathew Ninan   DeductCreditPlusForGamePlay - performance improvement
 *2.130.4     22-Feb-2022     Mathew Ninan      Modified DateTime to ServerDateTime     
 *2.130.12    16-Dec-2022     Mathew Ninan    Fine tuned GetPromotionPriceForProduct method for optimizing consumption balance 
 *                                            GetCreditPlusForGamePlay -- TicketAllowed logic modified to consider 0 balance as N
 *****************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Customer.Accounts;

//using Semnox.Parafait.Tags.CardActivityLog;

namespace Semnox.Parafait.Transaction
{
    public class CreditPlus
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        public CreditPlus(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            log.LogMethodExit(null);
        }

        public double getCreditPlusRefund(int cardId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(cardId, SQLTrx);
            double value =  Convert.ToDouble(Utilities.executeScalar("select creditPlusRefundableBalance from CardView  where card_id = @cardId", SQLTrx,
                                                            new SqlParameter("@cardId", cardId)));

            log.LogVariableState("@cardId", cardId);
            log.LogMethodExit(value);
            return value;
        }

        public double getBalanceCreditPlus(int cardId, ref double CreditPlusCardBalance, ref double CreditPlusCredits, ref double CreditPlusBonus, ref double creditPlusItemPurchase)
        {
            log.LogMethodEntry(cardId, CreditPlusCardBalance, CreditPlusCredits, CreditPlusBonus, creditPlusItemPurchase);

            DataTable dt = Utilities.executeDataTable(@"select CreditPlusCardBalance, CreditPlusCredits, CreditPlusBonus, creditPlusItemPurchase 
                                                        from CardView
                                                        where card_id = @card_id", 
                                                       new SqlParameter("@card_id", cardId));

            log.LogVariableState("@card_id", cardId);

            if (dt.Rows.Count > 0)
            {
                CreditPlusCardBalance = Convert.ToDouble(dt.Rows[0]["CreditPlusCardBalance"]);
                CreditPlusCredits = Convert.ToDouble(dt.Rows[0]["CreditPlusCredits"]);
                CreditPlusBonus = Convert.ToDouble(dt.Rows[0]["CreditPlusBonus"]);
                creditPlusItemPurchase = Convert.ToDouble(dt.Rows[0]["creditPlusItemPurchase"]);
            }

            log.LogVariableState("CreditPlusCardBalance ", CreditPlusCardBalance);
            log.LogMethodExit(CreditPlusCardBalance + CreditPlusCredits);
            log.LogVariableState("CreditPlusBonus ", CreditPlusBonus);
            log.LogVariableState("creditPlusItemPurchase ", creditPlusItemPurchase);

            double returnValueNew = (CreditPlusCardBalance + CreditPlusCredits);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public decimal getCreditPlusForGamePlay(int cardId, int MachineId, ref decimal CardBalance, ref decimal GamePlayCredits, ref decimal GamePlayBonus, ref bool TicketAllowed, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(cardId, MachineId, CardBalance, GamePlayCredits, GamePlayBonus, TicketAllowed);

            DataTable dt = Utilities.executeDataTable(@"select isnull(sum(case when CreditPlusType = 'A' then CreditPlusBalance else 0 end), 0) CardBalance,
                                                 isnull(sum(case when CreditPlusType = 'G' then CreditPlusBalance else 0 end), 0) GamePlayCredits,
                                                 isnull(sum(case when CreditPlusType = 'B' then CreditPlusBalance else 0 end), 0) GamePlayBonus,
                                                 MAX(case when isnull(creditPlusBalance, 0) <= 0 then 'N' 
												          else case 
														         when ticketallowed  IS NULL
														         then 'N' 
																 WHEN ticketallowed = 0 
                                                                 then 'N' 
																 else 'Y' end 
                                                      end) TicketAllowed
                                        from CardCreditPlus cp
                                        where cp.card_id = @card_id
                                        and CreditPlusType in ('G', 'A', 'B')
                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate())
                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                          and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then cp.Sunday 
                                                        when 2 then cp.Monday 
                                                        when 3 then cp.Tuesday 
                                                        when 4 then cp.Wednesday 
                                                        when 5 then cp.Thursday 
                                                        when 6 then cp.Friday 
                                                        when 7 then cp.Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
													                                                AND ed.EntityName = 'CARDCREDITPLUS'
													                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                            OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                AND  ed.IncludeExcludeFlag = 1)
                                               )
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                          and (exists (select 1 
                                                        from CardCreditPlusConsumption cn, machines m, games g 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId 
                                                        and (cn.GameId = m.Game_Id or cn.GameId is null) 
                                                        and (cn.GameProfileId = g.game_profile_id or cn.GameProfileId is null)
                                                        and m.machine_id = @machineId
                                                        and m.game_id = g.game_id
                                                        and cn.ProductId is null
                                                        and cn.CategoryId is null
                                                        and cn.POSTypeId is null)
                                                 OR 
                                                not exists (select 1 
                                                        from CardCreditPlusConsumption cn 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId
                                                        /*and (GameId is not null or GameProfileId is not null)*/))", sqlTrx,
                                            new SqlParameter("@card_id", cardId),
                                            new SqlParameter("@machineId", MachineId));

            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@machineId", MachineId);

            if (dt.Rows.Count > 0)
            {
                CardBalance = Convert.ToDecimal(dt.Rows[0]["CardBalance"]);
                GamePlayCredits = Convert.ToDecimal(dt.Rows[0]["GamePlayCredits"]);
                GamePlayBonus = Convert.ToDecimal(dt.Rows[0]["GamePlayBonus"]);
                if ((CardBalance + GamePlayCredits + GamePlayBonus) <= 0)
                    TicketAllowed = true;
                else 
                    TicketAllowed = dt.Rows[0]["TicketAllowed"].ToString() == "N" ? false : true;
            }

            log.LogVariableState("CardBalance ,", CardBalance);
            log.LogVariableState("GamePlayCredits ,", GamePlayCredits);
            log.LogVariableState("GamePlayBonus ,", GamePlayBonus);
            log.LogVariableState("TicketAllowed ,", TicketAllowed);

            decimal returnValueNew = (CardBalance + GamePlayCredits + GamePlayBonus);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public decimal getCreditPlusTimeForGamePlay(int cardId, int MachineId, ref int CardCreditPlusId, ref bool TicketAllowed, ref DateTime PlayStartTime, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(cardId, MachineId, CardCreditPlusId, TicketAllowed, PlayStartTime);

            DataTable dt = Utilities.executeDataTable(@"select case when PlayStartTime is null 
	                                                then CreditPlusBalance 			
	                                                else case when TimeTo is null 
		                                                then datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))
		                                                else datediff(MI, getdate(), 
			                                                (select min(endTime) 
				                                                from (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
						                                                union all 
					                                                  select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)) 
		                                                end
	                                                end TimeBalance, CardCreditPlusId, TicketAllowed, PlayStartTime
                                        from CardCreditPlus cp
                                        where cp.card_id = @card_id
                                        and CreditPlusType in ('M')
                                        and isnull(cp.ValidityStatus, 'Y') != 'H'
                                        and (case when PlayStartTime is null then getdate() else dateadd(MI, CreditPlusBalance, PlayStartTime) end) >= getdate()
                                          and (cp.TimeFrom is null or DATEADD(MI, (timefrom - cast(timefrom as integer))*100, DateAdd(HH, cast(timefrom as integer), dateadd(D, 0, datediff(D, 0, getdate())))) <= getdate())
                                          and (cp.TimeTo is null or DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else cast(timeto as integer) end, dateadd(D, 0, datediff(D, 0, getdate())))) >= getdate())  
                                          and (((cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
												  and (cp.PeriodTo is null or cp.PeriodTo > getdate())
												  and isnull(case DATEPART(WEEKDAY, getdate()) 
																when 1 then cp.Sunday 
																when 2 then cp.Monday 
																when 3 then cp.Tuesday 
																when 4 then cp.Wednesday 
																when 5 then cp.Thursday 
																when 6 then cp.Friday 
																when 7 then cp.Saturday 
																else 'Y' end, 'Y') = 'Y')
												OR EXISTS (select 1 
												             from EntityOverrideDates ed 
															WHERE ed.EntityGuid = cp.Guid 
													          AND ed.EntityName = 'CARDCREDITPLUS'
													          AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															        OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													          AND  ed.IncludeExcludeFlag = 1)
												)
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                          and (@machineId = -1
                                               OR
                                               exists (select 1 
                                                        from CardCreditPlusConsumption cn, machines m, games g 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId 
                                                        and (cn.GameId = m.Game_Id or cn.GameId is null) 
                                                        and (cn.GameProfileId = g.game_profile_id or cn.GameProfileId is null)
                                                        and m.machine_id = @machineId
                                                        and m.game_id = g.game_id
                                                        and cn.ProductId is null
                                                        and cn.CategoryId is null
                                                        and cn.POSTypeId is null)
                                               OR 
                                               not exists (select 1 
                                                        from CardCreditPlusConsumption cn 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId
                                                        and (GameId is not null or GameProfileId is not null)))
                                          order by PlayStartTime desc, cp.PeriodTo", sqlTrx,
                                        new SqlParameter("@card_id", cardId),
                                        new SqlParameter("@machineId", MachineId));

            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@machineId", MachineId);

            decimal TimeBalance = 0;
            if (dt.Rows.Count > 0)
            {
                //TimeBalance = Convert.ToDecimal(dt.Rows[0]["TimeBalance"]);
                CardCreditPlusId = Convert.ToInt32(dt.Rows[0]["CardCreditPlusId"]);
                TicketAllowed = Convert.ToBoolean(dt.Rows[0]["TicketAllowed"].ToString());
                PlayStartTime = dt.Rows[0]["PlayStartTime"] == DBNull.Value ? ServerDateTime.Now : Convert.ToDateTime(dt.Rows[0]["PlayStartTime"]);
            }
            foreach (DataRow rw in dt.Rows)
            {
                TimeBalance += Convert.ToDecimal(rw["TimeBalance"]);
            }

            log.LogVariableState("CardCreditPlusId ,", CardCreditPlusId);
            log.LogVariableState("TicketAllowed ,", TicketAllowed);
            log.LogVariableState("PlayStartTime ,", PlayStartTime);
            log.LogMethodExit(TimeBalance);
            return TimeBalance;
        }

        public double getCreditPlusForPOS(int cardId, int POSTypeId, object pTrx)
        {
            log.LogMethodEntry(cardId, POSTypeId, pTrx);

            string TrxProducts = "(-1, ";
            Transaction Trx = pTrx as Transaction;
            if (Trx != null)
            {
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    if (Trx.TrxLines[i] != null && Trx.TrxLines[i].LineValid && Trx.TrxLines[i].ProductID > 0)
                    {
                        TrxProducts += Trx.TrxLines[i].ProductID.ToString() + ", ";
                    }
                }
            }

            TrxProducts = TrxProducts.TrimEnd(',', ' ') + ")";

            string commandText = @"select isnull(sum(CreditPlus), 0), isnull(sum(TrxProdPrice), 0)
                                          from
                                          (select isnull(sum(CreditPlusBalance), 0) CreditPlus,
                                                  (select isnull(SUM(case when p.TaxInclusivePrice='Y' then price else price * (100 + t.tax_percentage) / 100 end), cp.CreditPlusBalance) 
                                                    from CardCreditPlusConsumption cn, products p left outer join tax t on t.tax_id = p.tax_id
                                                    where cn.CardCreditPlusId = cp.CardCreditPlusId
                                                    and p.product_id = cn.ProductId
                                                        and (cn.POSTypeId is null or cn.POSTypeId = @POSTypeId) 
                                                        and (cn.ProductId is null or cn.ProductId in " + TrxProducts + @")
                                                    --    and (cn.CategoryId is null or cn.CategoryId in (select CategoryId from products where product_id in " + TrxProducts + @"))
                                                        and (cn.CategoryId is null 
					                                        or 
						                                        exists
						                                        (
						                                          select 1 
                                                                    from getcategoryList(cn.CategoryId) c, products p
						                                           where c.CategoryId = p.categoryid and product_id in " + TrxProducts + @"
                                                                )
					                                        )
                                                        and cn.GameProfileId is null
                                                        and cn.GameId is null) TrxProdPrice
                                        from CardCreditPlus cp
                                        where card_id = @cardId 
                                          and cp.CreditPlusType in ('P', 'A') 
                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                          and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then cp.Sunday 
                                                        when 2 then cp.Monday 
                                                        when 3 then cp.Tuesday 
                                                        when 4 then cp.Wednesday 
                                                        when 5 then cp.Thursday 
                                                        when 6 then cp.Friday 
                                                        when 7 then cp.Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
													                                                AND ed.EntityName = 'CARDCREDITPLUS'
													                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                            OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                AND  ed.IncludeExcludeFlag = 1)
                                               )
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                           and (MinimumSaleAmount <= @SaleAmount
                                               OR MinimumSaleAmount is null
                                                  and (
                                                        not exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId)
                                                        OR exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId
                                                                    and ProductId in " + TrxProducts + @")
                                                      )
                                              )
                                          and (exists (select 1 
                                                        from CardCreditPlusConsumption cn 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId 
                                                        and (cn.POSTypeId is null or cn.POSTypeId = @POSTypeId) 
                                                        and (cn.ProductId is null or cn.ProductId in " + TrxProducts + @")
                                                     --   and (cn.CategoryId is null or cn.CategoryId in (select CategoryId from products where product_id in " + TrxProducts + @"))
                                                        and (cn.CategoryId is null 
					                                        or 
						                                        exists
						                                        (
						                                          select 1 
                                                                    from getcategoryList(cn.CategoryId) c, products p
						                                           where c.CategoryId = p.categoryid and product_id in " + TrxProducts + @"
                                                                )
					                                        )
                                                        and cn.GameProfileId is null
                                                        and cn.GameId is null)
                                                 OR 
                                                not exists (select 1 
                                                        from CardCreditPlusConsumption cn 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId
                                                       /* and (POSTypeId is not null or ProductId is not null or categoryId is not null)*/))
                                           group by CardCreditPlusId, CreditPlusBalance) viw";

            DataTable dt = Utilities.executeDataTable(commandText,
                                                    new SqlParameter("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount)),
                                                    new SqlParameter("@cardId", cardId),
                                                    new SqlParameter("@POSTypeId", POSTypeId));

            log.LogVariableState("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount));
            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@POSTypeId", POSTypeId);

            double creditPlus = Convert.ToDouble(dt.Rows[0][0]);
            double TrxProdPrice = Convert.ToDouble(dt.Rows[0][1]);
            //return (Math.Min(creditPlus, TrxProdPrice));

            log.LogMethodExit(creditPlus);
            return creditPlus;
        }

        public double getPromotionPriceForProduct(int cardId, int ProductId, int POSTypeId, object pTrx, ref int CreditPlusConsumptionId, ref bool AllProducts, ref int orderTypeId)
        {
            log.LogMethodEntry(cardId, ProductId, POSTypeId, pTrx, CreditPlusConsumptionId, AllProducts);

            string TrxProducts = "(" + ProductId.ToString() + ", ";
            string CPCIds = "(-1, ";
            string CPCQuery = "(select -1 CPCId, 0 used";
            bool cardConsumptionQuantityLimitExists = false;//Use this flag to add quantity limit query
            Dictionary<int, int> CPCDict = new Dictionary<int, int>();//Dictionary to hold consumptionId occurence and count of same
            Transaction Trx = pTrx as Transaction;
            if (Trx != null)
            {
                List<int> previousProductId = new List<int>();
                previousProductId.Add(ProductId);
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    if (Trx.TrxLines[i] != null && Trx.TrxLines[i].LineValid && Trx.TrxLines[i].ProductID > 0)
                    {
                        if (!previousProductId.Contains(Trx.TrxLines[i].ProductID))
                        {
                            TrxProducts += Trx.TrxLines[i].ProductID.ToString() + ", ";
                            previousProductId.Add(Trx.TrxLines[i].ProductID); 
                        }
                        if (Trx.TrxLines[i].CreditPlusConsumptionId > 0)
                        {
                            if (CPCDict.ContainsKey(Trx.TrxLines[i].CreditPlusConsumptionId))
                            {
                                CPCDict[Trx.TrxLines[i].CreditPlusConsumptionId] = CPCDict[Trx.TrxLines[i].CreditPlusConsumptionId] + 1;
                            }
                            else
                            {
                                CPCDict.Add(Trx.TrxLines[i].CreditPlusConsumptionId, 1);
                            }
                            CPCIds += Trx.TrxLines[i].CreditPlusConsumptionId.ToString() + ", ";
                           // CPCQuery += " union all select " + Trx.TrxLines[i].CreditPlusConsumptionId.ToString() + ", 1";
                        }
                    }
                }
                if (CPCDict.Count > 0) //has values
                {
                    foreach (KeyValuePair<int, int> cpcValue in CPCDict)
                    {
                        CPCQuery += " union all select " + cpcValue.Key.ToString() + ", " + cpcValue.Value.ToString() + " ";
                    }
                }
            }
            CPCQuery += ") v where v.CPCId = cpn.PKId) ";

            TrxProducts = TrxProducts.TrimEnd(',', ' ') + ")";
            CPCIds = CPCIds.TrimEnd(',', ' ') + ") ";

            object objCPCConsumptionExists = Utilities.executeScalar(@"select 1 
                                                                        from cards c
                                                                        where card_id = @cardId
                                                                        and exists (select 'x' 
                                                                                      from CardCreditPlus ccp, CardCreditPlusConsumption ccpc
				                                                                     where ccp.CardCreditPlusId = ccpc.CardCreditPlusId
				                                                                        and ccp.Card_id = c.card_id
				                                                                        and isnull(ccpc.QuantityLimit, 0) > 0)",
                                                                       new SqlParameter("@cardId", cardId));
            if(objCPCConsumptionExists != null && objCPCConsumptionExists != DBNull.Value)
            {
                cardConsumptionQuantityLimitExists = Convert.ToBoolean(objCPCConsumptionExists);
            }
            //21-Apr-2019 - Added consumptionBalance to be returned from the query for validation
            string commandText = @"select cpn.PKId, isnull(isnull(DiscountedPrice, isnull((case when sign(Price - case when DiscountAmount = 0 then null else DiscountAmount end) = -1 then 0 else Price - DiscountAmount end), 
                                                                                                Price * (1 - (DiscountPercentage / 100)))), Price),
                                             ISNULL(cpn.ConsumptionBalance, 0) ConsumptionBalance, isnull(cpn.ProductId, 0) AllProducts, cpn.OrderTypeId 
                                        from CardCreditPlus cp, CardCreditPlusConsumption cpn, products p
                                        where card_id = @cardId 
                                          and p.product_id = @ProductId 
                                          and cpn.CardCreditPlusId = cp.CardCreditPlusId 
                                          and isnull(cp.CreditPlus, 0) = 0
                                          and (DiscountedPrice is not null or DiscountPercentage is not null or DiscountAmount is not null)
                                          and GameId is null and GameProfileId is null
                                          and (cpn.ProductId is null or cpn.ProductId = @ProductId)
                                          and (cpn.POSTypeId is null or cpn.POSTypeId = @POSTypeId)
                                          and (cpn.CategoryId is null 
					                            or 
						                            exists
						                            (
						                              select 1 
                                                        from getcategoryList(cpn.CategoryId) c
						                               where c.CategoryId = p.categoryid 
                                                         and product_id in (@productId)
                                                    )
					                            )
                                          and cpn.ConsumptionBalance > (select isnull(sum(v.used), 0) from " + CPCQuery +
                                        @"and cp.CreditPlusType in ('P', 'A') 
                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                           and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then cp.Sunday 
                                                        when 2 then cp.Monday 
                                                        when 3 then cp.Tuesday 
                                                        when 4 then cp.Wednesday 
                                                        when 5 then cp.Thursday 
                                                        when 6 then cp.Friday 
                                                        when 7 then cp.Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
													                                                AND ed.EntityName = 'CARDCREDITPLUS'
													                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                            OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                AND  ed.IncludeExcludeFlag = 1)
                                               )
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                          and (MinimumSaleAmount <= @SaleAmount
                                               OR MinimumSaleAmount is null
                                                  and (
                                                        not exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId)
                                                        OR exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId
                                                                    and ProductId in " + TrxProducts + @")
                                                      )
                                              ) "
                                        + (cardConsumptionQuantityLimitExists ? @"
                                         and (cpn.QuantityLimit > (select count(1) 
                                                                     from trx_lines l, trx_header h
                                                                     where h.trxId = l.trxId
                                                                     and h.trxdate between dateadd(HOUR, 6, dateadd(D, datediff(D, 0, getdate()), 0)) 
                                                                                       and dateadd(HOUR, 6, dateadd(D, datediff(D, 0, getdate()) + 1, 0))
                                                                     and l.CreditPlusConsumptionId = cpn.PKId) 
                                                                  + (select isnull(sum(v.used), 0) from " + CPCQuery + @") 
                                        order by cpn.ProductId desc, cpn.CategoryId desc, cpn.POSTypeId desc, 1" : " order by cpn.ProductId desc, cpn.CategoryId desc, cpn.POSTypeId desc, 1");

            DataTable dt = Utilities.executeDataTable(commandText,
                                                    new SqlParameter("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount)),
                                                    new SqlParameter("@cardId", cardId),
                                                    new SqlParameter("@POSTypeId", POSTypeId),
                                                    new SqlParameter("@ProductId", ProductId));

            log.LogVariableState("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount));
            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@POSTypeId", POSTypeId);
            log.LogVariableState("@ProductId", ProductId);


            if (dt.Rows.Count == 0)
            {
                log.LogVariableState("CreditPlusConsumptionId ,", CreditPlusConsumptionId);
                log.LogVariableState("AllProducts ,", AllProducts);
                log.LogMethodExit(-1);
                return (-1);
            }
            else
            {
                double returnValueNew = Convert.ToDouble(dt.Rows[0][1]);
                //21-Apr-2019 - Check consumption Balance based on checkedin Units. Validation is required to ensure all checked in units are
                //covered in the Consumption Balance. 
                //if (Trx != null && Trx.gCheckIn != null && Trx.gCheckIn.CheckedInUnits > 1)
                //{
                //    if (Convert.ToInt32(dt.Rows[0]["ConsumptionBalance"]) < Trx.gCheckIn.CheckedInUnits)
                //    {
                //        log.LogMethodExit(-1);
                //        return (-1);
                //    }
                //    returnValueNew = returnValueNew * Trx.gCheckIn.CheckedInUnits;
                //}
                ////21-Apr-2019 - Check consumption Balance based on Checked out Units. Validation is required to ensure all checked out units are
                ////covered in the Consumption Balance. 
                //if (Trx != null && Trx.gCheckOut != null && Trx.gCheckOut.balanceCheckIns > 1)
                //{
                //    if (Convert.ToInt32(dt.Rows[0]["ConsumptionBalance"]) < Trx.gCheckOut.balanceCheckIns)
                //    {
                //        log.LogMethodExit(-1);
                //        return (-1);
                //    }
                //    returnValueNew = returnValueNew * Trx.gCheckOut.balanceCheckIns;
                //}
                CreditPlusConsumptionId = Convert.ToInt32(dt.Rows[0]["PKId"]);
                AllProducts = (dt.Rows[0]["AllProducts"].ToString() == "0");
                orderTypeId = dt.Rows[0]["OrderTypeId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["OrderTypeId"]) : -1;
                log.LogVariableState("CreditPlusConsumptionId ,", CreditPlusConsumptionId);
                log.LogVariableState("AllProducts ,", AllProducts);
                log.LogVariableState("OrderType ,", orderTypeId);

                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
        }

        public void deductCreditPlus(int TrxId, int cardId, double usedCreditPlus, object pTrx, SqlTransaction SQLTrx, int POSTypeId, string loginId, int splitId = -1, string paymentOTP = null)
        {
            log.LogMethodEntry(TrxId, cardId, usedCreditPlus, pTrx, SQLTrx, POSTypeId, loginId, paymentOTP);

            SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);

            string TrxProducts = "(-1, ";
            Transaction Trx = pTrx as Transaction;
            if (Trx != null)
            {
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    if (Trx.TrxLines[i] != null && Trx.TrxLines[i].LineValid && Trx.TrxLines[i].ProductID > 0)
                    {
                        TrxProducts += Trx.TrxLines[i].ProductID.ToString() + ", ";
                    }
                }
            }

            TrxProducts = TrxProducts.TrimEnd(',', ' ') + ")";

            creditPluscmd.CommandText = @"select distinct cp.CardCreditPlusId, CreditPlusBalance, 1 sort, isnull(PeriodTo, getdate() + 999) expiryDate 
                                        from CardCreditPlus cp, CardCreditPlusConsumption cn 
                                        where cn.CardCreditPlusId = cp.CardCreditPlusId 
                                          and cp.CreditPlusBalance > 0 
                                          and (cn.POSTypeId is null or cn.POSTypeId = @POSTypeId) 
                                          and (cn.ProductId is null or cn.ProductId in " + TrxProducts + @")
                                      --    and (cn.CategoryId is null or cn.CategoryId in (select CategoryId from products where product_id in " + TrxProducts + @"))
                                          and (cn.CategoryId is null 
					                            or 
						                            exists
						                            (
						                              select 1 
                                                        from getcategoryList(cn.CategoryId) c, products p
						                               where c.CategoryId = p.categoryid and product_id in " + TrxProducts + @"
                                                    )
					                            )
                                          and cn.GameProfileId is null
                                          and cn.GameId is null
                                          and card_id = @cardId 
                                          and cp.CreditPlusType in ('P', 'A')
                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                           and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then cp.Sunday 
                                                        when 2 then cp.Monday 
                                                        when 3 then cp.Tuesday 
                                                        when 4 then cp.Wednesday 
                                                        when 5 then cp.Thursday 
                                                        when 6 then cp.Friday 
                                                        when 7 then cp.Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
													                                                AND ed.EntityName = 'CARDCREDITPLUS'
													                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                            OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                AND  ed.IncludeExcludeFlag = 1)
                                               )
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                          and (MinimumSaleAmount <= @SaleAmount
                                               OR MinimumSaleAmount is null
                                                  and (
                                                        not exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId)
                                                        OR exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId
                                                                    and ProductId in " + TrxProducts + @")
                                                      )
                                              )
                                        union all
                                        select cp.CardCreditPlusId, CreditPlusBalance, 2, isnull(PeriodTo, getdate() + 999) 
                                        from CardCreditPlus cp
                                        where card_id = @cardId
                                          and cp.CreditPlusBalance > 0 
                                          and cp.CreditPlusType in ('P', 'A')
                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                           and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then cp.Sunday 
                                                        when 2 then cp.Monday 
                                                        when 3 then cp.Tuesday 
                                                        when 4 then cp.Wednesday 
                                                        when 5 then cp.Thursday 
                                                        when 6 then cp.Friday 
                                                        when 7 then cp.Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
													                                                AND ed.EntityName = 'CARDCREDITPLUS'
													                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                            OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                AND  ed.IncludeExcludeFlag = 1)
                                               )
                                            AND NOT EXISTS (select 1
                                                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                                AND ed.EntityName = 'CARDCREDITPLUS'
                                                AND isnull(IncludeExcludeFlag, 0) = 0
                                                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                )
                                          and (MinimumSaleAmount <= @SaleAmount
                                               OR MinimumSaleAmount is null
                                                  and (
                                                        not exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId)
                                                        OR exists (select 1
                                                                    from CardCreditPlusPurchaseCriteria
                                                                    where CardCreditPlusId = cp.CardCreditPlusId
                                                                    and ProductId in " + TrxProducts + @")
                                                      )
                                              )
                                          and not exists (select 1 
                                                        from CardCreditPlusConsumption cn 
                                                        where cn.CardCreditPlusId = cp.CardCreditPlusId
                                                       /* and (POSTypeId is not null or ProductId is not null or CategoryId is not null)*/)
                                          order by 3, 4";

            creditPluscmd.Parameters.AddWithValue("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount));
            creditPluscmd.Parameters.AddWithValue("@cardId", cardId);
            creditPluscmd.Parameters.AddWithValue("@POSTypeId", POSTypeId);

            log.LogVariableState("@SaleAmount", (Trx == null ? 0 : Trx.Transaction_Amount));
            log.LogVariableState("@cardId", cardId);
            log.LogVariableState("@POSTypeId", POSTypeId);

            SqlDataAdapter da = new SqlDataAdapter(creditPluscmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "update CardCreditPlus set CreditPlusBalance = case when CreditPlusBalance - @reduceAmount <= 0 then 0 else CreditPlusBalance - @reduceAmount end, " +
                                    "LastupdatedDate = getdate(), LastUpdatedBy = @user " +
                                "where CardCreditPlusId = @CardCreditPlusId";

            double cardCreditPlus;
            double reduceAmount;
            double saveusedCreditPlus = usedCreditPlus;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cardCreditPlus = Convert.ToDouble(dt.Rows[i]["CreditPlusBalance"]);
                if (usedCreditPlus > cardCreditPlus)
                {
                    reduceAmount = cardCreditPlus;
                }
                else
                    reduceAmount = usedCreditPlus;
                usedCreditPlus -= reduceAmount; 

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reduceAmount", reduceAmount);
                cmd.Parameters.AddWithValue("@user", loginId);
                cmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                cmd.ExecuteNonQuery();
                
                log.LogVariableState("@reduceAmount", reduceAmount);
                log.LogVariableState("@user", loginId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);

                if (TrxId > 0)
                {
                    string creditPlusEntitlementType = string.Empty;
                    AccountDTO creditPlusAccountDTO = new AccountBL(Utilities.ExecutionContext, cardId, true, true, SQLTrx).AccountDTO;
                    if (creditPlusAccountDTO != null && creditPlusAccountDTO.AccountCreditPlusDTOList.Count > 0)
                    {
                        creditPlusEntitlementType = CreditPlusTypeConverter.ToString(creditPlusAccountDTO.AccountCreditPlusDTOList.Find(x => x.AccountCreditPlusId == Convert.ToInt32(dt.Rows[i]["CardCreditPlusId"])).CreditPlusType);
                    }
                    PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.IsCorporate ? Convert.ToString(Utilities.ExecutionContext.GetSiteId()) : "-1"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if (paymentModeDTOList != null)
                    {
                        TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                        debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                        debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        debitTrxPaymentDTO.TransactionId = TrxId;
                        debitTrxPaymentDTO.Amount = reduceAmount;
                        debitTrxPaymentDTO.CardId = (int)cardId;
                        debitTrxPaymentDTO.SplitId = splitId;
                        debitTrxPaymentDTO.PosMachine = Trx.Utilities.ParafaitEnv.POSMachine;
                        debitTrxPaymentDTO.CardEntitlementType = String.IsNullOrEmpty(creditPlusEntitlementType) ? "C" : creditPlusEntitlementType;
                        debitTrxPaymentDTO.CardCreditPlusId = Convert.ToInt32(dt.Rows[i]["CardCreditPlusId"]);
                        debitTrxPaymentDTO.PaymentModeOTP = paymentOTP;
                        TransactionPaymentsBL debitTrxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, debitTrxPaymentDTO);
                        debitTrxPaymentBL.Save(SQLTrx);
                    }
                    
                    //TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                    //transactionUtils.CreateCreditPlusTrxPayment(TrxId, Convert.ToInt32(dt.Rows[i]["CardCreditPlusId"]), reduceAmount, SQLTrx);
                }

                if (usedCreditPlus <= 0)
                    break;
            }

            cmd.CommandText = "update cards set credits_played = isnull(credits_played, 0) + @usedCreditPlus, " +
                                        "last_update_time = getdate(), " +
                                        "LastUpdatedBy = @LastUpdatedBy " +
                                        "where card_id = @cardId";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@cardId", cardId);
            if (saveusedCreditPlus > 0)
            {
                cmd.Parameters.AddWithValue("@usedCreditPlus", saveusedCreditPlus);
            }
            else
            {
                cmd.Parameters.AddWithValue("@usedCreditPlus", 0);
            }
            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cmd.ExecuteNonQuery();

            log.LogVariableState("@cardId", cardId);
            log.LogVariableState("@usedCreditPlus", saveusedCreditPlus);
            log.LogVariableState("@LastUpdatedBy", loginId);

            log.LogMethodExit(null);
        }

        public bool deductCreditPlusConsumptionBalance(object pTrx, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(pTrx, SQLTrx);

            Transaction Trx = pTrx as Transaction;
            SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);
            creditPluscmd.Parameters.AddWithValue("@PKId", 0);
            //21-Apr-2019 - New parameter added to pass consumed value. default value is 1
            creditPluscmd.Parameters.AddWithValue("@ConsumedValues", 1);
            creditPluscmd.CommandText = "update CardCreditPlusConsumption set ConsumptionBalance = ConsumptionBalance - @ConsumedValues where ConsumptionBalance > 0 and PKID = @PKId";
            if (Trx != null)
            {
                int prevCardId = -1;
                //TransactionUtils trxUtils = new TransactionUtils(Utilities);
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    if (Trx.TrxLines[i] != null
                        && Trx.TrxLines[i].LineValid
                        && Trx.TrxLines[i].CreditPlusConsumptionId > 0)
                    {
                        // check if line already has creditplus consumption already utilized
                        if (Utilities.executeScalar(@"select 1 
                                                       from trx_lines 
                                                        where trxId = @trxId 
                                                        and lineId = @lineId 
                                                        and CreditPlusConsumptionId is not null",
                                                   //     SQLTrx,
                                                    new SqlParameter("@trxId", Trx.Trx_id),
                                                    new SqlParameter("@lineId", Trx.TrxLines[i].DBLineId)) == null)
                        {
                            log.LogVariableState("@trxId", Trx.Trx_id);
                            log.LogVariableState("@lineId", Trx.TrxLines[i].DBLineId);

                            creditPluscmd.Parameters[0].Value = Trx.TrxLines[i].CreditPlusConsumptionId;
                            //21-Apr-2019 - If Check in object exists and checked in unit is more than 1
                            //set value to CheckedinUnits
                            //if (Trx.TrxLines[i].LineCheckIn != null && Trx.TrxLines[i].LineCheckIn.CheckedInUnits > 1)
                            //{
                            //    creditPluscmd.Parameters[1].Value = Trx.TrxLines[i].LineCheckIn.CheckedInUnits;
                            //}
                            ////21-Apr-2019 - If Check out object exists and checked out unit is more than 1
                            ////set value to checked out units
                            //if (Trx.TrxLines[i].LineCheckOut != null && Trx.TrxLines[i].LineCheckOut.balanceCheckIns > 1)
                            //{
                            //    creditPluscmd.Parameters[1].Value = Trx.TrxLines[i].LineCheckOut.balanceCheckIns;
                            //}
                            if (creditPluscmd.ExecuteNonQuery() == 0)
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                            else
                            {
                                int cardId = -1;
                                int cardCreditPlusId = -1;
                                DataTable dtcarddetails = Utilities.executeDataTable(@"select cp.card_id, cp.CardCreditPlusId 
                                                                        from cardCreditPlus cp, cardCreditPlusConsumption cpc 
                                                                        where cpc.CardCreditplusId = cp.CardCreditPlusId
                                                                        and cpc.PkId = @cpcId",
                                                                            SQLTrx,
                                                                            new SqlParameter("@cpcId", Trx.TrxLines[i].CreditPlusConsumptionId));

                                log.LogVariableState("@cpcId", Trx.TrxLines[i].CreditPlusConsumptionId);
                                if (dtcarddetails != null && dtcarddetails.Rows.Count > 0)
                                {
                                    cardId = (int)dtcarddetails.Rows[0]["card_id"];
                                    cardCreditPlusId = (int)dtcarddetails.Rows[0]["CardCreditPlusId"];
                                    if (prevCardId != cardId)
                                    {
                                        Card updateCard = new Card(cardId, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                                        updateCard.updateCardTime(SQLTrx);
                                        prevCardId = cardId;
                                    }
                                }
                                PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                                if (paymentModeDTOList != null)
                                {
                                    TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                                    debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                    debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                    debitTrxPaymentDTO.TransactionId = Trx.Trx_id;
                                    debitTrxPaymentDTO.Amount = 0;
                                    debitTrxPaymentDTO.CardId = cardId;
                                    debitTrxPaymentDTO.CardCreditPlusId = cardCreditPlusId;
                                    debitTrxPaymentDTO.PosMachine = Trx.Utilities.ParafaitEnv.POSMachine;
                                    debitTrxPaymentDTO.CardEntitlementType = "C";
                                    TransactionPaymentsBL debitTrxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, debitTrxPaymentDTO);
                                    debitTrxPaymentBL.Save(SQLTrx);
                                }
                                   // trxUtils.CreateCardTrxPayment(Trx.Trx_id, (int)cardId, 0, SQLTrx);
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(true);
            return true;
        }

        public void refundCreditPlus(int TrxId, int cardId, double refundCreditPlus, SqlTransaction Trx, string loginId)
        {
            log.LogMethodEntry(TrxId, cardId, refundCreditPlus, Trx, loginId);

            SqlCommand creditPluscmd = Utilities.getCommand(Trx);
            creditPluscmd.CommandText = "select cp.CardCreditPlusId, CreditPlusBalance " +
                                        "from CardCreditPlus cp " +
                                        "where card_id = @cardId " +
                                        "and refundable = 'Y' " +
                                        "and CreditPlusBalance > 0 " +
                                        "and CreditPlusType in ('A', 'G', 'P') " +
                                        @"and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate())";

            creditPluscmd.Parameters.AddWithValue("@cardId", cardId);

            SqlDataAdapter da = new SqlDataAdapter(creditPluscmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            SqlCommand cmd = Utilities.getCommand(Trx);
            //SqlCommand cmdLog = Utilities.getCommand(Trx);
            cmd.CommandText = "update CardCreditPlus set CreditPlusBalance = CreditPlusBalance - @reduceAmount, " +
                                    "LastupdatedDate = getdate(), LastUpdatedBy = @user " +
                                "where CardCreditPlusId = @CardCreditPlusId";

            double cardCreditPlus;
            double reduceAmount;
            double saveusedCreditPlus = refundCreditPlus;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cardCreditPlus = Convert.ToDouble(dt.Rows[i]["CreditPlusBalance"]);
                if (refundCreditPlus > cardCreditPlus)
                {
                    reduceAmount = cardCreditPlus;
                }
                else
                    reduceAmount = refundCreditPlus;
                refundCreditPlus -= reduceAmount;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reduceAmount", reduceAmount);
                cmd.Parameters.AddWithValue("@user", loginId);
                cmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                cmd.ExecuteNonQuery();

                log.LogVariableState("@reduceAmount", reduceAmount);
                log.LogVariableState("@user", loginId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);

                if (refundCreditPlus <= 0)
                    break;
            }
            Card updateCard = new Card((int)cardId, loginId, Utilities, Trx);
            updateCard.updateCardTime(Trx);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Refund Consumption Balance in case of reversal of transaction
        /// </summary>
        /// <param name="pkID">CCPC primary key</param>
        /// <param name="refundBalance">Quantity to be refunded</param>
        /// <param name="cardId">Card Id</param>
        /// <param name="sqlTrx"></param>
        public void refundCreditPlusConsumption(int pkID, int cardId, int refundBalance, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(pkID, cardId, refundBalance, sqlTrx);
            Utilities.executeNonQuery(@"update CardCreditPlusConsumption 
                                           set ConsumptionBalance = ConsumptionBalance + @ConsumedValues,
                                               LastUpdatedDate = getdate()
                                         where PKID = @PKId", sqlTrx,
                                         new SqlParameter("@PKId", pkID),
                                         new SqlParameter("@ConsumedValues", refundBalance));
            Card updateCard = new Card(cardId, Utilities.ParafaitEnv.LoginID, Utilities, sqlTrx);
            updateCard.updateCardTime(sqlTrx);
            log.LogMethodExit();
        }

        int iteration = 0;
        public void deductCreditPlusOnGamePlay(string CardNumber, int MachineId, double usedCreditPlusCredits, double usedCreditPlusBonus, ref double usedCardBalance, ref double usedGameplayCredits, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(CardNumber, MachineId, usedCreditPlusCredits, usedCreditPlusBonus, usedCardBalance, usedGameplayCredits, SQLTrx);

            SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);

            double usedCreditPlus;
            string CreditPlusType = "";
            if (usedCreditPlusCredits > 0)
            {
                CreditPlusType = "('G', 'A')";
                usedCreditPlus = usedCreditPlusCredits;
            }
            else if (usedCreditPlusBonus > 0)
            {
                CreditPlusType = "('B')";
                usedCreditPlus = usedCreditPlusBonus;
            }
            else
            {
                log.LogVariableState("usedCardBalance ,", usedCardBalance);
                log.LogVariableState("usedGameplayCredits ,", usedGameplayCredits);

                log.LogVariableState("usedCardBalance ", usedCardBalance);
                log.LogVariableState("usedGameplayCredits ", usedGameplayCredits);
                log.LogMethodExit(null);
                return;
            }

            creditPluscmd.CommandText =
                                //@"select distinct * from
                                //        (select cp.CardCreditPlusId, CreditPlusBalance, 1 sort, isnull(PeriodTo, getdate() + 999) ExpiryDate, cp.CreditPlusType
                                //        from CardCreditPlus cp, CardCreditPlusConsumption cpn, cards c, machines m, games g
                                //        where cp.card_id = c.card_id
                                //          and cpn.CardCreditPlusId = cp.CardCreditPlusId 
                                //          and (cpn.GameId = m.Game_Id or cpn.GameId is null) 
                                //          and (cpn.GameProfileId = g.game_profile_id or cpn.GameProfileId is null)
                                //          and m.machine_id = @machineId
                                //          and m.game_id = g.game_id
                                //          and cpn.ProductId is null
                                //          and cpn.POSTypeId is null
                                //          and cpn.CategoryId is null
                                //          and c.valid_flag = 'Y'
                                //          and c.card_number = @card_number
                                //          and cp.CreditPlusType in " + CreditPlusType +
                                //         @"and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                //          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                //          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                //          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                //          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                //           and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                //                        when 1 then cp.Sunday 
                                //                        when 2 then cp.Monday 
                                //                        when 3 then cp.Tuesday 
                                //                        when 4 then cp.Wednesday 
                                //                        when 5 then cp.Thursday 
                                //                        when 6 then cp.Friday 
                                //                        when 7 then cp.Saturday 
                                //                        else 'Y' end, 'Y') = 'Y'
                                //            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                //                             AND ed.EntityName = 'CARDCREDITPLUS'
                                //                             AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                //                           OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                //                             AND  ed.IncludeExcludeFlag = 1)
                                //               )
                                //            AND NOT EXISTS (select 1
                                //                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                //                AND ed.EntityName = 'CARDCREDITPLUS'
                                //                AND isnull(IncludeExcludeFlag, 0) = 0
                                //                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                //                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                //                )
                                //   union all
                                //        select cp.CardCreditPlusId, CreditPlusBalance, 2, isnull(PeriodTo, getdate() + 999), cp.CreditPlusType
                                //        from CardCreditPlus cp, cards c
                                //        where cp.card_id = c.card_id
                                //          and c.valid_flag = 'Y'
                                //          and c.card_number = @card_number
                                //          and cp.CreditPlusType in " + CreditPlusType +
                                //         @"and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                //          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                //          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                //          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                //          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                //           and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                //                        when 1 then cp.Sunday 
                                //                        when 2 then cp.Monday 
                                //                        when 3 then cp.Tuesday 
                                //                        when 4 then cp.Wednesday 
                                //                        when 5 then cp.Thursday 
                                //                        when 6 then cp.Friday 
                                //                        when 7 then cp.Saturday 
                                //                        else 'Y' end, 'Y') = 'Y'
                                //            OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                //                             AND ed.EntityName = 'CARDCREDITPLUS'
                                //                             AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                //                           OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                //                             AND  ed.IncludeExcludeFlag = 1)
                                //               )
                                //            AND NOT EXISTS (select 1
                                //                from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
                                //                AND ed.EntityName = 'CARDCREDITPLUS'
                                //                AND isnull(IncludeExcludeFlag, 0) = 0
                                //                AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                //                or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                //                )
                                //          and not exists (select 1 
                                //                        from CardCreditPlusConsumption cn 
                                //                        where cn.CardCreditPlusId = cp.CardCreditPlusId
                                //                        /*and (GameId is not null or GameProfileId is not null)*/)) v1
                                //          order by 3, 4";
                                @";With n(CardCreditPlusId) as
                                        (select cpn.cardcreditplusid from CardCreditPlusConsumption cpn, machines m, games g
			                                                        WHERE (cpn.GameId = m.Game_Id or cpn.GameId is null) 
							                                          and (cpn.GameProfileId = g.game_profile_id or cpn.GameProfileId is null)
							                                          and m.machine_id = @machineId
							                                          and m.game_id = g.game_id
							                                          and cpn.ProductId is null
							                                          and cpn.POSTypeId is null
							                                          and cpn.CategoryId is null)
                                        select * from
		                                        (select cp.CardCreditPlusId, CreditPlusBalance, 
                                                        CASE WHEN ccp.CardCreditPlusId IS NOT NULL 
                                                             THEN 1 else 2 END sort, 
                                                        isnull(PeriodTo, getdate() + 999) ExpiryDate, cp.CreditPlusType
		                                        from CardCreditPlus cp 
		                                             left outer join CardCreditplusConsumption ccp 
                                                                    on cp.CardCreditPlusId = ccp.CardCreditPlusId, 
			                                         cards c
		                                        where cp.card_id = c.card_id
		                                          and c.valid_flag = 'Y'
		                                          and c.card_number = @card_number
                                                  and cp.CreditPlusType in " + CreditPlusType +
                                              @" and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
		                                          and isnull(cp.ValidityStatus, 'Y') != 'H'
                                                  and isnull(cp.CreditPlusBalance, 0) > 0 
		                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
		                                          and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
		                                          and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
		                                           and (isnull(case DATEPART(WEEKDAY, getdate()) 
						                                        when 1 then cp.Sunday 
						                                        when 2 then cp.Monday 
						                                        when 3 then cp.Tuesday 
						                                        when 4 then cp.Wednesday 
						                                        when 5 then cp.Thursday 
						                                        when 6 then cp.Friday 
						                                        when 7 then cp.Saturday 
						                                        else 'Y' end, 'Y') = 'Y'
			                                        OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
																	                                        AND ed.EntityName = 'CARDCREDITPLUS'
																	                                        AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
																		                                        OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
																	                                        AND  ed.IncludeExcludeFlag = 1)
			                                           )
			                                        AND NOT EXISTS (select 1
				                                        from EntityOverrideDates ed WHERE ed.EntityGuid = cp.Guid 
				                                        AND ed.EntityName = 'CARDCREDITPLUS'
				                                        AND isnull(IncludeExcludeFlag, 0) = 0
				                                        AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
				                                        or ed.Day = DATEPART(WEEKDAY, GETDATE()))
				                                        )
		                                          and (not exists (select 1 
						                                        from CardCreditPlusConsumption cn 
						                                        where cn.CardCreditPlusId = cp.CardCreditPlusId
						                                        /*and (GameId is not null or GameProfileId is not null)*/
						                                        )
			                                           OR Exists ( select 1 from n
			                                                        WHERE n.CardCreditPlusId = cp.CardCreditPlusId 
			                                                      )
			                                          )
			                                          ) v1
		                                          order by 3, 4";

            creditPluscmd.Parameters.AddWithValue("@card_number", CardNumber);
            creditPluscmd.Parameters.AddWithValue("@machineId", MachineId);

            log.LogVariableState("@card_number", CardNumber);
            log.LogVariableState("@machineId", MachineId);

            SqlDataAdapter da = new SqlDataAdapter(creditPluscmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "update CardCreditPlus set CreditPlusBalance = case when CreditPlusBalance - @reduceAmount <= 0 then 0 else CreditPlusBalance - @reduceAmount end, LastupdatedDate = getdate() " +
                                "where CardCreditPlusId = @CardCreditPlusId" +
                                "  and CreditPlusBalance = @creditPlusBalance";

            double cardCreditPlus;
            double reduceAmount;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cardCreditPlus = Convert.ToDouble(dt.Rows[i]["CreditPlusBalance"]);
                if (usedCreditPlus > cardCreditPlus)
                {
                    reduceAmount = cardCreditPlus;
                }
                else
                    reduceAmount = usedCreditPlus;
                
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reduceAmount", reduceAmount);
                cmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                cmd.Parameters.AddWithValue("@creditPlusBalance", dt.Rows[i]["CreditPlusBalance"]);
                int nrecs = cmd.ExecuteNonQuery();

                //cmdLog.Parameters.Clear();
                //cmdLog.Parameters.AddWithValue("@lastUpdatedBy", Utilities.ParafaitEnv.LoginID);  
                //cmdLog.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                //cmdLog.ExecuteNonQuery();

                log.LogVariableState("@reduceAmount", reduceAmount);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                log.LogVariableState("@creditPlusBalance", dt.Rows[i]["CreditPlusBalance"]);
                if (nrecs == 0)
                {
                    log.Debug("Creditplus balance has changed before update! Continue to next record for re-processing. Low Balance CreditPlus Id: " + dt.Rows[i]["CardCreditPlusId"]);
                    if (dt.Rows.Count > 1)
                        continue;//continue to next record
                    else
                    {//Recursive call with latest info to check if rerun of get will result in consumption
                        if (iteration > 0)
                        {
                            iteration = 0;
                            log.Debug("Came for second iteration. Break from the loop. " + dt.Rows[i]["CardCreditPlusId"]);
                            break;
                        }
                        else
                        {
                            iteration++;
                            deductCreditPlusOnGamePlay(CardNumber, MachineId,
                                usedCreditPlusCredits > 0 ? usedCreditPlus : 0,
                                usedCreditPlusBonus > 0 ? usedCreditPlus : 0,
                                ref usedCardBalance, ref usedGameplayCredits, SQLTrx);
                            return;//break the loop as recursive call was done, return
                        }
                    }
                }
                usedCreditPlus -= reduceAmount;
                if (dt.Rows[i]["CreditPlusType"].ToString() == "A")
                    usedCardBalance += reduceAmount;
                else if (dt.Rows[i]["CreditPlusType"].ToString() == "G")
                    usedGameplayCredits += reduceAmount;

                if (usedCreditPlus <= 0)
                    break;
            }

            log.LogVariableState("usedCardBalance ,", usedCardBalance);
            log.LogVariableState("usedGameplayCredits ,", usedGameplayCredits);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Deduct credit plus for tickets and loyalty points. Based on tickets being more than 0, it will deduct
        /// tickets else loyalty points
        /// 2.80 - Change done to ignore ValidityStatus H
        /// </summary>
        /// <param name="CardNumber">Card Number</param>
        /// <param name="Tickets">value of tickets to be deducted</param>
        /// <param name="LoyaltyPoints">value of loyalty points to be deducted</param>
        /// <param name="SQLTrx">SQL TRx</param>
        public void deductCreditPlusTicketsLoyaltyPoints(string CardNumber, double Tickets, double LoyaltyPoints, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(CardNumber, Tickets, LoyaltyPoints, SQLTrx);

            string type = "";
            if (Tickets > 0)
                type = "T";
            else if (LoyaltyPoints > 0)
                type = "L";
            else
            {
                log.LogMethodExit(null);
                return;
            }

            SqlCommand creditPluscmd = Utilities.getCommand(SQLTrx);

            creditPluscmd.CommandText = @"select cp.CardCreditPlusId, CreditPlusBalance, 2, isnull(PeriodTo, getdate() + 999) 
                                        from CardCreditPlus cp, cards c
                                        where cp.card_id = c.card_id
                                          and c.valid_flag = 'Y'
                                          and c.card_number = @card_number
                                          and cp.CreditPlusType = @type 
                                          and ISNULL(cp.ValidityStatus, 'Y') != 'H'
                                          and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                          and (cp.PeriodTo is null or cp.PeriodTo > getdate())                                           
                                          and CASE WHEN @type = 'L' THEN
                                                    ISNULL(cp.forMembershipOnly, 'N')
                                                    ELSE 'N' END  = 'N'
                                          order by 3, 4";

            creditPluscmd.Parameters.AddWithValue("@card_number", CardNumber);
            creditPluscmd.Parameters.AddWithValue("@type", type);

            log.LogVariableState("@card_number", CardNumber);
            log.LogVariableState("@type", type);

            SqlDataAdapter da = new SqlDataAdapter(creditPluscmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            cmd.CommandText = @"UPDATE CardCreditPlus 
                                   SET CreditPlusBalance = CreditPlusBalance - @reduceAmount,  
                                       LastupdatedDate = getdate(), LastUpdatedBy = @user  
                                 WHERE CardCreditPlusId = @CardCreditPlusId 
                                   AND CASE WHEN @type = 'L' THEN
                                                ISNULL(forMembershipOnly, 'N')
                                           ELSE 'N' END  = 'N'";

             double cardCreditPlus;
            double reduceAmount, usedCreditPlus;
            double saveusedCreditPlus = usedCreditPlus = (type == "T" ? Tickets : LoyaltyPoints);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cardCreditPlus = Convert.ToDouble(dt.Rows[i]["CreditPlusBalance"]);
                if (usedCreditPlus > cardCreditPlus)
                {
                    reduceAmount = cardCreditPlus;
                }
                else
                    reduceAmount = usedCreditPlus;
                usedCreditPlus -= reduceAmount;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reduceAmount", reduceAmount);
                cmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                cmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.ExecuteNonQuery();

                log.LogVariableState("@reduceAmount", reduceAmount);
                log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[i]["CardCreditPlusId"]);

                if (usedCreditPlus <= 0)
                    break;
            }

            if (usedCreditPlus > 0)
            {
                cmd.CommandText = @" IF EXISTS(SELECT 'X'
                                                 FROM CARDS 
                                                WHERE card_number = @card_number 
                                                 AND valid_flag = 'Y'
                                                 AND (ISNULL(ticket_count,0) >= @balanceTickets AND  @type = 'T' )
                                                     OR 
                                                     (ISNULL(loyalty_points,0) >= @balanceLoyaltyPoints AND  @type = 'L' ))
                                            BEGIN
                                                UPDATE cards 
                                                   SET ticket_count = isnull(ticket_count, 0) - @balanceTickets, 
                                                       loyalty_points = isnull(loyalty_points, 0) - @balanceLoyaltyPoints,  
                                                       last_update_time = getdate() 
                                                 WHERE card_number = @card_number 
                                                   AND valid_flag = 'Y'
                                            END
                                          ELSE
                                            BEGIN;
                                                IF @balanceTickets <> 0 
                                                   THROW 51000, 'Not enough tickets. Please Check', 1;
                                                ELSE
                                                   THROW 51000, 'Not enough loyalty points. Please Check', 1; 
                                            END";
                 
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@card_number", CardNumber);
                cmd.Parameters.AddWithValue("@type", type);
                if (type == "T")
                {
                    cmd.Parameters.AddWithValue("@balanceTickets", usedCreditPlus);
                    cmd.Parameters.AddWithValue("@balanceLoyaltyPoints", 0); 

                    log.LogVariableState("@balanceTickets", usedCreditPlus);
                    log.LogVariableState("@balanceLoyaltyPoints", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@balanceTickets", 0);
                    cmd.Parameters.AddWithValue("@balanceLoyaltyPoints", usedCreditPlus);

                    log.LogVariableState("@balanceTickets", 0);
                    log.LogVariableState("@balanceLoyaltyPoints", usedCreditPlus);
                }
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.CommandText = @"update cards set last_update_time = getdate(), lastUpdatedBy = @LastUpdatedBy
                                        where card_number = @card_number and valid_flag = 'Y'";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@card_number", CardNumber);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                cmd.ExecuteNonQuery();
                log.LogVariableState("Card Update time: ", CardNumber);
            }
            log.LogMethodExit(null);
        }

        public double getCreditPlusLoyaltyPoints(int cardId)
        {
            log.LogMethodEntry(cardId); 
            double returnValue = Convert.ToDouble(Utilities.executeScalar("select CreditPlusLoyaltyPoints from CardView  where card_id = @card_id",
                                                            new SqlParameter("@card_id", cardId)));
            log.LogVariableState("@card_id", cardId);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// GetCreditPlusVirtualPoints
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public double GetCreditPlusVirtualPoints(int cardId)
        {
            log.LogMethodEntry(cardId); 
            double returnValue = Convert.ToDouble(Utilities.executeScalar("select CreditPlusVirtualPoints from CardView  where card_id = @card_id",
                                                            new SqlParameter("@card_id", cardId)));
            log.LogVariableState("@card_id", cardId);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public double GetRedeemableCreditPlusLoyaltyPoints(int cardId)
        {
            log.LogMethodEntry(cardId); 

            double returnValue = Convert.ToDouble(Utilities.executeScalar("select RedeemableCreditPlusLoyaltyPoints from CardView  where card_id = @card_id",
                                                            new SqlParameter("@card_id", cardId)));

            log.LogVariableState("@card_id", cardId);
            log.LogMethodExit(returnValue);

            return returnValue;
        }

        public int getCreditPlusTickets(int cardId)
        {
            log.LogMethodEntry(cardId);

            log.LogMethodExit(Convert.ToInt32(Utilities.executeScalar("select CreditPlusTickets from CardView where card_id = @card_id",
                                                            new SqlParameter("@card_id", cardId))));

            int returnValue = Convert.ToInt32(Utilities.executeScalar("select CreditPlusTickets from CardView where card_id = @card_id",
                                                            new SqlParameter("@card_id", cardId)));
            log.LogVariableState("@card_id", cardId);

            log.LogMethodExit(returnValue);
            return returnValue;
        }
		public DataTable getCreditPlusDetails(int cardId, string creditPlusType, SqlTransaction inSQLTrx)
        {
            SqlCommand cmd = Utilities.getCommand(inSQLTrx);
            string command = @"SELECT CardCreditPlusId, PlayStartTime, 
                                      CASE WHEN creditPlusType='M'
                                           THEN CASE WHEN PlayStartTime IS NULL
			                                         THEN CreditPlusBalance 
					                                 ELSE CASE WHEN TimeTo IS NULL
					    	                                   THEN  ( SELECT max(v) 
                                                                         FROM (values (datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))), (0)) as value(v)
							                                          ) 
						                                       ELSE (SELECT max(v) FROM (values (datediff(MI, getdate(), 
												                                (SELECT min(endTime) 
													                                FROM (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
															                                UNION ALL
															                                select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)
											                                )), (0)) as value(v) 
								                                   )
						                                   END
			                                      END
		                                     ELSE CreditPlusBalance
                                       END CreditPlusBalance
                                    FROM CardCreditPlus 
                                    WHERE CreditPlusType = @creditPlusType 
                                    AND isnull(CreditPlusBalance, 0) > 0
                                    AND Card_id = @card_id 
                                    AND ISNULL(validityStatus, 'Y') != 'H'
                                    AND (PeriodFrom is null or PeriodFrom <= GETDATE()) 
                                    AND (PeriodTo is null or PeriodTo >= GETDATE())
                                    ORDER BY ISNULL(PeriodTo, 999999)";

            DataTable dt = Utilities.executeDataTable(command, inSQLTrx, new SqlParameter("@card_id", cardId),
                                                                       new SqlParameter("@creditPlusType", creditPlusType));
            return dt;
        }

        //Added on June 20 2017
        /// <summary>
        /// Credit plus entitlement deduction based on type for specific card
        /// Modified as part of 2.80 to ignore credit plus lines which are on HOLD.
        /// ValidityStatus will be H if transaction is not yet saved
        /// </summary>
        /// <param name="card">card object</param>
        /// <param name="type">Credit Plus type like 'M', 'A', 'B', 'L'</param>
        /// <param name="deductvalues">value to be deducted</param>
        /// <param name="SQLTrx">SQL Transaction</param>
        /// <returns></returns>
        public double DeductGenericCreditPlus(Card card, string type, double deductvalues, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(card, type, deductvalues, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            //SqlCommand cmdLog = Utilities.getCommand(SQLTrx);
            string command = @"SELECT CardCreditPlusId, --CreditPlusBalance , 
                                      CASE WHEN creditPlusType='M'
                                           THEN CASE WHEN PlayStartTime IS NULL
			                                         THEN CreditPlusBalance 
					                                 ELSE CASE WHEN TimeTo IS NULL
					    	                                   THEN  ( SELECT max(v) 
                                                                         FROM (values (datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))), (0)) as value(v)
							                                          ) 
						                                       ELSE (SELECT max(v) FROM (values (datediff(MI, getdate(), 
												                                (SELECT min(endTime) 
													                                FROM (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
															                                UNION ALL
															                                select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)
											                                )), (0)) as value(v) 
								                                   )
						                                   END
			                                      END
		                                     ELSE CreditPlusBalance
                                       END CreditPlusBalance
                                    FROM CardCreditPlus 
                                    WHERE CreditPlusType = @creditPlusType 
                                    AND isnull(CreditPlusBalance, 0) > 0
                                    AND Card_id = @card_id
                                    AND ISNULL(validityStatus, 'Y') != 'H'
                                    AND (PeriodFrom is null or PeriodFrom <= GETDATE()) 
                                    AND (PeriodTo is null or PeriodTo >= GETDATE())
                                    ORDER BY ISNULL(PeriodTo, 999999)";

            #region  Code to deduct Entitlement from CardCreditPlus table
            DataTable dt = Utilities.executeDataTable(command, SQLTrx, new SqlParameter("@card_id", card.card_id),
                                                                       new SqlParameter("@creditPlusType", type));

            log.LogVariableState("@card_id", card.card_id);
            log.LogVariableState("@creditPlusType", type);

            cmd.CommandText = @"UPDATE CardCreditPlus 
                                        SET CreditPlusBalance = @cardBalance, LastupdatedDate = Getdate(), LastUpdatedBy = @LastUpdatedBy 
                                    WHERE CardCreditPlusId = @cardCreditPlusId";

            double balance = deductvalues;
            
            foreach (DataRow rw in dt.Rows)
            {
                double creditPlusBalance = Convert.ToDouble(rw["CreditPlusBalance"]);

                if (balance > 0)
                {
                    if (balance > creditPlusBalance)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@cardBalance", 0);
                        balance = balance - creditPlusBalance;

                        log.LogVariableState("@cardBalance", 0);
                    }
                    else
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@cardBalance", creditPlusBalance - balance);
                        balance = 0;

                        log.LogVariableState("@cardBalance", creditPlusBalance - balance);
                    }

                    cmd.Parameters.AddWithValue("@cardCreditPlusId", rw["CardCreditPlusId"]);
                    cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                    cmd.ExecuteNonQuery(); 

                    log.LogVariableState("@cardCreditPlusId", rw["CardCreditPlusId"]);
                    log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                }
                //Break the loop if Entitlement redeemed 
                if (balance <= 0)
                {
                    break;
                }
            }
            if (deductvalues > 0)
            {
                card.updateCardTime(SQLTrx);
            }
            log.LogMethodExit(balance);
            return balance;
            #endregion
        }

        public void TransferCreditPlus(int sourceCardId, int destinationCardId, int sourceCPLineId, double balanceCP, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sourceCardId, destinationCardId, sourceCPLineId, balanceCP, sqlTrx);
            SqlCommand cmd = Utilities.getCommand(sqlTrx);
            cmd.CommandText = @"INSERT INTO [dbo].[CardCreditPlus]
                                                 ([CreditPlus]
                                                 ,[CreditPlusType]
                                                 ,[Refundable]
                                                 ,[Remarks]
                                                 ,[Card_id] 
                                                 ,[CreditPlusBalance]
                                                 ,[PeriodFrom]
                                                 ,[PeriodTo]
                                                 ,[TimeFrom]
                                                 ,[TimeTo]
                                                 ,[NumberOfDays]
                                                 ,[Monday]
                                                 ,[Tuesday]
                                                 ,[Wednesday]
                                                 ,[Thursday]
                                                 ,[Friday]
                                                 ,[Saturday]
                                                 ,[Sunday]
                                                 ,[MinimumSaleAmount] 
                                                 ,[CreationDate]
                                                 ,[LastupdatedDate]
                                                 ,[LastUpdatedBy] 
                                                 ,[site_id]
                                                 ,[ExtendOnReload] 
                                                 ,[PlayStartTime]
                                                 ,[TicketAllowed]
                                                 ,[MasterEntityId]
                                                 ,[ForMembershipOnly]
                                                 ,[ExpireWithMembership] 
                                                 ,[MembershipRewardsId]
                                                 )
                                          SELECT  @balanceCP
                                                 ,[CreditPlusType]
                                                 ,[Refundable]
                                                 ,@remarks
                                                 ,@destinationCardId 
                                                 ,@balanceCP
                                                 ,[PeriodFrom]
                                                 ,[PeriodTo]
                                                 ,[TimeFrom]
                                                 ,[TimeTo]
                                                 ,[NumberOfDays]
                                                 ,[Monday]
                                                 ,[Tuesday]
                                                 ,[Wednesday]
                                                 ,[Thursday]
                                                 ,[Friday]
                                                 ,[Saturday]
                                                 ,[Sunday]
                                                 ,[MinimumSaleAmount] 
                                                 ,getdate()
                                                 ,getdate()
                                                 ,@loginId
                                                 ,@site_id
                                                 ,[ExtendOnReload] 
                                                 ,[PlayStartTime]
                                                 ,[TicketAllowed]
                                                 ,[MasterEntityId]
                                                 ,[ForMembershipOnly]
                                                 ,[ExpireWithMembership] 
                                                 ,[MembershipRewardsId]
                                            FROM [dbo].[CardCreditPlus]
                                           WHERE [CardCreditPlusId] = @sourceCPLineId";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@balanceCP", balanceCP));
            cmd.Parameters.Add(new SqlParameter("@loginId", Utilities.ParafaitEnv.LoginID));
            cmd.Parameters.Add(new SqlParameter("@sourceCPLineId", sourceCPLineId));
            cmd.Parameters.Add(new SqlParameter("@destinationCardId", destinationCardId));
            cmd.Parameters.Add(new SqlParameter("@site_id", Utilities.ParafaitEnv.IsCorporate? (object)Utilities.ParafaitEnv.SiteId : DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@remarks", "Transfer from cardId: "+ sourceCardId));            
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"UPDATE cardCreditPlus 
                                  SET CreditPlusBalance =  CreditPlusBalance - @balanceCP, LastUpdatedBy = @loginId, LastUpdatedDate = getdate()
                                WHERE cardCreditPlusId = @sourceCPLineId";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@balanceCP", balanceCP));
            cmd.Parameters.Add(new SqlParameter("@loginId", Utilities.ParafaitEnv.LoginID));
            cmd.Parameters.Add(new SqlParameter("@sourceCPLineId", sourceCPLineId));
            cmd.ExecuteNonQuery();
        }

        public void DeductFromCreditPlusRecord(int creditPlusId, double deductableValue, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(creditPlusId, deductableValue);

            SqlCommand cmd = Utilities.getCommand(sqlTrx);
            cmd.CommandText = @" UPDATE CardCreditPlus 
                                    SET CreditPlusBalance = CreditPlusBalance - @deductableValue, 
                                        LastupdatedDate = Getdate(), 
                                        LastUpdatedBy = @LastUpdatedBy 
                                  WHERE CardCreditPlusId = @cardCreditPlusId
                                    and CreditPlusBalance >= @deductableValue";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID));
            cmd.Parameters.Add(new SqlParameter("@cardCreditPlusId", creditPlusId));
            cmd.Parameters.Add(new SqlParameter("@deductableValue", deductableValue));
            cmd.ExecuteNonQuery();

            log.LogMethodExit();
        }
    }
}
