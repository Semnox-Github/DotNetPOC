using Semnox.Core.Utilities;
//using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public static class HelperClassBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Utilities utilities = new Utilities();

        public static void SetParafairEnvValues(ExecutionContext excecutionContext, Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            utilities.ParafaitEnv.SiteId = excecutionContext.GetSiteId();
            utilities.ParafaitEnv.LoginID = excecutionContext.GetUserId();
            Users user = new Users(excecutionContext, excecutionContext.GetUserId());
            utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
            utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
            utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            utilities.ParafaitEnv.IsCorporate = excecutionContext.GetIsCorporate();
            utilities.ExecutionContext.SetIsCorporate(excecutionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(excecutionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the corresponding dbcolumn name for loyaltyAttribute
        /// </summary>
        /// <param name="loyaltyAttribute"></param>
        /// <returns></returns>
        public static string GetDBColumnNameForLoyaltyRedeem(string loyaltyAttribute)
        {
            log.LogMethodEntry(loyaltyAttribute);
            string result = string.Empty;
            switch (loyaltyAttribute)
            {
                case "Card Balance": result = "credits"; break;
                case "Loyalty Points": result = "loyalty_points"; break;
                case "Tickets": result = "tickets"; break;
                case "Game Play Credits": result = "bonus"; break;
                case "Counter Items Only": result = "Courtesy"; break;
                case "Cash": result = "Cash"; break;
                case "Game Play Bonus": result = "Bonus"; break;
                case "Time": result = "time"; break;
            }
            log.LogMethodExit();
            return result;
        }


        public static double GetRefundAmount(int trxId, int trxLineId)
        {
            log.LogMethodEntry(trxId, trxLineId);
            try
            {
                double gameRefundAmount = 0;
                SqlCommand cmdRefundAmount = utilities.getCommand();
                cmdRefundAmount.CommandText = "select amount from trx_lines where TrxId = @TrxId and LineId = @LineId";
                cmdRefundAmount.Parameters.Clear();
                cmdRefundAmount.Parameters.AddWithValue("@TrxId", trxId);
                cmdRefundAmount.Parameters.AddWithValue("@LineId", trxLineId);
                object o = cmdRefundAmount.ExecuteScalar();
                gameRefundAmount = Math.Round(Convert.ToDouble(o));
                log.LogMethodExit(gameRefundAmount);
                return gameRefundAmount;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        //public static double GetRefundCreditPlus(int cardId)
        //{
        //    log.LogMethodEntry(cardId);
        //    try
        //    {
        //        double refundAmount = 0;
        //        CreditPlus creditPlus = new CreditPlus(utilities);
        //        refundAmount = creditPlus.getCreditPlusRefund(cardId);
        //        log.LogMethodExit(refundAmount);
        //        return refundAmount;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}


        //public static void UpdateGameBalance(int trxId)
        //{
        //    log.LogMethodEntry(trxId);
        //    try
        //    {
        //        SqlCommand cmdUpdate = utilities.getCommand();
        //        cmdUpdate.Parameters.AddWithValue("@TrxId", trxId);
        //        utilities.executeNonQuery(@"update cardGames set BalanceGames = 0 where TrxId = @TrxId and BalanceGames > 0",
        //                                               new SqlParameter("@TrxId", trxId));
        //        log.LogMethodExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}
        //public static void UpdateTimeBalance(int trxId , int cardId)
        //{
        //    log.LogMethodEntry(trxId);
        //    try
        //    {
        //        SqlCommand cmdUpdate = utilities.getCommand();
        //        cmdUpdate.Parameters.AddWithValue("@TrxId", trxId);
        //        if (trxId != -1)
        //        {
        //            utilities.executeNonQuery(@"update CardCreditPlus set CreditPlusBalance = 0
        //                                                                    where CreditPlusType ='M' 
        //                                                                    and CreditPlusBalance >0 and TrxId = @TrxId and Card_id = @cardId",
        //                                        new SqlParameter("@TrxId", trxId),
        //                                        new SqlParameter("@cardId",cardId));
        //        }
        //        else
        //        {
        //            utilities.executeNonQuery(@"update cards
        //                                                        set time = 0 ,start_time =null
        //                                                        where time > 0 and card_id = @cardId",
        //                                        new SqlParameter("@cardId", cardId));
        //        }
        //        log.LogMethodExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}

        public static bool ManagerApprovalCheck(decimal itemCount, string taskName = null)
        {
            log.LogMethodEntry(itemCount, taskName);
            decimal mgrApprovalLimit = 0;
            string mgtLimitValue = FetchManagerApprovalLimit(taskName);
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToDecimal(mgtLimitValue);
            }
            catch { mgrApprovalLimit = 0; }
            if (mgrApprovalLimit > 0 && itemCount > mgrApprovalLimit && utilities.ParafaitEnv.ManagerId == -1)
            {
                Users approveUser = new Users(utilities.ExecutionContext, utilities.ParafaitEnv.ManagerId);
                utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                utilities.ParafaitEnv.ApprovalTime = utilities.getServerTime();
            }
            log.LogMethodExit(true);
            return true;
        }

        private static string FetchManagerApprovalLimit(string taskName)
        {
          
            switch (taskName)
            {

                case TaskProcs.EXCHANGETOKENFORCREDIT:
                    return utilities.getParafaitDefaults("TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.LOADTICKETS:
                    return utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.LOADBONUS:
                    return utilities.getParafaitDefaults("LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMTICKETSFORBONUS:
                    return utilities.getParafaitDefaults("REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMBONUSFORTICKET:
                    return utilities.getParafaitDefaults("REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMLOYALTY:
                    return utilities.getParafaitDefaults("REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REFUNDCARD:
                    return utilities.getParafaitDefaults("REFUND_AMOUNT_LIMIT_FOR_MANAGER_APPROVAL");
                default:
                    return "";
            }
            
        }

       
    }
}
