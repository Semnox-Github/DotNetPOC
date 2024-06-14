using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Transaction
{
    public class clsCheckOut
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int CheckInDetailId;
        public int CheckInId;
        public int CheckOutTrxId;
        public int TrxLineId;
        public int BaseTimeForPrice;
        public string CustomerName, Detail;
        public int balanceCheckIns;
        public decimal EffectivePrice;
        public string TableNumber = "";

        public Utilities Utilities;
        public clsCheckOut(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            log.LogMethodExit(null);
        }

        public decimal getCheckOutPrice(int productId)
        {
            log.LogMethodEntry(productId);

            DataTable dt = Utilities.executeDataTable("select datediff(\"mi\", CheckInTime, getdate()) duration, " +
                                                        "isnull(AllowedTimeInMinutes, 0) time_allowed " +
                                                        "from CheckIns " +
                                                        "where CheckInId = @CheckInId",
                                                        new SqlParameter("@CheckInId", CheckInId));

            log.LogVariableState("@CheckInId", CheckInId);

            int duration = Convert.ToInt32(dt.Rows[0][0]);
            int time_allowed = Convert.ToInt32(dt.Rows[0][1]);
            int overdue = duration - time_allowed;

            int totalPauseTime;

            if (CheckInDetailId == -1)
            {
                DataTable dataTable = Utilities.executeDataTable(@"(select * from CheckInDetails where 
                                               CheckInId = @CheckInId and CheckOutTime is null)",
                                               new SqlParameter("@CheckInId", CheckInId));
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        int checkInDetailId = Convert.ToInt32(dr["CheckInDetailId"]);

                        totalPauseTime = Convert.ToInt32(Utilities.executeScalar(@"SELECT isnull(sum(case when pauseendtime is null then datediff(MI, pauseStartTime, getdate())
                                                                                               else totalpausetime
					                                                                           end),0)
                                                                         from CheckInPauseLog 
                                                                         WHERE CheckInDetailId= @CheckInDetailId",
                                                            new SqlParameter("@CheckInDetailId", checkInDetailId)));

                        overdue = overdue - totalPauseTime;
                        int tempoverdue = overdue - totalPauseTime;
                        EffectivePrice += GetTotalPrice(productId, tempoverdue, totalPauseTime, checkInDetailId);
                    }
                }
            }
            else
            {
                totalPauseTime = Convert.ToInt32(Utilities.executeScalar(@"SELECT isnull(sum(case when pauseendtime is null then datediff(MI, pauseStartTime, getdate())
                                                                                               else totalpausetime
					                                                                           end),0)
                                                                         from CheckInPauseLog 
                                                                         WHERE CheckInDetailId= @CheckInDetailId",
                                                            new SqlParameter("@CheckInDetailId", CheckInDetailId)));
                
                overdue = overdue - totalPauseTime;
                EffectivePrice = GetTotalPrice(productId, overdue, totalPauseTime, CheckInDetailId);
            }

            log.LogMethodExit(EffectivePrice);
            return EffectivePrice;
        }

        private decimal GetTotalPrice(int productId, int overdue, int totalPauseTime, int checkInDetailId)
        {
            if (overdue > 0)
            {
                string CommandText = @"select case when availableUnits is not null 
                                          then cast((select count(1) " +
                                                "from CheckInDetails " +
                                                "where CheckInId = @CheckInId " +
                                                "and (@CheckInDetailId = -1 or CheckInDetailId = @CheckInDetailId) " +
                                                "and CheckOutTime is null) as float) " +
                                                "/ cast((select count(1) from CheckInDetails where CheckInId = @CheckInId) as float) " +
                                          "else (select count(1) " +
                                              "from CheckInDetails " +
                                              "where CheckInId = @CheckInId " +
                                              "and (@CheckInDetailId = -1 or CheckInDetailId = @CheckInDetailId) " +
                                              "and CheckOutTime is null) end units, " +
                                    "isnull(price, 0) prodPrice, " +
                                    "(select top 1 price from CheckInPrices where productId = @product_id and TimeSlab >= @overdue order by TimeSlab) slabPrice," +
                                "isnull(MaxCheckOutAmount, 0) MaxCheckOutAmount " +
                          "from products " +
                          "where product_id = @product_id";

                DataTable dtPrice = Utilities.executeDataTable(CommandText,
                                                            new SqlParameter("@CheckInDetailId", checkInDetailId),
                                                            new SqlParameter("@product_id", productId),
                                                            new SqlParameter("@overdue", overdue),
                                                            new SqlParameter("@CheckInId", CheckInId));
                log.LogVariableState("@CheckInDetailId", checkInDetailId);
                log.LogVariableState("@product_id", productId);
                log.LogVariableState("@overdue", overdue);
                log.LogVariableState("@CheckInId", CheckInId);

                decimal units = Convert.ToDecimal(dtPrice.Rows[0][0]);
                decimal prodPrice = Convert.ToDecimal(dtPrice.Rows[0][1]);
                if (dtPrice.Rows[0][2] != DBNull.Value)
                    prodPrice = Convert.ToDecimal(dtPrice.Rows[0][2]);
                decimal effectivePrice = units * prodPrice;
                decimal maxCheckOutAmount = Convert.ToDecimal(dtPrice.Rows[0][3]);
                
                if (BaseTimeForPrice != 0)
                {
                    int multiple = Convert.ToInt32(Math.Ceiling(overdue / Convert.ToDouble(BaseTimeForPrice)));
                    effectivePrice = effectivePrice * multiple;

                    if (maxCheckOutAmount > 0 && effectivePrice > maxCheckOutAmount)
                        effectivePrice = maxCheckOutAmount;
                }

                Detail = (string.IsNullOrEmpty(Detail) ? "" : Detail + ": ") + overdue.ToString() + " Min overdue";
                EffectivePrice = effectivePrice;
            }
            else
                EffectivePrice = 0;

            log.LogMethodExit(EffectivePrice);
            return EffectivePrice;
        }

        //public bool CheckOut(SqlTransaction SQLTrx, string Username, ref string message)
        //{
        //    log.LogMethodEntry(SQLTrx, Username, message);

        //    SqlCommand cmd = Utilities.getCommand(SQLTrx);

        //    try
        //    {
        //        if (CheckInDetailId != -1) //Single check-out
        //        {
        //            cmd.CommandText = "update checkindetails set checkouttime = getdate(), " +
        //                                "CheckOutTrxId = @trxId, " +
        //                                "TrxLineId = @trxLineId, " +
        //                                "last_update_date = getdate(), " +
        //                                "last_updated_user = @last_updated_user " +
        //                                "where checkinDetailId = @id and (checkouttime is null or checkoutTime > getdate())";
        //            cmd.Parameters.AddWithValue("@id", CheckInDetailId);
        //            cmd.Parameters.AddWithValue("@trxId", CheckOutTrxId);
        //            cmd.Parameters.AddWithValue("@trxLineId", TrxLineId);
        //            cmd.Parameters.AddWithValue("@last_updated_user", Username);

        //            log.LogVariableState("@id", CheckInDetailId);
        //            log.LogVariableState("@trxId", CheckOutTrxId);
        //            log.LogVariableState("@trxLineId", TrxLineId);
        //            log.LogVariableState("@last_updated_user", Username);

        //            if (cmd.ExecuteNonQuery() == 0)
        //            {
        //                message = Utilities.MessageUtils.getMessage(344);

        //                log.LogVariableState("Message ", message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }

        //            CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(Utilities.ExecutionContext);
        //            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
        //            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, CheckInDetailId.ToString()));
        //            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
        //            List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
        //            if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
        //            {
        //                using (SqlCommand commandText = Utilities.getCommand(SQLTrx))
        //                {
        //                    commandText.CommandText = @"update CheckInPauseLog set PauseEndTime = GETDATE(),
        //                                            TotalPauseTime = (SELECT datediff(minute, PauseStartTime, GETDATE()))
        //                                            where CheckInDetailId = @CheckInDetailId and PauseEndTime is null";
        //                    commandText.Parameters.AddWithValue("@CheckInDetailId", CheckInDetailId);
        //                    if (commandText.ExecuteNonQuery() == 0)
        //                    {
        //                        message = Utilities.MessageUtils.getMessage(2084);

        //                        log.LogVariableState("Message ", message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }
        //                }
        //            }
        //        }
        //        else //group check-out
        //        {
        //            cmd.CommandText = "update checkindetails set checkouttime = getdate(), " +
        //                                "CheckOutTrxId = @trxId, " +
        //                                "TrxLineId = @trxLineId, " +
        //                                "last_update_date = getdate(), " +
        //                                "last_updated_user = @last_updated_user " +
        //                                "where checkinId = @id and (checkouttime is null or checkoutTime > getdate())";
        //            cmd.Parameters.AddWithValue("@id", CheckInId);
        //            cmd.Parameters.AddWithValue("@trxId", CheckOutTrxId);
        //            cmd.Parameters.AddWithValue("@trxLineId", TrxLineId);
        //            cmd.Parameters.AddWithValue("@last_updated_user", Username);

        //            log.LogVariableState("@id", CheckInId);
        //            log.LogVariableState("@trxId", CheckOutTrxId);
        //            log.LogVariableState("@trxLineId", TrxLineId);
        //            log.LogVariableState("@last_updated_user", Username);

        //            if (cmd.ExecuteNonQuery() < balanceCheckIns)
        //            {
        //                message = Utilities.MessageUtils.getMessage(344);

        //                log.LogVariableState("Message ", message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }

        //            CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(Utilities.ExecutionContext);
        //            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
        //            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, CheckInDetailId.ToString()));
        //            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
        //            List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
        //            if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
        //            {
        //                using (SqlCommand commandText = Utilities.getCommand(SQLTrx))
        //                {
        //                    DataTable dataTable = Utilities.executeDataTable(@"(select * from CheckInDetails where 
        //                                       CheckInId = @CheckInId and CheckOutTime is null)",
        //                                       new SqlParameter("@CheckInId", CheckInId));
        //                    if (dataTable.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow dr in dataTable.Rows)
        //                        {
        //                            int checkInDetailId = Convert.ToInt32(dr["CheckInDetailId"]);


        //                            commandText.CommandText = @"update CheckInPauseLog set PauseEndTime = GETDATE(),
        //                                            TotalPauseTime = (SELECT datediff(minute, PauseStartTime, GETDATE()))
        //                                            where CheckInDetailId = @CheckInDetailId and PauseEndTime is null";
        //                            commandText.Parameters.AddWithValue("@CheckInDetailId", checkInDetailId);
        //                            if (commandText.ExecuteNonQuery() == 0)
        //                            {
        //                                message = Utilities.MessageUtils.getMessage(2084);

        //                                log.LogVariableState("Message ", message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }

        //        try
        //        {
        //            CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, CheckInId);
        //            checkInBL.ExternalInterfaces(false);
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("Error occured while switching off facility table", ex);
        //            message= "External Interface: " + ex.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while updating check-in details", ex);
        //        message = "CheckOut Save: " + ex.Message;
        //        log.LogVariableState("Message ", message);
        //        log.LogMethodExit(false);
        //        return false;
        //    }

        //    log.LogVariableState("Message ", message);
        //    log.LogMethodExit(true);
        //    return true;
        //}

        //void ExternalInterfaces()
        //{
        //    log.LogMethodEntry();

        //    DataTable dt = Utilities.executeDataTable(@"select * from FacilityTables f, CheckIns c where f.tableId = c.tableId and c.CheckInId = @CheckInId",
        //                                                new SqlParameter("@CheckInId", CheckInId));

        //    log.LogVariableState("@CheckInId", CheckInId);

        //    if (dt.Rows.Count > 0)
        //        Semnox.Parafait.Transaction.ExternalInterfaces.SwitchOff(dt.Rows[0]);

        //    log.LogMethodExit(null);
        //}
    }
}
