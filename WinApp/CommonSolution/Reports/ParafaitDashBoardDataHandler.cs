/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of WirelessDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        03-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Semnox.Parafait.Reports
{
    public class ParafaitDashBoardDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private Semnox.Core.Utilities.Utilities utilities = new Utilities();
        //private List<SqlParameter> parameters;


        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ParafaitDashBoardDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TransactionDTO calss type
        /// </summary>
        /// <param name="transactionDataRow">Transaction DataRow</param>
        /// <returns>Returns TransactionDTO</returns>
        public List<ParafaitDashBoardDTO> GetCollections(DateTime fromdate, DateTime todate, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromdate, todate, sqlTransaction);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<ParafaitDashBoardDTO> parafaitDashBoardDTOList = new List<ParafaitDashBoardDTO>();
            string query = @"select pos_machine, 'Cash' mode, " +
                                "sum(net_amount * 0) void_amount, " +
                                "sum(net_amount * CashRatio) net_amount, " +
                                "sum(tax * CashRatio) tax, " +
                                "sum(amount * CashRatio) gross_total " +
                                "from TransactionView v " +
                                "where trxdate >= @fromdate and trxdate < @todate " +
                                "and CashRatio > 0 " +
                                "and (site_id = @site_id or @site_id = -1) " +
                                "group by pos_machine " +
                           "union all " +
                               "select pos_machine, 'Credit Card' mode, " +
                               "sum(net_amount * 0) void_amount, " +
                               "sum(net_amount * CreditCardRatio) net_amount, " +
                               "sum(tax * CreditCardRatio) tax, " +
                               "sum(amount * CreditCardRatio) gross_total " +
                               "from TransactionView v " +
                               "where trxdate >= @fromdate and trxdate < @todate " +
                               "and CreditCardRatio > 0 " +
                               "and (site_id = @site_id or @site_id = -1) " +
                               "group by pos_machine " +
                           "union all " +
                               "select pos_machine, 'Game Card' mode, " +
                               "sum(net_amount * GameCardRatio) void_amount, " +
                               "sum(net_amount * 0) net_amount, " +
                               "sum(tax * GameCardRatio) tax, " +
                               "sum(amount * 0) gross_total " +
                               "from TransactionView v " +
                               "where trxdate >= @fromdate and trxdate < @todate " +
                               "and GameCardRatio > 0 " +
                               "and (site_id = @site_id or @site_id = -1) " +
                               "group by pos_machine " +
                               "order by 1";
            try
            {
                sqlParameters.Add(new SqlParameter("@fromdate", fromdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                sqlParameters.Add(new SqlParameter("@todate", todate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                sqlParameters.Add(new SqlParameter("@site_id", ExecutionContext.GetExecutionContext().GetSiteId()));
                DataTable parafaitDashBoardData = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
                if (parafaitDashBoardData.Rows.Count > 0)
                {
                    parafaitDashBoardDTOList = new List<ParafaitDashBoardDTO>();
                    foreach (DataRow parafaitDashBoardDataRow in parafaitDashBoardData.Rows)
                    {
                        ParafaitDashBoardDTO parafaitDashBoardDataObject = new ParafaitDashBoardDTO();
                        parafaitDashBoardDataObject.PosMachine = parafaitDashBoardDataRow["pos_machine"] == DBNull.Value ? string.Empty : parafaitDashBoardDataRow["pos_machine"].ToString();
                        parafaitDashBoardDataObject.Mode = parafaitDashBoardDataRow["mode"] == DBNull.Value ? string.Empty : parafaitDashBoardDataRow["mode"].ToString();
                        parafaitDashBoardDataObject.VoidAmount = parafaitDashBoardDataRow["void_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(parafaitDashBoardDataRow["void_amount"]);
                        parafaitDashBoardDataObject.NetAmount = parafaitDashBoardDataRow["net_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(parafaitDashBoardDataRow["net_amount"]);
                        parafaitDashBoardDataObject.Tax = parafaitDashBoardDataRow["tax"] == DBNull.Value ? 0 : Convert.ToDecimal(parafaitDashBoardDataRow["tax"]);
                        parafaitDashBoardDataObject.GrossTotal = parafaitDashBoardDataRow["gross_total"] == DBNull.Value ? 0 : Convert.ToDecimal(parafaitDashBoardDataRow["gross_total"]);
                        GetOtherMetics(parafaitDashBoardDataObject, fromdate, todate);
                        GetCards(parafaitDashBoardDataObject, siteId);
                        parafaitDashBoardDTOList.Add(parafaitDashBoardDataObject);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return parafaitDashBoardDTOList;
        }

        public void GetCards(ParafaitDashBoardDTO parafaitDashBoardDTO, int siteId)
        {
            log.LogMethodEntry(parafaitDashBoardDTO, siteId);
            try
            {
                string query = @"select isnull(sum(number), 0) from card_inventory " +
                "where (site_id = @site_id or @site_id = -1)";
                SqlParameter[] selectParameters = new SqlParameter[1];
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    selectParameters[0] = new SqlParameter("@site_id", utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    selectParameters[0] = new SqlParameter("@site_id", -1);
                }
                int totalCards = Convert.ToInt32(dataAccessHandler.executeScalar(query, selectParameters, sqlTransaction));
                parafaitDashBoardDTO.TotalCards = totalCards.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                string selectCardsQuery = @"select count(1) + (select count(1) from cards where ExpiryDate < getdate() and (site_id is null or site_id = @site_id)),
                min(issue_date), max(issue_date) 
                from cards 
                where valid_flag = 'Y' 
                and (site_id is null or site_id = @site_id or site_id = -1)";

                SqlParameter[] cardsParameters = new SqlParameter[1];
                cardsParameters[0] = new SqlParameter("@site_id", siteId);
                DataTable cardsDataTable = dataAccessHandler.executeSelectQuery(selectCardsQuery, cardsParameters, sqlTransaction);

                long issuedCards = Convert.ToInt64(cardsDataTable.Rows[0][0]);
                parafaitDashBoardDTO.IssuedCards = issuedCards.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                parafaitDashBoardDTO.BalanceCards = (totalCards - issuedCards).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                int numDays = (Convert.ToDateTime(cardsDataTable.Rows[0][2]) - Convert.ToDateTime(cardsDataTable.Rows[0][1])).Days + 1;
                parafaitDashBoardDTO.AvgDaliyCardRequirement = (issuedCards / numDays).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                double reorderdays = (totalCards - issuedCards) / (issuedCards * 1.0 / numDays * 1.0) - 30;
                if (reorderdays > 1000)
                {
                    reorderdays = 1000;
                }
                parafaitDashBoardDTO.RecomReorderDate = DateTime.Now.AddDays(reorderdays).ToString(utilities.ParafaitEnv.DATE_FORMAT);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public void GetOtherMetics(ParafaitDashBoardDTO parafaitDashBoardDTO, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(parafaitDashBoardDTO, fromDate, toDate);

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("@fromdate", fromDate.Date.AddHours(6));
                sqlParameters[1] = new SqlParameter("@todate", toDate.Date.AddDays(1).AddHours(6));
                if (ExecutionContext.GetExecutionContext().GetIsCorporate())
                {
                    sqlParameters[2] = new SqlParameter("@site_id", ExecutionContext.GetExecutionContext().GetSiteId());
                }
                else
                {
                    sqlParameters[2] = new SqlParameter("@site_id", -1);
                }

                string query = @"select count(distinct cardId) 
                from (select card_id cardId
                          from trx_lines l, trx_header h
                          where trxdate >= @fromdate and trxdate < @todate 
                          and (h.site_id = @site_id or @site_id = -1)
                          and h.trxid = l.trxid
                    union all
                       select card_id
                        from gameplay
                        where play_date >= @fromdate and play_date < @todate
                        and (site_id = @site_id or @site_id = -1)) v";

                int customers = Convert.ToInt32(dataAccessHandler.executeScalar(query, sqlParameters, sqlTransaction));
                parafaitDashBoardDTO.TotalCustomer = customers.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                int TotalDays = (toDate - fromDate).Days + 1;

                double TotalCollection = 0;

                try
                {
                    TotalCollection = Convert.ToDouble(parafaitDashBoardDTO.GrossTotal);
                }
                catch { }

                if (TotalDays != 0)
                {
                    parafaitDashBoardDTO.AvgCustomerPerDay = (customers / TotalDays).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                }

                if (customers != 0 && TotalDays != 0)
                {
                    parafaitDashBoardDTO.AvgCollectionPerCustomer = (TotalCollection / customers).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }

                string playCountQuery = @"select count(1) " +
                  "from gameplay " +
                  "where play_date >= @fromdate and play_date < @todate " +
                  "and (site_id = @site_id or @site_id = -1)";

                SqlParameter[] playCountParameters = new SqlParameter[3];
                playCountParameters[0] = new SqlParameter("@fromdate", fromDate.Date.AddHours(6));
                playCountParameters[1] = new SqlParameter("@todate", toDate.Date.AddDays(1).AddHours(6));
                playCountParameters[2] = new SqlParameter("@site_id", ExecutionContext.GetExecutionContext().GetSiteId());

                int playCount = Convert.ToInt32(dataAccessHandler.executeScalar(playCountQuery, playCountParameters, sqlTransaction));
                parafaitDashBoardDTO.TotalPlayCount = playCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                if (customers != 0)
                    parafaitDashBoardDTO.AvgPlayCountPerCustomer = (playCount / customers).ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public DataTable GetGraphTable(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromDate, toDate, sqlTransaction);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            TimeSpan ts = toDate - fromDate;
            double total_days = ts.Days + 1;

            string query = @"select machine_name, machine_address, total_plays, credits + courtesy + bonus + time as total_amount,
                DailyPlayCount, 
                (credits + courtesy + bonus + time) / @total_days DailyCollection, 
                machine_count, dummy_for_grouping 
                from 
                    (select machine_name, machine_address, 
                     count(1) total_plays,
                     sum(t.credits + cpCardBalance + cpCredits + cardGame) as credits, 
                     sum(t.courtesy) as courtesy, 
                     sum(t.bonus + cpBonus) as bonus, 
                     sum(t.time) as time, 
                     count(1) / @total_days DailyPlayCount, 
                     1 machine_count, 
                     '' dummy_for_grouping 
                     from gameplay t, machines m, cards c 
                     where play_date >= @fromdate and play_date < @todate 
                    and m.machine_id = t.machine_id 
                    and t.card_id = c.card_id 
                    and c.technician_card = 'N' 
                    and (@site_id = -1 or (t.site_id = @site_id and m.site_id = @site_id)) 
                     group by machine_address, machine_name) a 
                 order by 6 desc";
            sqlParameters.Add(new SqlParameter("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            sqlParameters.Add(new SqlParameter("@todate", toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            sqlParameters.Add(new SqlParameter("@total_days", total_days));
            if (utilities.ParafaitEnv.IsCorporate)
            {
                sqlParameters.Add(new SqlParameter("@site_id", ExecutionContext.GetExecutionContext().GetSiteId()));
            }
            else
            {
                sqlParameters.Add(new SqlParameter("@site_id", -1));
            }
            DataTable parafaitDashBoardData = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
            DataTable graphData;
            graphData = utilities.getReportGridTable(parafaitDashBoardData, 7, new int[] { 2, 3, 6 });
            try
            {
                graphData.Rows.RemoveAt(graphData.Rows.Count - 1);
                graphData.Rows.RemoveAt(graphData.Rows.Count - 1);
                graphData.Rows[graphData.Rows.Count - 1][0] = "Grand Total";
            }
            catch { }

            for (int i = 0; i < graphData.Rows.Count; i++)
            {
                if (graphData.Rows[i]["DailyCollection"] == DBNull.Value) // summary row
                {
                    try
                    {
                        graphData.Rows[i]["DailyPlayCount"] = Convert.ToDouble(graphData.Rows[i]["total_plays"]) / (total_days * Convert.ToDouble(graphData.Rows[i]["machine_count"]));
                    }
                    catch
                    {
                    }

                    try
                    {
                        graphData.Rows[i]["DailyCollection"] = Convert.ToDouble(graphData.Rows[i]["total_amount"]) / (total_days * Convert.ToDouble(graphData.Rows[i]["machine_count"]));
                    }
                    catch
                    {
                    }

                    graphData.Rows[i]["machine_address"] = graphData.Rows[i]["machine_count"];
                }
            }

            DataTable BarGraphDT = new DataTable();
            BarGraphDT.Columns.Add("GameName");
            BarGraphDT.Columns.Add("DailyCollection");
            BarGraphDT.Columns.Add("TotalCollection");

            for (int i = 0; i < graphData.Rows.Count; i++) // populate a new DT with only machine rows and grand total
            {
                BarGraphDT.Rows.Add(new object[] { graphData.Rows[i]["machine_name"], graphData.Rows[i]["DailyCollection"], graphData.Rows[i]["total_amount"] });
            }

            log.LogMethodExit(BarGraphDT);
            return BarGraphDT;
        }
    }
}
