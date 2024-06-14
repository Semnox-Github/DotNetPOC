/********************************************************************************************
 * Project Name - ShiftDataHandler
 * Description  - Data handler of Shift Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        04-Mar-2019   Indhu          Created 
 *2.60.2      28-May-2019   Nitin Pai      Fixes 
 *2.70.2        10-Dec-2019   Jinto Thomas   Removed siteid from update query
 *2.80        15-Sep-2019   Nitin Pai      BIR Enhancement 
 *2.90        26-May-2020   Vikas Dwivedi  Modified as per the Standard CheckList
 *2.140.0     16-Aug-2021   Deeksha        Modified : Provisional Shift changes
*2.140.0     16-Aug-2021   Girish         Modified : Multicash drawer changes, Added cashdrawerId column
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class ShiftDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Shift as shift ";
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<ShiftDTO.SearchByShiftParameters, string> DBSearchParameters = new Dictionary<ShiftDTO.SearchByShiftParameters, string>
            {
                {ShiftDTO.SearchByShiftParameters.SHIFT_KEY, "shift.shift_key"},
                {ShiftDTO.SearchByShiftParameters.POS_MACHINE, "shift.pos_machine"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID, "shift.ShiftLoginId"},
                {ShiftDTO.SearchByShiftParameters.SITE_ID, "shift.site_id"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME, "shift.shift_username"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_USERTYPE, "shift.shift_usertype"},
                {ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "shift.shift_time"},
                {ShiftDTO.SearchByShiftParameters.TIMESTAMP, "shift.shift_time"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME, "shift.shift_time"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME, "shift.shift_time"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_ACTION, "shift.shift_action"},
                {ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN, "shift.shift_action"},
                {ShiftDTO.SearchByShiftParameters.CASHDRAWER_ID, "shift.CashdrawerId"},
                {ShiftDTO.SearchByShiftParameters.LAST_X_DAYS_LOGIN, "shift.shift_time"}
             };

        /// <summary>
        /// Default constructor of ShiftDataHandler class
        /// </summary>
        public ShiftDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountCreditPlusConsumption Record.
        /// </summary>
        /// <param name="shiftDTO">shiftDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ShiftDTO shiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_key", shiftDTO.ShiftKey, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_username", shiftDTO.ShiftUserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_usertype", shiftDTO.ShiftUserType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_time", shiftDTO.ShiftTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_action", shiftDTO.ShiftAction));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_amount", shiftDTO.ShiftAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_count", shiftDTO.CardCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_ticketnumber", shiftDTO.ShiftTicketNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_remarks", string.IsNullOrEmpty(shiftDTO.ShiftRemarks) ? string.Empty : shiftDTO.ShiftRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pos_machine", shiftDTO.POSMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_amount", shiftDTO.ActualAmount < 0 ? DBNull.Value : (object)shiftDTO.ActualAmount)); // In Shift Open these values are null
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_cards", shiftDTO.ActualCards == null ? DBNull.Value : (object)shiftDTO.ActualCards));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_tickets", shiftDTO.ActualTickets < 0 ? DBNull.Value : (object)shiftDTO.ActualTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameCardamount", shiftDTO.GameCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditCardamount", shiftDTO.CreditCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChequeAmount", shiftDTO.ChequeAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CouponAmount", shiftDTO.CouponAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualGameCardamount", shiftDTO.ActualGameCardamount < 0 ? DBNull.Value : (object)shiftDTO.ActualGameCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualCreditCardamount", shiftDTO.ActualCreditCardamount < 0 ? DBNull.Value : (object)shiftDTO.ActualCreditCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualChequeAmount", shiftDTO.ActualChequeAmount < 0 ? DBNull.Value : (object)shiftDTO.ActualChequeAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualCouponAmount", shiftDTO.ActualCouponAmount < 0 ? DBNull.Value : (object)shiftDTO.ActualCouponAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ShiftLoginId", shiftDTO.ShiftLoginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", shiftDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CashdrawerId", shiftDTO.CashdrawerId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Shift record to the database
        /// </summary>
        /// <param name="shiftDTO">ShiftDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public ShiftDTO InsertShiftDTO(ShiftDTO shiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftDTO, loginId, siteId);
            string query = @"insert into shift
                                        (
                                        shift_username,
                                        shift_time,
                                        shift_usertype,
                                        shift_action,
                                        shift_amount,
                                        card_count,
                                        shift_ticketnumber,
                                        shift_remarks,
                                        pos_machine,
                                        actual_amount,
                                        actual_cards,
                                        actual_tickets,
                                        GameCardamount,
                                        CreditCardamount,
                                        ChequeAmount,
                                        CouponAmount,
                                        ActualGameCardamount,
                                        ActualCreditCardamount,
                                        ActualChequeAmount,
                                        ActualCouponAmount,
                                        ShiftLoginId,
                                        site_id,
                                        MasterEntityId,
                                        CreatedBy,
                                        CreationDate,
                                        LastUpdatedBy,
                                        LastUpdateDate,
                                        CashdrawerId
                                        )
                                    values
                                        (
                                        @shift_username,
                                        @shift_time,
                                        @shift_usertype,
                                        @shift_action,
                                        @shift_amount,
                                        @card_count,
                                        @shift_ticketnumber,
                                        @shift_remarks,
                                        @pos_machine,
                                        @actual_amount,
                                        @actual_cards,
                                        @actual_tickets,
                                        @GameCardamount,
                                        @CreditCardamount,
                                        @ChequeAmount,
                                        @CouponAmount,
                                        @ActualGameCardamount,
                                        @ActualCreditCardamount,
                                        @ActualChequeAmount,
                                        @ActualCouponAmount,
                                        @ShiftLoginId,
                                        @site_id,
                                        @MasterEntityId,
                                        @CreatedBy,
                                        GETDATE(),
                                        @LastUpdatedBy,
                                        GETDATE(),
                                        @CashdrawerId
                                        )
                          SELECT * FROM shift WHERE shift_key = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftDTO(shiftDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }

        /// <summary>
        /// Update the Shift record to the database
        /// </summary>
        /// <param name="shiftDTO">ShiftDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public ShiftDTO UpdateShiftDTO(ShiftDTO shiftDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftDTO, loginId, siteId);
            string query = @"update shift set 
                                            shift_username = @shift_username,
                                            shift_time = @shift_time,
                                            shift_usertype = shift_usertype,
                                            shift_action = @shift_action,
                                            shift_amount = @shift_amount,
                                            card_count = @card_count,
                                            shift_ticketnumber = @shift_ticketnumber,
                                            shift_remarks = @shift_remarks,
                                            pos_machine = @pos_machine,
                                            actual_amount = @actual_amount,
                                            actual_cards = @actual_cards,
                                            actual_tickets = @actual_tickets,
                                            GameCardamount = @GameCardamount,
                                            CreditCardamount = @CreditCardamount,
                                            ChequeAmount = @ChequeAmount,
                                            CouponAmount = @CouponAmount,
                                            ActualGameCardamount = @ActualGameCardamount,
                                            ActualCreditCardamount = @ActualCreditCardamount,
                                            ActualChequeAmount = @ActualChequeAmount,
                                            ActualCouponAmount = @ActualCouponAmount,
                                            ShiftLoginId = @ShiftLoginId,
                                            -- site_id = @site_id,
                                            MasterEntityId = @masterEntityId,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            CashdrawerId = @CashdrawerId,
                                            LastUpdateDate = GETDATE()
                                            where shift_key = @shift_key
                                SELECT * FROM shift WHERE shift_key = @shift_key";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftDTO(shiftDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }
        private void RefreshShiftDTO(ShiftDTO shiftDTO, DataTable dt)
        {
            log.LogMethodEntry(shiftDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                shiftDTO.ShiftKey = Convert.ToInt32(dt.Rows[0]["shift_key"]);
                shiftDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                shiftDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                shiftDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                shiftDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                shiftDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                shiftDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        public ShiftCollections[] GetNetAmountAndPaymentMode(DateTime shiftTime, string posName, DateTime shiftEndTime)
        {
            log.LogMethodEntry();
            string selectQuery = @"select PaymentMode, isCreditCard, " +
                                               "isnull(sum(th.Amount), 0) netAmount " +
                                         " from shift s left outer join (select isnull(tp.posMachine, th.pos_machine)" +
                                          "pos_machine,p.PaymentMode, " +
                                          "p.isCreditCard,tp.Amount,tp.PaymentDate " +
                                           "from trxPayments tp , trx_header th , PaymentModes p " +
                                           "where  tp.trxId = th.trxId " +
                                            "and p.PaymentModeId = tp.paymentModeId " +
                                             "and tp.PaymentDate >= @shiftTime and tp.PaymentDate < @shiftEndTime " +
                                          "and (p.isCreditCard = 'Y' or (p.isCash = 'N' and p.isDebitCard = 'N' and p.isCreditCard = 'N')) )th " +
                                          "on   s.pos_machine = th.pos_machine " +
                                         "where s.shift_time = @shiftTime and s.pos_machine = @pos " +
                                         "and th.PaymentDate >= @shiftTime and th.PaymentDate < @shiftEndTime " +
                                         "group by PaymentMode, isCreditCard order by 2 desc, 1";
            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@shiftTime", shiftTime);
            selectParameters[1] = new SqlParameter("@pos", posName);
            selectParameters[2] = new SqlParameter("@shiftEndTime", shiftEndTime);
            DataTable dtP = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);

            ShiftCollections[] paymentModes = new ShiftCollections[20];
            for (int i = 0; i < dtP.Rows.Count; i++)
            {
                paymentModes[i] = new ShiftCollections();
                paymentModes[i].Mode = dtP.Rows[i]["PaymentMode"].ToString();
                paymentModes[i].isCreditCard = (dtP.Rows[i]["isCreditCard"].ToString() == "Y" ? true : false);
                paymentModes[i].Amount = Convert.ToDouble(dtP.Rows[i]["netAmount"]);

                if (paymentModes[i].isCreditCard)
                    paymentModes[i].Mode = " " + paymentModes[i].Mode;
            }
            log.LogMethodExit(paymentModes);
            return paymentModes;
        }

        internal ShiftCollections[] GetShiftAmounts(DateTime shiftTime, string pOSMachine, DateTime shiftEndTime)
        {
            string selectQuery = @"select isnull(sum(TaxableAmount), 0) TaxableAmount, isnull(sum(DiscountOnTaxableAmount), 0)DiscountOnTaxableAmount ,
                                                            isnull(sum(NonTaxableAmount), 0) NonTaxableAmount,isnull(sum(DiscountOnNonTaxableAmount), 0) DiscountOnNonTaxableAmount, 
                                                            isnull(sum(TaxAmount), 0)TaxAmount, isnull(sum(DiscountOnTaxAmount), 0) DiscountOnTaxAmount 
                                        from (select 
                                                   case when tax_id is not null then p.price * p.quantity else 0 end TaxableAmount,
                                                   case when tax_id is not null then p.price * p.quantity * (-1.0 * isnull(td.discPerc, 0)/100.0) else 0 end DiscountOnTaxableAmount,
                                                   case when tax_id is null then p.price * p.quantity else 0 end NonTaxableAmount,
                                                   case when tax_id is null then p.price * p.quantity * (-1.0 * isnull(td.discPerc, 0)/100.0) else 0 end DiscountOnNonTaxableAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 TaxAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 * (-1.0 * isnull(td.discPerc, 0)/100.0) DiscountOnTaxAmount
                                               from trx_lines p 
                                                    left outer join (select trxId, lineId, sum(discountPercentage) discPerc
                                                                      from trxDiscounts td
                                                                     group by trxId, lineId) td
                                                    on td.trxId = p.trxId
                                                    and td.lineId = p.lineId,
                                                    trx_header h, shift s
                                              where h.trxDate >= s.shift_time
                                               and h.trxDate < @shiftEndTime
                                               and h.pos_machine = s.pos_machine
                                               and s.shift_time = @shiftTime
                                               and s.pos_machine = @pos
                                               and p.trxId = h.TrxId 
                                               and product_id is not null) v";
            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@shiftTime", shiftTime);
            selectParameters[1] = new SqlParameter("@pos", pOSMachine);
            selectParameters[2] = new SqlParameter("@shiftEndTime", shiftEndTime);
            DataTable dtP = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);

            ShiftCollections[] shiftAmounts = new ShiftCollections[1];
            if (dtP.Rows.Count > 0)
            {
                shiftAmounts[0] = new ShiftCollections();
                shiftAmounts[0].TaxableAmount = Convert.ToDouble(dtP.Rows[0]["TaxableAmount"]);
                shiftAmounts[0].DiscountOnTaxableAmount = Convert.ToDouble(dtP.Rows[0]["DiscountOnTaxableAmount"]); ;
                shiftAmounts[0].NonTaxableAmount = Convert.ToDouble(dtP.Rows[0]["NonTaxableAmount"]);
                shiftAmounts[0].DiscountOnNonTaxableAmount = Convert.ToDouble(dtP.Rows[0]["DiscountOnNonTaxableAmount"]);
                shiftAmounts[0].TaxAmount = Convert.ToDouble(dtP.Rows[0]["TaxAmount"]);
                shiftAmounts[0].DiscountOnTaxAmount = Convert.ToDouble(dtP.Rows[0]["DiscountOnTaxAmount"]);
            }
            log.LogMethodExit(shiftAmounts);
            return shiftAmounts;
        }

        internal ShiftCollections[] GetShiftDiscountedAmounts(DateTime shiftTime, string pOSMachine, DateTime shiftEndTime)
        {
            string selectQuery = @"select discount_name, isnull(sum(DiscountAmount), 0) DiscountAmount, isnull(sum(DiscountedTaxAmount), 0) DiscountedTaxAmount
                                        from (select d.discount_name,
                                                   p.price * p.quantity * dv.discountPercentage/100.0 * -1 DiscountAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 * (-1.0 * dv.discountPercentage/100.0) DiscountedTaxAmount
                                               from trx_lines p, trx_header h, TrxDiscounts dv, discounts d, shift s
                                              where dv.discountId = d.discount_id
                                               and dv.trxId = p.trxId
                                               and dv.lineId = p.lineId
                                               and h.trxDate >= s.shift_time
                                               and h.trxDate < @shiftEndTime
                                               and h.pos_machine = s.pos_machine
                                               and s.shift_time = @shiftTime
                                               and s.pos_machine = @pos
                                               and p.trxId = h.TrxId) v
                                        group by discount_name";

            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@shiftTime", shiftTime);
            selectParameters[1] = new SqlParameter("@pos", pOSMachine);
            selectParameters[2] = new SqlParameter("@shiftEndTime", shiftEndTime);
            DataTable dtP = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            ShiftCollections[] discountedAmounts = new ShiftCollections[20];
            for (int i = 0; i < dtP.Rows.Count; i++)
            {
                discountedAmounts[i] = new ShiftCollections();
                discountedAmounts[i].discountName = dtP.Rows[i]["discount_name"].ToString();
                discountedAmounts[i].discountAmount = Convert.ToDouble(dtP.Rows[i]["DiscountAmount"]);
                discountedAmounts[i].DiscountedTaxAmount = Convert.ToDouble(dtP.Rows[i]["DiscountedTaxAmount"]);
            }
            log.LogMethodExit(discountedAmounts);
            return discountedAmounts;
        }


        internal ShiftCollections[] GetShiftPaymentModes(DateTime shiftTime, string pOSMachine, DateTime shiftEndTime)
        {
            string selectQuery = @"select 'Cash' Mode, isnull(sum(tp.Amount), 0) Amount, count(h.trxid) TrxId, 'Y' sort
                                     from trx_header h, TrxPayments tp, shift s, PaymentModes p 
                                     where tp.TrxId = h.TrxId
									 and p.PaymentModeId = tp.PaymentModeId
                                     and tp.PaymentDate >= s.shift_time
                                     and tp.PaymentDate < @shiftEndTime
                                     and isnull(tp.PosMachine,h.pos_machine) = s.pos_machine
									 and p.isCash = 'Y'
									 and s.shift_time = @shiftTime
                                     and s.pos_machine = @pos
                                    union all
                                    select 'Game Card', isnull(sum(tp.Amount), 0), count(h.trxid), 'Y' sort
                                     from trx_header h, TrxPayments tp, shift s, PaymentModes p
                                     where tp.TrxId = h.TrxId
									 and p.PaymentModeId = tp.PaymentModeId
                                     and tp.PaymentDate >= s.shift_time
                                     and tp.PaymentDate < @shiftEndTime
									 and p.isDebitCard = 'Y'
                                     and isnull(tp.PosMachine,h.pos_machine) = s.pos_machine
                                     and s.shift_time = @shiftTime
                                     and s.pos_machine = @pos
                                    union all
                                    select case p.isCreditCard when 'Y' then 'Credit Card' else p.PaymentMode end,
		                                    sum(tp.Amount), count(h.trxid), p.isCreditCard
                                     from trx_header h, trxPayments tp, PaymentModes p, shift s
                                     where tp.paymentModeId = p.PaymentModeId
                                     and tp.trxId = h.trxId
                                     and (p.isCreditCard = 'Y' or (isCreditCard = 'N' and isCash = 'N' and isDebitCard = 'N'))
                                     and tp.PaymentDate >= s.shift_time
                                     and tp.PaymentDate < @shiftEndTime
                                     and isnull(tp.PosMachine,h.pos_machine) = s.pos_machine
                                     and s.shift_time = @shiftTime
                                     and s.pos_machine = @pos
                                     group by case p.isCreditCard when 'Y' then 'Credit Card' else p.PaymentMode end, p.isCreditCard
                                    order by sort desc, 1";

            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@shiftTime", shiftTime);
            selectParameters[1] = new SqlParameter("@pos", pOSMachine);
            selectParameters[2] = new SqlParameter("@shiftEndTime", shiftEndTime);
            DataTable dtP = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            ShiftCollections[] discountedAmounts = new ShiftCollections[20];
            for (int i = 0; i < dtP.Rows.Count; i++)
            {
                discountedAmounts[i] = new ShiftCollections();
                discountedAmounts[i].Mode = dtP.Rows[i]["Mode"].ToString();
                discountedAmounts[i].Amount = Convert.ToDouble(dtP.Rows[i]["Amount"]);
                discountedAmounts[i].TrxCount = Convert.ToDouble(dtP.Rows[i]["TrxId"]);
                discountedAmounts[i].SortType = dtP.Rows[i]["sort"].ToString();
            }
            log.LogMethodExit(discountedAmounts);
            return discountedAmounts;
        }


        internal ShiftDTO GetSystemNumbers(ShiftDTO shiftDTO, DateTime shiftEndTime)
        {

            string selectQuery = @"SELECT isnull(sum(th.cashAmount), 0) netCash, s.shift_amount Cash,
                                    isnull(sum(th.CreditCardAmount), 0) netCreditCard,
                                    s.CreditCardAmount CreditCard,
                                    isnull(sum(th.gameCardAmount), 0) netGameCard,
                                    s.gameCardAmount GameCard, 0 netCheque,
                                    s.ChequeAmount Cheque,
                                    isnull(sum(th.OtherPaymentModeAmount), 0) netCoupon,
                                    s.CouponAmount Coupon, s.card_count, s.shift_ticketnumber
                                FROM Shift s
                                left outer join (SELECT p.PaymentDate,
                                                           ISNULL(p.PosMachine, th.pos_machine) pos_machine,
                                                           isnull(SUM(case when pm.isCash = 'Y' then amount else 0 end), 0) CashAmount,
                                                                 isnull(SUM(case when pm.isDebitCard = 'Y' then amount else 0 end), 0) GameCardAmount,
                                                                 isnull(SUM(case when pm.isCreditCard = 'Y' then amount else 0 end), 0) CreditCardAmount,
                                                                 isnull(SUM(case when pm.isCash+pm.isCreditCard+pm.isDebitCard = 'NNN' then amount else 0 end), 0) OtherPaymentModeAmount
                                                   FROM TRX_HEADER TH, TrxPayments P, PaymentModes PM
                                                  WHERE P.PaymentModeId = PM.PaymentModeId
                                                    AND TH.TrxId = P.TrxId
                                                 GROUP BY p.PaymentDate, ISNULL(p.PosMachine, th.pos_machine)
                                                 ) th on th.PaymentDate >= s.shift_time and th.PaymentDate < @shiftEndTime and th.pos_machine = s.pos_machine              
                                WHERE s.shift_time = @shiftTime
                                     and s.pos_machine = @pos
                                GROUP BY s.shift_amount, s.CreditCardAmount, s.gameCardAmount, s.ChequeAmount, s.CouponAmount, s.card_count, s.shift_ticketnumber";
            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@shiftTime", shiftDTO.ShiftTime);
            selectParameters[1] = new SqlParameter("@shiftEndTime", shiftEndTime);
            selectParameters[2] = new SqlParameter("@pos", shiftDTO.POSMachine);
            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (dt.Rows.Count > 0)
            {
                shiftDTO.ActualAmount = Convert.ToDouble(dt.Rows[0]["netCash"]);
                shiftDTO.ActualCreditCardamount = Convert.ToDouble(dt.Rows[0]["netCreditCard"]);
                shiftDTO.ActualGameCardamount = Convert.ToDouble(dt.Rows[0]["netGameCard"]);
                shiftDTO.ActualChequeAmount = Convert.ToDouble(dt.Rows[0]["netCheque"]);
                shiftDTO.ActualCouponAmount = Convert.ToDouble(dt.Rows[0]["netCoupon"]);
                shiftDTO.ShiftTicketNumber = dt.Rows[0]["shift_ticketnumber"].ToString();
                shiftDTO.CreditCardamount = Convert.ToDouble(dt.Rows[0]["CreditCard"]);
                shiftDTO.GameCardamount = Convert.ToDouble(dt.Rows[0]["GameCard"]);
                shiftDTO.ChequeAmount = Convert.ToDouble(dt.Rows[0]["Cheque"]);
                shiftDTO.CouponAmount = Convert.ToDouble(dt.Rows[0]["Coupon"]);
                shiftDTO.ShiftAmount = Convert.ToDouble(dt.Rows[0]["Cash"]);
                shiftDTO.CardCount = Convert.ToInt32(dt.Rows[0]["card_count"]);
            }

            string query = "select (select count(distinct h.refund_card_id) - count(distinct h.new_card_id) " +
                                        "from transactionView h, shift s " +
                                        "where h.trxdate > @shiftTime " +
                                        "and h.trxdate <= @shiftEndTime  " +
                                        "and shift_time = @shiftTime  " +
                                        "and s.pos_machine = @pos " +
                                        "and h.pos_machine = s.pos_machine) - " +
                                     "isnull(  " +
                                    "(select sum(1) transfers  " +
                                        "from tasks t, shift s, cards c  " +
                                        "where transfer_to_card_id = c.card_id " +
                                        "and task_date > shift_time  " +
                                        "and c.issue_date < shift_time " +
                                        "and s.pos_machine = @pos " +
                                        "and t.pos_machine = s.pos_machine  " +
                                        "and s.shift_time = @shiftTime), 0) ";
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@shiftTime", shiftDTO.ShiftTime);
            parameters[1] = new SqlParameter("@shiftEndTime", shiftEndTime);
            parameters[2] = new SqlParameter("@pos", shiftDTO.POSMachine);

            int netCards = Convert.ToInt32(dataAccessHandler.executeScalar(query, parameters, sqlTransaction));
            shiftDTO.ActualCards = netCards;
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }

        internal DateTime? GetShiftModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from Shift WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from ShiftLog WHERE (site_id = @siteId or @siteId = -1)
                               ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
        

        /// <summary>
        /// Converts the Data row object to ShiftDTO class type
        /// </summary>
        /// <param name="shiftDataRow">ShiftDTO DataRow</param>
        /// <returns>Returns ShiftDTO</returns>
        public ShiftDTO GetShiftDTO(DataRow shiftDataRow)
        {
            log.LogMethodEntry(shiftDataRow);
            ShiftDTO shiftDataObject = new ShiftDTO(Convert.ToInt32(shiftDataRow["shift_key"]),
                                                    shiftDataRow["shift_username"].ToString(),
                                                    shiftDataRow["shift_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(shiftDataRow["shift_time"]),
                                                    shiftDataRow["shift_usertype"].ToString(),
                                                    shiftDataRow["shift_action"].ToString(),
                                                    shiftDataRow["shift_amount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["shift_amount"]),
                                                    shiftDataRow["card_count"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["card_count"]),
                                                    shiftDataRow["shift_ticketnumber"].ToString(),
                                                    shiftDataRow["shift_remarks"].ToString(),
                                                    shiftDataRow["pos_machine"].ToString(),
                                                    shiftDataRow["actual_amount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["actual_amount"]),
                                                    shiftDataRow["actual_cards"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["actual_cards"]),
                                                    shiftDataRow["actual_tickets"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["actual_tickets"]),
                                                    shiftDataRow["GameCardamount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["GameCardamount"]),
                                                    shiftDataRow["CreditCardamount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["CreditCardamount"]),
                                                    shiftDataRow["ChequeAmount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["ChequeAmount"]),
                                                    shiftDataRow["CouponAmount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["CouponAmount"]),
                                                    shiftDataRow["ActualGameCardamount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["ActualGameCardamount"]),
                                                    shiftDataRow["ActualCreditCardamount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["ActualCreditCardamount"]),
                                                    shiftDataRow["ActualChequeAmount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["ActualChequeAmount"]),
                                                    shiftDataRow["ActualCouponAmount"] == DBNull.Value ? 0 : Convert.ToDouble(shiftDataRow["ActualCouponAmount"]),
                                                    shiftDataRow["Guid"].ToString(),
                                                    shiftDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(shiftDataRow["site_id"]),
                                                    shiftDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(shiftDataRow["SynchStatus"]),
                                                    shiftDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(shiftDataRow["MasterEntityId"]),//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
                                                    shiftDataRow["CreatedBy"].ToString(),
                                                    shiftDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(shiftDataRow["CreationDate"]),
                                                    shiftDataRow["LastUpdatedBy"].ToString(),
                                                    shiftDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(shiftDataRow["LastUpdateDate"]),
                                                    shiftDataRow["ShiftLoginId"].ToString(),
                                                    shiftDataRow["CashdrawerId"] == DBNull.Value ? -1 : Convert.ToInt32(shiftDataRow["CashdrawerId"])
                                                    );
            log.LogMethodExit(shiftDataObject);
            return shiftDataObject;
        }


        /// <summary>
        /// Gets the shift data of passed shiftKey
        /// </summary>
        /// <param name="shiftKey">integer type parameter</param>
        /// <returns>Returns ShiftDTO</returns>
        internal ShiftDTO GetShiftDTO(int shiftKey)
        {
            log.LogMethodEntry(shiftKey);
            string selectQuery = @"select *
                                         from shift
                                        where shift_key = @shiftKey";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@shiftKey", shiftKey);
            DataTable shiftDataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters,sqlTransaction);
            if (shiftDataTable.Rows.Count > 0)
            {
                DataRow shiftRow = shiftDataTable.Rows[0];
                ShiftDTO shiftDataObject = GetShiftDTO(shiftRow);
                log.LogMethodExit(shiftDataObject);
                return shiftDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Gets the ShiftDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrx">SqlTransaction object</param>
        /// <returns>Returns the list of ShiftDTO matching the search criteria</returns>
        public List<ShiftDTO> GetShiftDTOList(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParameters, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ShiftDTO> shiftDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder orderBy = new StringBuilder(" Order By ");
                string orderByJoiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ShiftDTO.SearchByShiftParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.POS_MACHINE) ||
                            searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION) ||
                            searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME) ||
                            searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_KEY)
                             || searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.CASHDRAWER_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ShiftDTO.SearchByShiftParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.TIMESTAMP))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP))
                        {
                            orderBy.Append(orderByJoiner + DBSearchParameters[searchParameter.Key] + "  " + searchParameter.Value);
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ShiftDTO.SearchByShiftParameters.LAST_X_DAYS_LOGIN))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + "DATEADD(day, -" + dataAccessHandler.GetParameterName(searchParameter.Key) + ",GETDATE())");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "  Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
                if (orderBy.ToString() != " Order By ")
                    selectQuery += orderBy;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                shiftDTOList = new List<ShiftDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ShiftDTO shiftDTO = GetShiftDTO(dataRow);
                    shiftDTOList.Add(shiftDTO);
                }
            }
            log.LogMethodExit(shiftDTOList);
            return shiftDTOList;
        }

        public ShiftDTO AssignCashdrawer(int shiftId, int cashdrawerId)
        {
            log.LogMethodEntry(shiftId, cashdrawerId);
            ShiftDTO shiftDTO = null;
            string query = @"UPDATE shift SET  CashdrawerId = @CashdrawerId, LastUpdateDate = GetDate()  WHERE shift_key = @shift_key";
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@CashdrawerId", cashdrawerId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@shift_key", shiftId));
                int updated = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray(), sqlTransaction);
                log.Debug("updated: " + updated);
                shiftDTO = GetShiftDTO(shiftId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit("throwing exception");
                throw;
            }
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }


        public ShiftDTO UnAssignCashdrawer(int shiftId)
        {
            log.LogMethodEntry(shiftId);
            ShiftDTO shiftDTO = null;
            string query = @"UPDATE shift SET   CashdrawerId = null , LastUpdateDate = GetDate() where shift_key = @shift_key";
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@shift_key", shiftId));
                int updated = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray(), sqlTransaction);
                log.Debug("updated: " + updated);
                shiftDTO = GetShiftDTO(shiftId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit( "throwing exception");
                throw;
            }
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }
    }
}
