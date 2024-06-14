/***********************************************************************************************************************
* Project Name - Semnox.Parafait.Transaction - TaskProcs
* Description  - TaskProcs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
************************************************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint and Pause allowed changes 
*2.60.0      13-Mar-2019      Raghuveera         Pause card changes done
*2.60.0      08-May-2019      Nitin Pai          Guest App Changes
*2.70.2.0      22-Aug-2019    Jinto Thomas       Card Consolidation Task Changes
*2.70.2.0      04-Oct-2019    Jinto Thomas       Modified insert trx_header query. Added createdby,
*                                                                          creationdate fields
*2.70.2.0      14-Nov-2019    Jinto Thomas       Modified PauseTimeEntitlement() method, added creditplus balance
*                                                check on update playstarttime on game pause
*2.70.2        26-Nov-2019    Lakshminarayana    Virtual store enhancement
*2.70.3        05-Feb-2020    Nitin Pai          Changing Balance Transfer to include Credit Plus records also 
*2.70.3        30-Jan-2020    Archana            Modified LoadTickets() method to include 
*                                                per day limit check while adding manual ticket
*2.70.3        19-Mar-2020   Jinto Thomas        RedeemLoyalty Task Changes
*2.70.3        19-Mar-2020   Jinto Thomas        LoadBonus Task Changes                                                 
                                                 Tasks should explicity check for Hold status and prevent tasks to be completed
*2.70.3        15-Apr-2020   Jinto Thomas        Modified consilidate() method: added default parameter mergeHistoryDuringSourceInactivation
*2.70.3        20-Apr-2020   Archana             Added TrxId while creating load ticket task
*2.80.0        07-May-2020   Jinto Thomas        Modified GetBonusTypeCodeValue() replaced bonus type C with A
*2.80.0        11-May-2020   Jinto Thomas        Modified Consolidate(), added ConsolidateCardDiscountDetails() update carddiscount details
*2.80.0        13-Apr-2020   Deeksha             Split product entitlement for product type Recharge/Cardsale/Gametime
*2.80.0        23-Jun-2020   Deeksha             Issue Fix : Miami time play can not cancel line
*2.90.0       23-Jun-2020      Raghuveera        Variable refund changes to ignore the saving of trx_header and lines if the saving 
*                                                is already done in payment detail screen
*2.90.0       27-Jul-2020     Girish Kundar      Modified : TrxUserLogBL() constructor changes.
*2.90.0       14-Aug-2020     Girish Kundar      Modified : Removed BL constructor with parameters for CardCreditPlusPauseTimeLogBL and passed sql transaction to datahandler
*2.100.0      12-Oct-2020    Mathew Ninan        Modified: RefundCard method to handle ParentChildCards 
*2.120.0      12-Mar-2021    Girish Kundar       Modified: Card hold task enhancment changes
*2.130.4      22-Feb-2022    Mathew Ninan         Modified DateTime to ServerDateTime 
*2.130.10     19-Sep-2022    Vignesh             Remove entitlement hold check from TransferCard task
*2.130.11     27-Sep-2022    Mathew Ninan        Added RentalAllocation update for TransferCard scenario. Card Number should be updated
*                                                This will be evaluated again once we start creating new instance for TransferCard
*2.150.1      22-Feb-2023    Guru S A            Kiosk Cart Enhancements
*2.150.3      30-Jun-2023    Abhishek            Hecere Locker Integration, modified to write card to Hecere Locker.
 ************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.logging;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Product;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using System.Reflection;
using Semnox.Parafait.Tags;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Transaction
{
    public class TaskProcs
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string TRANSFERCARD = "TRANSFERCARD";
        public const string EXCHANGETOKENFORCREDIT = "EXCHANGETOKENFORCREDIT";
        public const string EXCHANGECREDITFORTOKEN = "EXCHANGECREDITFORTOKEN";
        public const string LOADTICKETS = "LOADTICKETS";
        public const string CONSOLIDATE = "CONSOLIDATE";
        public const string LOADMULTIPLE = "LOADMULTIPLE";
        public const string REALETICKET = "REALETICKET";
        public const string REFUNDCARD = "REFUNDCARD";
        public const string LOADBONUS = "LOADBONUS";
        public const string DISCOUNT = "DISCOUNT";
        public const string REDEEMLOYALTY = "REDEEMLOYALTY";
        public const string SPECIALPRICING = "SPECIALPRICING";
        public const string REDEEMTICKETSFORBONUS = "REDEEMTICKETSFORBONUS";
        public const string SETCHILDSITECODE = "SETCHILDSITECODE";
        public const string GETMIFAREGAMEPLAY = "GETMIFAREGAMEPLAY";
        public const string BALANCETRANSFER = "BALANCETRANSFER";
        public const string SALESRETURNEXCHANGE = "SALESRETURNEXCHANGE"; //Added for sales return/exchange process 10-Jun-2016 
        public const string REDEEMBONUSFORTICKET = "REDEEMBONUSFORTICKET"; //Added for redeem bonus for tickets on 16-jun-2017 
        public const string EXCHANGECREDITFORTIME = "EXCHANGECREDITFORTIME";
        public const string EXCHANGETIMEFORCREDIT = "EXCHANGETIMEFORCREDIT";
        public const string BALANCETRANSFERTIME = "BALANCETRANSFERTIME";
        public const string PAUSETIMEENTITLEMENT = "PAUSETIMEENTITLEMENT";
        public const string LINKCARD = "LINKCARD";
        public const string HOLDENTITLEMENTS = "HOLDENTITLEMENTS";
        public const string DEDUCTBALANCE = "DEDUCTBALANCE";
        public const string REDEEMVIRTUALPOINTS = "REDEEMVIRTUALPOINTS";

        public enum EntitlementType
        {
            Credits, Courtesy, Bonus, Time, GamePlayCredits, GamePlayBonus, CardBalance, LoyaltyPoints, CounterItemsOnly, Tickets, License_D, License_E, License_F, License_H,
            License_I, License_J, License_K, License_N, License_O, License_Q, License_R, License_S, License_U, License_W, License_X, License_Y, License_Z
        };

        //public staticDataExchange StaticDataExchange;
        public Utilities Utilities;
        TransactionUtils TransactionUtils;
        Semnox.Core.GenericUtilities.EventLog TasksEventLog;

        public int TransactionId = -1;
        public int TaskId = -1;

        public decimal transferLimit = 0;
        public decimal staffCreditLimit = 0;
        public decimal gameCardCreditLimit = 0;

        private Dictionary<int, SubscriptionHeaderBL> subScriptionHeaderDictionary = new Dictionary<int, SubscriptionHeaderBL>();

        public TaskProcs(Utilities pUtilities)
        {
            log.LogMethodEntry(pUtilities);

            Utilities = pUtilities;
            TransactionUtils = new TransactionUtils(Utilities);
            TasksEventLog = new Semnox.Core.GenericUtilities.EventLog(Utilities.ExecutionContext);
            try
            {
                transferLimit = Convert.ToDecimal(Utilities.getParafaitDefaults("STAFF_CARD_TRANSFER_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("STAFF_CARD_TRANSFER_LIMIT doesn't have a valid value", ex);
                transferLimit = 0;
                log.LogVariableState("transferLimit", transferLimit);
            }
            try
            {
                staffCreditLimit = Convert.ToDecimal(Utilities.getParafaitDefaults("STAFF_CARD_CREDITS_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("staffCreditLimit doesn't have a valid value!", ex);
                staffCreditLimit = 0;
                log.LogVariableState("staffCreditLimit ", staffCreditLimit);
            }
            try
            {
                gameCardCreditLimit = Convert.ToDecimal(Utilities.getParafaitDefaults("GAMECARD_CREDIT_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("gameCardCreditLimit doesn't have a valid value! ", ex);
                gameCardCreditLimit = 0;
                log.LogVariableState("gameCardCreditLimit ", gameCardCreditLimit);
            }
        }

        public int createTask(int card_id, string task_type, double value_loaded, int transfer_to_card_id,
                                    double credits_exchanged, int tokens_exchanged,
                                    int sourceCard,
                                    int attribute1,
                                    int attribute2,
                                    string Remarks,
                                    SqlTransaction SQLTrx,
                                    double credits = 0,
                                    double courtesy = 0,
                                    double bonus = 0,
                                    int tickets = 0,
                                    int trxId = -1,
                                    decimal virtualPoints = 0)
        {
            log.LogMethodEntry(card_id, task_type, value_loaded, transfer_to_card_id, credits_exchanged, tokens_exchanged,
                sourceCard, attribute1, attribute2, Remarks, SQLTrx, credits, courtesy, bonus, tickets, trxId, virtualPoints
                );
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "insert into tasks ( " +
                                "card_id, " +
                                "task_type_id, " +
                                "value_loaded, " +
                                "transfer_to_card_id, " +
                                "credits_exchanged, " +
                                "tokens_exchanged, " +
                                "consolidate_card1, " +
                                "attribute1, " +
                                "attribute2, " +
                                "remarks, " +
                                "task_date, " +
                                "user_id, " +
                                "ApprovedBy, " +
                                "pos_machine, " +
                                "site_id, " +
                                "credits, " +
                                "courtesy, " +
                                "bonus, " +
                                "tickets, " +
                                "trxId ," +
                                "VirtualPoints )" +
                                "(select " +
                                "@card_id, " +
                                "task_type_id, " +
                                "@value_loaded, " +
                                "@transfer_to_card_id, " +
                                "@credits_exchanged, " +
                                "@tokens_exchanged, " +
                                "@sourceCardId, " +
                                "@attribute1, " +
                                "@attribute2, " +
                                "@remarks, " +
                                "getdate(), " +
                                "@user_id, " +
                                "@ApprovedBy, " +
                                "@pos_machine, " +
                                "site_id, " +
                                "@credits, " +
                                "@courtesy, " +
                                "@bonus, " +
                                "@tickets, " +
                                "@trxId ," +
                                "@VirtualPoints " +
                                "from task_type " +
                                "where task_type = @task_type  and (site_id = @site_id OR @site_id = -1)); select @@IDENTITY";

            if (card_id != -1)
            {
                cmd.Parameters.AddWithValue("@card_id", card_id);
                log.LogVariableState("@card_id", card_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                log.LogVariableState("@card_id", DBNull.Value);
            }

            if (value_loaded != -1)
            {
                cmd.Parameters.AddWithValue("@value_loaded", value_loaded);
                log.LogVariableState("@value_loaded", value_loaded);
            }
            else
            {
                cmd.Parameters.AddWithValue("@value_loaded", DBNull.Value);
                log.LogVariableState("@value_loaded", DBNull.Value);
            }

            if (transfer_to_card_id != -1)
            {
                cmd.Parameters.AddWithValue("@transfer_to_card_id", transfer_to_card_id);
                log.LogVariableState("@transfer_to_card_id", transfer_to_card_id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@transfer_to_card_id", DBNull.Value);
                log.LogVariableState("@transfer_to_card_id", DBNull.Value);
            }

            if (credits_exchanged != -1)
            {
                cmd.Parameters.AddWithValue("@credits_exchanged", credits_exchanged);
                log.LogVariableState("@credits_exchanged", credits_exchanged);
            }
            else
            {
                cmd.Parameters.AddWithValue("@credits_exchanged", DBNull.Value);
                log.LogVariableState("@credits_exchanged", DBNull.Value);
            }

            if (tokens_exchanged != -1)
            {
                cmd.Parameters.AddWithValue("@tokens_exchanged", tokens_exchanged);
                log.LogVariableState("@tokens_exchanged", tokens_exchanged);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tokens_exchanged", DBNull.Value);
                log.LogVariableState("@tokens_exchanged", DBNull.Value);
            }

            if (sourceCard != -1)
            {
                cmd.Parameters.AddWithValue("@sourceCardId", sourceCard);
                log.LogVariableState("@sourceCardId", sourceCard);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sourceCardId", DBNull.Value);
                log.LogVariableState("@sourceCardId", DBNull.Value);
            }


            if (attribute1 != -1)
            {
                cmd.Parameters.AddWithValue("@attribute1", attribute1);
                log.LogVariableState("@attribute1", attribute1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@attribute1", DBNull.Value);
                log.LogVariableState("@attribute1", DBNull.Value);
            }

            if (attribute2 != -1)
            {
                cmd.Parameters.AddWithValue("@attribute2", attribute2);
                log.LogVariableState("@attribute2", attribute2);
            }
            else
            {
                cmd.Parameters.AddWithValue("@attribute2", DBNull.Value);
                log.LogVariableState("@attribute2", DBNull.Value);
            }

            if (credits != -1)
            {
                cmd.Parameters.AddWithValue("@credits", credits);
                log.LogVariableState("@credits", credits);
            }
            else
            {
                cmd.Parameters.AddWithValue("@credits", DBNull.Value);
                log.LogVariableState("@credits", DBNull.Value);
            }

            if (courtesy != -1)
            {
                cmd.Parameters.AddWithValue("@courtesy", courtesy);
                log.LogVariableState("@courtesy", courtesy);
            }
            else
            {
                cmd.Parameters.AddWithValue("@courtesy", DBNull.Value);
                log.LogVariableState("@courtesy", DBNull.Value);
            }

            if (bonus != -1)
            {
                cmd.Parameters.AddWithValue("@bonus", bonus);
                log.LogVariableState("@bonus", bonus);
            }
            else
            {
                cmd.Parameters.AddWithValue("@bonus", DBNull.Value);
                log.LogVariableState("@bonus", DBNull.Value);
            }

            if (tickets != -1)
            {
                cmd.Parameters.AddWithValue("@tickets", tickets);
                log.LogVariableState("@tickets", tickets);
            }
            else
            {
                cmd.Parameters.AddWithValue("@tickets", DBNull.Value);
                log.LogVariableState("@tickets", DBNull.Value);
            }
            // Virtual arcade changes
            if (virtualPoints != -1)
            {
                cmd.Parameters.AddWithValue("@VirtualPoints", virtualPoints);
                log.LogVariableState("@VirtualPoints", virtualPoints);
            }
            else
            {
                cmd.Parameters.AddWithValue("@VirtualPoints", DBNull.Value);
                log.LogVariableState("@VirtualPoints", DBNull.Value);
            }
            // ends here 

            if (trxId != -1)
            {
                cmd.Parameters.AddWithValue("@trxId", trxId);
                log.LogVariableState("@trxId", trxId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@trxId", DBNull.Value);
                log.LogVariableState("@trxId", DBNull.Value);
            }

            if (Utilities.ParafaitEnv.IsCorporate)
            {
                cmd.Parameters.AddWithValue("@site_id", Utilities.ParafaitEnv.SiteId);
                log.LogVariableState("@site_id", Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@site_id", -1);
                log.LogVariableState("@site_id", -1);
            }

            cmd.Parameters.AddWithValue("@user_id", (Utilities.ParafaitEnv.User_Id <= 0 ? DBNull.Value : (object)Utilities.ParafaitEnv.User_Id));
            cmd.Parameters.AddWithValue("@pos_machine", (Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : Utilities.ParafaitEnv.POSMachine));
            cmd.Parameters.AddWithValue("@task_type", task_type);
            cmd.Parameters.AddWithValue("@remarks", Remarks);
            if (string.IsNullOrWhiteSpace(Utilities.ParafaitEnv.ApproverId) == false
                && Utilities.ParafaitEnv.ApproverId != "-1" && Utilities.ParafaitEnv.ManagerId == -1)
            {
                Users approveUser = new Users(Utilities.ExecutionContext, Utilities.ParafaitEnv.ApproverId);
                if (approveUser.UserDTO != null)
                {
                    Utilities.ParafaitEnv.ManagerId = approveUser.UserDTO.UserId;
                }
            }
            cmd.Parameters.AddWithValue("@ApprovedBy", (Utilities.ParafaitEnv.ManagerId == -1 ? (Utilities.ParafaitEnv.User_Id <= 0 ? DBNull.Value : (object)Utilities.ParafaitEnv.User_Id) : Utilities.ParafaitEnv.ManagerId));
            Utilities.ParafaitEnv.ManagerId = -1;
            object o = cmd.ExecuteScalar();

            log.LogVariableState("@user_id", (Utilities.ParafaitEnv.User_Id <= 0 ? DBNull.Value : (object)Utilities.ParafaitEnv.User_Id));
            log.LogVariableState("@pos_machine", (Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : Utilities.ParafaitEnv.POSMachine));
            log.LogVariableState("@task_type", task_type);
            log.LogVariableState("@remarks", Remarks);
            log.LogVariableState("@ApprovedBy", (Utilities.ParafaitEnv.ManagerId == -1 ? (Utilities.ParafaitEnv.User_Id <= 0 ? DBNull.Value : (object)Utilities.ParafaitEnv.User_Id) : Utilities.ParafaitEnv.ManagerId));


            if (o != null)
            {
                TaskId = Convert.ToInt32(o);
                if (card_id > -1)
                {
                    Card card = new Card(card_id, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                    card.updateCardTime(SQLTrx);
                }
                if (sourceCard > -1 && sourceCard != card_id)
                {
                    Card card = new Card(sourceCard, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                    card.updateCardTime(SQLTrx);
                }
                if (transfer_to_card_id > -1 && transfer_to_card_id != card_id)
                {
                    Card card = new Card(transfer_to_card_id, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                    card.updateCardTime(SQLTrx);
                }
            }
            else
                TaskId = -1;

            log.LogMethodExit(TaskId);
            return TaskId;
        }

        /// <summary>
        /// DeductGameBalance
        /// </summary>
        /// <param name="CurrentCard"></param>
        /// <param name="deductValue"></param>
        /// <param name="message"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool DeductGameBalance(Card CurrentCard, double deductValue, int gameId, ref string message, Utilities utilities, SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(CurrentCard, deductValue, gameId, utilities);
                SqlCommand cmd = utilities.getCommand(sqlTransaction);
                double sumOfCardGameBalance = 0;
                if (CurrentCard != null)
                {
                    DataTable gameDataTable = utilities.executeDataTable(@"select cg.card_game_id, cg.TrxId,cg.TrxLineId, isnull(gp.profile_name, case when g.game_name is null then ' - All -' else null end) Profile, 
	                                                    isnull(g.game_name, case when gp.profile_name is null then ' - All -' else null end) Game,
                                                        sum(cg.Quantity) [Games Loaded],
                                                        sum(cg.BalanceGames) [Balance Games]
                                                        from CardGames cg 
                                                            left outer join game_profile gp on gp.game_profile_id = cg.game_profile_id
                                                            left outer join games g on g.game_id = cg.game_id
                                                        where cg.card_id = @cardId
                                                        and isnull(cg.Frequency, 'N') = 'N'
                                                        and isnull(cg.validityStatus, 'Y') != 'H'
                                                                   and quantity > 0 and cg.game_id =@gameId
                                                        and isnull(ExpiryDate, getdate()) >= getdate()
                                                        group by cg.TrxId,cg.TrxLineId, gp.profile_name, g.game_name,cg.card_game_id
                                                        having sum(cg.BalanceGames) > 0",
                                                              new SqlParameter("@cardId", CurrentCard.card_id),
                                                              new SqlParameter("@gameId", gameId)
                                                              );

                    log.LogVariableState("@cardId", CurrentCard.card_id);
                    cmd.CommandText = @"UPDATE CardGames SET BalanceGames  = @BalanceGames, last_update_date = Getdate(), LastUpdatedBy = @LastUpdatedBy 
                                        WHERE card_game_id = @cardGameId";
                    double balance = deductValue;
                    if (gameDataTable.Rows.Count == 0)
                    {
                        log.Error("Game balance is zero. Cannot refund");
                        throw new ValidationException("Game balance is zero. Cannot refund");
                    }
                    if (gameDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow rw in gameDataTable.Rows)
                        {
                            sumOfCardGameBalance = sumOfCardGameBalance + Convert.ToDouble(rw["Balance Games"]);
                        }
                        if (deductValue > sumOfCardGameBalance)
                        {
                            log.Error("Insufficient Game balance: Required: " + deductValue + " Available: " + sumOfCardGameBalance);
                            throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Insufficient Game balance: Required: " + deductValue + " Available: " + sumOfCardGameBalance));
                        }
                        foreach (DataRow rw in gameDataTable.Rows)
                        {
                            double cardGameBalance = Convert.ToDouble(rw["Balance Games"]);
                            if (balance > 0)
                            {
                                if (balance > cardGameBalance)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@BalanceGames", 0);
                                    balance = balance - cardGameBalance;
                                    log.LogVariableState("@BalanceGames", 0);
                                }
                                else
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@BalanceGames", cardGameBalance - balance);
                                    balance = 0;
                                    log.LogVariableState("@BalanceGames", cardGameBalance - balance);
                                }
                                cmd.Parameters.AddWithValue("@cardGameId", rw["card_game_id"]);
                                cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                                cmd.ExecuteNonQuery();

                                log.LogVariableState("@cardGameId", rw["card_game_id"]);
                                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                            }
                            if (balance <= 0)
                            {
                                break;
                            }
                        }
                        log.LogMethodExit(true);
                        return true;
                    }
                }
                else
                {
                    log.Debug("No game balance to deduct");
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while refunding card games ", ex);
                message = ex.Message;
                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                throw ex;
            }
            log.LogMethodExit(true);
            return true;
        }

        public bool loadTickets(Card card, int tickets, string remarks, int redemptionId, ref string message, SqlTransaction sqlTrx = null, bool? considerLoyalty = null)
        {
            log.LogMethodEntry(card, tickets, remarks, redemptionId, message, sqlTrx);
            try
            {
                AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, loadChildRecords: true, activeChildRecords: true);
                if (accountBL != null && accountBL.AccountDTO != null)
                {
                    if (tickets < 0)
                    {
                        decimal ticketsAvailable = 0;                        
                        if (accountBL.AccountDTO.AccountSummaryDTO != null)
                        {
                            if (accountBL.AccountDTO.AccountSummaryDTO.TotalTicketsBalance != null)
                            {
                                ticketsAvailable = ticketsAvailable + (decimal)accountBL.AccountDTO.AccountSummaryDTO.TotalTicketsBalance;
                            }
                        }
                        if ((tickets + ticketsAvailable) < 0)
                        {
                            message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 1647);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
            if (tickets > 0 && tickets > Utilities.ParafaitEnv.LOAD_TICKETS_LIMIT)
            {
                //message = Utilities.MessageUtils.getMessage(35, Utilities.ParafaitEnv.LOAD_TICKETS_LIMIT.ToString(), Utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT"));
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2830, tickets,
                                                       Utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT"), Utilities.ParafaitEnv.LOAD_TICKETS_LIMIT.ToString());

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            else if (tickets < 0 && (-1 * tickets) > Convert.ToInt32(Utilities.getParafaitDefaults("LOAD_TICKETS_DEDUCTION_LIMIT")))
            {
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5094, Utilities.getParafaitDefaults("LOAD_TICKETS_DEDUCTION_LIMIT"),
                                                       Utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT"));
                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            if (tickets > 0)
            {
                int mgrApprovalLimit = 0;
                try
                {
                    mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                }
                catch (Exception ex)
                {
                    log.Error("mgrApprovalLimit doesn't have a valid value!", ex);
                    mgrApprovalLimit = 0;
                    log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                }
                if (mgrApprovalLimit > 0)
                {
                    if (tickets > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                    {
                        message = Utilities.MessageUtils.getMessage(1211);
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            else
            {
                int mgrApprovalLoadTicketDeductionLimit = 0;
                try
                {
                    mgrApprovalLoadTicketDeductionLimit = Convert.ToInt32(Utilities.getParafaitDefaults("LOAD_TICKET_DEDUCTION_LIMIT_FOR_MANAGER_APPROVAL"));
                }
                catch (Exception ex)
                {
                    log.Error("mgrApprovalLoadTicketDeductionLimit doesn't have a valid value!", ex);
                    mgrApprovalLoadTicketDeductionLimit = 0;
                    log.LogVariableState("mgrApprovalLoadTicketDeductionLimit", mgrApprovalLoadTicketDeductionLimit);
                }
                if (mgrApprovalLoadTicketDeductionLimit > 0)
                {
                    if ((-1 * tickets) > mgrApprovalLoadTicketDeductionLimit && Utilities.ParafaitEnv.ManagerId == -1)
                    {
                        message = Utilities.MessageUtils.getMessage(1211);
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            
            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx;
            SqlCommand cmd;
            if (sqlTrx == null)
            {
                SQLTrx = cnn.BeginTransaction();
                cmd = Utilities.getCommand(SQLTrx);
            }
            else
            {
                cmd = Utilities.getCommand(sqlTrx);
                SQLTrx = sqlTrx;
            }
            try
            {
                //create transaction & Task
                CreateLoadTicketTransaction(card, tickets, remarks, redemptionId, SQLTrx, considerLoyalty);
                if (sqlTrx == null)
                {
                    SQLTrx.Commit();
                }
                cnn.Close();
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                if (sqlTrx == null)
                {
                    SQLTrx.Rollback();
                }
                cnn.Close();
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool RealETicket(Card card, bool realTicket, string remarks, ref string message)
        {
            log.LogMethodEntry(card, realTicket, remarks, message);

            char real_ticket_mode;
            if (realTicket)
                real_ticket_mode = 'Y';
            else
                real_ticket_mode = 'N';

            try
            {
                Utilities.executeNonQuery("update cards set real_ticket_mode = @real_ticket_mode, " +
                                                "last_update_time = getdate(), " +
                                                "LastUpdatedBy = @LastUpdatedBy " +
                                                "where card_id = @card_id",
                                            new SqlParameter("@card_id", card.card_id),
                                            new SqlParameter("@real_ticket_mode", real_ticket_mode),
                                            new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID));

                log.LogVariableState("@card_id", card.card_id);
                log.LogVariableState("@real_ticket_mode", real_ticket_mode);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while changing real ticket mode", ex);
                message = ex.Message;

                log.LogVariableState("@card_id", card.card_id);
                log.LogVariableState("@real_ticket_mode", real_ticket_mode);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            log.LogVariableState("message ", message);
            log.LogMethodExit(true);
            return true;
        }

        void CheckManualTicketPerDayLimit(int tickets, int redemptionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(tickets, redemptionId, sqlTransaction);
            try
            {
                if (redemptionId != -1)
                {
                    Type redemptionTicketAllocationDTO = Type.GetType("Semnox.Parafait.Redemption.RedemptionTicketAllocationDTO,RedemptionUtils");
                    Type type = Type.GetType("Semnox.Parafait.Redemption.RedemptionBL,RedemptionUtils");
                    object redemptionBL = null;
                    if (type != null)
                    {
                        ConstructorInfo constructorN = type.GetConstructor(new Type[] { typeof(int), Utilities.ExecutionContext.GetType(), typeof(SqlTransaction) });
                        redemptionBL = constructorN.Invoke(new object[] { redemptionId, Utilities.ExecutionContext, sqlTransaction });
                    }
                    else
                        throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1479, "RedemptionBL"));

                    int totalManualTikets = 0;
                    List<Tuple<string, decimal, int, int>> manualTicketData = null;

                    if (redemptionBL != null)
                    {
                        Type enumType = Type.GetType("Semnox.Parafait.Redemption.RedemptionDTO+RedemptionTicketSource,RedemptionUtils");
                        if (enumType != null)
                        {
                            var enumVal = enumType.GetEnumValues();
                            manualTicketData = (List<Tuple<string, decimal, int, int>>)type.GetMethod("GetTicketAllocationDetails").Invoke(redemptionBL, new object[] { enumVal.GetValue(4), sqlTransaction });

                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1479, "RedemptionDTO.RedemptionTicketSource"));

                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1479, "RedemptionBL"));
                    }
                    if (manualTicketData != null)
                    {
                        totalManualTikets = manualTicketData.Sum(x => x.Item3);
                    }

                    tickets = totalManualTikets;
                }
                Type redemptionTicketAllocationListBL = Type.GetType("Semnox.Parafait.Redemption.RedemptionTicketAllocationListBL,RedemptionUtils");
                object redemptionTicketAllocationBLObject = null;
                if (redemptionTicketAllocationListBL != null)
                {
                    ConstructorInfo constructorN = redemptionTicketAllocationListBL.GetConstructor(new Type[] { Utilities.ExecutionContext.GetType() });
                    redemptionTicketAllocationBLObject = constructorN.Invoke(new object[] { Utilities.ExecutionContext });
                }
                else
                {
                    throw new Exception("Unable to retrive RedemptionAllocationBL class from assembly");
                }

                MethodInfo redemptionTicketAllocationMethodType = redemptionTicketAllocationListBL.GetMethod("CanAddManualTicketForTheDay", new[] { Utilities.ExecutionContext.GetUserId().GetType(), typeof(int), sqlTransaction.GetType() });
                object retVal = redemptionTicketAllocationMethodType.Invoke(redemptionTicketAllocationBLObject, new object[] { Utilities.ExecutionContext.GetUserId(), tickets, sqlTransaction });

                if (Convert.ToBoolean(retVal) == false)
                {
                    MethodInfo rtaMethodTypeToGetRemainingTicketLimit = redemptionTicketAllocationListBL.GetMethod("GetRemainingAddManualTicketLimitForTheDay", new[] { Utilities.ExecutionContext.GetUserId().GetType(), sqlTransaction.GetType() });
                    object remainitingTicketCount = rtaMethodTypeToGetRemainingTicketLimit.Invoke(redemptionTicketAllocationBLObject, new object[] { Utilities.ExecutionContext.GetUserId(), sqlTransaction });
                    if (Convert.ToInt32(remainitingTicketCount) > 0)
                    {
                        throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2488, Convert.ToInt32(remainitingTicketCount)));//2488:"You can add only " + Convert.ToInt32(remainitingTicketCount) + " more manual tickets for the day"
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2489));//2489:Manual Ticket limit per day exceeded
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public bool exchangeTokenForCredit(Card card, int tokens, double credits, string remarks, ref string message, int sourceTrxId)
        {
            log.LogMethodEntry(card, tokens, credits, remarks, message, sourceTrxId);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL_IN_POS"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get the value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (tokens > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1210);
                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            try
            {
                if (card != null && card.technician_card.Equals('Y'))
                {
                    message = Utilities.MessageUtils.getMessage(197, card.CardNumber);
                    TasksEventLog.logEvent("Parafait POS", 'D', card.CardNumber, "ExchangeTokenForCredit - Technician card accessed redeem tokens", "", 3);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                if (card != null && gameCardCreditLimit > 0 && card.technician_card != 'Y')
                {
                    if ((credits + card.credits) > Convert.ToDouble(gameCardCreditLimit))
                    {
                        message = Utilities.MessageUtils.getMessage(1168);

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                if (!card.AddCreditsToCard(credits, SQLTrx, ref message))
                {
                    SQLTrx.Rollback();
                    cnn.Close();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                createTask(card.card_id, TaskProcs.EXCHANGETOKENFORCREDIT, -1, -1, credits, tokens, -1, -1, -1, remarks, SQLTrx, -1, -1, -1, -1, sourceTrxId);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while exchanging token for credit", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool exchangeCreditForToken(Card card, int tokens, double credits, string remarks, ref string message)
        {
            log.LogMethodEntry(card, tokens, credits, remarks, message);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            double cardCredits, creditPlusCardBalance;

            if (credits >= card.credits)
            {
                cardCredits = card.credits;
                creditPlusCardBalance = credits - cardCredits;
            }
            else
            {
                cardCredits = credits;
                creditPlusCardBalance = 0;
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            try
            {
                if (!card.AddCreditsToCard(cardCredits * -1, SQLTrx, ref message, 0, creditPlusCardBalance * -1))
                {
                    SQLTrx.Rollback();
                    cnn.Close();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                if (creditPlusCardBalance > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    creditPlus.deductCreditPlus(-1, card.card_id, creditPlusCardBalance, null, SQLTrx, -1, Utilities.ParafaitEnv.LoginID);
                }

                createTask(card.card_id, TaskProcs.EXCHANGECREDITFORTOKEN, -1, -1, credits, tokens, -1, -1, -1, remarks, SQLTrx);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while exchanging credits for token", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        //Begin: Added to refund Time as Cash on 25-Sep-2015//
        public bool RefundTime(Card card, double refundAmount, int CardTimeTrxId, string remarks, ref string message)
        {
            log.LogMethodEntry(card, refundAmount, CardTimeTrxId, remarks, message);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            AccountDTO accountDTO = accountBL.AccountDTO;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
            if ((accountDTO.AccountCreditPlusDTOList != null
                          && accountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusBalance != 0
                                                                             && (x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                             && ((x.SubscriptionBillingScheduleId == -1)
                                                                                || (x.SubscriptionBillingScheduleId != -1
                                                                                    && subscriptionBillingScheduleDTOList != null
                                                                                    && subscriptionBillingScheduleDTOList.Any()
                                                                                    && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive))))))) //Ignore subscription holds
            {
                message = Utilities.MessageUtils.getMessage(2610);

                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand RefundCmd = Utilities.getCommand(SQLTrx);

            try
            {
                if (!CreateRefundTransaction(card, 0, 0, 0, 0, refundAmount, SQLTrx, ref message))
                {
                    SQLTrx.Rollback();
                    cnn.Close();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                //Added condition for credit plus time with transaction id
                if (CardTimeTrxId != -1)
                {
                    RefundCmd.CommandText = "update CardCreditPlus set CreditPlusBalance = 0 " +
                                                "where CreditPlusType ='M' " +
                                                "and CreditPlusBalance > 0 and TrxId = @TrxId and Card_id = @cardId";
                    RefundCmd.Parameters.AddWithValue("@TrxId", CardTimeTrxId);
                    RefundCmd.Parameters.AddWithValue("@cardId", card.card_id);
                    RefundCmd.ExecuteNonQuery();

                    log.LogVariableState("@TrxId", CardTimeTrxId);
                    log.LogVariableState("@cardId", card.card_id);
                }
                else
                {
                    RefundCmd.Parameters.Clear();
                    RefundCmd.CommandText = "update cards set time = 0 ,start_time =null where time > 0 and card_id = @cardId";
                    RefundCmd.Parameters.AddWithValue("@cardId", card.card_id);
                    RefundCmd.ExecuteNonQuery();
                    log.LogVariableState("@cardId", card.card_id);
                }

                createTask(card.card_id, TaskProcs.REFUNDCARD, -1, -1, -1, -1, -1, TransactionId, CardTimeTrxId, remarks, SQLTrx);

                TrxUserLogsBL trxUserLogs = new TrxUserLogsBL(TransactionId, -1, Utilities.ParafaitEnv.LoginID, Utilities.getServerTime(), Utilities.ParafaitEnv.POSMachineId, "REFUNDCARD", "Card(s) Refunded", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                trxUserLogs.Save(SQLTrx);

                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while refunding time ", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }
        //End: Added to refund Time as Cash on 25-Sep-2015//

        public bool RefundCardGames(Card card, double refundGameAmount, int CardGameTrxId, string remarks, ref string message)
        {
            log.LogMethodEntry(card, refundGameAmount, CardGameTrxId, remarks, message);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            AccountDTO accountDTO = accountBL.AccountDTO;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
            if ((accountDTO.AccountGameDTOList != null
                          && accountDTO.AccountGameDTOList.Exists(x => x.BalanceGames != 0
                                                                        && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                        && ((x.SubscriptionBillingScheduleId == -1)
                                                                            || (x.SubscriptionBillingScheduleId != -1
                                                                                 && subscriptionBillingScheduleDTOList != null
                                                                                 && subscriptionBillingScheduleDTOList.Any()
                                                                                 && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
                        )
                       )
            {
                message = Utilities.MessageUtils.getMessage(2610);

                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand RefundCmd = Utilities.getCommand(SQLTrx);

            try
            {
                if (!CreateRefundTransaction(card, 0, 0, 0, 0, refundGameAmount, SQLTrx, ref message))
                {
                    SQLTrx.Rollback();
                    cnn.Close();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                RefundCmd.CommandText = "update cardGames set BalanceGames = 0 where TrxId = @TrxId and BalanceGames > 0 and card_id = @cardId";
                RefundCmd.Parameters.AddWithValue("@TrxId", CardGameTrxId);
                RefundCmd.Parameters.AddWithValue("@cardId", card.card_id);
                RefundCmd.ExecuteNonQuery();

                log.LogVariableState("@TrxId", CardGameTrxId);
                log.LogVariableState("@cardId", card.card_id);

                createTask(card.card_id, TaskProcs.REFUNDCARD, -1, -1, -1, -1, -1, TransactionId, CardGameTrxId, remarks, SQLTrx);

                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while refunding card games ", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        //Begin: Refund Tax related changes.Calculate the tax amount when Tax is inclusive / Exclusive and check if tax is Inclusive/Exclusive on 25-Sept-2015
        public double CalculateRefundAmount(double cardDeposit, double refundAmount, double credits, double creditPlus, ref bool refundTaxInclucisve, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(cardDeposit, refundAmount, credits, creditPlus, refundTaxInclucisve, SQLTrx);

            double taxAmount = 0;
            double refundTaxAmount = 0;



            DataTable dtRefundTax = Utilities.executeDataTable(@"select top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                from Products p, product_type pt ,tax t
                                                                where product_type = 'REFUND' 
                                                                and p.product_type_id = pt.product_type_id
                                                                and t.tax_id = p.tax_id");
            if (dtRefundTax.Rows.Count > 0)
            {
                if (dtRefundTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    refundTaxAmount = refundAmount - cardDeposit;
                    refundAmount = refundTaxAmount / (1 + Convert.ToDouble(dtRefundTax.Rows[0]["taxPercentage"]) / 100);
                    taxAmount = refundTaxAmount - refundAmount;// (refundTaxAmount * Convert.ToDouble(dtRefundTax.Rows[0]["taxPercentage"]) / 100);
                    refundTaxInclucisve = true;
                }
                else
                {
                    refundTaxAmount = refundAmount - cardDeposit;
                    taxAmount = (refundTaxAmount * Utilities.ParafaitEnv.RefundCardTaxPercent / 100);
                    //refundAmount = Math.Round(cardDeposit + credits + creditPlus, 4, MidpointRounding.AwayFromZero);
                }
            }
            //Begin Calculate Deposit Tax 01-Mar-2016
            DataTable dtRefundDepositTax = Utilities.executeDataTable(@"SELECT top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                          FROM Products p, product_type pt ,tax t
                                                                         WHERE product_type = 'REFUNDCARDDEPOSIT' 
                                                                           AND p.product_type_id = pt.product_type_id
                                                                           AND t.tax_id = p.tax_id");

            if (dtRefundDepositTax.Rows.Count > 0)
            {
                if (dtRefundDepositTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    taxAmount = taxAmount + (cardDeposit - cardDeposit / (1 + Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100));
                }
                else
                {
                    taxAmount = taxAmount + (cardDeposit * Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100);
                }
            }
            //End Calculate Deposit Tax 01-Mar-2016

            log.LogMethodExit(taxAmount);
            return taxAmount;
        }
        //end: Refund Tax related changes.25-Sept-2015

        public bool RefundCard(Card card, double inCardDeposit, double inCredits, double inCreditPlus, string remarks, ref string message, bool makeNewOnFullRefund = true, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(card, inCardDeposit, inCredits, inCreditPlus, remarks, message, makeNewOnFullRefund);

            List<Card> CardList = new List<Card>();
            CardList.Add(card);

            bool returnValue = RefundCard(CardList, inCardDeposit, inCredits, inCreditPlus, remarks, ref message, makeNewOnFullRefund, inSQLTrx);

            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public bool RefundCard(List<Card> CardList, double inCardDeposit, double inCredits, double inCreditPlus, string remarks, ref string message, bool makeNewOnFullRefund = true, SqlTransaction inSQLTrx = null, Transaction transaction = null)
        {
            log.LogMethodEntry(CardList, inCardDeposit, inCredits, inCreditPlus, remarks, message, makeNewOnFullRefund);

            double totalCredits = 0;
            double totalCardDeposit = 0;
            double totalCreditPlus = 0;
            double totalRefundAmount = 0;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
                SQLTrx = Utilities.createConnection().BeginTransaction();
            else
                SQLTrx = inSQLTrx;
            SqlConnection cnn = SQLTrx.Connection;

            SqlCommand RefundCmd = Utilities.getCommand(SQLTrx);
            List<KeyValuePair<int, int>> cardTaskIdList = new List<KeyValuePair<int, int>>();
            foreach (Card c in CardList)
            {
                log.LogVariableState("Card", c);
                AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, c.card_id, true, true);
                if (accountBL.IsAccountUpdatedByOthers(c.last_update_time))
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                AccountDTO accountDTO = accountBL.AccountDTO;
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, SQLTrx);
                if ((accountDTO.AccountCreditPlusDTOList != null
                              && accountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusBalance != 0
                                                                                 && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                 && ((x.SubscriptionBillingScheduleId == -1)
                                                                                     || (x.SubscriptionBillingScheduleId != -1
                                                                                           && subscriptionBillingScheduleDTOList != null
                                                                                           && subscriptionBillingScheduleDTOList.Any()
                                                                                           && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription hold
                            )
                            ||
                            (accountDTO.AccountGameDTOList != null
                              && accountDTO.AccountGameDTOList.Exists(x => x.BalanceGames != 0
                                                                           && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                           && ((x.SubscriptionBillingScheduleId == -1)
                                                                                || (x.SubscriptionBillingScheduleId != -1
                                                                                   && subscriptionBillingScheduleDTOList != null
                                                                                   && subscriptionBillingScheduleDTOList.Any()
                                                                                   && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive))))//Ignore subscription hold
                            )
                            ||
                            (accountDTO.AccountDiscountDTOList != null
                              && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                               && ((x.SubscriptionBillingScheduleId == -1)
                                                                                || (x.SubscriptionBillingScheduleId != -1
                                                                                   && subscriptionBillingScheduleDTOList != null
                                                                                   && subscriptionBillingScheduleDTOList.Any()
                                                                                   && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription hold
                            )
                           )
                {
                    message = Utilities.MessageUtils.getMessage(2610);

                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
                try
                {
                    CanRefundTheCard(accountDTO, subscriptionBillingScheduleDTOList, SQLTrx);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    message = ex.Message;
                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
                double cardDeposit = 0;
                double credit_amount = 0;
                double creditPlusAmount = 0;
                double totalAmount = 0;
                double refundAmount;
                bool refundTaxInclucisve = false; //Added for refund tax calculation - 25-Sep-2015
                double taxAmount = 0;


                CreditPlus creditPlus = new CreditPlus(Utilities);
                if (Utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_DEPOSIT == "Y")
                {
                    if (CardList.Count == 1)
                    {
                        cardDeposit = c.face_value;
                        totalCardDeposit = inCardDeposit;
                    }
                    else
                    {
                        cardDeposit = inCardDeposit = c.face_value;
                        totalCardDeposit += cardDeposit;
                    }
                }

                if (Utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y")
                {
                    if (CardList.Count == 1)
                    {
                        credit_amount = c.credits;
                        totalCredits = inCredits;
                    }
                    else
                    {
                        credit_amount = inCredits = c.credits;
                        totalCredits += credit_amount;
                    }
                }
                if (Utilities.ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y")
                {

                    if (CardList.Count == 1)
                    {
                        creditPlusAmount = creditPlus.getCreditPlusRefund(c.card_id);
                        totalCreditPlus = inCreditPlus;
                    }
                    else
                    {
                        creditPlusAmount = inCreditPlus = creditPlus.getCreditPlusRefund(c.card_id);
                        totalCreditPlus += creditPlusAmount;
                    }
                }
                if (inCardDeposit + inCredits + inCreditPlus > 0 && Utilities.ParafaitEnv.IsClientServer && transaction == null)
                {
                    DataTable dt = Utilities.executeDataTable("select distinct top 5 trxdate, creditcardamount  " +
                                     "from trx_header h, trx_lines l " +
                                     "where l.card_Id = @cardId " +
                                     "and h.trxid = l.trxid " +
                                     "and h.creditCardAmount > 0 " +
                                     "order by trxdate desc",
                                    new SqlParameter("@cardId", c.card_id));

                    log.LogVariableState("@cardId", c.card_id);

                    string temp = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        temp += Convert.ToDateTime(dt.Rows[i][0]).ToString(Utilities.ParafaitEnv.DATE_FORMAT) + ": " + Convert.ToDecimal(dt.Rows[i][1]).ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + Environment.NewLine;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (MessageBox.Show(Utilities.MessageUtils.getMessage(355, "") + Environment.NewLine + Environment.NewLine +
                                            temp + Environment.NewLine +
                                            Utilities.MessageUtils.getMessage(356), "Refund Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            message = Utilities.MessageUtils.getMessage(357);

                            log.LogVariableState("message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }

                totalAmount = Math.Round(cardDeposit + credit_amount + creditPlusAmount, 4, MidpointRounding.AwayFromZero);
                refundAmount = Math.Round(inCardDeposit + inCredits + inCreditPlus, 4, MidpointRounding.AwayFromZero);
                totalRefundAmount += refundAmount;
                //Begin:Calculate the tax amount when Tax is inclusive / Exclusive on 25-Sept-2015
                taxAmount = CalculateRefundAmount(inCardDeposit, refundAmount, inCredits, inCreditPlus, ref refundTaxInclucisve);
                //End: Refund Tax 25-Sept-2015

                double mgrApprovalLimit = 0;
                try { mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("REFUND_AMOUNT_LIMIT_FOR_MANAGER_APPROVAL")); }
                catch (Exception ex)
                {
                    log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                    mgrApprovalLimit = 0;
                    log.LogVariableState("mgrApprovalLimit ", mgrApprovalLimit);
                }
                if (mgrApprovalLimit > 0)
                {
                    if (refundAmount > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                    {
                        message = Utilities.MessageUtils.getMessage(1216);

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                //int lockerAllocationId = -1;
                LockerAllocationDTO lockerAllocationDTO = null;
                if (totalAmount == refundAmount && makeNewOnFullRefund) // card will be invalidated. check for any dependencies 
                {
                    if (Utilities.executeScalar("select top 1 1" +
                                                 "from CheckIns h, CheckInDetails d " +
                                                 "where h.cardId = @cardId " +
                                                 "and h.CheckInId = d.CheckInId " +
                                                 "and (CheckOutTime is null OR (CheckoutTime is not null and CheckoutTime > getdate()))" +
                                                  "and (d.Status = 'CHECKEDIN') " +
                                                 "union all " +
                                                 "select top 1 1 " +
                                                 "from CheckInDetails d " +
                                                 "where d.cardId = @cardId " +
                                                 "and (CheckOutTime is null OR (CheckoutTime is not null and CheckoutTime > getdate())) " +
                                                 "and (d.Status = 'CHECKEDIN') ", new SqlParameter("cardId", c.card_id)) != null)
                    {
                        message = Utilities.MessageUtils.getMessage(358);

                        log.LogVariableState("cardId", c.card_id);
                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    LockerAllocation lockerAllocation = new LockerAllocation();

                    lockerAllocationDTO = lockerAllocation.GetValidAllocation(-1, c.card_id);
                    //if (lockerAllocationDTO != null && !lockerAllocationDTO.Refunded)
                    //{
                    //    lockerAllocationDTO = lockerAllocation.GetLockerAllocationDTO;
                    //}
                    if (lockerAllocationDTO != null && lockerAllocationDTO.Id != -1)
                    {
                        double otherDeposits = c.getOtherDeposits(lockerAllocationDTO);
                        if (otherDeposits > 0)
                        {
                            message = "Card has a locker deposit of " + otherDeposits.ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + ". Card can be refunded after Locker Return.";

                            log.LogVariableState("message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        else if (lockerAllocationDTO != null && lockerAllocationDTO.Id != -1)
                        {
                            message = "Card has a locker allocated. Card can be refunded after Locker Return.";

                            log.LogVariableState("message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                try
                {
                    //Begin modification for refund tax - 25-Sep-2015
                    double refundWithTax = 0;
                    if (refundTaxInclucisve)
                        refundWithTax = refundAmount;//Calculate refund Amount when tax is Inclusive.
                                                     //End: Refund tax related changes. 25- sept-2015
                    else
                        refundWithTax = refundAmount + taxAmount;//Calculate refund Amount when tax is Exclusive.
                                                                 //End: Refund tax related changes. 25- sep-2015

                    RefundCmd.CommandText = @"update cards set refund_amount = isnull(refund_amount, 0) + @refund_amount, 
                                                    credits = credits - @credits, 
                                                    face_value = face_value - @faceValue, 
                                                    last_update_time = getdate(), 
                                                    LastUpdatedBy = @LastUpdatedBy, 
                                                    refund_date = case @valid when 'Y' then refund_date else getdate() end, 
                                                    refund_flag = case @valid when 'Y' then refund_flag else 'Y' end, 
                                                    valid_flag = @valid 
                                                    where card_id = @card_id";

                    RefundCmd.Parameters.Clear();
                    RefundCmd.Parameters.AddWithValue("@card_id", c.card_id);
                    RefundCmd.Parameters.AddWithValue("@refund_amount", refundWithTax);
                    RefundCmd.Parameters.AddWithValue("@credits", inCredits);
                    RefundCmd.Parameters.AddWithValue("@faceValue", inCardDeposit);
                    RefundCmd.Parameters.AddWithValue("@valid", (refundAmount == totalAmount ? (makeNewOnFullRefund ? "N" : "Y") : "Y"));
                    RefundCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                    RefundCmd.ExecuteNonQuery();

                    log.LogVariableState("@card_id", c.card_id);
                    log.LogVariableState("@refund_amount", refundWithTax);
                    log.LogVariableState("@credits", inCredits);
                    log.LogVariableState("@faceValue", inCardDeposit);
                    log.LogVariableState("@valid", (refundAmount == totalAmount ? (makeNewOnFullRefund ? "N" : "Y") : "Y"));
                    log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                    bool response;
                    if (refundAmount == totalAmount && makeNewOnFullRefund)
                    {
                        response = c.refund_MCard(ref message);

                        if (c.isMifare && (lockerAllocationDTO != null && lockerAllocationDTO.Id >= 0))
                        {
                            LockerZones lockerZones = new LockerZones(c.Utilities.ExecutionContext, lockerAllocationDTO.LockerId);
                            ParafaitLockCardHandler locker;
                            Locker lockerBl = new Locker(lockerAllocationDTO.LockerId);
                            string lockerMake = (lockerZones.GetLockerZonesDTO != null && !string.IsNullOrEmpty(lockerZones.GetLockerZonesDTO.LockerMake)) ? lockerZones.GetLockerZonesDTO.LockerMake : Utilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
                            if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.COCY.ToString()))
                                locker = new CocyLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext, Convert.ToByte(c.Utilities.ParafaitEnv.MifareCustomerKey));
                            else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()))
                                locker = new InnovateLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext, Convert.ToByte(c.Utilities.ParafaitEnv.MifareCustomerKey), c.CardNumber);
                            else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                                locker = new PassTechLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext);
                            else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                                locker = new MetraLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext, c.CardNumber, lockerBl.getLockerDTO.Identifier.ToString(), lockerZones.GetLockerZonesDTO.ZoneCode, lockerZones.GetLockerZonesDTO.LockerMake, lockerZones.GetLockerZonesDTO.LockerMode);
                            else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                                locker = new HecereLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext, c.CardNumber);
                            else
                                locker = new ParafaitLockCardHandler(c.ReaderDevice, c.Utilities.ExecutionContext);
                            locker.EraseCard();
                        }
                    }
                    else
                        response = c.updateMifareCard(false, ref message, -inCredits, 0, 0, -inCreditPlus);

                    if (!response)
                    {
                        message = Utilities.MessageUtils.getMessage(318, message);
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                            cnn.Close();
                        }

                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    int newTaskId = createTask(c.card_id, TaskProcs.REFUNDCARD, -1, -1, -1, -1, -1, -1, -1, remarks, SQLTrx);
                    cardTaskIdList.Add(new KeyValuePair<int, int>(c.card_id, newTaskId));
                    //SQLTrx.Commit();
                    //cnn.Close();
                    //return true;

                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while refunding card", ex);

                    if (inSQLTrx == null)
                    {
                        SQLTrx.Rollback();
                        cnn.Close();
                    }
                    message = ex.Message;

                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            try
            {
                //cnn = Utilities.createConnection();
                //SQLTrx = cnn.BeginTransaction();
                //RefundCmd = Utilities.getCommand(SQLTrx);
                if (transaction == null)
                {
                    if (!CreateRefundTransaction(CardList, totalCredits, totalCreditPlus, totalCardDeposit, totalRefundAmount, 0, SQLTrx, ref message))
                    {
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                            cnn.Close();
                        }

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    TrxUserLogsBL trxUserLogs = new TrxUserLogsBL(TransactionId, -1, Utilities.ParafaitEnv.LoginID, Utilities.getServerTime(), Utilities.ParafaitEnv.POSMachineId, "REFUNDCARD", "Card(s) Refunded", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                    trxUserLogs.Save(SQLTrx);
                }
                else
                {
                    if (inCreditPlus > 0)
                    {
                        CreditPlus cp = new CreditPlus(Utilities);
                        cp.refundCreditPlus((int)transaction.Trx_id, (int)CardList[0].card_id, inCreditPlus, SQLTrx, Utilities.ParafaitEnv.LoginID);
                    }
                    TransactionId = transaction.Trx_id;
                    transaction.SaveTransacation(SQLTrx, ref message);
                }
                //Handle Notification Tag issued in case of card refund
                //Perform refund action even after tag has expired in NotificationTagIssued
                List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.IS_ACTIVE, 1.ToString()));
                //tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.EXPIRY_DATE_NULL_AFTER, (Utilities.getServerTime()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, CardList[0].card_id.ToString()));
                NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(Utilities.ExecutionContext);
                List<NotificationTagIssuedDTO> NotificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters, SQLTrx);
                foreach (NotificationTagIssuedDTO notificationTagIssuedDTO in NotificationTagIssuedListDTO)
                {
                    notificationTagIssuedDTO.IsReturned = true;
                    notificationTagIssuedDTO.ReturnDate = Utilities.getServerTime();
                    NotificationTagIssuedBL notificationTagIssuedBL
                        = new NotificationTagIssuedBL(Utilities.ExecutionContext, notificationTagIssuedDTO);
                    notificationTagIssuedBL.Save(SQLTrx);
                }

                SqlCommand updateTaskCmd = Utilities.getCommand(SQLTrx);
                foreach (KeyValuePair<int, int> cardTaskInfo in cardTaskIdList)
                {
                    updateTaskCmd.CommandText = @"update tasks 
                                                     set attribute1 = @trxId, lastUpdatedBy = @lastUpdatedBy , lastUpdateDate = getdate() 
                                                   where task_id = @taskId and card_id = @refundCardId "; //since task created before refund transaction
                    updateTaskCmd.Parameters.Clear();
                    updateTaskCmd.Parameters.AddWithValue("@refundCardId", cardTaskInfo.Key);
                    updateTaskCmd.Parameters.AddWithValue("@taskId", cardTaskInfo.Value);
                    updateTaskCmd.Parameters.AddWithValue("@trxId", TransactionId);
                    updateTaskCmd.Parameters.AddWithValue("@lastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                    updateTaskCmd.ExecuteNonQuery();
                }
                if (inSQLTrx == null)
                {
                    SQLTrx.Commit();
                    cnn.Close();
                }
                try
                {
                    foreach (Card c in CardList)
                    {
                        ParentChildCardsListBL parentChildCardsListBL = new ParentChildCardsListBL(Utilities.ExecutionContext);
                        List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.CHILD_CARD_ID, c.card_id.ToString()));
                        List<ParentChildCardsDTO> parentChildCardsDTOList = parentChildCardsListBL.GetParentChildCardsDTOList(searchParameters);
                        parentChildCardsListBL.SplitDailyLimitPercentage();
                        parentChildCardsListBL.SaveParentChildCardsList();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while checking Parent Child Cards linkage", ex);
                }
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Refund Transaction is Incomplete! ", ex);
                if (inSQLTrx == null)
                {
                    SQLTrx.Rollback();
                    cnn.Close();
                }
                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

        }

        bool CreateRefundTransaction(Card card,
                                    double inCredits,
                                    double inCreditPlus,
                                    double inCardDeposit,
                                    double refundCreditsAmount,
                                    double refundGamesAmount,
                                    SqlTransaction SQLTrx,
                                    ref string message)
        {
            log.LogMethodEntry(card, inCredits, inCreditPlus, inCardDeposit, refundCreditsAmount,
                refundGamesAmount, SQLTrx, message);

            List<Card> refundCardList = new List<Card>();
            refundCardList.Add(card);

            bool returnValue = CreateRefundTransaction(refundCardList, inCredits, inCreditPlus, inCardDeposit, refundCreditsAmount, refundGamesAmount, SQLTrx, ref message);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        bool CreateRefundTransaction(List<Card> refundCardList,
                                    double inCredits,
                                    double inCreditPlus,
                                    double inCardDeposit,
                                    double refundCreditsAmount,
                                    double refundGamesAmount,
                                    SqlTransaction SQLTrx,
                                    ref string message)
        {
            log.LogMethodEntry(refundCardList, inCredits, inCreditPlus, inCardDeposit,
                refundCreditsAmount, refundGamesAmount, SQLTrx, message);

            try
            {
                SqlCommand RefundCmd = Utilities.getCommand(SQLTrx);

                double refundAmount = 0;
                double refundWithTax = 0;
                double taxAmount = 0;
                double inclusiveTaxAmount = 0;//Added the parameter to calculate Tax on 25-Sept-2015
                bool refundTaxInclusive = false;
                int refundCardDepositTaxId = -1; //Added to get the RefundCardDeposit Tax id on Dec-14-2015
                double refundCardDepositTaxPercentage = 0;//Added to get the RefundCardDeposit Tax id on Dec-14-2015
                bool refundCardDepositTaxInclusive = false; //Added to handle deposit value if tax is defined against card deposit
                int orderTypeId = -1;
                int orderTypeGroupId = -1;

                //Begin: Calculate the tax amount when Tax is inclusive / Exclusive on 25-Sept-2015
                inclusiveTaxAmount = CalculateRefundAmount(inCardDeposit, refundCreditsAmount, inCredits, inCreditPlus, ref refundTaxInclusive, SQLTrx);
                //End calculate Tax 25-sept-2015
                if (refundGamesAmount > 0)
                {
                    refundAmount = refundGamesAmount;
                    refundWithTax = refundGamesAmount * -1;
                }
                else
                {
                    refundAmount = refundCreditsAmount;
                    //Begin: Added the below condition to check if tax is inclusive or exclusive of the product price on 25-sept-2015//
                    if (refundTaxInclusive)
                    {
                        refundWithTax = refundAmount * -1;
                        taxAmount = inclusiveTaxAmount * -1;
                    }
                    else
                    {
                        refundWithTax = (refundAmount + inclusiveTaxAmount) * -1;
                        taxAmount = inclusiveTaxAmount * -1;
                    }
                    //end :  Refund  Tax related changes on 25-sept-2015//
                }
                //Begin: Added to get the RefundCardDeposit Tax id on De -14-2015//
                DataTable dtRefundTax = Utilities.executeDataTable(@"select top 1 t.tax_id taxId , isnull(TaxInclusivePrice,'N') TaxInclusivePrice, 
                                                                                      t.tax_percentage taxPercentage, IsNull(p.OrderTypeId, pt.OrderTypeId) as OrderTypeId
                                                                    from Products p, product_type pt ,tax t
                                                                    where product_type = 'REFUNDCARDDEPOSIT' 
                                                                    and p.product_type_id = pt.product_type_id
                                                                    and t.tax_id = p.tax_id", SQLTrx);
                if (dtRefundTax.Rows.Count > 0)
                {
                    refundCardDepositTaxId = Convert.ToInt32(dtRefundTax.Rows[0]["taxId"]);
                    refundCardDepositTaxPercentage = Convert.ToDouble(dtRefundTax.Rows[0]["taxPercentage"]);
                    orderTypeId = dtRefundTax.Rows[0]["OrderTypeId"] != DBNull.Value ? Convert.ToInt32(dtRefundTax.Rows[0]["OrderTypeId"]) : -1;
                    refundCardDepositTaxInclusive = (dtRefundTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y" ? true : false);
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
                //End:Added to get the RefundCardDeposit Tax id on De -14-2015//
                RefundCmd.CommandText = @"Insert into trx_header (
                                           TrxDate, TrxAmount, payment_mode, PaymentReference, status,
                                           TrxNetAmount, posMachineId, pos_machine, POSTypeId, user_id, cashAmount, CreditCardAmount, GameCardAmount, 
                                           OtherPaymentModeAmount, taxAmount, PrimaryCardId, CreatedBy, OrderTypeGroupId, 
                                            CreationDate, LastUpdatedBy, LastUpdateTime) 
                                           values (
                                           getdate(), @TrxAmount, @payment_mode, @PaymentReference, 'CLOSED',
                                           @TrxNetAmount, @posMachineId, @pos_machine, @POSTypeId, @user_id, @cashAmount, 0, 0, 0, @taxAmount, @PrimaryCardId,
                                            @user_id, @OrderTypeGroupId, getdate(), @loginId, getdate()); select @@identity;";

                RefundCmd.Parameters.AddWithValue("@TrxAmount", refundWithTax);
                RefundCmd.Parameters.AddWithValue("@TaxAmount", taxAmount);
                RefundCmd.Parameters.AddWithValue("@TrxNetAmount", refundWithTax);
                RefundCmd.Parameters.AddWithValue("@cashAmount", refundWithTax);
                RefundCmd.Parameters.AddWithValue("@pos_machine", Utilities.ParafaitEnv.POSMachine);
                RefundCmd.Parameters.AddWithValue("@PrimaryCardId", refundCardList[0].card_id);
                if (orderTypeGroupId < 0)
                {
                    RefundCmd.Parameters.AddWithValue("@OrderTypeGroupId", DBNull.Value);
                }
                else
                {
                    RefundCmd.Parameters.AddWithValue("@OrderTypeGroupId", orderTypeGroupId);
                }

                log.LogVariableState("@TrxAmount", refundWithTax);
                log.LogVariableState("@TaxAmount", taxAmount);
                log.LogVariableState("@TrxNetAmount", refundWithTax);
                log.LogVariableState("@cashAmount", refundWithTax);
                log.LogVariableState("@pos_machine", Utilities.ParafaitEnv.POSMachine);
                log.LogVariableState("@PrimaryCardId", refundCardList[0].card_id);

                if (Utilities.ParafaitEnv.POSMachineId == -1)
                {
                    RefundCmd.Parameters.AddWithValue("@posMachineId", DBNull.Value);
                    log.LogVariableState("@posMachineId", DBNull.Value);
                }
                else
                {
                    RefundCmd.Parameters.AddWithValue("@posMachineId", Utilities.ParafaitEnv.POSMachineId);
                    log.LogVariableState("@posMachineId", Utilities.ParafaitEnv.POSMachineId);
                }

                if (Utilities.ParafaitEnv.POSTypeId == -1)
                {
                    RefundCmd.Parameters.AddWithValue("@POSTypeId", DBNull.Value);
                    log.LogVariableState("@POSTypeId", DBNull.Value);
                }
                else
                {
                    RefundCmd.Parameters.AddWithValue("@POSTypeId", Utilities.ParafaitEnv.POSTypeId);
                    log.LogVariableState("@POSTypeId", Utilities.ParafaitEnv.POSTypeId);
                }
                if (Utilities.ParafaitEnv.User_Id <= 0)
                {
                    RefundCmd.Parameters.AddWithValue("@user_id", DBNull.Value);
                    log.LogVariableState("@user_id", DBNull.Value);
                }
                else
                {
                    RefundCmd.Parameters.AddWithValue("@user_id", Utilities.ParafaitEnv.User_Id);
                    log.LogVariableState("@user_id", Utilities.ParafaitEnv.User_Id);
                }
                RefundCmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);
                RefundCmd.Parameters.AddWithValue("@payment_mode", 1); //Cash
                RefundCmd.Parameters.AddWithValue("@PaymentReference", "Refund"); //Cash

                int Trx_id = Convert.ToInt32(RefundCmd.ExecuteScalar());
                TransactionId = Trx_id;
                CommonFuncs CommonFuncs = new CommonFuncs(Utilities);

                log.LogVariableState("@user_id", Utilities.ParafaitEnv.User_Id);
                log.LogVariableState("@payment_mode", 1);
                log.LogVariableState("@PaymentReference", "Refund");


                object o;
                //29-Feb-2016:: Added creditTrxNo if different series is required to be generated
                string Trx_No = "";
                if (Utilities.ParafaitEnv.USE_ORIGINAL_TRXNO_FOR_REFUND == "Y")
                {
                    RefundCmd.CommandText = @"select trx_no 
                                                    from trx_header 
                                                    where trxid = (select min(trxid)
                                                                    from trx_lines l
                                                                    where l.card_id = @card_id)";
                    RefundCmd.Parameters.AddWithValue("@card_id", refundCardList[0].card_id);

                    o = RefundCmd.ExecuteScalar();

                    if (o != null)
                        Trx_No = o.ToString();
                }

                if (string.IsNullOrEmpty(Trx_No))
                {
                    List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
                    List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();
                    InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(Utilities.ExecutionContext);
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "1"));
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE, "CREDIT"));
                    invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
                    if (invoiceSequenceMappingDTOList != null)
                    {
                        var newinvoiceSequenceMappingDTOList = invoiceSequenceMappingDTOList.OrderByDescending(x => x.EffectiveDate).ToList();
                        InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(Utilities.ExecutionContext, newinvoiceSequenceMappingDTOList[0].InvoiceSequenceSetupId, SQLTrx);
                        try
                        {
                            Trx_No = invoiceSequenceSetupBL.GetSequenceNumber(SQLTrx);
                        }
                        catch (SeriesExpiredException ex)
                        {
                            log.Error("Unable to get the value for Trx_No - SeriesExpiredException", ex);
                            message = Utilities.MessageUtils.getMessage(1333);

                            log.LogVariableState("Trx_No", Trx_No);
                            log.LogVariableState("message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        catch (SeriesEndNumberExceededException ex)
                        {
                            log.Error("Unable to get the value for Trx_No - SeriesEndNumberExceededException", ex);
                            message = Utilities.MessageUtils.getMessage(1334);

                            log.LogVariableState("Trx_No", Trx_No);
                            log.LogVariableState("message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                if (string.IsNullOrEmpty(Trx_No))
                {
                    Trx_No = CommonFuncs.getNextCreditTrxNo(Utilities.ParafaitEnv.POSMachineId, orderTypeGroupId, SQLTrx);
                    log.LogVariableState("Refund Card Trx_No", Trx_No);
                }
                if (string.IsNullOrEmpty(Trx_No))
                {

                    Trx_No = CommonFuncs.getNextTrxNo(Utilities.ParafaitEnv.POSMachineId, orderTypeGroupId, SQLTrx);
                    log.LogVariableState("Trx_No", Trx_No);
                }

                RefundCmd.CommandText = "update trx_header set trx_no = @trx_no where trxId = @trxId";
                RefundCmd.Parameters.AddWithValue("trxId", Trx_id);
                RefundCmd.Parameters.AddWithValue("trx_no", Trx_No);
                RefundCmd.ExecuteNonQuery();

                log.LogVariableState("trxId", Trx_id);
                log.LogVariableState("trx_no", Trx_No);

                CreditPlus creditPlus = new CreditPlus(Utilities);
                double lineInclusiveTaxAmount = 0;//Added the parameter to calculate Tax on 25-Sept-2015
                bool lineRefundTaxInclusive = false;
                double lineRefundAmount = 0;
                double lineRefundWithTax = 0;
                int j = 1;
                for (int i = 0; i < refundCardList.Count; i++) //foreach (var cl in refundCardList)
                {
                    if (refundCardList.Count > 1)
                    {
                        inCardDeposit = refundCardList[i].face_value;
                        inCredits = refundCardList[i].credits;
                        inCreditPlus = creditPlus.getCreditPlusRefund(refundCardList[i].card_id, SQLTrx);
                    }
                    RefundCmd.CommandText = @"insert into trx_lines ( 
                                                LineId, 
                                                TrxId, 
                                                product_id, 
                                                price, 
                                                quantity, 
                                                amount, 
                                                credits, 
                                                card_number, 
                                                card_id, tax_id, tax_percentage) 
                                         (select top 1 
                                                @LineId, 
                                                @TrxId, 
                                                product_id, 
                                                @price, 
                                                @quantity, 
                                                @amount, 
                                                @credits, 
                                                @card_number, 
                                                @card_id, tax_id, @tax_percentage 
                                                from products p, product_type pt 
                                                where p.product_type_id = pt.product_type_id 
                                                and pt.product_type = @productType)";

                    double refundWithoutDeposit = inCredits + inCardDeposit + inCreditPlus; // Total refund for particular card // refundAmount;
                    lineInclusiveTaxAmount = CalculateRefundAmount(inCardDeposit, refundWithoutDeposit, inCredits, inCreditPlus, ref lineRefundTaxInclusive);

                    lineRefundAmount = refundWithoutDeposit;
                    //Begin: Added the below condition to check if tax is inclusive or exclusive of the product price on 25-sept-2015//
                    if (refundGamesAmount > 0)
                    {
                        lineRefundAmount = refundGamesAmount;
                        refundWithoutDeposit = refundGamesAmount;
                        lineRefundWithTax = refundGamesAmount * -1;
                    }
                    else
                    {
                        if (lineRefundTaxInclusive)
                        {
                            lineRefundWithTax = lineRefundAmount * -1;
                        }
                        else
                        {
                            lineRefundWithTax = (lineRefundAmount + lineInclusiveTaxAmount) * -1;
                        }
                    }
                    double refundWithoutDepositWithTax = lineRefundWithTax;
                    double refundWithoutDepositWithoutTax = 0;

                    if (inCardDeposit > 0)
                    {
                        refundWithoutDeposit -= refundCardList[i].face_value;
                        if (lineRefundTaxInclusive) //Added for refund tax calculation 25-Sep-2015
                        {
                            refundWithoutDepositWithTax = refundWithoutDeposit * -1;
                            refundWithoutDepositWithoutTax = refundWithoutDeposit / (1 + Utilities.ParafaitEnv.RefundCardTaxPercent / 100);//Added to exclude Tax for price columnn in Trx_lines on Dec-14-2015
                        }

                        else
                        {
                            refundWithoutDepositWithTax = refundWithoutDeposit * -1 * (1 + Utilities.ParafaitEnv.RefundCardTaxPercent / 100);
                            refundWithoutDepositWithoutTax = refundWithoutDeposit;//Added to exclude Tax for price columnn in Trx_lines on Dec-14-2015
                        }
                    }
                    else
                    {
                        refundWithoutDepositWithoutTax = (refundWithoutDeposit) / (1 + Utilities.ParafaitEnv.RefundCardTaxPercent / 100);
                    }

                    if (refundWithoutDeposit >= 0)
                    {
                        RefundCmd.Parameters.Clear();
                        RefundCmd.Parameters.AddWithValue("@LineId", j++);
                        RefundCmd.Parameters.AddWithValue("@TrxId", Trx_id);
                        RefundCmd.Parameters.AddWithValue("@price", refundWithoutDepositWithoutTax * -1);
                        RefundCmd.Parameters.AddWithValue("@quantity", 1);

                        //log.LogVariableState("@LineId", j++);
                        log.LogVariableState("@TrxId", Trx_id);
                        log.LogVariableState("@price", refundWithoutDepositWithoutTax * -1);
                        log.LogVariableState("@quantity", 1);

                        if (Utilities.ParafaitEnv.RefundCardTaxId == -1)
                        {
                            RefundCmd.Parameters.AddWithValue("@tax_percentage", DBNull.Value);
                            log.LogVariableState("@tax_percentage", DBNull.Value);
                        }
                        else
                        {
                            RefundCmd.Parameters.AddWithValue("@tax_percentage", Utilities.ParafaitEnv.RefundCardTaxPercent);
                            log.LogVariableState("@tax_percentage", Utilities.ParafaitEnv.RefundCardTaxPercent);
                        }
                        RefundCmd.Parameters.AddWithValue("@amount", refundWithoutDepositWithTax);
                        RefundCmd.Parameters.AddWithValue("@credits", (inCredits + inCreditPlus) * -1);
                        RefundCmd.Parameters.AddWithValue("@card_number", refundCardList[i].CardNumber);
                        RefundCmd.Parameters.AddWithValue("@card_id", refundCardList[i].card_id);
                        RefundCmd.Parameters.AddWithValue("@productType", "REFUND");

                        log.LogVariableState("@amount", refundWithoutDepositWithTax);
                        log.LogVariableState("@credits", (inCredits + inCreditPlus) * -1);
                        log.LogVariableState("@card_number", refundCardList[i].CardNumber);
                        log.LogVariableState("@card_id", refundCardList[i].card_id);
                        log.LogVariableState("@productType", "REFUND");

                        if (RefundCmd.ExecuteNonQuery() < 1)
                        {
                            message = Utilities.MessageUtils.getMessage(359);

                            log.LogVariableState("message", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }

                    if (inCardDeposit > 0)
                    {
                        //01-Mar-2016: Begin Modification to handle deposit tax
                        double refundDepositWithTax = 0;
                        double refundDepositWithoutTax = 0;
                        if (refundCardDepositTaxInclusive)
                        {
                            refundDepositWithTax = inCardDeposit;
                            refundDepositWithoutTax = inCardDeposit / (1 + refundCardDepositTaxPercentage / 100);
                        }
                        else
                        {
                            refundDepositWithTax = inCardDeposit * (1 + refundCardDepositTaxPercentage / 100);
                            refundDepositWithoutTax = inCardDeposit;
                        }
                        //01-Mar-2016: End Modification to handle deposit tax                    
                        RefundCmd.Parameters.Clear();
                        RefundCmd.Parameters.AddWithValue("@LineId", j++);
                        RefundCmd.Parameters.AddWithValue("@TrxId", Trx_id);
                        double refund_value = refundDepositWithoutTax * -1;
                        RefundCmd.Parameters.AddWithValue("@price", refund_value); //Modified to handle deposit tax
                        RefundCmd.Parameters.AddWithValue("@quantity", 1);

                        log.LogVariableState("@LineId", j);
                        log.LogVariableState("@TrxId", Trx_id);
                        log.LogVariableState("@price", refund_value);
                        log.LogVariableState("@quantity", 1);

                        if (refundCardDepositTaxId == -1)
                        {
                            RefundCmd.Parameters.AddWithValue("@tax_percentage", DBNull.Value);
                            log.LogVariableState("@tax_percentage", DBNull.Value);
                        }
                        else
                        {
                            RefundCmd.Parameters.AddWithValue("@tax_percentage", refundCardDepositTaxPercentage);
                            log.LogVariableState("@tax_percentage", refundCardDepositTaxPercentage);
                        }
                        double refund_deposit_amount = -1 * refundDepositWithTax;
                        RefundCmd.Parameters.AddWithValue("@amount", refund_deposit_amount); //Modified to handle deposit tax
                        RefundCmd.Parameters.AddWithValue("@credits", 0);
                        RefundCmd.Parameters.AddWithValue("@card_number", refundCardList[i].CardNumber);
                        RefundCmd.Parameters.AddWithValue("@card_id", refundCardList[i].card_id);
                        RefundCmd.Parameters.AddWithValue("@productType", "REFUNDCARDDEPOSIT");

                        log.LogVariableState("@amount", refund_deposit_amount);
                        log.LogVariableState("@credits", 0);
                        log.LogVariableState("@card_number", refundCardList[i].CardNumber);
                        log.LogVariableState("@card_id", refundCardList[i].card_id);
                        log.LogVariableState("@productType", "REFUNDCARDDEPOSIT");

                        if (RefundCmd.ExecuteNonQuery() < 1)
                        {
                            message = Utilities.MessageUtils.getMessage(359);

                            log.LogVariableState("message", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }

                    if (inCreditPlus > 0)
                    {
                        CreditPlus cp = new CreditPlus(Utilities);
                        cp.refundCreditPlus((int)Trx_id, (int)refundCardList[i].card_id, inCreditPlus, SQLTrx, Utilities.ParafaitEnv.LoginID);
                    }

                }
                if (refundAmount > 0)
                {
                    PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if (paymentModeDTOList != null)
                    {
                        TransactionPaymentsDTO cashTrxPaymentDTO = new TransactionPaymentsDTO();
                        cashTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                        cashTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        cashTrxPaymentDTO.Amount = refundWithTax;
                        cashTrxPaymentDTO.CurrencyCode = string.Empty;
                        cashTrxPaymentDTO.CurrencyRate = null;
                        cashTrxPaymentDTO.TenderedAmount = refundWithTax;
                        cashTrxPaymentDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                        cashTrxPaymentDTO.TransactionId = Trx_id;
                        TransactionPaymentsBL cashTrxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, cashTrxPaymentDTO);
                        cashTrxPaymentBL.Save(SQLTrx);
                    }
                }
                //TransactionUtils.CreateCashTrxPayment((int)Trx_id, refundWithTax, refundWithTax, SQLTrx);

                log.LogVariableState("message", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating refund transaction", ex);
                message = ex.Message;

                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        void CreateLoadTicketTransaction(Card card, int tickets, string remarks, int redemptionId, SqlTransaction sqlTrx, bool? considerForLoyalty = null)
        {
            log.LogMethodEntry(card, tickets, remarks, redemptionId, sqlTrx);
            try
            {
                SqlCommand loadTicketCmd = Utilities.getCommand(sqlTrx);
                DateTime taskdate = ServerDateTime.Now;
                if (redemptionId == -1)
                {
                    redemptionId = CreateLoadTicketRedemptionOrder(card, tickets, sqlTrx);
                }
                if (redemptionId > -1)
                {
                    DataTable dtRedemptionTicketAllocation = Utilities.executeDataTable(@"select rta.id, rta.RedemptionId, rta.RedemptionGiftId, rta.ManualTickets, rta.GraceTickets, rta.CardId, rta.ETickets, rta.CurrencyQuantity, rta.CurrencyTickets,
                                                                                                 rta.ReceiptTickets, rta.TurnInTickets, rta.ManualTicketReceiptId, rta.TrxId, rta.TrxLineId,
                                                                                                 mtr.IssueDate,  mtr.ManualTicketReceiptNo, rta.RedemptionCurrencyRuleId, rta.RedemptionCurrencyRuleTicket, rta.SourceCurrencyRuleId 
                                                                                            from RedemptionTicketAllocation rta left outer join ManualTicketReceipts mtr on rta.ManualTicketReceiptId = mtr.id
                                                                                           where RedemptionId = @redemptionId
                                                                                             and RedemptionGiftId is null  order by
																							 case
																							 WHEN ISNULL(manualtickets,0) <> 0 then 0
																							 WHEN ISNULL(ETickets,0)>0 then 1
																							 WHEN ISNULL(CurrencyTickets,0)>0 then 2
																							 WHEN ISNULL(GraceTickets,0)>0 then 3
																							 WHEN ISNULL(TurnInTickets,0)>0 then 4
																							 WHEN ISNULL(ReceiptTickets,0)>0 then 5
																							 END ASC, mtr.IssueDate DESC ", sqlTrx, new SqlParameter("@redemptionId", redemptionId));

                    loadTicketCmd.CommandText = @"Insert into trx_header (
                                           TrxDate, TrxAmount, TrxDiscountPercentage, payment_mode, PaymentReference, status,
                                           TrxNetAmount, posMachineId, pos_machine, POSTypeId, user_id, cashAmount, CreditCardAmount, GameCardAmount, OtherPaymentModeAmount, taxAmount, PrimaryCardId, CreatedBy, CreationDate, Remarks, LastUpdatedBy, LastUpdateTime, customerId, ReprintCount, OrderTypeGroupId, site_id) 
                                           values (
                                           getdate(), 0,0, null, null, 'CLOSED',
                                           0, @PosMachineId, @PosMachine, @POSTypeId, @UserId, 0, 0, 0, 0, 0, @PrimaryCardId,  @UserId, getdate(), 'LOADTICKETS',  @CreatedBy, getdate(), @CustomerId, 0, @OrderTypeGroupId, @SiteId); select @@identity;";

                    loadTicketCmd.Parameters.AddWithValue("@PosMachine", Utilities.ParafaitEnv.POSMachine);
                    loadTicketCmd.Parameters.AddWithValue("@PrimaryCardId", card.card_id);
                    log.LogVariableState("@PosMachine", Utilities.ParafaitEnv.POSMachine);
                    log.LogVariableState("@PrimaryCardId", card.card_id);

                    loadTicketCmd.Parameters.AddWithValue("@OrderTypeGroupId", DBNull.Value);
                    log.LogVariableState("@OrderTypeGroupId", DBNull.Value);

                    if (card.customer_id == -1)
                    {
                        loadTicketCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                        log.LogVariableState("@CustomerId", DBNull.Value);
                    }
                    else
                    {
                        loadTicketCmd.Parameters.AddWithValue("@CustomerId", card.customer_id);
                        log.LogVariableState("@CustomerId", card.customer_id);
                    }

                    if (Utilities.ParafaitEnv.POSMachineId == -1)
                    {
                        loadTicketCmd.Parameters.AddWithValue("@PosMachineId", DBNull.Value);
                        log.LogVariableState("@PosMachineId", DBNull.Value);
                    }
                    else
                    {
                        loadTicketCmd.Parameters.AddWithValue("@PosMachineId", Utilities.ParafaitEnv.POSMachineId);
                        log.LogVariableState("@PosMachineId", Utilities.ParafaitEnv.POSMachineId);
                    }

                    if (Utilities.ParafaitEnv.POSTypeId == -1)
                    {
                        loadTicketCmd.Parameters.AddWithValue("@POSTypeId", DBNull.Value);
                        log.LogVariableState("@POSTypeId", DBNull.Value);
                    }
                    else
                    {
                        loadTicketCmd.Parameters.AddWithValue("@POSTypeId", Utilities.ParafaitEnv.POSTypeId);
                        log.LogVariableState("@POSTypeId", Utilities.ParafaitEnv.POSTypeId);
                    }

                    loadTicketCmd.Parameters.AddWithValue("@UserId", Utilities.ParafaitEnv.User_Id);
                    log.LogVariableState("@UserId", Utilities.ParafaitEnv.User_Id);

                    loadTicketCmd.Parameters.AddWithValue("@CreatedBy", Utilities.ParafaitEnv.LoginID);
                    log.LogVariableState("@CreatedBy", Utilities.ParafaitEnv.LoginID);

                    if (!Utilities.ParafaitEnv.IsCorporate)
                    {
                        loadTicketCmd.Parameters.AddWithValue("@SiteId", DBNull.Value);
                        log.LogVariableState("@SiteId", DBNull.Value);
                    }
                    else
                    {
                        loadTicketCmd.Parameters.AddWithValue("@SiteId", Utilities.ParafaitEnv.SiteId);
                        log.LogVariableState("@SiteId", Utilities.ParafaitEnv.SiteId);
                    }

                    int trxId = Convert.ToInt32(loadTicketCmd.ExecuteScalar());
                    TransactionId = trxId;
                    if (considerForLoyalty.HasValue == true)
                    {
                        TrxUserLogsBL trxUserLogs = new TrxUserLogsBL(TransactionId, -1, Utilities.ParafaitEnv.LoginID, Utilities.getServerTime(), Utilities.ParafaitEnv.POSMachineId, "CREATE_TRANSACTION", "Consider For Loyalty: " + (considerForLoyalty.Value == true? "Yes" : "No"), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                        trxUserLogs.Save(sqlTrx);
                    }
                    CommonFuncs CommonFuncs = new CommonFuncs(Utilities);
                    int OrderTypeGroupId = -1;
                    string trxNo = "";
                    trxNo = CommonFuncs.getNextTrxNo(Utilities.ParafaitEnv.POSMachineId, OrderTypeGroupId, loadTicketCmd.Transaction);
                    log.LogVariableState("Trx_No with getNextTrxNo", trxNo);

                    loadTicketCmd.CommandText = "update trx_header set trx_no = @Trx_no where trxId = @TrxId";
                    loadTicketCmd.Parameters.AddWithValue("@TrxId", trxId);
                    loadTicketCmd.Parameters.AddWithValue("@Trx_no", trxNo);
                    loadTicketCmd.ExecuteNonQuery();

                    log.LogVariableState("@trxId", trxId);
                    log.LogVariableState("@trx_no", trxNo);
                    int lineId = 1;
                    bool loyaltyexists = false;

                    int GAMEPLAY_TICKETS_EXPIRY_DAYS;
                    try
                    {
                        GAMEPLAY_TICKETS_EXPIRY_DAYS = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("GAMEPLAY_TICKETS_EXPIRY_DAYS")));
                    }
                    catch (Exception ex)
                    {
                        log.Error("GAMEPLAY_TICKETS_EXPIRY_DAYS doesn't have a valid value!", ex);
                        GAMEPLAY_TICKETS_EXPIRY_DAYS = 0;
                        log.LogVariableState("GAMEPLAY_TICKETS_EXPIRY_DAYS ", GAMEPLAY_TICKETS_EXPIRY_DAYS);
                    }
                    string AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = "";
                    if (GAMEPLAY_TICKETS_EXPIRY_DAYS > 0)
                    {
                        AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD = Utilities.getParafaitDefaults("AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD");
                    }
                    if (dtRedemptionTicketAllocation.Rows.Count > 0)
                    {
                        try
                        {
                            if (dtRedemptionTicketAllocation.Compute("Sum(ManualTickets)", "") != DBNull.Value)
                            {
                                int totalManualTickets = Convert.ToInt32(dtRedemptionTicketAllocation.Compute("Sum(ManualTickets)", ""));
                                // ValidateManualTicketLimit(totalManualTickets, redemptionId, sqlTrx);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new Exception(ex.Message);
                        }
                        Loyalty Loyalty = new Loyalty(Utilities);
                        List<Tuple<int, int, int>> allocationTrxData = new List<Tuple<int, int, int>>();
                        List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();
                        List<AccountCreditPlusDTO> accountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                        int ticketReceiptCutOffDays = 0;
                        List<LookupValuesContainerDTO> ticketReceiptCutOffList = LookupsContainerList.GetLookupsContainerDTO(Utilities.ParafaitEnv.ExecutionContext.GetSiteId(), "LOADTICKET_CUTOFF_DAYS").LookupValuesContainerDTOList;
                        if (ticketReceiptCutOffList != null && ticketReceiptCutOffList.Any(x => x.LookupValue == "CUTOFF_DAYS"))
                        {
                            try
                            {
                                ticketReceiptCutOffDays = Convert.ToInt32(ticketReceiptCutOffList.FirstOrDefault(x => x.LookupValue == "CUTOFF_DAYS").Description);
                            }
                            catch
                            {
                                ticketReceiptCutOffDays = 0;
                            }
                        }
                        foreach (DataRow redemptionTicketAllocationRow in dtRedemptionTicketAllocation.Rows)
                        {
                            //lineId = lineId + 1;
                            int linetickets = 0;
                            string lineSource = "";
                            string manualReceiptNo = "";
                            DateTime? lineManualReceiptIssueDate = null;
                            if (redemptionTicketAllocationRow["ManualTickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["ManualTickets"]);
                                lineSource = "ManualTickets";
                            }
                            else if (redemptionTicketAllocationRow["GraceTickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["GraceTickets"]);
                                lineSource = "GraceTickets";
                            }
                            else if (redemptionTicketAllocationRow["ETickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["ETickets"]);
                                lineSource = "Card";
                            }
                            else if (redemptionTicketAllocationRow["CurrencyTickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["CurrencyTickets"]);
                                lineSource = "RdemptionCurrency";
                            }
                            else if (redemptionTicketAllocationRow["ReceiptTickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["ReceiptTickets"]);
                                lineSource = "ManualReceipt";
                                if (redemptionTicketAllocationRow["IssueDate"] != DBNull.Value)
                                    lineManualReceiptIssueDate = Convert.ToDateTime(redemptionTicketAllocationRow["IssueDate"]);

                                manualReceiptNo = redemptionTicketAllocationRow["ManualTicketReceiptNo"].ToString();
                            }
                            else if (redemptionTicketAllocationRow["TurnInTickets"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["TurnInTickets"]);
                                lineSource = "TurnIn";
                            }
                            else if (redemptionTicketAllocationRow["RedemptionCurrencyRuleTicket"] != DBNull.Value)
                            {
                                linetickets = Convert.ToInt32(redemptionTicketAllocationRow["RedemptionCurrencyRuleTicket"]);
                                lineSource = "RedemptionCurrencyRule";
                            }
                            // batch insert
                            //CreateLoadTicketTransactionLine(trxId, lineId, card, linetickets, lineSource, lineManualReceiptIssueDate, manualReceiptNo, sqlTrx);
                            bool withLoyaltyProduct = true;
                            if (lineManualReceiptIssueDate != null && lineSource == "ManualReceipt")
                            {
                                if (lineManualReceiptIssueDate > Utilities.getServerTime().AddDays(-ticketReceiptCutOffDays))
                                {
                                    withLoyaltyProduct = true;
                                }
                                else
                                {
                                    withLoyaltyProduct = false;
                                }
                            }
                            TransactionLineDTO transactionLineDTO = new TransactionLineDTO();
                            //transactionLineDTO.LineId = lineId;
                            transactionLineDTO.TransactionId = trxId;
                            transactionLineDTO.CardNumber = card.CardNumber;
                            transactionLineDTO.CardId = card.card_id;
                            transactionLineDTO.Tickets = linetickets;
                            //if (lineSource == "ManualReceipt") 
                            //{
                            //    transactionLineDTO.Remarks= "LOADTICKET " + manualReceiptNo;
                            //    log.LogVariableState("@remarks", "LOADTICKET " + manualReceiptNo);
                            //}
                            //else
                            //{
                            transactionLineDTO.Remarks = "LOADTICKET ";
                            //}
                            if (withLoyaltyProduct && (considerForLoyalty.HasValue == false || (considerForLoyalty.HasValue == true && considerForLoyalty.Value == true)))
                            {
                                transactionLineDTO.LineId = lineId;
                                loyaltyexists = true;
                                transactionLineDTO.ProductId = ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), "LOADTICKETS", "LOADTICKETS").ProductId;
                            }
                            else
                            {
                                if (loyaltyexists)
                                {
                                    lineId = 2;
                                }
                                transactionLineDTO.LineId = lineId;
                                transactionLineDTO.ProductId = ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), "LOADTICKETS", "LOADTICKETS_NOLOYALTY").ProductId;
                            }
                            transactionLineDTO.Price = 0;
                            transactionLineDTO.Quantity = 1;
                            transactionLineDTO.Amount = 0;
                            transactionLineDTO.Credits = 0;
                            transactionLineDTOList.Add(transactionLineDTO);
                            allocationTrxData.Add(Tuple.Create(Convert.ToInt32(redemptionTicketAllocationRow["id"]), trxId, lineId));
                            //UpdateRedemptionTicketAllocation(Convert.ToInt32(redemptionTicketAllocationRow["id"]), trxId, lineId, sqlTrx);
                        }
                        List<TransactionLineDTO> savetransactionLineDTOList = new List<TransactionLineDTO>();
                        if (transactionLineDTOList != null && transactionLineDTOList.Any())
                        {

                            if (transactionLineDTOList.Any(x => x.LineId == 1))
                            {
                                savetransactionLineDTOList.Add(transactionLineDTOList.FirstOrDefault(x => x.LineId == 1));
                                savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).Tickets = transactionLineDTOList.Where(x => x.LineId == 1).Sum(y => y.Tickets);
                                savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).TaxId = ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).ProductId).TaxId;
                                if (savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).TaxId >= 0)
                                {
                                    Tax tax = new Tax(Utilities.ParafaitEnv.ExecutionContext, savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).TaxId);
                                    savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 1).TaxPercentage = (decimal)tax.GetTaxDTO().TaxPercentage;
                                }
                            }
                            if (transactionLineDTOList.Any(x => x.LineId == 2))
                            {
                                savetransactionLineDTOList.Add(transactionLineDTOList.FirstOrDefault(x => x.LineId == 2));
                                savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).Tickets = transactionLineDTOList.Where(x => x.LineId == 2).Sum(y => y.Tickets);
                                savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).TaxId = ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).ProductId).TaxId;
                                if (savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).TaxId >= 0)
                                {
                                    Tax tax = new Tax(Utilities.ParafaitEnv.ExecutionContext, savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).TaxId);
                                    savetransactionLineDTOList.FirstOrDefault(x => x.LineId == 2).TaxPercentage = (decimal)tax.GetTaxDTO().TaxPercentage;
                                }
                            }
                            TransactionLineListBL transactionLineListBL = new TransactionLineListBL(Utilities.ExecutionContext, savetransactionLineDTOList);
                            transactionLineListBL.Save(sqlTrx);
                        }
                        AccountDTO.AccountValidityStatus accountValidityStatus = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE") == "Y" ? AccountDTO.AccountValidityStatus.Hold : AccountDTO.AccountValidityStatus.Valid;
                        if (savetransactionLineDTOList != null && savetransactionLineDTOList.Any())
                        {
                            foreach (TransactionLineDTO trxLine in savetransactionLineDTOList)
                            {
                                if (GAMEPLAY_TICKETS_EXPIRY_DAYS > 0)
                                {
                                    DateTime? periodto = DateTime.Parse(ServerDateTime.Now.AddDays(GAMEPLAY_TICKETS_EXPIRY_DAYS).ToString("d") + " 06:00 AM");
                                    AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, trxLine.Tickets, CreditPlusType.TICKET, false,
                    "Load Tickets", card.card_id, trxId, lineId, trxLine.Tickets, null, periodto, null, null, null, true, true, true, true, true, true, true, null, -1,
                    AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD == "Y" ? true : false, null, ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), trxLine.ProductId).TicketAllowed == "Y" ? true : false, false, false, -1, -1, ServerDateTime.Now, Utilities.ExecutionContext.GetUserId(),
                    ServerDateTime.Now, Utilities.ExecutionContext.GetSiteId(), -1, false, "", false, true, "", -1, accountValidityStatus, -1);
                                    accountCreditPlusDTOList.Add(accountCreditPlusDTO);
                                    //Loyalty.CreateGenericCreditPlusLine(card.card_id, "T", linetickets, false, GAMEPLAY_TICKETS_EXPIRY_DAYS, AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD, Utilities.ParafaitEnv.LoginID, "Load Tickets", sqlTrx, null, trxId, lineId);
                                }
                                else
                                {
                                    if (trxLine.Tickets < 0)
                                    {
                                        AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true, sqlTrx);
                                        if (accountBL.AccountDTO != null)
                                        {
                                            if (accountBL.AccountDTO.AccountCreditPlusDTOList != null && accountBL.AccountDTO.AccountCreditPlusDTOList.Any(cp => cp.CreditPlusType.Equals(CreditPlusType.TICKET)))
                                            {
                                                List<AccountCreditPlusDTO> negativeAccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                                                decimal ticketsToReduce = (decimal)(-1 * trxLine.Tickets);
                                                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                                                DateTime currentTime = lookupValuesList.GetServerDateTime();
                                                List<AccountCreditPlusDTO> ticketCreditPlusDTOs = accountBL.AccountDTO.AccountCreditPlusDTOList.Where(cp => cp.CreditPlusType.Equals(CreditPlusType.TICKET) &&
                                                                                                                                                            !cp.ValidityStatus.Equals(AccountDTO.AccountValidityStatus.Hold) &&
                                                                                                                                                            (cp.PeriodFrom == null || (cp.PeriodFrom != null && cp.PeriodFrom <= currentTime)) &&
                                                                                                                                                            (cp.PeriodTo == null || (cp.PeriodTo != null && cp.PeriodTo >= currentTime))).ToList();
                                                ticketCreditPlusDTOs = ticketCreditPlusDTOs.OrderBy(x => x.PeriodTo == null ? DateTime.MaxValue : x.PeriodTo).ToList();
                                                foreach (AccountCreditPlusDTO accountCreditPlusDTO in ticketCreditPlusDTOs)
                                                {
                                                    if (ticketsToReduce > 0 && accountCreditPlusDTO.CreditPlusBalance > 0)
                                                    {
                                                        if (ticketsToReduce <= accountCreditPlusDTO.CreditPlusBalance)
                                                        {
                                                            accountCreditPlusDTO.CreditPlusBalance = accountCreditPlusDTO.CreditPlusBalance - ticketsToReduce;
                                                            negativeAccountCreditPlusDTOList.Add(new AccountCreditPlusDTO(-1, -1 * ticketsToReduce, accountCreditPlusDTO.CreditPlusType, accountCreditPlusDTO.Refundable, (string.IsNullOrEmpty(remarks.Trim()) ? "Load Tickets" : remarks), accountCreditPlusDTO.AccountId, trxId, 1, 0,
                                                            accountCreditPlusDTO.PeriodFrom, currentTime, accountCreditPlusDTO.TimeFrom, accountCreditPlusDTO.TimeTo, accountCreditPlusDTO.NumberOfDays, accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday, accountCreditPlusDTO.Wednesday, accountCreditPlusDTO.Thursday, accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday, accountCreditPlusDTO.Sunday, accountCreditPlusDTO.MinimumSaleAmount,
                                                            accountCreditPlusDTO.LoyaltyRuleId, accountCreditPlusDTO.ExtendOnReload, accountCreditPlusDTO.PlayStartTime, accountCreditPlusDTO.TicketAllowed, accountCreditPlusDTO.ForMembershipOnly,
                                                            accountCreditPlusDTO.ExpireWithMembership, accountCreditPlusDTO.MembershipId, accountCreditPlusDTO.MembershipRewardsId, accountCreditPlusDTO.PauseAllowed,
                                                            accountCreditPlusDTO.IsActive, accountCreditPlusDTO.AccountCreditPlusId, accountCreditPlusDTO.ValidityStatus, accountCreditPlusDTO.SubscriptionBillingScheduleId));
                                                            ticketsToReduce = 0;
                                                        }
                                                        else
                                                        {
                                                            ticketsToReduce = (decimal)(ticketsToReduce - accountCreditPlusDTO.CreditPlusBalance);
                                                            negativeAccountCreditPlusDTOList.Add(new AccountCreditPlusDTO(-1, -1 * accountCreditPlusDTO.CreditPlusBalance, accountCreditPlusDTO.CreditPlusType, accountCreditPlusDTO.Refundable, (string.IsNullOrEmpty(remarks.Trim()) ? "Load Tickets" : remarks), accountCreditPlusDTO.AccountId, trxId, 1, 0,
                                                            accountCreditPlusDTO.PeriodFrom, currentTime, accountCreditPlusDTO.TimeFrom, accountCreditPlusDTO.TimeTo, accountCreditPlusDTO.NumberOfDays, accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday, accountCreditPlusDTO.Wednesday, accountCreditPlusDTO.Thursday, accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday, accountCreditPlusDTO.Sunday, accountCreditPlusDTO.MinimumSaleAmount,
                                                            accountCreditPlusDTO.LoyaltyRuleId, accountCreditPlusDTO.ExtendOnReload, accountCreditPlusDTO.PlayStartTime, accountCreditPlusDTO.TicketAllowed, accountCreditPlusDTO.ForMembershipOnly,
                                                            accountCreditPlusDTO.ExpireWithMembership, accountCreditPlusDTO.MembershipId, accountCreditPlusDTO.MembershipRewardsId, accountCreditPlusDTO.PauseAllowed,
                                                            accountCreditPlusDTO.IsActive, accountCreditPlusDTO.AccountCreditPlusId, accountCreditPlusDTO.ValidityStatus, accountCreditPlusDTO.SubscriptionBillingScheduleId));
                                                            accountCreditPlusDTO.CreditPlusBalance = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ticketsToReduce == 0)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                                accountBL.AccountDTO.AccountCreditPlusDTOList.AddRange(negativeAccountCreditPlusDTOList);
                                                accountBL.Save(sqlTrx);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        AccountCreditPlusDTO accountCreditPlusDTO = new AccountCreditPlusDTO(-1, trxLine.Tickets, CreditPlusType.TICKET, false,
                                        "Load Tickets", card.card_id, trxId, lineId, trxLine.Tickets, null, null, null, null, null, true, true, true, true, true, true, true, null, -1,
                                        false, null, ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.GetSiteId(), trxLine.ProductId).TicketAllowed == "Y" ? true : false, false, false, -1, -1, ServerDateTime.Now, Utilities.ExecutionContext.GetUserId(),
                                        ServerDateTime.Now, Utilities.ExecutionContext.GetSiteId(), -1, false, "", false, true, "", -1, accountValidityStatus, -1);
                                        accountCreditPlusDTOList.Add(accountCreditPlusDTO);
                                        //Loyalty.CreateGenericCreditPlusLine(card.card_id, "T", linetickets, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Load Tickets", sqlTrx, null, trxId, lineId);
                                    }
                                }
                            }
                        }
                        if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Any())
                        {
                            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true, sqlTrx);
                            if (accountBL.AccountDTO != null)
                            {
                                if (accountBL.AccountDTO.AccountCreditPlusDTOList == null)
                                    accountBL.AccountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                                accountBL.AccountDTO.AccountCreditPlusDTOList.AddRange(accountCreditPlusDTOList);
                                accountBL.Save(sqlTrx);
                            }
                        }
                        if (allocationTrxData != null && allocationTrxData.Any())
                        {
                            Type redemptionTicketAllocationListBL = Type.GetType("Semnox.Parafait.Redemption.RedemptionTicketAllocationListBL,RedemptionUtils");
                            object redemptionTicketAllocationBLObject = null;
                            if (redemptionTicketAllocationListBL != null)
                            {
                                ConstructorInfo constructorN = redemptionTicketAllocationListBL.GetConstructor(new Type[] { Utilities.ExecutionContext.GetType() });
                                redemptionTicketAllocationBLObject = constructorN.Invoke(new object[] { Utilities.ExecutionContext });
                            }
                            else
                            {
                                throw new Exception("Unable to retrive RedemptionAllocationBL class from assembly");
                            }
                            MethodInfo redemptionTicketAllocationMethodType = redemptionTicketAllocationListBL.GetMethod("SaveTrxDetails", new[] { typeof(int), allocationTrxData.GetType(), sqlTrx.GetType() });
                            redemptionTicketAllocationMethodType.Invoke(redemptionTicketAllocationBLObject, new object[] { redemptionId, allocationTrxData, sqlTrx });
                        }
                        if (AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD == "Y")
                            Loyalty.ExtendOnReload(card.card_id, "T", sqlTrx);
                    }

                    LoyaltyRuleListBL loyaltyRuleListBL = new LoyaltyRuleListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                    searchParameters.Add(new KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>(LoyaltyRuleDTO.SearchByParameters.APPLY_IMMEDIATE, "Y"));
                    List<LoyaltyRuleDTO> loyaltyRuleDTOList = loyaltyRuleListBL.GetLoyaltyRuleDTOList(searchParameters);
                    if (loyaltyRuleDTOList != null && loyaltyRuleDTOList.Count > 0)
                    {
                        Loyalty loyalty = new Loyalty(Utilities);
                        loyalty.LoyaltyOnPurchase(trxId, "Y", sqlTrx);
                    }
                    if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOAD_CARD_ENTITLEMENT_ON_TRANSACTION_COMPLETE").Equals("Y"))
                    {
                        //clear hold flag for created transaction
                        TransactionUtils trxUtils = new TransactionUtils(Utilities);
                        Transaction Trx = trxUtils.CreateTransactionFromDB(trxId, Utilities, false, false, sqlTrx);
                        string message = "";
                        Trx.CompleteTransaction(sqlTrx, ref message);
                    }
                    int taskTypeId = TaskTypesContainerList.GetTaskTypesContainerDTOList(Utilities.ExecutionContext.GetSiteId()).FirstOrDefault(x => x.TaskType == "LOADTICKETS").TaskTypeId;
                    //createTask(card.card_id, TaskProcs.LOADTICKETS, tickets, -1, -1, -1, -1, trxId, -1, remarks, sqlTrx, -1, -1, -1, -1, trxId);
                    TaskDTO taskDTO = new TaskDTO(-1, card.card_id, taskTypeId, tickets, -1, 0, 0,
                               -1, -1, -1, -1, -1, taskdate, Utilities.ParafaitEnv.User_Id, Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : Utilities.ParafaitEnv.POSMachine, remarks,
                               (Utilities.ParafaitEnv.ManagerId == -1 ? Utilities.ParafaitEnv.User_Id : Utilities.ParafaitEnv.ManagerId), trxId, -1,
                               0, 0, 0, tickets, trxId, 0, 0, 0, 0);
                    TaskBL taskBL = new TaskBL(Utilities.ExecutionContext, taskDTO);
                    taskBL.Save(sqlTrx);
                    log.LogMethodExit();
                }
                else
                {
                    throw new Exception("Redemption order for load ticket is not created or passed");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating Load Ticket transaction", ex);
                log.LogMethodExit();
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2597) + ": " + ex.Message);
            }
        }
        void ValidateManualTicketLimit(int manualTicketsCount, int redemptionId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            try
            {
                Type redemptionBLType = Type.GetType("Semnox.Parafait.Redemption.RedemptionBL,RedemptionUtils");
                object redemptionBLObject = null;
                if (redemptionBLType != null)
                {
                    ConstructorInfo constructorN = redemptionBLType.GetConstructor(new Type[] { typeof(int), Utilities.ExecutionContext.GetType(), typeof(SqlTransaction) });
                    redemptionBLObject = constructorN.Invoke(new object[] { redemptionId, Utilities.ExecutionContext, sqlTransaction });
                }
                else
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1479, "RedemptionBL"));

                MethodInfo redemptionBLMethodType = redemptionBLType.GetMethod("ManualTicketLimitChecks", new[] { typeof(bool), typeof(int) });
                object redemptionAllocationDTOList = redemptionBLMethodType.Invoke(redemptionBLObject, new object[] { Utilities.ParafaitEnv.ManagerId == -1 ? false : true, manualTicketsCount });
            }
            catch (TargetInvocationException tie)
            {
                log.Error(tie);
                throw new Exception(tie.InnerException.Message);
            }
            catch (Exception exp)
            {
                log.Error(exp);
                throw new Exception(exp.Message);
            }
            log.LogMethodExit();
        }

        int CreateLoadTicketRedemptionOrder(Card card, int tickets, SqlTransaction sqltrx)
        {
            log.LogMethodEntry(card, tickets, sqltrx);
            string message = "";
            int redemptionId = -1;

            SqlCommand sqlCmd = Utilities.getCommand(sqltrx);
            SqlTransaction cmd_trx = sqlCmd.Transaction;
            try
            {
                CommonFuncs commonFuncs = new CommonFuncs(Utilities);

                sqlCmd.CommandText = @"Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus) 
                                           Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @graceTickets, @currencyTickets, getdate(), @lastUpdatedBy, @source, @redemptionOrderNo, getdate(), getdate(), getdate(), @redemptionStatus); SELECT @@IDENTITY";

                if (card != null)
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", card.card_id);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", card.CardNumber);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", "");
                }

                sqlCmd.Parameters.AddWithValue("@receiptTickets", 0);
                sqlCmd.Parameters.AddWithValue("@currencyTickets", 0);
                sqlCmd.Parameters.AddWithValue("@manual_tickets", tickets);
                sqlCmd.Parameters.AddWithValue("@eTickets", 0);
                sqlCmd.Parameters.AddWithValue("@source", "LoadTickets");
                sqlCmd.Parameters.AddWithValue("@graceTickets", 0);
                sqlCmd.Parameters.AddWithValue("@lastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@createdBy", Utilities.ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@redemptionOrderNo", commonFuncs.GetNextRedemptionOrderNo(Utilities.ParafaitEnv.POSMachineId, sqltrx));
                sqlCmd.Parameters.AddWithValue("@redemptionStatus", "DELIVERED");
                redemptionId = Convert.ToInt32((Decimal)sqlCmd.ExecuteScalar());

                if (card != null)
                {
                    try
                    {
                        sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                                " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@card_no", card.CardNumber);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@card_id", card.card_id);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", 0);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", Utilities.ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        message = Utilities.MessageUtils.getMessage(123, ex.Message);
                        log.Error(ex);
                        throw new Exception(message);
                    }
                }

                sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[ManualTickets],[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate] )
                                               VALUES
                                              (@redemptionId, @manualTickets, @userId, GETDATE(), @userId, getDATE() )";

                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                sqlCmd.Parameters.AddWithValue("@manualTickets", tickets);
                sqlCmd.Parameters.AddWithValue("@userId", Utilities.ParafaitEnv.LoginID);
                sqlCmd.ExecuteNonQuery();

                log.LogMethodExit(redemptionId);
                return redemptionId;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                redemptionId = -1;
                log.LogMethodExit(redemptionId);
                throw new Exception(ex.Message);
            }
        }

        void CreateLoadTicketTransactionLine(int trxId, int lineId, Card card, int tickets, string lineSource, DateTime? lineManualReceiptIssueDate, string manualReceiptNo, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(trxId, lineId, card, tickets, lineSource, lineManualReceiptIssueDate, manualReceiptNo, sqlTrx);
            SqlCommand loadTicketCmd = Utilities.getCommand(sqlTrx);
            int ticketReceiptCutOffDays = 0;
            bool withLoyaltyProduct = true;
            if (lineManualReceiptIssueDate != null && lineSource == "ManualReceipt")
            {
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LOADTICKET_CUTOFF_DAYS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "CUTOFF_DAYS"));
                List<LookupValuesDTO> ticketReceiptCutOff = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (ticketReceiptCutOff != null && ticketReceiptCutOff.Count > 0)
                {
                    try { ticketReceiptCutOffDays = Convert.ToInt32(ticketReceiptCutOff[0].Description); }
                    catch { ticketReceiptCutOffDays = 0; }
                }
                if (lineManualReceiptIssueDate > Utilities.getServerTime().AddDays(-ticketReceiptCutOffDays))
                {
                    withLoyaltyProduct = true;
                }
                else
                {
                    withLoyaltyProduct = false;
                }
            }
            if (withLoyaltyProduct)
            {
                loadTicketCmd.CommandText = @"insert into trx_lines ( 
                                            LineId, 
                                            TrxId, 
                                            product_id, 
                                            price, 
                                            quantity, 
                                            amount, 
                                            credits, 
                                            card_number, 
                                            card_id, 
                                            tax_id, 
                                            tax_percentage,
                                            tickets, site_id, Remarks,  CreatedBy,  CreationDate, LastUpdatedBy, LastUpdateDate
                                            ) 
                                        (select top 1 
                                            @LineId, 
                                            @TrxId, 
                                            p.product_id, 
                                            0, 
                                            1, 
                                            0, 
                                            0, 
                                            @CardNumber, 
                                            @CardId, 
                                            p.tax_id, 
                                            t.tax_percentage,
                                            @Tickets, @SiteId, @remarks, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
                                            from products p left join tax t on t.tax_id = p.tax_id, product_type pt 
                                            where p.product_type_id = pt.product_type_id 
                                            and p.Product_name ='LOADTICKETS'
                                            and pt.product_type = 'LOADTICKETS' )";
            }
            else
            {
                loadTicketCmd.CommandText = @"insert into trx_lines ( 
                                            LineId, 
                                            TrxId, 
                                            product_id, 
                                            price, 
                                            quantity, 
                                            amount, 
                                            credits, 
                                            card_number, 
                                            card_id, 
                                            tax_id, 
                                            tax_percentage,
                                            tickets, site_id, Remarks,  CreatedBy,  CreationDate, LastUpdatedBy, LastUpdateDate
                                            ) 
                                        (select top 1 
                                            @LineId, 
                                            @TrxId, 
                                            p.product_id, 
                                            0, 
                                            1, 
                                            0, 
                                            0, 
                                            @CardNumber, 
                                            @CardId, 
                                            p.tax_id, 
                                            t.tax_percentage,
                                            @Tickets, @SiteId, @remarks, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
                                            from products p left join tax t on t.tax_id = p.tax_id, product_type pt 
                                            where p.product_type_id = pt.product_type_id 
                                            and p.Product_name ='LOADTICKETS_NOLOYALTY'
                                            and pt.product_type = 'LOADTICKETS' )";
            }
            loadTicketCmd.Parameters.Clear();
            loadTicketCmd.Parameters.AddWithValue("@TrxId", trxId);
            loadTicketCmd.Parameters.AddWithValue("@LineId", lineId);
            loadTicketCmd.Parameters.AddWithValue("@CreatedBy", Utilities.ParafaitEnv.LoginID);
            loadTicketCmd.Parameters.AddWithValue("@CardNumber", card.CardNumber);
            loadTicketCmd.Parameters.AddWithValue("@CardId", card.card_id);
            loadTicketCmd.Parameters.AddWithValue("@Tickets", tickets);
            if (lineSource == "ManualReceipt")
            {
                loadTicketCmd.Parameters.AddWithValue("@remarks", "LOADTICKET " + manualReceiptNo);
                log.LogVariableState("@remarks", "LOADTICKET " + manualReceiptNo);
            }
            else
            {
                loadTicketCmd.Parameters.AddWithValue("@remarks", "LOADTICKET ");
                log.LogVariableState("@remarks", "LOADTICKET ");
            }

            if (!Utilities.ParafaitEnv.IsCorporate)
            {
                loadTicketCmd.Parameters.AddWithValue("@SiteId", DBNull.Value);
                log.LogVariableState("@SiteId", DBNull.Value);
            }
            else
            {
                loadTicketCmd.Parameters.AddWithValue("@SiteId", Utilities.ParafaitEnv.SiteId);
                log.LogVariableState("@SiteId", Utilities.ParafaitEnv.SiteId);
            }

            log.LogVariableState("@TrxId", trxId);
            log.LogVariableState("@LineId", lineId);
            log.LogVariableState("@CreatedBy", Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@CardNumber", card.CardNumber);
            log.LogVariableState("@CardId", card.card_id);
            log.LogVariableState("@Tickets", tickets);

            if (loadTicketCmd.ExecuteNonQuery() < 1)
            {
                log.LogVariableState("message", "Error creating transaction lines");
                log.LogMethodExit();
                throw new Exception("Error creating transaction lines");
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// HoldEntitlements
        /// </summary>
        /// <param name="holdCard"></param>
        /// <param name="remarks"></param>
        /// <param name="message"></param>
        /// <param name="hold"></param>
        /// <param name="inSQLTrx"></param>
        /// <returns></returns>
        public bool HoldEntitlements(Card holdCard, string remarks, ref string message, bool hold = true, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(holdCard, remarks, message, hold, inSQLTrx);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, holdCard.card_id, true, true, inSQLTrx);
            if (accountBL.IsAccountUpdatedByOthers(holdCard.last_update_time, inSQLTrx))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }
            AccountDTO accountDTO = accountBL.AccountDTO;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, inSQLTrx);
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
            {
                foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                {
                    if (accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid && hold == true)
                    {
                        accountCreditPlusDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
                    }
                    else if (accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold && hold == false)
                    {
                        //can not release unbilled subscription related entitlements from hold
                        if (false == (accountCreditPlusDTO.SubscriptionBillingScheduleId != -1 && subscriptionBillingScheduleDTOList != null
                                       && subscriptionBillingScheduleDTOList.Any()
                                       && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == accountCreditPlusDTO.SubscriptionBillingScheduleId
                                                                                           && sbs.TransactionId == -1 && sbs.IsActive)))
                        {
                            accountCreditPlusDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
                        }
                    }
                }
            }
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
            {
                foreach (AccountGameDTO accountGameDTO in accountDTO.AccountGameDTOList)
                {
                    if (accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid && hold == true)
                    {
                        accountGameDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
                    }
                    else if (accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold && hold == false)
                    {
                        //can not release unbilled subscription related entitlements from hold
                        if (false == (accountGameDTO.SubscriptionBillingScheduleId != -1 && subscriptionBillingScheduleDTOList != null
                                       && subscriptionBillingScheduleDTOList.Any()
                                       && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == accountGameDTO.SubscriptionBillingScheduleId
                                                                                          && sbs.TransactionId == -1 && sbs.IsActive)))
                        {
                            accountGameDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
                        }
                    }
                }
            }
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
            {
                foreach (AccountDiscountDTO accountDiscountDTO in accountDTO.AccountDiscountDTOList)
                {
                    if (accountDiscountDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Valid && hold == true)
                    {
                        accountDiscountDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
                    }
                    else if (accountDiscountDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold && hold == false)
                    {
                        //can not release unbilled subscription related entitlements from hold
                        if (false == (accountDiscountDTO.SubscriptionBillingScheduleId != -1 && subscriptionBillingScheduleDTOList != null
                                        && subscriptionBillingScheduleDTOList.Any()
                                        && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == accountDiscountDTO.SubscriptionBillingScheduleId
                                                                                           && sbs.TransactionId == -1 && sbs.IsActive)))
                        {
                            accountDiscountDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
                        }
                    }
                }
            }
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
                SQLTrx = Utilities.createConnection().BeginTransaction();
            else
            {
                SQLTrx = inSQLTrx;
            }
            accountBL = new AccountBL(Utilities.ExecutionContext, accountDTO);
            accountBL.Save(SQLTrx);
            createTask(holdCard.card_id, TaskProcs.HOLDENTITLEMENTS, -1, -1, -1, -1, -1, -1, -1, remarks, SQLTrx);
            Card card = new Card(holdCard.card_id, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
            card.updateCardTime(SQLTrx);
            SQLTrx.Commit();
            log.LogMethodExit();
            return true;
        }
        private bool IsTicketAllowedForRedeemVirtualPoints()
        {
            log.LogMethodEntry();
            bool ticketAllowed = false;
            ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetSystemProductContainerDTO(Utilities.ExecutionContext.SiteId, ProductTypeValues.LOYALTY);
            if (productsContainerDTO != null)
            {
                if (!string.IsNullOrWhiteSpace(productsContainerDTO.TicketAllowed) && productsContainerDTO.TicketAllowed == "Y")
                {
                    ticketAllowed = true;
                }
            }
            log.LogMethodExit(ticketAllowed);
            return ticketAllowed;
        }

        /// <summary>
        /// RedeemVirtualLoyaltyPoints
        /// </summary>
        /// <param name="card"></param>
        /// <param name="virtualPoints"></param>
        /// <param name="dgvRedeemVirtualPoint"></param>
        /// <param name="remarks"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool RedeemVirtualPoints(Card card, double virtualPoints, DataGridView dgvRedeemVirtualPoint, string remarks, ref string message)
        {
            log.LogMethodEntry(card, virtualPoints, dgvRedeemVirtualPoint, remarks, message);
            bool result = false;
            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);
                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            AccountDTO accountDTO = accountBL.AccountDTO;
            if ((accountDTO.AccountCreditPlusDTOList != null
                          && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Valid
                                                                            && x.CreditPlusType == CreditPlusType.VIRTUAL_POINT)))  //Ignore subscription holds)
            {
                double mgrApprovalLimit = 0;
                try
                {
                    mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("REDEEM_VIRTUAL_POINT_LIMIT_FOR_MANAGER_APPROVAL"));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                    mgrApprovalLimit = 0;
                    log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                }
                if (mgrApprovalLimit > 0)
                {
                    if (virtualPoints > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                    {
                        message = Utilities.MessageUtils.getMessage(1215);
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                bool found = false;
                string creditPlusType = string.Empty;
                int index = 0;
                for (int i = 0; i < dgvRedeemVirtualPoint.Rows.Count; i++)
                {
                    if (dgvRedeemVirtualPoint["SelectedVirtualPoint", i].Value.ToString() == "Y")
                    {
                        found = true;
                        index = i;
                        break;
                    }
                }
                if (found)
                {
                    string columnName = dgvRedeemVirtualPoint["DBColumnName", index].Value.ToString();
                    if (!columnName.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                    {
                        switch (columnName)
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
                    }
                    double value = 0;
                    try
                    {
                        try
                        {
                            value = Convert.ToDouble(dgvRedeemVirtualPoint["Redemption_Value", index].Value);
                            log.Debug("Redemption_Value : " + value);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to get a value for the variable value", ex);
                            log.LogVariableState("value", value);
                        }

                        if (value <= 0)
                        {
                            message = Utilities.MessageUtils.getMessage(493);
                            log.Debug("message : " + message);
                            log.LogMethodExit(false);
                            return false;
                        }

                        virtualPoints = value / (Convert.ToDouble(dgvRedeemVirtualPoint["Rate", index].Value) / Convert.ToDouble(dgvRedeemVirtualPoint["VirtualLoyaltyPoints", index].Value));
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to get the  value for virtualLoyaltyPoints", ex);
                        message = ex.Message;
                        log.LogVariableState("virtualLoyaltyPoints", virtualPoints);
                        log.Debug("message : " + message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    SqlConnection cnn = Utilities.createConnection();
                    SqlTransaction SQLTrx = cnn.BeginTransaction();
                    SqlCommand cmd = Utilities.getCommand(SQLTrx);

                    try
                    {
                        Loyalty loyalty = new Loyalty(Utilities);
                        bool ticketAllowed = IsTicketAllowedForRedeemVirtualPoints();
                        TransactionId = loyalty.RedeemVirtualPoints((int)card.card_id, card.CardNumber, virtualPoints, columnName, value, Utilities.ParafaitEnv.POSMachine, Utilities.ParafaitEnv.User_Id, SQLTrx, ticketAllowed);
                        createTask(card.card_id, TaskProcs.REDEEMVIRTUALPOINTS, value, -1, -1, -1, -1, creditPlusType[0], -1, remarks, SQLTrx, -1, -1, -1, -1, TransactionId, Convert.ToDecimal(-1 * virtualPoints));
                        SQLTrx.Commit();
                        cnn.Close();
                        log.Debug("message : " + message);
                        log.LogMethodExit(true);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while redeeming loyalty", ex);
                        SQLTrx.Rollback();
                        message = ex.Message;
                        cnn.Close();
                        log.Debug("message : " + message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    log.Debug("message : " + message);
                    log.LogMethodExit(false);
                    result = false;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public bool transferCard(Card fromCard, Card toCard, string remarks, ref string message, SqlTransaction inSQLTrx = null, int sourceTrxId = -1)
        {
            log.LogMethodEntry(fromCard, toCard, remarks, message, inSQLTrx, sourceTrxId);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, fromCard.card_id, true, true);
            if (accountBL.IsAccountUpdatedByOthers(fromCard.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);
                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }
            //AccountDTO accountDTO = accountBL.AccountDTO;
            //List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, inSQLTrx);
            //if ((accountDTO.AccountCreditPlusDTOList != null
            //    && accountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusBalance != 0
            //                                                    && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
            //                                                    && ((x.SubscriptionBillingScheduleId == -1)
            //                                                        || (x.SubscriptionBillingScheduleId != -1
            //                                                                && subscriptionBillingScheduleDTOList != null
            //                                                                && subscriptionBillingScheduleDTOList.Any()
            //                                                                && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
            //                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
            //            )
            //            ||
            //            (accountDTO.AccountGameDTOList != null
            //              && accountDTO.AccountGameDTOList.Exists(x => x.BalanceGames != 0
            //                                                           && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
            //                                                           && ((x.SubscriptionBillingScheduleId == -1)
            //                                                                || (x.SubscriptionBillingScheduleId != -1
            //                                                                  && subscriptionBillingScheduleDTOList != null
            //                                                                  && subscriptionBillingScheduleDTOList.Any()
            //                                                                  && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
            //                                                                                                                  && sbs.TransactionId != -1 && sbs.IsActive))))//Ignore subscription holds
            //            )
            //            ||
            //            (accountDTO.AccountDiscountDTOList != null
            //              && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
            //                                                               && ((x.SubscriptionBillingScheduleId == -1)
            //                                                                    || (x.SubscriptionBillingScheduleId != -1
            //                                                                       && subscriptionBillingScheduleDTOList != null
            //                                                                       && subscriptionBillingScheduleDTOList.Any()
            //                                                                       && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
            //                                                                                                                     && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
            //            )
            //           )
            //{
            //    message = Utilities.MessageUtils.getMessage(2610);

            //    log.LogVariableState("message", message);
            //    log.LogMethodExit(false);
            //    return false;
            //}
            string lockerMode = "";
            string lockerMake = Utilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
            string zoneCode = string.Empty;
            ParafaitLockCardHandler lockerBasic = new ParafaitLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext);
            LockerAllocationDTO lockerAllocationDTO;
            lockerAllocationDTO = lockerBasic.GetLockerAllocationCardDetails(fromCard.card_id);
            if (lockerAllocationDTO != null)
            {
                Locker lockerbl = new Locker(lockerAllocationDTO.LockerId);
                LockerZones lockerZones = null;
                if (lockerbl.getLockerDTO != null && lockerbl.getLockerDTO.LockerId == lockerAllocationDTO.LockerId)
                {
                    LockerPanel lockerPanel = new LockerPanel(toCard.Utilities.ExecutionContext, lockerbl.getLockerDTO.PanelId);
                    if (lockerPanel.getLockerPanelDTO != null && lockerPanel.getLockerPanelDTO.PanelId > -1)
                    {
                        lockerZones = new LockerZones(toCard.Utilities.ExecutionContext, lockerPanel.getLockerPanelDTO.ZoneId);
                        if (lockerZones.GetLockerZonesDTO != null && lockerZones.GetLockerZonesDTO.ZoneId > -1)
                        {
                            lockerMode = lockerZones.GetLockerZonesDTO.LockerMode;
                            zoneCode = lockerZones.GetLockerZonesDTO.ZoneCode;
                            lockerMake = string.IsNullOrEmpty(lockerZones.GetLockerZonesDTO.LockerMake) ? lockerMake : lockerZones.GetLockerZonesDTO.LockerMake;
                        }
                    }
                }
                if (!lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                {
                    if (string.IsNullOrEmpty(lockerMode))
                    {
                        if (!string.IsNullOrEmpty(lockerAllocationDTO.ZoneCode) && lockerAllocationDTO.LockerId == -1)
                        {
                            lockerMode = ParafaitLockCardHandlerDTO.LockerSelectionMode.FREE.ToString();
                        }
                    }
                    if (string.IsNullOrEmpty(lockerMode))
                    {
                        lockerMode = Utilities.getParafaitDefaults("LOCKER_SELECTION_MODE");
                    }
                    bool isFixedMode = (lockerMode.Equals(ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString())) | lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString());

                    ParafaitLockCardHandler locker = null;
                    if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.COCY.ToString()))
                        locker = new CocyLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext, Convert.ToByte(toCard.Utilities.ParafaitEnv.MifareCustomerKey));
                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()))
                        locker = new InnovateLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext, Convert.ToByte(toCard.Utilities.ParafaitEnv.MifareCustomerKey), toCard.CardNumber);
                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                        locker = new PassTechLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext);
                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                        locker = new MetraLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext, toCard.CardNumber, null, zoneCode, lockerMake, lockerMode);
                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                        locker = new HecereLockCardHandler(toCard.ReaderDevice, toCard.Utilities.ExecutionContext, toCard.CardNumber);
                    if (locker != null)
                    {
                        lockerAllocationDTO.ValidFromTime = lockerAllocationDTO.ValidFromTime.AddMinutes(1);
                        lockerAllocationDTO.ValidToTime = lockerAllocationDTO.ValidToTime.AddMinutes(1);
                        locker.SetAllocation(lockerAllocationDTO);

                        locker.CreateGuestCard(lockerAllocationDTO.ValidFromTime, lockerAllocationDTO.ValidToTime, isFixedMode, ((lockerbl.getLockerDTO != null && lockerbl.getLockerDTO.LockerId > -1) ? Convert.ToUInt32(lockerbl.getLockerDTO.Identifier) : 0), lockerAllocationDTO.ZoneCode, -1, lockerMake, lockerbl.getLockerDTO.ExternalIdentifier);
                        try
                        {
                            List<String> cardList = new List<string>();
                            cardList.Add(toCard.CardNumber);
                            string onlineServiceUrl = Utilities.getParafaitDefaults("ONLINE_LOCKER_SERVICE_URL");
                            if (!string.IsNullOrEmpty(onlineServiceUrl))
                            {
                                locker.SendOnlineCommand(onlineServiceUrl, RequestType.UNBLOCK_CARD, null, cardList, null, lockerMake);
                                cardList = new List<string>();
                                cardList.Add(fromCard.CardNumber);
                                locker.SendOnlineCommand(onlineServiceUrl, RequestType.BLOCK_CARD, null, cardList, null, lockerMake);
                            }
                        }
                        catch { }
                    }

                }
            }

            // toCard.addBonus = fromCard.bonus;
            // toCard.addCourtesy = fromCard.courtesy;
            // toCard.addCredits = fromCard.credits;
            // toCard.CreditPlusCredits = fromCard.CreditPlusCredits;
            // toCard.addCreditPlusCardBalance = fromCard.CreditPlusCardBalance;
            // toCard.credits_played = fromCard.credits_played;
            // toCard.loyalty_points = fromCard.loyalty_points;
            // toCard.customer_id = fromCard.customer_id;
            // toCard.face_value = fromCard.face_value;
            toCard.issue_date = fromCard.issue_date;
            toCard.last_update_time = fromCard.last_update_time;
            toCard.notes = "Replaced by Card Number " + toCard.CardNumber;
            // toCard.refund_amount = fromCard.refund_amount;
            // toCard.refund_date = fromCard.refund_date;
            toCard.refund_flag = fromCard.refund_flag;
            toCard.ticket_allowed = fromCard.ticket_allowed;
            // toCard.addTicketCount = fromCard.ticket_count;
            // toCard.addTime = fromCard.time;
            toCard.valid_flag = fromCard.valid_flag;
            toCard.vip_customer = fromCard.vip_customer;
            toCard.real_ticket_mode = fromCard.real_ticket_mode;
            toCard.tech_games = fromCard.tech_games;
            toCard.start_time = fromCard.start_time;
            toCard.siteId = fromCard.siteId;
            toCard.ExpiryDate = fromCard.ExpiryDate;
            toCard.last_played_time = fromCard.last_played_time;

            if (toCard.isMifare)
                toCard.GetCardEntitlements(fromCard.card_id);

            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
                SQLTrx = Utilities.createConnection().BeginTransaction();
            else
                SQLTrx = inSQLTrx;

            SqlConnection cnn = SQLTrx.Connection;
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            try
            {
                toCard.createCard(SQLTrx);

                cmd.Parameters.AddWithValue("@new_card_id", toCard.card_id);

                log.LogVariableState("@new_card_id", toCard.card_id);

                cmd.CommandText = "update cards set card_number = @swapCard, last_update_time = getdate(), LastUpdatedBy = @userId where card_id = @cardId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@cardId", fromCard.card_id);
                cmd.Parameters.AddWithValue("@swapCard", toCard.CardNumber);
                cmd.Parameters.AddWithValue("@userId", Utilities.ExecutionContext.GetUserId());
                cmd.ExecuteNonQuery();

                log.LogVariableState("@cardId", fromCard.card_id);
                log.LogVariableState("@swapCard", toCard.CardNumber);
                log.LogVariableState("@userId", Utilities.ExecutionContext.GetUserId());

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@cardId", toCard.card_id);
                cmd.Parameters.AddWithValue("@swapCard", fromCard.CardNumber + "-Replaced-" + Utilities.getServerTime().ToString("yyyyMMddHHmmss"));//Change card number
                cmd.Parameters.AddWithValue("@userId", Utilities.ExecutionContext.GetUserId());
                cmd.ExecuteNonQuery();

                log.LogVariableState("@cardId", toCard.card_id);
                log.LogVariableState("@swapCard", fromCard.CardNumber);
                log.LogVariableState("@userId", Utilities.ExecutionContext.GetUserId());

                cmd.CommandText = @"update trx_lines 
                                    set card_number = @cardNumber , lastupdatedate = getdate(), LastUpdatedBy = @userId
                                    where card_id = @cardId; 
                                    update lockerAllocation 
                                    set cardNumber = @cardNumber, ValidFromTime= DATEADD(MINUTE,1,ValidFromTime), ValidToTime= DATEADD(MINUTE,1,ValidToTime) 
                                    where cardId = @cardId; 
                                    update RentalAllocation 
                                    set cardNumber = @cardNumber, LastUpdatedTime= getdate(), LastUpdatedBy = @userId
                                    where cardId = @cardId";
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@cardId", fromCard.card_id);
                cmd.Parameters.AddWithValue("@cardNumber", toCard.CardNumber);
                cmd.Parameters.AddWithValue("@userId", Utilities.ExecutionContext.GetUserId());
                cmd.ExecuteNonQuery();

                log.LogVariableState("@cardId", fromCard.card_id);
                log.LogVariableState("@cardNumber", toCard.CardNumber);
                log.LogVariableState("@userId", Utilities.ExecutionContext.GetUserId());

                try
                {
                    //Add new card to device list by touching existing record
                    cmd.CommandText = @"UPDATE NotificationTagIssued
                                       SET LastUpdateDate = getdate()
                                     WHERE cardId = @cardId
                                       AND (IsReturned = 0
                                           OR ExpiryDate > getdate()
	                                       )";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cardId", fromCard.card_id);
                    cmd.ExecuteNonQuery();
                    ////Remove from card from device list in Server
                    //List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> tagsSearchParameters = new List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>();
                    //tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    //tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.IS_ACTIVE, 1.ToString()));
                    //tagsSearchParameters.Add(new KeyValuePair<NotificationTagsDTO.SearchByParameters, string>(NotificationTagsDTO.SearchByParameters.TAGNUMBER, fromCard.CardNumber));
                    //NotificationTagsListBL notificationTagsListBL = new NotificationTagsListBL(Utilities.ExecutionContext);
                    //List<NotificationTagsDTO> notificationTagsDTOList = notificationTagsListBL.GetAllNotificationTagsList(tagsSearchParameters);
                    //int notificationTagId = -1;
                    //if (notificationTagsDTOList != null && notificationTagsDTOList.Count > 0)
                    //{
                    //    notificationTagId = notificationTagsDTOList[0].NotificationTagId;
                    //    NotificationTagManualEventsDTO notificationTagManualEventsDTO = new NotificationTagManualEventsDTO(-1, notificationTagId, RadianDeviceCommandConverter.ToString(RadianDeviceCommand.SOFT_WB_REMOVE)
                    //                                                                , -1, null, null, null, ServerDateTime.Now
                    //                                                                 , "P", null, true, "");
                    //    NotificationTagManualEventsBL manualEventsBL = new NotificationTagManualEventsBL(Utilities.ExecutionContext, notificationTagManualEventsDTO);
                    //    manualEventsBL.Save();
                    //}
                }
                catch (Exception ex)
                {
                    log.Error("Notification Tag update during Transfer card process failed: ", ex);
                }

                createTask(toCard.card_id, TaskProcs.TRANSFERCARD, -1, fromCard.card_id, -1, -1, -1, -1, -1, remarks, SQLTrx, -1, -1, -1, -1, sourceTrxId);

                toCard.invalidateCard(SQLTrx);
                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while transfering card", ex);
                message = ex.Message;
                SQLTrx.Rollback();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        void UpdateTrxCardIds(long fromCardId, long ToCardId, DateTime timeStampBeforeRefund, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(fromCardId, ToCardId, timeStampBeforeRefund, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.Parameters.AddWithValue("@prev_card_id", fromCardId);
            cmd.Parameters.AddWithValue("@new_card_id", ToCardId);
            cmd.Parameters.AddWithValue("@timeStampBeforeRefund", timeStampBeforeRefund);
            cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

            cmd.CommandText = @"update trx_lines 
                                   set card_id = @new_card_id, 
                                       card_number = (select card_number from cards where card_id = @new_card_id) ,
                                       LastUpdateDate = getdate(), 
                                       LastUpdatedBy = @LastUpdatedBy
                                 where card_id = @prev_card_id  
                                   and creationDate < @timeStampBeforeRefund
                                   and not exists (SELECT 1 from tasks t, task_type tt 
                                                    WHERE t.trxId = trx_lines.trxid
                                                      AND t.task_type_id = tt.task_type_id
                                                      AND tt.task_type in ('CONSOLIDATE'))";
            cmd.ExecuteNonQuery();

            //cmd.CommandText = "update trx_header set PrimaryCardId = @new_card_id " +
            //                                   "where PrimaryCardId = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update OrderHeader set CardId = @new_card_id " +
            //                                  "where CardId = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update trxPayments set CardId = @new_card_id " +
            //                                    "where CardId = @prev_card_id";
            //cmd.ExecuteNonQuery();

            // cmd.CommandText = "update CardGames set card_id = @new_card_id " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update CardCreditPlus set card_id = @new_card_id " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update CardDiscounts set card_id = @new_card_id " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set card_id = @new_card_id " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set consolidate_card1 = @new_card_id " +
            //                                    "where consolidate_card1 = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set consolidate_card2 = @new_card_id " +
            //                                    "where consolidate_card2 = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set consolidate_card3 = @new_card_id " +
            //                                    "where consolidate_card3 = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set consolidate_card4 = @new_card_id " +
            //                                    "where consolidate_card4 = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Tasks set consolidate_card5 = @new_card_id " +
            //                                    "where consolidate_card5 = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update CheckIns set cardId = @new_card_id " +
            //                                    "where cardId = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update CheckInDetails set cardId = @new_card_id " +
            //                                    "where cardId = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update CustomerQueue set card_id = @new_card_id " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Redemption set card_id = @new_card_id, primary_card_number = (select card_number from cards where card_id = @new_card_id) " +
            //                                    "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update Redemption_cards set card_id = @new_card_id, card_number = (select card_number from cards where card_id = @new_card_id) " +
            //                                     "where card_id = @prev_card_id";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "update RedemptionTicketAllocation set cardid = @new_card_id  where cardid = @prev_card_id ";
            //cmd.ExecuteNonQuery();

            DataTable dt = GetCardUpdateTable();

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                cmd.Parameters.Clear();

                cmd.CommandText = "update " + dt.Rows[j]["TableName"]
                                  + " set " + dt.Rows[j]["ColName"] + " = @new_card_id "
                                   + (dt.Rows[j]["lastUpdateDateColName"] != DBNull.Value ? ", " + dt.Rows[j]["lastUpdateDateColName"] + " = getdate()  " : " ")
                                  + " where " + dt.Rows[j]["ColName"] + " = @prev_card_id";
                cmd.Parameters.AddWithValue("@prev_card_id", fromCardId);
                cmd.Parameters.AddWithValue("@new_card_id", ToCardId);

                cmd.ExecuteNonQuery();
            }

            log.LogMethodExit(null);
        }

        private DataTable GetCardUpdateTable()
        {
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = @"select * from (SELECT
                                                    OBJECT_NAME(f.parent_object_id) TableName,
                                                    COL_NAME(fc.parent_object_id, fc.parent_column_id) ColName,
					                                ISNULL((SELECT COLUMN_NAME 
							                                  FROM INFORMATION_SCHEMA.COLUMNS 
							                                 WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
							                                   and COLUMN_NAME = 'LastupdatedDate'), 
						                                ISNULL((SELECT COLUMN_NAME 
									                              FROM INFORMATION_SCHEMA.COLUMNS 
									                             WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
									                               and COLUMN_NAME = 'LastupdateDate'),
								                                ISNULL((SELECT COLUMN_NAME 
											                              FROM INFORMATION_SCHEMA.COLUMNS 
											                             WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
											                               and COLUMN_NAME = 'last_update_time'),
											                                ISNULL((SELECT COLUMN_NAME 
														                              FROM INFORMATION_SCHEMA.COLUMNS 
														                             WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
														                               and COLUMN_NAME = 'last_updated_date'),
														                                ISNULL((SELECT COLUMN_NAME 
																	                              FROM INFORMATION_SCHEMA.COLUMNS 
																	                             WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
																	                               and COLUMN_NAME = 'LastUpdateTime'), 
																	                                (SELECT COLUMN_NAME 
																		                               FROM INFORMATION_SCHEMA.COLUMNS 
																		                              WHERE TABLE_NAME = OBJECT_NAME(f.parent_object_id)
																		                                and COLUMN_NAME = 'last_update_date'))
											                                )))) as lastUpdateDateColName
                                                    FROM
                                                    sys.foreign_keys AS f
                                                INNER JOIN
                                                    sys.foreign_key_columns AS fc
                                                    ON f.OBJECT_ID = fc.constraint_object_id
                                                INNER JOIN
                                                    sys.tables t
                                                    ON t.OBJECT_ID = fc.referenced_object_id
                                                WHERE
                                                    OBJECT_NAME(f.referenced_object_id) = 'Cards'
                                                    ) as linkedTablesForCard
                                                where
                                                TableName NOT in ('CardsAudit', 'CustomerFingerPrint', 'LockerAllocation', 'Maint_ChecklistDetails', 'Maint_Tasks', 'UserIdentificationTags','trx_lines')
                                                    AND CASE WHEN TableName = 'ParentChildCards' and ColName = 'ChildCardId' THEN
                                                                0 ELSE 1 END = 1
                                                        AND CASE WHEN TableName = 'tasks'
                                                                and ColName IN('transfer_to_card_id', 'consolidate_card1', 'consolidate_card2',
                                                                                    'consolidate_card3', 'consolidate_card4', 'consolidate_card5') THEN   0 ELSE 1 END = 1";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // checks if there are already valid queue products in card2, when it is being consolidated from card 1
        bool checkForQueueProducts(Card srcCard, Card dstCard)
        {
            log.LogMethodEntry(srcCard, dstCard);

            if (Utilities.executeScalar(@"select top 1 1 
                                      from cardGames cg1
                                     where cg1.card_id = @card_id1
                                        and cg1.BalanceGames > 0
                                        and (cg1.ExpiryDate is null or cg1.ExpiryDate > getdate())
                                        and dbo.GetGameProfileValue(cg1.game_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                        and exists (select 1 
                                                     from cardGames cg2
                                                    where cg2.card_id = @card_id2
                                                      and cg2.BalanceGames > 0
                                                      and (cg2.ExpiryDate is null or cg2.ExpiryDate > getdate())
                                                      and dbo.GetGameProfileValue(cg2.game_id, 'QUEUE_SETUP_REQUIRED') = '1')",
                        new SqlParameter("@card_id1", srcCard.card_id),
                        new SqlParameter("@card_id2", dstCard.card_id)) != null)
            {

                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        public bool Consolidate(List<Card> cards, int cardCount, string remarks, ref string message, SqlTransaction inSQLTrx = null, bool InvalidateSourceCard = false, bool mergeHistoryDuringSourceInactivation = false)
        {
            log.LogMethodEntry(cards, cardCount, remarks, message, inSQLTrx, InvalidateSourceCard, mergeHistoryDuringSourceInactivation);
            log.LogVariableState("mergeHistoryDuringSourceInactivation", mergeHistoryDuringSourceInactivation);
            if (mergeHistoryDuringSourceInactivation == false)
            {
                //override using configuration if caller has sent the value as false
                mergeHistoryDuringSourceInactivation = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext,
                                                                                                 "MERGE_SOURCE_CARD_HISTORY_ON_INACTIVATION", false);
            }
            log.LogVariableState("mergeHistoryDuringSourceInactivation", mergeHistoryDuringSourceInactivation);
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
                SQLTrx = Utilities.createConnection().BeginTransaction();
            else
                SQLTrx = inSQLTrx;
            SqlConnection cnn = SQLTrx.Connection;
            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);
            SqlCommand creditPlusNegativeEntryCmd = Utilities.getCommand(SQLTrx);
            SqlCommand creditPlusCmd = Utilities.getCommand(SQLTrx);
            SqlCommand creditPlusConsumptionCmd = Utilities.getCommand(SQLTrx);
            SqlCommand creditPlusPurchaseCritCmd = Utilities.getCommand(SQLTrx);
            SqlCommand updateCmd = Utilities.getCommand(SQLTrx);
            SqlCommand insertGamesCmd = Utilities.getCommand(SQLTrx);
            SqlCommand insertGamesNegativeEntryCmd = Utilities.getCommand(SQLTrx);
            SqlCommand updateGamesCmd = Utilities.getCommand(SQLTrx);
            SqlCommand insertCardGmeExtended = Utilities.getCommand(SQLTrx);
            SqlCommand gameCmd = Utilities.getCommand(SQLTrx);
            SqlCommand consolidateCardDiscountCmd = Utilities.getCommand(SQLTrx);

            cardCmd.CommandText = @"update cards 
                                       Set last_update_time = getdate(), LastUpdatedBy = @LastUpdatedBy, credits = @credits,
                                           courtesy = @courtesy,  bonus = @bonus, time = @time, ticket_count = @ticket_count, 
                                           loyalty_points = @loyaltyPoints 
                                     Where card_id = @card_id
                                       and last_update_time <= @lastUpdateTime;
                                     ";

            creditPlusNegativeEntryCmd.CommandText = @"insert into CardCreditPlus (CreditPlus, CreditPlusType, Refundable, Remarks, Card_Id,
                                                                      CreditPlusBalance, PeriodFrom, PeriodTo, TimeFrom, TimeTo,
                                                                      NumberOfDays, Monday, TuesDay, WednesDay, Thursday, Friday,
                                                                      Saturday, Sunday, MinimumSaleAmount, LoyaltyRuleId, CreationDate,
                                                                      LastUpdatedDate, LastUpdatedBy, ExtendOnReload, PlayStartTime, 
                                                                      TicketAllowed, ForMembershipOnly, ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                      PauseAllowed,CreatedBy, TrxId, lineId)  
                                                              (select -1 * CreditPlusBalance, CreditPlusType, Refundable, Remarks,Card_id,
                                                                      -1 * CreditPlusBalance, PeriodFrom, getdate(), TimeFrom, TimeTo,
                                                                      NumberOfDays, Monday, TuesDay, WednesDay, Thursday, Friday,
                                                                      Saturday, Sunday, MinimumSaleAmount, LoyaltyRuleId, getdate(),
                                                                      getdate(), @LastUpdatedBy, ExtendOnReload, PlayStartTime, TicketAllowed, ForMembershipOnly, 
                                                                      ExpireWithMembership, MembershipId, MembershipRewardsId, PauseAllowed, @LastUpdatedBy, @trxId,@lineId
                                                                 from cardCreditPlus
                                                                where CardCreditPlusId = @CardCreditPlusId); select @@identity";
            creditPlusCmd.CommandText = @"insert into CardCreditPlus (CreditPlus, CreditPlusType, Refundable, Remarks, Card_Id,
                                                                      CreditPlusBalance, PeriodFrom, PeriodTo, TimeFrom, TimeTo,
                                                                      NumberOfDays, Monday, TuesDay, WednesDay, Thursday, Friday,
                                                                      Saturday, Sunday, MinimumSaleAmount, LoyaltyRuleId, CreationDate,
                                                                      LastUpdatedDate, LastUpdatedBy, ExtendOnReload, PlayStartTime, 
                                                                      TicketAllowed, ForMembershipOnly, ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                      PauseAllowed,CreatedBy,TrxId, lineId, ValidityStatus, SubscriptionBillingScheduleId)  
                                                              (select CreditPlusBalance, CreditPlusType, Refundable, Remarks,@ToCardId,
                                                                      CreditPlusBalance, PeriodFrom, PeriodTo, TimeFrom, TimeTo,
                                                                      NumberOfDays, Monday, TuesDay, WednesDay, Thursday, Friday,
                                                                      Saturday, Sunday, MinimumSaleAmount, LoyaltyRuleId, CreationDate,
                                                                      getdate(), @LastUpdatedBy, ExtendOnReload, PlayStartTime, TicketAllowed, ForMembershipOnly, 
                                                                      ExpireWithMembership, MembershipId, MembershipRewardsId, PauseAllowed,@LastUpdatedBy, @trxId,@lineId,
                                                                      ValidityStatus, SubscriptionBillingScheduleId
                                                                 from cardCreditPlus
                                                                where CardCreditPlusId = @CardCreditPlusId); select @@identity";


            creditPlusConsumptionCmd.CommandText = @"insert into CardCreditPlusConsumption (CardCreditPlusId, POSTypeId, ExpiryDate,
                                                                                ProductId, GameProfileId, GameId, CategoryId,
                                                                                DiscountPercentage, DiscountedPrice, DiscountAmount,
                                                                                LastUpdatedDate, LastUpdatedBy, ConsumptionBalance, QuantityLimit,IsActive, ConsumptionQty)
                                                                        (select @NewCardCreditPlusId, POSTypeId, ExpiryDate,
                                                                                ProductId, GameProfileId, GameId, CategoryId,
                                                                                DiscountPercentage, DiscountedPrice, DiscountAmount,
                                                                                getdate(), @LastUpdatedBy, ConsumptionBalance, QuantityLimit, IsActive, ConsumptionQty
                                                                         from CardCreditPlusConsumption
                                                                        where CardCreditPlusId = @CardCreditPlusId)";

            creditPlusPurchaseCritCmd.CommandText = @"insert into CardCreditPlusPurchaseCriteria (CardCreditPlusId, POSTypeId,
                                                                                ProductId, LastUpdatedDate, LastUpdatedBy)
                                                                        (select @NewCardCreditPlusId, POSTypeId,
                                                                                ProductId, getdate(), @LastUpdatedBy
                                                                         from CardCreditPlusPurchaseCriteria
                                                                        where CardCreditPlusId = @CardCreditPlusId)";

            //updateCmd.CommandText = @"update cardCreditPlus set CreditPlus = 0, CreditPlusBalance = 0
            //                          where Card_Id = @FromCardId;
            //                          update CardCreditPlusConsumption set ConsumptionBalance = 0
            //                          where CardCreditPlusId in (select CardCreditPlusId 
            //                                                       from cardCreditPlus 
            //                                                      where Card_Id = @FromCardId)";
            updateCmd.CommandText = @"update cardCreditPlus 
                                         set PeriodTo = CASE WHEN PeriodTo IS NULL OR PeriodFrom > getdate() THEN 
                                                                  CASE WHEN ISNULL(PeriodFrom, getdate()) <= getdate() THEN 
                                                                            getdate() 
                                                                       ELSE PeriodFrom END  
			                                                  ELSE 
														          CASE WHEN ISNULL(periodTo,getdate()) >=  getdate() THEN 
                                                                            getdate() 
                                                                       ELSE periodTo  END
														 END,
                                              LastUpdatedDate = getdate(), LastUpdatedBy = @LastUpdatedBy
                                        where Card_Id = @FromCardId 
                                            and (ExtendOnReload = 'Y' 
                                                    or (CreditPlusBalance > 0 and (periodTo is null OR periodTO >= getdate()))
		                                            or (CreditPlus = 0 and (periodTo is null OR periodTO >= getdate()) 
		                                                and exists (SELECT 1 
			                                                        from CardCreditPlusConsumption ccp 
						                                             where ccp.CardCreditPlusId = cardCreditPlus.CardCreditPlusId 
						                                               and (ccp.ExpiryDate is null OR ccp.ExpiryDate >= getdate()))))";
            //and (ExtendOnReload = 'Y' or (CreditPlusBalance != 0 and (periodTo is null OR periodTO >= getdate())))";

            insertGamesNegativeEntryCmd.CommandText = @"insert into cardGames (card_id, game_id, game_profile_id, quantity, ExpiryDate, Frequency, BalanceGames,  
                                                                   CardTypeId,last_Update_Date, EntitlementType, OptionalAttribute, CustomDataSetId, FromDate,
                                                                  ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                  Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,
                                                                  creationDate, CreatedBy, LastUpdatedBy,TrxId,TrxLineId,TicketAllowed,IsActive)
                                          select card_id, game_id, game_profile_id, -1 * (CASE when ISNULL(frequency,'N') = 'N' THEN BalanceGames ELSE quantity end) as Qty, getdate(), Frequency, -1 * BalanceGames, null, 
                                                                  getdate(), EntitlementType, OptionalAttribute, CustomDataSetId, FromDate,
                                                                  ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                  Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,
                                                                  getdate(), @LastUpdatedBy, @LastUpdatedBy,@trxId,@lineId, TicketAllowed, IsActive 
                                         from cardGames
                                        where card_game_id = @cardGameId 
                                          and (BalanceGames > 0 OR frequency != 'N' ) 
                                          and (ExpiryDate is null OR ExpiryDate >= getdate()); select @@identity";

            insertGamesCmd.CommandText = @"insert into cardGames (card_id, game_id, game_profile_id, quantity, ExpiryDate, Frequency, BalanceGames, CardTypeId,  
                                                                  last_Update_Date, EntitlementType, OptionalAttribute, CustomDataSetId, FromDate,
                                                                  ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                  Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,
                                                                  creationDate, CreatedBy, LastUpdatedBy,TrxId,TrxLineId,TicketAllowed,IsActive,
                                                                  ValidityStatus, SubscriptionBillingScheduleId)
                                          select @ToCardId, game_id, game_profile_id, CASE when ISNULL(frequency,'N') = 'N' THEN BalanceGames ELSE quantity end as Qty,
                                                                  ExpiryDate, Frequency, BalanceGames, null, 
                                                                  getdate(), EntitlementType, OptionalAttribute, CustomDataSetId, FromDate,
                                                                  ExpireWithMembership, MembershipId, MembershipRewardsId,
                                                                  Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,
                                                                  CreationDate, CreatedBy, @LastUpdatedBy,@trxId,@lineId,TicketAllowed,IsActive,
                                                                  ValidityStatus, SubscriptionBillingScheduleId
                                         from cardGames
                                        where card_game_id = @cardGameId 
                                          and (BalanceGames > 0 OR frequency != 'N' ) 
                                          and (ExpiryDate is null OR ExpiryDate >= getdate()); select @@identity";

            insertCardGmeExtended.CommandText = @"insert into CardGameExtended (CardGameId, GameId, GameProfileId, Exclude, PlayLimitPerGame, IsActive)
                                        select @NewCardGameId, GameId, GameProfileId, Exclude, PlayLimitPerGame, IsActive 
                                    from CardGameExtended
                                      where CardGameId = @cardGameId";

            //updateGamesCmd.CommandText = @"update cardGames set BalanceGames = 0
            //                                where Card_Id = @FromCardId
            //                                and isnull(frequency, 'N') = 'N' and BalanceGames != 0;
            //                               update cardGames set quantity = 0, BalanceGames = 0
            //                                where Card_Id = @FromCardId
            //                                and frequency != 'N';";
            updateGamesCmd.CommandText = @"update cardGames 
                                              set ExpiryDate = getdate(), 
                                                  last_Update_Date = getdate(), LastUpdatedBy = @LastUpdatedBy
                                            where Card_Id = @FromCardId 
                                              and (BalanceGames != 0 OR frequency != 'N' ) 
                                              and (ExpiryDate is null OR ExpiryDate >= getdate())";

            consolidateCardDiscountCmd.CommandText = @"insert into CardDiscounts
                                                                 (card_id, discount_id, expiry_date,InternetKey,IsActive, TransactionId, LineId, ExpireWithMembership,
                                                                  MembershipId, MembershipRewardsId, CreatedBy, CreationDate, last_updated_user, last_updated_date,site_id,
                                                                  ValidityStatus, SubscriptionBillingScheduleId)
                                                                  select @newCardId, discount_id, expiry_date, InternetKey, IsActive, @transactionId, @lineId, ExpireWithMembership,MembershipId,
                                                                         MembershipRewardsId, @loginId, getdate(),  @loginId, getdate(), @siteId,
                                                                          ValidityStatus, SubscriptionBillingScheduleId
                                                                     from CardDiscounts cd
                                                                    where card_id = @oldCardId
                                                                      and IsActive = 'Y'
                                                                      and ISNULL(expiry_date, getdate()) >= getdate()
                                                                      and not exists (SELECT top 1 1
                                                                                         from CardDiscounts cdin
                                                                                        where cdin.card_id = @newCardId
                                                                                          and IsActive = 'Y'
                                                                                          and ISNULL(expiry_date, getdate()) >= getdate()
                                                                                          and cdin.discount_id = cd.discount_id);
                                                            Update CardDiscounts
                                                            Set IsActive = 'N', expiry_date = getdate(), last_updated_user = @loginId, last_updated_date = getdate()
                                                          where card_id = @oldCardId
                                                            and IsActive = 'Y'
                                                            and ISNULL(expiry_date, getdate()) >= getdate()";

            double credits = 0;
            double courtesy = 0;
            double bonus = 0;
            double time = 0;
            double gameplayCreditPlus = 0;
            double loyaltyPoint = 0;
            int ticket_count = 0;

            //int consolidate_card1 = -1; 
            int card_id = -1;
            int fromCardProductId = -1;
            int toCardProductId = -1;
            int trxId = -1;
            //int lineId = -1;

            try
            {
                for (int i = 0; i < cardCount; i++)
                {
                    if (checkForQueueProducts(cards[i], cards[cardCount - 1]) == false)
                    {
                        message = Utilities.MessageUtils.getMessage(495) + " [" + cards[cardCount - 1].CardNumber + "]";
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                        }

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    AccountDTO consolidateCardDTO = new AccountBL(Utilities.ExecutionContext, cards[i].card_id, true, true, SQLTrx).AccountDTO;
                    List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(consolidateCardDTO, SQLTrx);
                    if ((consolidateCardDTO.AccountCreditPlusDTOList != null
                          && consolidateCardDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                     && ((x.SubscriptionBillingScheduleId == -1)
                                                                                         || (x.SubscriptionBillingScheduleId != -1
                                                                                            && subscriptionBillingScheduleDTOList != null
                                                                                            && subscriptionBillingScheduleDTOList.Any()
                                                                                            && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                       && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
                        )
                        ||
                        (consolidateCardDTO.AccountGameDTOList != null
                          && consolidateCardDTO.AccountGameDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                               && ((x.SubscriptionBillingScheduleId == -1)
                                                                                    || (x.SubscriptionBillingScheduleId != -1
                                                                                       && subscriptionBillingScheduleDTOList != null
                                                                                       && subscriptionBillingScheduleDTOList.Any()
                                                                                       && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
                        )
                        ||
                        (consolidateCardDTO.AccountDiscountDTOList != null
                          && consolidateCardDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                  && ((x.SubscriptionBillingScheduleId == -1)
                                                                                      || (x.SubscriptionBillingScheduleId != -1
                                                                                           && subscriptionBillingScheduleDTOList != null
                                                                                           && subscriptionBillingScheduleDTOList.Any()
                                                                                           && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))) //Ignore subscription holds
                        )
                       )
                    {
                        message = Utilities.MessageUtils.getMessage(2610) + " [" + cards[i].CardNumber + "]";
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                        }

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                for (int i = 0; i < cardCount; i++)
                {
                    AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, cards[i].card_id, false, false, SQLTrx);
                    if (accountBL.IsAccountUpdatedByOthers(cards[i].last_update_time, SQLTrx))
                    {
                        message = Utilities.MessageUtils.getMessage(360, cards[i].CardNumber);
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                        }

                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    cardCmd.Parameters.Clear();
                    cardCmd.Parameters.AddWithValue("@card_id", cards[i].card_id);
                    cardCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                    log.LogVariableState("@card_id", cards[i].card_id);
                    log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                    credits += cards[i].credits;
                    courtesy += cards[i].courtesy;
                    bonus += cards[i].bonus;
                    time += cards[i].time;
                    gameplayCreditPlus += cards[i].CreditPlusCredits;
                    ticket_count += cards[i].ticket_count;
                    loyaltyPoint += cards[i].loyalty_points;

                    if (i == cardCount - 1) // last card - consolidate to card
                    {
                        if (cards[i] != null && gameCardCreditLimit > 0 && cards[i].technician_card != 'Y')
                        {
                            if (credits != 0)
                            {
                                if (Convert.ToDecimal(credits) > gameCardCreditLimit)
                                {
                                    message = Utilities.MessageUtils.getMessage(1168);
                                    if (inSQLTrx == null)
                                    {
                                        SQLTrx.Rollback();
                                    }

                                    log.LogVariableState("message ", message);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                        cardCmd.Parameters.AddWithValue("@credits", credits);
                        cardCmd.Parameters.AddWithValue("@courtesy", courtesy);
                        cardCmd.Parameters.AddWithValue("@bonus", bonus);
                        cardCmd.Parameters.AddWithValue("@time", time);
                        cardCmd.Parameters.AddWithValue("@ticket_count", ticket_count);
                        cardCmd.Parameters.AddWithValue("@loyaltyPoints", loyaltyPoint);
                        cardCmd.Parameters.AddWithValue("@lastUpdateTime", cards[i].last_update_time);
                        card_id = cards[i].card_id;

                        log.LogVariableState("@credits", credits);
                        log.LogVariableState("@courtesy", courtesy);
                        log.LogVariableState("@bonus", bonus);
                        log.LogVariableState("@time", time);
                        log.LogVariableState("@ticket_count", ticket_count);
                        log.LogVariableState("@lastUpdateTime", cards[i].last_update_time);
                    }
                    else
                    {
                        cardCmd.Parameters.AddWithValue("@credits", 0);
                        cardCmd.Parameters.AddWithValue("@courtesy", 0);
                        cardCmd.Parameters.AddWithValue("@bonus", 0);
                        cardCmd.Parameters.AddWithValue("@time", 0);
                        cardCmd.Parameters.AddWithValue("@ticket_count", 0);
                        cardCmd.Parameters.AddWithValue("@loyaltyPoints", 0);
                        cardCmd.Parameters.AddWithValue("@lastUpdateTime", cards[i].last_update_time);
                        //switch (i)
                        // {
                        //     case 0: consolidate_card1 = Cards[i].card_id; break;
                        //     case 1: consolidate_card2 = Cards[i].card_id; break;
                        //     case 2: consolidate_card3 = Cards[i].card_id; break;
                        //     case 3: consolidate_card4 = Cards[i].card_id; break;
                        //     case 4: consolidate_card5 = Cards[i].card_id; break;
                        //     default: break;
                        // }

                        log.LogVariableState("@credits", 0);
                        log.LogVariableState("@courtesy", 0);
                        log.LogVariableState("@bonus", 0);
                        log.LogVariableState("@time", 0);
                        log.LogVariableState("@ticket_count", 0);
                        log.LogVariableState("@lastUpdateTime", cards[i].last_update_time);
                    }

                    if (cardCmd.ExecuteNonQuery() < 1)
                    {
                        message = Utilities.MessageUtils.getMessage(360, cards[i].CardNumber);
                        if (inSQLTrx == null)
                        {
                            SQLTrx.Rollback();
                        }

                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                Transaction trx = new Transaction(Utilities);
                //try
                // {
                fromCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CONSOLIDATION_TASK_FROM_CARD_PRODUCT"));
                toCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CONSOLIDATION_TASK_TO_CARD_PRODUCT"));
                if (fromCardProductId == -1 || toCardProductId == -1)
                {
                    //throw new Exception("Consolidation task product value is not set");
                    MessageBox.Show(Utilities.MessageUtils.getMessage(2252));
                    if (inSQLTrx == null)
                    {
                        SQLTrx.Rollback();
                    }

                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
                //}
                //catch (Exception ex)
                //{
                //    log.Error("Error occurred while fetching card consolidation task product id", ex);
                //    message = ex.Message;
                //    if (inSQLTrx == null)
                //    {
                //        SQLTrx.Rollback();
                //    } 
                //    log.LogMethodExit(false);
                //    return false;
                //}

                //add transaction line for source card and target card, which will be pointed by creditplus and credits information of cards for tracking consolidation
                //information

                List<Tuple<int, int, int>> cardTrxLineList = new List<Tuple<int, int, int>>();
                int managerOriginalId;
                int lineCount = 1;
                for (int i = 0; i < cardCount - 1; i++)
                {
                    trx.createTransactionLine(cards[i], fromCardProductId, 0, 1, ref message);
                    trx.createTransactionLine(cards[cardCount - 1], toCardProductId, 0, 1, ref message);
                    cardTrxLineList.Add(new Tuple<int, int, int>(cards[i].card_id, lineCount++, lineCount++));
                }
                trx.SaveOrder(ref message, SQLTrx);
                trxId = trx.Trx_id;
                //cardCmd.CommandText = "select cardCreditPlusId from CardCreditPlus where card_Id = @fromCardId and (ExtendOnReload = 'Y' or (CreditPlusBalance > 0 and (periodTo is null OR periodTO >= getdate())))";
                cardCmd.CommandText = @"select cardCreditPlusId
                                            from CardCreditPlus cp
                                            where card_Id = @fromCardId
                                               and (ExtendOnReload = 'Y' 
                                                    or (CreditPlusBalance > 0 and (periodTo is null OR periodTO >= getdate()))
		                                            or (CreditPlus = 0 and (periodTo is null OR periodTO >= getdate()) 
		                                                and exists (SELECT 1 
			                                                        from CardCreditPlusConsumption ccp 
						                                             where ccp.CardCreditPlusId = cp.CardCreditPlusId 
						                                               and (ccp.ExpiryDate is null OR ccp.ExpiryDate >= getdate()))))";
                gameCmd.CommandText = "select card_game_id from CardGames where card_id = @fromCardId and (BalanceGames > 0 OR frequency != 'N' ) and(ExpiryDate is null OR ExpiryDate >= getdate())";
                managerOriginalId = Utilities.ParafaitEnv.ManagerId;
                for (int i = 0; i < cardCount - 1; i++)
                {
                    ConsolidateCardCreditPlusDetails(cards[i].card_id, cards[cardCount - 1].card_id, cardCmd, creditPlusNegativeEntryCmd, creditPlusCmd, creditPlusConsumptionCmd, creditPlusPurchaseCritCmd, trxId, cardTrxLineList[i].Item2, cardTrxLineList[i].Item3);

                    ConsolidateCardGameDetails(cards[i].card_id, cards[cardCount - 1].card_id, updateCmd, insertGamesCmd, insertGamesNegativeEntryCmd, updateGamesCmd, gameCmd, insertCardGmeExtended, trxId, cardTrxLineList[i].Item2, cardTrxLineList[i].Item3);

                    ConsolidateCardDiscountDetails(cards[i].card_id, cards[cardCount - 1].card_id, consolidateCardDiscountCmd, trxId, cardTrxLineList[i].Item3);

                    Utilities.ParafaitEnv.ManagerId = managerOriginalId;

                    createTask(card_id, TaskProcs.CONSOLIDATE, -1, -1, -1, -1, cards[i].card_id,
                        trxId, cardTrxLineList[i].Item2, remarks, SQLTrx, cards[i].credits, cards[i].courtesy, cards[i].bonus, cards[i].ticket_count, trxId);
                    trx.UpdateTransactionLine(cardTrxLineList[i].Item2, cards[i].credits * -1, cards[i].courtesy * -1, cards[i].bonus * -1, cards[i].ticket_count * -1, cards[i].loyalty_points * -1);
                    trx.UpdateTransactionLine(cardTrxLineList[i].Item3, cards[i].credits, cards[i].courtesy, cards[i].bonus, cards[i].ticket_count, cards[i].loyalty_points);
                }
                trx.SaveTransacation(SQLTrx, ref message);

                if (Utilities.getParafaitDefaults("INVALIDATE_SRC_CARD_ON_CONSOLIDATE") == "Y" || InvalidateSourceCard)
                {
                    long savCustomerId = cards[cardCount - 1].customer_id;
                    // int savCardTypeId = Cards[cardCount - 1].CardTypeId;
                    char savVIPFlag = cards[cardCount - 1].vip_customer;

                    for (int i = 0; i < cardCount - 1; i++)
                    {
                        DateTime timeStampBeforeRefund = ServerDateTime.Now;
                        if (!RefundCard(cards[i], 0, 0, 0, "Deactivated By Card Consolidation Task", ref message, true, SQLTrx))
                        {
                            log.Error("Unable to Deactivate card [" + cards[i].CardNumber + " ]" + " error: " + message);
                        }
                        else
                        {
                            cards[i].invalidateCard(SQLTrx);
                            cards[i] = new Card(cards[i].CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                            if (mergeHistoryDuringSourceInactivation)
                            {
                                UpdateTrxCardIds(cards[i].card_id, cards[cardCount - 1].card_id, timeStampBeforeRefund,  SQLTrx);
                                cards[cardCount - 1].updateCardTime(SQLTrx);
                            }
                        }

                        if (cards[cardCount - 1].customer_id == -1 && cards[i].customer_id != -1)
                            cards[cardCount - 1].customer_id = cards[i].customer_id;

                        //if (Cards[cardCount - 1].CardTypeId == -1 && Cards[i].CardTypeId != -1)
                        //    Cards[cardCount - 1].CardTypeId = Cards[i].CardTypeId;

                        if (cards[cardCount - 1].vip_customer == 'N' && cards[i].vip_customer == 'Y')
                            cards[cardCount - 1].vip_customer = 'Y';

                        if (cards[i].isMifare)
                        {
                            // MessageBox.Show("Place the source card " + cards[i].CardNumber + " on reader");
                            MessageBox.Show(Utilities.MessageUtils.getMessage(2253, cards[i].CardNumber));
                            if (!cards[i].refund_MCard(ref message))
                            {
                                if (inSQLTrx == null)
                                    SQLTrx.Rollback();

                                log.LogVariableState("message ", message);
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }

                    // if (savCustomerId != Cards[cardCount - 1].customer_id || savCardTypeId != Cards[cardCount - 1].CardTypeId || savVIPFlag != Cards[cardCount - 1].vip_customer)
                    if (savCustomerId != cards[cardCount - 1].customer_id || savVIPFlag != cards[cardCount - 1].vip_customer)
                    {
                        updateCmd.CommandText = @"update cards set customer_id = @customerId,  
                                                            vip_customer = @vip , last_update_time=getdate()
                                                        where card_id = @cardId";
                        if (cards[cardCount - 1].customer_id == -1)
                        {
                            updateCmd.Parameters.AddWithValue("@customerId", DBNull.Value);
                            log.LogVariableState("@customerId", DBNull.Value);
                        }
                        else
                        {
                            updateCmd.Parameters.AddWithValue("@customerId", cards[cardCount - 1].customer_id);
                            log.LogVariableState("@customerId", cards[cardCount - 1].customer_id);
                        }

                        //if (Cards[cardCount - 1].CardTypeId == -1)
                        //{
                        //    updateCmd.Parameters.AddWithValue("@CardTypeId", DBNull.Value);
                        //    log.LogVariableState("@CardTypeId", DBNull.Value);
                        //}
                        //else
                        //{
                        //    updateCmd.Parameters.AddWithValue("@CardTypeId", Cards[cardCount - 1].CardTypeId);
                        //    log.LogVariableState("@CardTypeId", Cards[cardCount - 1].CardTypeId);
                        //}

                        updateCmd.Parameters.AddWithValue("@vip", cards[cardCount - 1].vip_customer);
                        updateCmd.Parameters.AddWithValue("@cardId", cards[cardCount - 1].card_id);
                        updateCmd.ExecuteNonQuery();

                        log.LogVariableState("@vip", cards[cardCount - 1].vip_customer);
                        log.LogVariableState("@cardId", cards[cardCount - 1].card_id);
                    }
                }
                else
                {
                    for (int i = 0; i < cardCount - 1; i++)
                    {
                        if (cards[i].isMifare)
                        {
                            //MessageBox.Show("Place the source card " + cards[i].CardNumber + " on reader");
                            MessageBox.Show(Utilities.MessageUtils.getMessage(2253, cards[i].CardNumber));

                            if (inSQLTrx == null)
                                if (!cards[i].updateMifareCard(true, ref message, 0, 0, 0, 0)) // make zero
                                {
                                    if (inSQLTrx == null)
                                        SQLTrx.Rollback();

                                    log.LogVariableState("message ", message);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                        }
                    }
                }

                if (cards[cardCount - 1].isMifare)
                {
                    //MessageBox.Show("Place the destination card " + cards[cardCount - 1].CardNumber + " on reader");
                    MessageBox.Show(Utilities.MessageUtils.getMessage(2246, cards[cardCount - 1].CardNumber));
                    if (!cards[cardCount - 1].updateMifareCard(true, ref message, credits, bonus, courtesy, gameplayCreditPlus))
                    {
                        if (inSQLTrx == null)
                            SQLTrx.Rollback();

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                if (inSQLTrx == null)
                    SQLTrx.Commit();
                TransactionId = trxId;
                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while consolidating the card", ex);
                message = ex.Message;
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        private void ConsolidateCardGameDetails(int fromCardId, int targetCardId, SqlCommand updateCmd, SqlCommand insertGamesCmd, SqlCommand insertGamesNegativeEntryCmd, SqlCommand updateGamesCmd, SqlCommand gameCmd, SqlCommand insertCardGmeExtended, int trxId, int trxlineId1, int trxlineId2)
        {
            log.LogMethodEntry(fromCardId, targetCardId, updateCmd, insertGamesCmd, insertGamesNegativeEntryCmd, updateGamesCmd, gameCmd, insertCardGmeExtended, trxId, trxlineId1, trxlineId2);
            updateCmd.Parameters.Clear();
            updateCmd.Parameters.AddWithValue("@FromCardId", fromCardId);
            updateCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
            updateCmd.ExecuteNonQuery();

            log.LogVariableState("@FromCardId", fromCardId);

            gameCmd.Parameters.Clear();
            gameCmd.Parameters.AddWithValue("@fromCardId", fromCardId);
            SqlDataAdapter gda = new SqlDataAdapter(gameCmd);
            DataTable gdt = new DataTable();
            gda.Fill(gdt);
            for (int j = 0; j < gdt.Rows.Count; j++)
            {
                insertGamesCmd.Parameters.Clear();
                insertGamesCmd.Parameters.AddWithValue("@cardGameId", gdt.Rows[j][0]);
                insertGamesCmd.Parameters.AddWithValue("@ToCardId", targetCardId);
                insertGamesCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                insertGamesCmd.Parameters.AddWithValue("@trxId", trxId);
                insertGamesCmd.Parameters.AddWithValue("@lineId", trxlineId2);
                int newCardGameId = Convert.ToInt32(insertGamesCmd.ExecuteScalar());

                log.LogVariableState("@cardGameId", gdt.Rows[j][0]);
                log.LogVariableState("@ToCardId", targetCardId);
                log.LogVariableState("@trxId", trxId);
                log.LogVariableState("@lineId", trxlineId2);

                insertCardGmeExtended.Parameters.Clear();
                insertCardGmeExtended.Parameters.AddWithValue("@cardGameId", gdt.Rows[j][0]);
                insertCardGmeExtended.Parameters.AddWithValue("@NewCardGameId", newCardGameId);
                insertCardGmeExtended.ExecuteNonQuery();

                log.LogVariableState("@cardGameId", gdt.Rows[j][0]);
                log.LogVariableState("@NewCardGameId", newCardGameId);

                insertGamesNegativeEntryCmd.Parameters.Clear();
                insertGamesNegativeEntryCmd.Parameters.AddWithValue("@cardGameId", gdt.Rows[j][0]);
                insertGamesNegativeEntryCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                insertGamesNegativeEntryCmd.Parameters.AddWithValue("@trxId", trxId);
                insertGamesNegativeEntryCmd.Parameters.AddWithValue("@lineId", trxlineId1);
                insertGamesNegativeEntryCmd.ExecuteNonQuery();

                log.LogVariableState("@fromCardId", fromCardId);
                log.LogVariableState("@trxId", trxId);
                log.LogVariableState("@lineId", trxlineId1);
            }


            updateGamesCmd.Parameters.Clear();
            updateGamesCmd.Parameters.AddWithValue("@FromCardId", fromCardId);
            updateGamesCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
            updateGamesCmd.ExecuteNonQuery();

            log.LogMethodExit();
        }

        private void ConsolidateCardCreditPlusDetails(int fromCardId, int targetCardId, SqlCommand cardCmd, SqlCommand creditPlusNegativeEntryCmd, SqlCommand creditPlusCmd, SqlCommand creditPlusConsumptionCmd, SqlCommand creditPlusPurchaseCritCmd, int trxId, int trxlineId1, int trxlineId2)
        {
            log.LogMethodEntry(fromCardId, targetCardId, cardCmd, creditPlusNegativeEntryCmd, creditPlusCmd, creditPlusConsumptionCmd, creditPlusPurchaseCritCmd, trxId, trxlineId1, trxlineId2);
            cardCmd.Parameters.Clear();
            cardCmd.Parameters.AddWithValue("@fromCardId", fromCardId);
            SqlDataAdapter da = new SqlDataAdapter(cardCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                creditPlusNegativeEntryCmd.Parameters.Clear();
                creditPlusNegativeEntryCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusNegativeEntryCmd.Parameters.AddWithValue("@sourceCardId", fromCardId);
                creditPlusNegativeEntryCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                creditPlusNegativeEntryCmd.Parameters.AddWithValue("@trxId", trxId);
                creditPlusNegativeEntryCmd.Parameters.AddWithValue("@lineId", trxlineId1);
                int NewCreditPlusNegId = Convert.ToInt32(creditPlusNegativeEntryCmd.ExecuteScalar());

                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@sourceCardId", fromCardId);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@trxId", trxId);
                log.LogVariableState("@lineId", trxlineId1);
                log.LogVariableState("@newCardCreditPlusId", NewCreditPlusNegId);

                creditPlusConsumptionCmd.Parameters.Clear();

                creditPlusConsumptionCmd.Parameters.AddWithValue("@NewCardCreditPlusId", NewCreditPlusNegId);
                creditPlusConsumptionCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusConsumptionCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                creditPlusConsumptionCmd.ExecuteNonQuery();

                log.LogVariableState("@NewCardCreditPlusId", NewCreditPlusNegId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                creditPlusPurchaseCritCmd.Parameters.Clear();
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@NewCardCreditPlusId", NewCreditPlusNegId);
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                creditPlusPurchaseCritCmd.ExecuteNonQuery();

                log.LogVariableState("@NewCardCreditPlusId", NewCreditPlusNegId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                creditPlusCmd.Parameters.Clear();
                creditPlusCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusCmd.Parameters.AddWithValue("@ToCardId", targetCardId);
                creditPlusCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                creditPlusCmd.Parameters.AddWithValue("@trxId", trxId);
                creditPlusCmd.Parameters.AddWithValue("@lineId", trxlineId2);
                int NewCreditPlusId = Convert.ToInt32(creditPlusCmd.ExecuteScalar());

                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@ToCardId", targetCardId);
                log.LogVariableState("@newCardCreditPlusId", NewCreditPlusId);
                log.LogVariableState("@trxId", trxId);
                log.LogVariableState("@lineId", trxlineId2);

                creditPlusConsumptionCmd.Parameters.Clear();

                creditPlusConsumptionCmd.Parameters.AddWithValue("@NewCardCreditPlusId", NewCreditPlusId);
                creditPlusConsumptionCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusConsumptionCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                creditPlusConsumptionCmd.ExecuteNonQuery();

                log.LogVariableState("@NewCardCreditPlusId", NewCreditPlusId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                creditPlusPurchaseCritCmd.Parameters.Clear();
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@NewCardCreditPlusId", NewCreditPlusId);
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@CardCreditPlusId", dt.Rows[j][0]);
                creditPlusPurchaseCritCmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                creditPlusPurchaseCritCmd.ExecuteNonQuery();

                log.LogVariableState("@NewCardCreditPlusId", NewCreditPlusId);
                log.LogVariableState("@CardCreditPlusId", dt.Rows[j][0]);
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
            }
            log.LogMethodExit();
        }

        public bool loadBonus(Card card, double bonus, EntitlementType entitlementType, bool Refundable, int gamePlayId, string remarks, ref string message)
        {
            log.LogMethodEntry(card, bonus, entitlementType, Refundable, gamePlayId, remarks, message);

            double loadBonusLimit = 0;
            try
            {
                loadBonusLimit = Convert.ToDouble(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for loadBonusLimit", ex);
                loadBonusLimit = 0;
                log.LogVariableState("loadBonusLimit", loadBonusLimit);
            }
            if (bonus > loadBonusLimit)
            {
                message = Utilities.MessageUtils.getMessage(43, loadBonusLimit.ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT));

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            double mgrApprovalLimit = 0;
            try
            { mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL")); }
            catch (Exception ex)
            {
                log.Error("Unable to  get a valid value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (bonus > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1212);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            string bonusType = GetBonusTypeCodeValue(entitlementType);

            try
            {
                //modification added on 30-May-2017
                #region Check No of times bonus allowed to Load in a day
                string bonusLoadLimit = Utilities.getParafaitDefaults("MAX_TIME_BONUS_ALLOWED_TO_LOAD");
                if (!string.IsNullOrEmpty(bonusLoadLimit))
                {
                    if (Convert.ToInt32(bonusLoadLimit) > 0)
                    {
                        DataTable bonusDT = Utilities.executeDataTable("select * from tasks " +
                                                                            "where task_type_id in (select task_type_id from task_type where task_type = 'LOADBONUS') " +
                                                                            "and task_date >=  DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                            "and task_date < 1 + DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                            "and card_id = @cardId", new SqlParameter("@cardId", card.card_id));
                        if (bonusDT != null && bonusDT.Rows.Count > 0)
                        {
                            if (bonusDT.Rows.Count >= Convert.ToInt32(bonusLoadLimit))
                            {
                                message = Utilities.MessageUtils.getMessage(1167);
                                return false;
                            }
                        }
                    }
                }

                //Security controls check for Staff and guest card credit limits
                decimal cardCreditsLoading = Convert.ToDecimal(bonus) + Convert.ToDecimal(card.credits) + Convert.ToDecimal(card.CreditPlusCardBalance) + Convert.ToDecimal(card.CreditPlusCredits);
                if (staffCreditLimit > 0 || gameCardCreditLimit > 0)
                {
                    if (card != null && staffCreditLimit > 0 && card.technician_card == 'Y')
                    {
                        if (bonus != 0)
                        {
                            if (cardCreditsLoading > staffCreditLimit)
                            {
                                message = Utilities.MessageUtils.getMessage(1164);
                                TasksEventLog.logEvent("Parafait POS", 'D', card.CardNumber, "LoadBonus - Tech card exceeded limit", "", 3);
                                return false;
                            }
                        }
                    }
                    else if (card != null && gameCardCreditLimit > 0 && card.technician_card != 'Y')
                    {
                        if (bonus != 0)
                        {
                            if (cardCreditsLoading > gameCardCreditLimit)
                            {
                                message = Utilities.MessageUtils.getMessage(1168);
                                return false;
                            }
                        }
                    }
                }

                int loadBonusProductId = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "LOAD_BONUS_TASK_PRODUCT", -1);
                if (loadBonusProductId == -1)
                {
                    throw new Exception("LOAD_BONUS_TASK_PRODUCT not defined");
                }
                Transaction trx = new Transaction(Utilities);
                trx.createTransactionLine(card, loadBonusProductId, 1, ref message);

                //if (gamePlayId > 0)
                //{
                //    trx.TrxLines[0].GameplayId = gamePlayId;
                //}
                if (string.IsNullOrEmpty(remarks) == false)
                {
                    trx.TrxLines[0].Remarks = remarks;
                }
                trx.SaveOrder(ref message, SQLTrx);
                int trxId = trx.Trx_id;
                #endregion
                //end modification added on 30-May-2017
                //if (bonusType == "C")
                //{

                //    cmd.CommandText = "update cards set credits = isnull(credits, 0) + @loadBonus," +
                //                                        "last_update_time = getdate(), " +
                //                                        "LastUpdatedBy = @LastUpdatedBy " +
                //                                        "where card_id = @card_id";
                //    cmd.Parameters.AddWithValue("@card_id", card.card_id);
                //    cmd.Parameters.AddWithValue("@loadBonus", bonus);
                //    cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                //    log.LogVariableState("@card_id", card.card_id);
                //    log.LogVariableState("@loadBonus", bonus);
                //    log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                //    if (card.isMifare)
                //    {
                //        try
                //        {
                //            card.updateMifareCard(false, ref message, bonus);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Error("Failed to Update Mifare Card", ex);
                //            message = ex.Message;
                //            SQLTrx.Rollback();
                //            cnn.Close();

                //            log.LogVariableState("message ", message);
                //            log.LogMethodExit(false);
                //            return false;
                //        }
                //    }

                //    cmd.ExecuteNonQuery();
                //}
                //else if (bonusType == "B" && Utilities.ParafaitEnv.LOAD_BONUS_EXPIRY_DAYS <= 0)
                //{
                //    cmd.CommandText = "update cards set bonus = isnull(bonus, 0) + @loadBonus," +
                //                                        "last_update_time = getdate(), " +
                //                                        "LastUpdatedBy = @LastUpdatedBy " +
                //                                        "where card_id = @card_id";
                //    cmd.Parameters.AddWithValue("@card_id", card.card_id);
                //    cmd.Parameters.AddWithValue("@loadBonus", bonus);
                //    cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                //    log.LogVariableState("@card_id", card.card_id);
                //    log.LogVariableState("@loadBonus", bonus);
                //    log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                //    if (card.isMifare)
                //    {
                //        try
                //        {
                //            card.updateMifareCard(false, ref message, 0, bonus);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Error("Failed to Update Mifare Card", ex);
                //            message = ex.Message;
                //            SQLTrx.Rollback();
                //            cnn.Close();

                //            log.LogVariableState("message", message);
                //            log.LogMethodExit(false);
                //            return false;
                //        }
                //    }

                //    cmd.ExecuteNonQuery();
                //}
                //else
                //{
                if (card.isMifare)
                {
                    try
                    {
                        card.updateMifareCard(false, ref message, 0, 0, 0, bonus);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to Update Mifare Card", ex);
                        message = ex.Message;
                        SQLTrx.Rollback();
                        cnn.Close();

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                Loyalty loyalty = new Loyalty(Utilities);
                loyalty.CreateGenericCreditPlusLine(card.card_id, bonusType, bonus, Refundable,
                                                    (bonusType == "B" ? Utilities.ParafaitEnv.LOAD_BONUS_EXPIRY_DAYS : 0),
                                                    Utilities.ParafaitEnv.AUTO_EXTEND_BONUS_ON_RELOAD,
                                                    Utilities.ParafaitEnv.LoginID,
                                                    string.IsNullOrEmpty(remarks.Trim()) ? "Load Bonus" : remarks,
                                                    SQLTrx, null, trxId, 1);
                if (bonusType == "B" && Utilities.ParafaitEnv.AUTO_EXTEND_BONUS_ON_RELOAD == "Y")
                    loyalty.ExtendOnReload(card.card_id, bonusType, SQLTrx);
                //}

                trx.SaveTransacation(SQLTrx, ref message);
                if (gamePlayId > 0)
                {//CreateTransactionLineGamePlaymapping entry
                    Utilities.executeScalar(@"insert into TransactionLineGamePlayMapping(TrxId, LineId, GamePlayId, IsActive,
                                                       guid, site_id, creationDate, createdBy, LastUpdateDate, LastUpdatedBy) 
                                          values (@TrxId, 1, @gameplayId, 1, newid(), @siteId, getdate(), @user, getdate(), @user)",
                                          SQLTrx,
                                          new SqlParameter("@TrxId", trxId),
                                          new SqlParameter("@gameplayId", gamePlayId),
                                          new SqlParameter("@siteId", Utilities.ParafaitEnv.IsCorporate ? (object)Utilities.ParafaitEnv.SiteId : DBNull.Value),
                                          new SqlParameter("@user", Utilities.ParafaitEnv.LoginID));
                }
                createTask(card.card_id, TaskProcs.LOADBONUS, bonus, -1, -1, -1, -1, bonusType[0], gamePlayId, remarks, SQLTrx, -1, -1, -1, -1, trxId);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading the bonus.", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        /// <param name="bonus"></param>
        /// <param name="entitlementType"></param>
        /// <param name="Refundable"></param>
        /// <param name="gamePlayId"></param>
        /// <param name="remarks"></param>
        /// <param name="message"></param>
        /// <param name="periodFrom"></param>
        /// <param name="expiryDays"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool LoadGenericEntitlement(Card card, double bonus, EntitlementType entitlementType, bool Refundable, int gamePlayId, string remarks, ref string message, DateTime? periodFrom, int? expiryDays, int? productId)
        {
            log.LogMethodEntry(card, bonus, entitlementType, Refundable, gamePlayId, remarks, message);

            double loadBonusLimit = 0;
            try
            {
                loadBonusLimit = Convert.ToDouble(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for loadBonusLimit", ex);
                loadBonusLimit = 0;
                log.LogVariableState("loadBonusLimit", loadBonusLimit);
            }
            if (bonus > loadBonusLimit)
            {
                message = Utilities.MessageUtils.getMessage(43, loadBonusLimit.ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT));

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            double mgrApprovalLimit = 0;
            try
            { mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL")); }
            catch (Exception ex)
            {
                log.Error("Unable to  get a valid value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (bonus > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1212);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            string bonusType = GetBonusTypeCodeValue(entitlementType);

            try
            {
                //modification added on 30-May-2017
                #region Check No of times bonus allowed to Load in a day
                string bonusLoadLimit = Utilities.getParafaitDefaults("MAX_TIME_BONUS_ALLOWED_TO_LOAD");
                if (!string.IsNullOrEmpty(bonusLoadLimit))
                {
                    if (Convert.ToInt32(bonusLoadLimit) > 0)
                    {
                        DataTable bonusDT = Utilities.executeDataTable("select * from tasks " +
                                                                            "where task_type_id in (select task_type_id from task_type where task_type = 'LOADBONUS') " +
                                                                            "and task_date >=  DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                            "and task_date < 1 + DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                            "and card_id = @cardId", new SqlParameter("@cardId", card.card_id));
                        if (bonusDT != null && bonusDT.Rows.Count > 0)
                        {
                            if (bonusDT.Rows.Count >= Convert.ToInt32(bonusLoadLimit))
                            {
                                message = Utilities.MessageUtils.getMessage(1167);
                                return false;
                            }
                        }
                    }
                }

                //Security controls check for Staff and guest card credit limits
                decimal cardCreditsLoading = Convert.ToDecimal(bonus) + Convert.ToDecimal(card.credits) + Convert.ToDecimal(card.CreditPlusCardBalance) + Convert.ToDecimal(card.CreditPlusCredits);
                if (staffCreditLimit > 0 || gameCardCreditLimit > 0)
                {
                    if (card != null && staffCreditLimit > 0 && card.technician_card == 'Y')
                    {
                        if (bonus != 0)
                        {
                            if (cardCreditsLoading > staffCreditLimit)
                            {
                                message = Utilities.MessageUtils.getMessage(1164);
                                TasksEventLog.logEvent("Parafait POS", 'D', card.CardNumber, "LoadBonus - Tech card exceeded limit", "", 3);
                                return false;
                            }
                        }
                    }
                    else if (card != null && gameCardCreditLimit > 0 && card.technician_card != 'Y')
                    {
                        if (bonus != 0)
                        {
                            if (cardCreditsLoading > gameCardCreditLimit)
                            {
                                message = Utilities.MessageUtils.getMessage(1168);
                                return false;
                            }
                        }
                    }
                }

                int loadBonusProductId = productId != null ? Convert.ToInt32(productId) : ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "LOAD_BONUS_TASK_PRODUCT", -1);
                if (loadBonusProductId == -1)
                {
                    throw new Exception("LOAD_BONUS_TASK_PRODUCT not defined");
                }

                ProductsDTO productsDTO = (new Products(loadBonusProductId)).GetProductsDTO;
                if (productsDTO == null || productsDTO.ProductId == -1)
                {
                    throw new Exception("LOAD_BONUS_TASK_PRODUCT not defined");
                }

                Transaction trx = new Transaction(Utilities);
                trx.createTransactionLine(card, loadBonusProductId, 1, ref message);

                //if (gamePlayId > 0)
                //{
                //    trx.TrxLines[0].GameplayId = gamePlayId;
                //}
                if (string.IsNullOrEmpty(remarks) == false)
                {
                    trx.TrxLines[0].Remarks = remarks;
                }
                trx.SaveOrder(ref message, SQLTrx);
                int trxId = trx.Trx_id;
                #endregion

                if (card.isMifare)
                {
                    try
                    {
                        card.updateMifareCard(false, ref message, 0, 0, 0, bonus);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to Update Mifare Card", ex);
                        message = ex.Message;
                        SQLTrx.Rollback();
                        cnn.Close();

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                String AutoExtend = Utilities.ParafaitEnv.AUTO_EXTEND_BONUS_ON_RELOAD;
                if (productsDTO.ProductCreditPlusDTOList != null && productsDTO.ProductCreditPlusDTOList.Any())
                {
                    AutoExtend = productsDTO.ProductCreditPlusDTOList[0].ExtendOnReload;
                }

                int expiresInDays = (bonusType == "B" ? Utilities.ParafaitEnv.LOAD_BONUS_EXPIRY_DAYS : 0);
                try
                {
                    if (expiryDays != null)
                        expiresInDays = Convert.ToInt32(expiryDays);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to calcuate expiry date");
                }

                if (gamePlayId > 0)
                {//CreateTransactionLineGamePlaymapping entry
                    Utilities.executeScalar(@"insert into TransactionLineGamePlayMapping(TrxId, LineId, GamePlayId, IsActive,
                                                       guid, site_id, creationDate, createdBy, LastUpdateDate, LastUpdatedBy) 
                                          values (@TrxId, 1, @gameplayId, 1, newid(), @siteId, getdate(), @user, getdate(), @user)",
                                          SQLTrx,
                                          new SqlParameter("@TrxId", trxId),
                                          new SqlParameter("@gameplayId", gamePlayId),
                                          new SqlParameter("@siteId", Utilities.ParafaitEnv.IsCorporate ? (object)Utilities.ParafaitEnv.SiteId : DBNull.Value),
                                          new SqlParameter("@user", Utilities.ParafaitEnv.LoginID));
                }

                Loyalty loyalty = new Loyalty(Utilities);
                loyalty.CreateGenericCreditPlusLine(card.card_id, bonusType, bonus, Refundable,
                                                    expiresInDays,
                                                    AutoExtend,
                                                    Utilities.ParafaitEnv.LoginID,
                                                    string.IsNullOrEmpty(remarks.Trim()) ? "Load Bonus" : remarks,
                                                    SQLTrx,
                                                    periodFrom,
                                                    trxId,
                                                    1);
                if (AutoExtend == "Y")
                    loyalty.ExtendOnReload(card.card_id, bonusType, SQLTrx);

                trx.SaveTransacation(SQLTrx, ref message);
                createTask(card.card_id, TaskProcs.LOADBONUS, bonus, -1, -1, -1, -1, bonusType[0], gamePlayId, remarks, SQLTrx, -1, -1, -1, -1, trx.Trx_id);

                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading the bonus.", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        internal string GetBonusTypeCodeValue(EntitlementType entitlementType)
        {
            log.LogMethodEntry(entitlementType);
            string bonusType;
            switch (entitlementType)
            {
                case EntitlementType.Bonus: bonusType = "B"; break;
                case EntitlementType.CardBalance:
                    {
                        //    if (Utilities.getParafaitDefaults("LOAD_CREDITS_INSTEAD_OF_CARD_BALANCE").Equals("Y"))
                        //        bonusType = "A";
                        //    else
                        bonusType = "A";
                        break;
                    }
                case EntitlementType.CounterItemsOnly: bonusType = "P"; break;
                case EntitlementType.Credits: bonusType = "A"; break;
                case EntitlementType.GamePlayBonus: bonusType = "B"; break;
                case EntitlementType.GamePlayCredits: bonusType = "G"; break;
                case EntitlementType.LoyaltyPoints: bonusType = "L"; break;
                case EntitlementType.Tickets: bonusType = "T"; break;
                case EntitlementType.License_D: bonusType = "D"; break;
                case EntitlementType.License_E: bonusType = "E"; break;
                case EntitlementType.License_F: bonusType = "F"; break;
                case EntitlementType.License_H: bonusType = "H"; break;
                case EntitlementType.License_I: bonusType = "I"; break;
                case EntitlementType.License_J: bonusType = "J"; break;
                case EntitlementType.License_K: bonusType = "K"; break;
                case EntitlementType.License_N: bonusType = "N"; break;
                case EntitlementType.License_O: bonusType = "O"; break;
                case EntitlementType.License_Q: bonusType = "Q"; break;
                case EntitlementType.License_R: bonusType = "R"; break;
                case EntitlementType.License_S: bonusType = "S"; break;
                case EntitlementType.License_U: bonusType = "U"; break;
                case EntitlementType.License_W: bonusType = "W"; break;
                case EntitlementType.License_X: bonusType = "X"; break;
                case EntitlementType.License_Y: bonusType = "Y"; break;
                case EntitlementType.License_Z: bonusType = "Z"; break;
                default: bonusType = "B"; break;
            }
            log.LogMethodExit(bonusType);
            return bonusType;
        }

        public bool applyDiscount(Card card, SortableBindingList<CardDiscountsDTO> cardDiscountsDTOList, string remarks, ref string message)
        {
            log.LogMethodEntry(card, cardDiscountsDTOList, remarks, message);
            if (cardDiscountsDTOList == null || cardDiscountsDTOList.Any(x => x.IsChanged) == false)
            {
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Discount"));
                return false;
            }
            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();

            try
            {
                foreach (var cardDiscountsDTO in cardDiscountsDTOList)
                {
                    if (cardDiscountsDTO.IsChanged)
                    {
                        int taskId = createTask(card.card_id, TaskProcs.DISCOUNT, cardDiscountsDTO.DiscountId, -1, -1, -1, -1, -1, -1, remarks, SQLTrx);
                        if (cardDiscountsDTO.CardDiscountId == -1)
                        {
                            cardDiscountsDTO.TaskId = taskId;
                        }
                        CardDiscountsBL cardDiscountsBL = new CardDiscountsBL(cardDiscountsDTO);
                        cardDiscountsBL.Save(SQLTrx);
                    }

                }
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to complete the transaction!", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool RedeemLoyalty(Card card, double loyaltyPoints, DataGridView dgvRedeemLoyalty, string remarks, ref string message)
        {
            log.LogMethodEntry(card, loyaltyPoints, dgvRedeemLoyalty, remarks, message);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, true, true);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            AccountDTO accountDTO = accountBL.AccountDTO;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
            if ((accountDTO.AccountCreditPlusDTOList != null
                          && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                            && x.SubscriptionBillingScheduleId == -1
                                                                            || (x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                && x.SubscriptionBillingScheduleId != -1
                                                                                && subscriptionBillingScheduleDTOList != null
                                                                                && subscriptionBillingScheduleDTOList.Any()
                                                                                && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive))) //Ignore subscription holds)
                        )
                       )
            {
                message = Utilities.MessageUtils.getMessage(2610);

                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }
            double mgrApprovalLimit = 0;
            try { mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL_IN_POS")); }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (loyaltyPoints > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1215);

                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }


            bool found = false;
            int index = 0;
            for (int i = 0; i < dgvRedeemLoyalty.Rows.Count; i++)
            {
                if (dgvRedeemLoyalty["Selected", i].Value.ToString() == "Y")
                {
                    found = true;
                    index = i;
                    break;
                }
            }

            if (found)
            {
                string columnName = dgvRedeemLoyalty["DBColumnName", index].Value.ToString();
                double value = 0;

                try
                {
                    try
                    {
                        value = Convert.ToDouble(dgvRedeemLoyalty["Redemption_Value", index].Value);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to get a value for the variable value", ex);
                        log.LogVariableState("value", value);
                    }

                    if (value <= 0)
                    {
                        message = Utilities.MessageUtils.getMessage(493);

                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }

                    loyaltyPoints = value / (Convert.ToDouble(dgvRedeemLoyalty["Rate", index].Value) / Convert.ToDouble(dgvRedeemLoyalty["LoyaltyPoints", index].Value));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to get the  value for loyaltyPoints", ex);

                    message = ex.Message;

                    log.LogVariableState("loyaltyPoints", loyaltyPoints);
                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }

                SqlConnection cnn = Utilities.createConnection();
                SqlTransaction SQLTrx = cnn.BeginTransaction();
                SqlCommand cmd = Utilities.getCommand(SQLTrx);

                try
                {
                    Loyalty loyalty = new Loyalty(Utilities);
                    bool ticketAllowed = IsTicketAllowedForRedeemVirtualPoints();
                    TransactionId = loyalty.RedeemLoyaltyPoints((int)card.card_id, card.CardNumber, loyaltyPoints, columnName, value, Utilities.ParafaitEnv.POSMachine, Utilities.ParafaitEnv.User_Id, SQLTrx, ticketAllowed);

                    createTask(card.card_id, TaskProcs.REDEEMLOYALTY, value, -1, -1, -1, -1, -1, -1, remarks, SQLTrx, -1, -1, -1, -1, TransactionId);
                    SQLTrx.Commit();
                    cnn.Close();

                    log.LogVariableState("message", message);
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while redeeming loyalty", ex);
                    SQLTrx.Rollback();
                    message = ex.Message;
                    cnn.Close();

                    log.LogVariableState("message", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogVariableState("message", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        //Added for redeem bonus for tickets on 16-jun-2017
        public bool RedeemBonusForTicket(Card card, double redeemBonus, int ticketsEligible, string remarks, ref string message)
        {
            log.LogMethodEntry(card, redeemBonus, ticketsEligible, remarks, message);

            string minimumBonus = Utilities.getParafaitDefaults("MINIMUM_BONUS_VALUE_FOR_TICKET_REDEMPTION");
            if (!string.IsNullOrEmpty(minimumBonus))
            {
                double minumumBonusValue = 0;
                try { minumumBonusValue = Convert.ToDouble(minimumBonus); }
                catch (Exception ex)
                {
                    log.Error("Unable to get a valid value for minumumBonusValue", ex);
                    minumumBonusValue = 0;
                    log.LogVariableState("minumumBonusValue", minumumBonusValue);
                }
                if (minumumBonusValue > redeemBonus)
                {
                    message = Utilities.MessageUtils.getMessage(1196, minimumBonus);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            double mgrApprovalLimit = 0;
            try { mgrApprovalLimit = Convert.ToDouble(Utilities.getParafaitDefaults("REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL")); }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (redeemBonus > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1214);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }


            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            //Check redeeming bonus is available in card
            Card objcard = new Card(card.card_id, Utilities.ParafaitEnv.LoginID, Utilities);
            if (redeemBonus > Convert.ToDouble(objcard.bonus + objcard.CreditPlusBonus))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            try
            {
                //Create creditplus line, when tickets are more than zero
                if (ticketsEligible > 0)
                {
                    Loyalty loyalty = new Loyalty(Utilities);
                    loyalty.CreateGenericCreditPlusLine(card.card_id, "T", ticketsEligible, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Redeem Bonus for Ticket", SQLTrx);
                }
                //Code to deduct bonus from creditplus
                double remainingBonus = redeemBonus;
                if (objcard.CreditPlusBonus > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    remainingBonus = creditPlus.DeductGenericCreditPlus(card, "B", redeemBonus, SQLTrx);
                }

                #region  Code to deduct bonus from cards table
                if (remainingBonus > 0)
                {
                    cmd.CommandText = @" IF EXISTS(SELECT 'X' FROM CARDS WHERE card_id = @card_id and ISNULL(Bonus,0) >= @bonus)
                                           BEGIN
                                             UPDATE cards 
                                                SET bonus = isnull(bonus, 0) - @bonus, 
                                                last_update_time = getdate(), 
                                                LastUpdatedBy = @LastUpdatedBy 
                                             WHERE card_id = @card_id
                                           END
                                         ELSE
                                           BEGIN;
                                             THROW 51000, 'Bonus not available..Please Check', 1;
                                            END";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@card_id", card.card_id);
                    cmd.Parameters.AddWithValue("@bonus", remainingBonus);
                    cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                createTask(card.card_id, TaskProcs.REDEEMBONUSFORTICKET, ticketsEligible, -1, redeemBonus, -1, -1, -1, -1, remarks, SQLTrx);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while redeeming bonus for tickets", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool RedeemTicketsForBonus(Card card, int redeemTickets, double bonusEligible, string remarks, ref string message)
        {
            log.LogMethodEntry(card, redeemTickets, bonusEligible, remarks, message);

            double loadBonusLimit = 0;
            try
            { loadBonusLimit = Convert.ToDouble(Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT")); }
            catch (Exception ex)
            {
                log.Error("Unable to get a valid value for loadBonusLimit", ex);
                loadBonusLimit = 0;
                log.LogVariableState("loadBonusLimit", loadBonusLimit);
            }
            if (bonusEligible > loadBonusLimit)
            {
                message = Utilities.MessageUtils.getMessage(43, loadBonusLimit.ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT));

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            int mgrApprovalLimit = 0;
            try
            { mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL")); }
            catch (Exception ex)
            {
                log.Error("Unable to get a avalid value for mgrApprovalLimit", ex);
                mgrApprovalLimit = 0;
                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
            }
            if (mgrApprovalLimit > 0)
            {
                if (redeemTickets > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1213);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            //Check the redeeming tickets available in card
            Card objcard = new Card(card.card_id, Utilities.ParafaitEnv.LoginID, Utilities);

            if (redeemTickets > Convert.ToDouble(objcard.ticket_count + objcard.CreditPlusTickets))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            //modification added on 30-May-2017
            #region Check No of times bonus allowed to Load in a day
            string bonusLoadLimit = Utilities.getParafaitDefaults("MAX_TIME_BONUS_ALLOWED_TO_LOAD");
            if (!string.IsNullOrEmpty(bonusLoadLimit))
            {
                if (Convert.ToInt32(bonusLoadLimit) > 0)
                {
                    DataTable bonusDT = Utilities.executeDataTable("select * from tasks " +
                                                                        "where task_type_id in (select task_type_id from task_type where task_type = 'LOADBONUS') " +
                                                                        "and task_date >=  DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                        "and task_date < 1 + DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                        "and card_id = @cardId", new SqlParameter("@cardId", card.card_id));
                    if (bonusDT != null && bonusDT.Rows.Count > 0)
                    {
                        if (bonusDT.Rows.Count >= Convert.ToInt32(bonusLoadLimit))
                        {
                            message = Utilities.MessageUtils.getMessage(1167);
                            return false;
                        }
                    }
                }
            }
            #endregion
            //end modification added on 30-May-2017

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            try
            {
                double remainingTicktes = redeemTickets;
                if (card.CreditPlusTickets > 0 && redeemTickets > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    remainingTicktes = creditPlus.DeductGenericCreditPlus(card, "T", redeemTickets, SQLTrx);
                }

                #region Code to update tickets, bonus in Card table
                if (remainingTicktes >= 1)
                {
                    cmd.CommandText = @" IF EXISTS(SELECT 'X' FROM CARDS WHERE card_id = @card_id and ISNULL(ticket_count,0) >= @tickets)
                                            BEGIN
                                                UPDATE cards 
                                                    SET ticket_count = isnull(ticket_count, 0) - @tickets, bonus = isnull(bonus, 0) + @bonus,
                                                        last_update_time = getdate(), 
                                                        LastUpdatedBy = @LastUpdatedBy 
                                                    WHERE card_id = @card_id
                                            END
                                          ELSE
                                            BEGIN;
                                                THROW 51000, 'tickets not available..Please Check', 1;
                                            END";
                }
                else //Update only bonus
                {
                    cmd.CommandText = @"UPDATE cards 
                                    SET bonus = isnull(bonus, 0) + @bonus, 
                                        last_update_time = getdate(), 
                                        LastUpdatedBy = @LastUpdatedBy 
                                    WHERE card_id = @card_id";
                }
                #endregion

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@card_id", card.card_id);
                cmd.Parameters.AddWithValue("@bonus", bonusEligible);
                cmd.Parameters.AddWithValue("@tickets", Math.Floor(remainingTicktes)); //Round off to 15.0 when number like 15.4
                cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                cmd.ExecuteNonQuery();
                //End Modification on 16-May-2017

                log.LogVariableState("@card_id", card.card_id);
                log.LogVariableState("@bonus", bonusEligible);
                log.LogVariableState("@tickets", Math.Floor(remainingTicktes)); //Round off to 15.0 when number like 15.4
                log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);

                //Attribute2 value is 2 for RedeemTicketforCredit and 1 for RedeemTicketforBonus
                createTask(card.card_id, TaskProcs.REDEEMTICKETSFORBONUS, bonusEligible, -1, -1, -1, -1, redeemTickets, 1, remarks, SQLTrx);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to perform transaction for Redeem Ticket Bonus", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        //Added on 26 April 2017 for redeeming tickets to credits
        public bool RedeemTicketsForCredit(Card card, int redeemTickets, double creditsEligible, string remarks, ref string message)
        {
            log.LogMethodEntry(card, redeemTickets, creditsEligible, remarks, message);

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            int mgrApprovalLimit = 0;
            try { Convert.ToInt32(Utilities.getParafaitDefaults("REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL")); }
            catch (Exception ex)
            {
                log.Error("Unable to perform REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL", ex);
                mgrApprovalLimit = 0;
            }
            if (mgrApprovalLimit > 0)
            {
                if (redeemTickets > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
                {
                    message = Utilities.MessageUtils.getMessage(1213);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            //Check the redeeming tickets available in card
            Card objcard = new Card(card.card_id, Utilities.ParafaitEnv.LoginID, Utilities);
            if (redeemTickets > (objcard.ticket_count + objcard.CreditPlusTickets))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            //modification added on 30-May-2017
            //Security controls checks guest card credit limits  before loading credits into card
            decimal cardCreditsLoading = Convert.ToDecimal(creditsEligible) + Convert.ToDecimal(card.credits) + Convert.ToDecimal(card.CreditPlusCardBalance) + Convert.ToDecimal(card.CreditPlusCredits);
            if (card != null && gameCardCreditLimit > 0)
            {
                if (creditsEligible != 0)
                {
                    if (cardCreditsLoading > gameCardCreditLimit)
                    {
                        message = Utilities.MessageUtils.getMessage(1168);

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            //end modification added on 30-May-2017

            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction SQLTrx = cnn.BeginTransaction();
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            try
            {
                //Create creditplus
                if (creditsEligible > 0)
                {
                    Loyalty loyalty = new Loyalty(Utilities);
                    loyalty.CreateGenericCreditPlusLine(card.card_id, "A", creditsEligible, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Redeem Tickets for credit", SQLTrx);
                }
                //Deduct Tickets from creditplus
                double remainingTickets = redeemTickets;
                if (card.CreditPlusTickets > 0 && redeemTickets > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    remainingTickets = creditPlus.DeductGenericCreditPlus(card, "T", redeemTickets, SQLTrx);
                }

                #region  Code to deduct tickets from cards table
                if (remainingTickets >= 1)
                {
                    cmd.CommandText = @" IF EXISTS(SELECT 'X' FROM CARDS WHERE card_id = @card_id and ISNULL(ticket_count,0) >= @tickets)
                                            BEGIN
                                                UPDATE cards 
                                                    SET ticket_count = isnull(ticket_count, 0) - @tickets,
                                                        last_update_time = getdate(), 
                                                        LastUpdatedBy = @LastUpdatedBy 
                                                    WHERE card_id = @card_id
                                            END
                                          ELSE
                                            BEGIN;
                                                THROW 51000, 'tickets not available..Please Check', 1;
                                            END";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@card_id", card.card_id);
                    cmd.Parameters.AddWithValue("@tickets", remainingTickets);
                    cmd.Parameters.AddWithValue("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                //Attribute2 value is 2 for RedeemTicketforCredit and 1 for RedeemTicketforBonus
                createTask(card.card_id, TaskProcs.REDEEMTICKETSFORBONUS, creditsEligible, -1, -1, -1, -1, redeemTickets, 2, remarks, SQLTrx);
                SQLTrx.Commit();
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while redeeming tickets for credits", ex);
                SQLTrx.Rollback();
                message = ex.Message;
                cnn.Close();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool ReverseTask(int TaskId, string Remarks, ref string message)
        {
            log.LogMethodEntry(TaskId, Remarks, message);

            string CommandText = @"select top 1 1 
                                from cards c, tasks t, task_type ty
                                where t.card_id = c.card_id
                                and t.task_id = @taskId
                                and t.task_type_id = ty.task_type_id
                                and (c.refund_flag = 'Y'
                                    or exists (select 1
                                                from gameplay g
                                                where g.card_id = c.card_id
                                                and play_date > t.task_date
                                                and ty.task_type = 'LOADBONUS')
                                    or exists (select 1
                                                from redemption r
                                                where c.card_id = r.card_id
                                                and ty.task_type = 'LOADBONUS'
                                                and redeemed_date > t.task_date))";

            if (Utilities.executeScalar(CommandText, new SqlParameter("@taskId", TaskId)) != null)
            {
                message = Utilities.MessageUtils.getMessage(361);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            CommandText = @"select ty.task_type, t.value_loaded, t.card_id, isnull(char(attribute1), 'B') bonusType, ISNULL(t.trxId, attribute1) as attribute1, t.trxId
                                from tasks t, task_type ty
                                where t.task_id = @taskId
                                and t.task_type_id = ty.task_type_id";

            DataTable dt = Utilities.executeDataTable(CommandText, new SqlParameter("@taskId", TaskId));

            switch (dt.Rows[0]["task_type"].ToString())
            {
                case TaskProcs.LOADTICKETS:
                    {
                        // bool returnValueNew = loadTickets(new Card(Convert.ToInt32(dt.Rows[0]["card_id"]), Utilities.ParafaitEnv.LoginID, Utilities), Convert.ToInt32(dt.Rows[0]["value_loaded"]) * -1, Remarks, null, ref message);
                        try
                        {
                            int orginalTrxHeaderId = -1;
                            double ticketsToReverse = Convert.ToInt32(dt.Rows[0]["value_loaded"]);
                            if (dt.Rows[0]["attribute1"] != DBNull.Value)
                                orginalTrxHeaderId = Convert.ToInt32(dt.Rows[0]["attribute1"]);
                            Card cardToReverse = new Card(Convert.ToInt32(dt.Rows[0]["card_id"]), Utilities.ParafaitEnv.LoginID, Utilities);

                            bool returnValueNew = ReverseLoadTickets(cardToReverse, orginalTrxHeaderId, ticketsToReverse, Remarks);
                            log.LogMethodExit(returnValueNew);
                            return returnValueNew;
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                case TaskProcs.LOADBONUS:
                    {
                        try
                        {
                            int orginalTrxHeaderId = -1;
                            double bonusValueToReverse = Convert.ToInt32(dt.Rows[0]["value_loaded"]);
                            if (dt.Rows[0]["trxId"] != DBNull.Value)
                                orginalTrxHeaderId = Convert.ToInt32(dt.Rows[0]["trxId"]);
                            Card cardToReverse = new Card(Convert.ToInt32(dt.Rows[0]["card_id"]), Utilities.ParafaitEnv.LoginID, Utilities);

                            bool returnValueNew = ReverseLoadBonus(cardToReverse, orginalTrxHeaderId, bonusValueToReverse * -1, getEntitlementType(dt.Rows[0]["bonusType"].ToString()), Remarks);
                            log.LogMethodExit(returnValueNew);
                            return returnValueNew;
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                default: message = Utilities.MessageUtils.getMessage(362, dt.Rows[0]["task_type"]); break;
            }

            log.LogVariableState("message ", message);
            log.LogMethodExit(false);
            return false;
        }

        public EntitlementType getEntitlementType(string entitlementType)
        {
            log.LogMethodEntry(entitlementType);

            switch (entitlementType)
            {
                case "C":
                    {
                        log.LogMethodExit(EntitlementType.Credits);
                        return EntitlementType.Credits;
                    }
                case "A":
                    {
                        log.LogMethodExit(EntitlementType.CardBalance);
                        return EntitlementType.CardBalance;
                    }
                case "P":
                    {
                        log.LogMethodExit(EntitlementType.CounterItemsOnly);
                        return EntitlementType.CounterItemsOnly;
                    }
                case "B":
                    {
                        log.LogMethodExit(EntitlementType.GamePlayBonus);
                        return EntitlementType.GamePlayBonus;
                    }
                case "G":
                    {
                        log.LogMethodExit(EntitlementType.GamePlayCredits);
                        return EntitlementType.GamePlayCredits;
                    }
                case "L":
                    {
                        log.LogMethodExit(EntitlementType.LoyaltyPoints);
                        return EntitlementType.LoyaltyPoints;
                    }
                case "T":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.Tickets;
                    }
                case "D":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_D;
                    }
                case "E":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_E;
                    }
                case "F":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_F;
                    }
                case "I":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_I;
                    }
                case "J":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_J;
                    }
                case "K":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_K;
                    }
                case "N":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_N;
                    }
                case "O":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_O;
                    }
                case "Q":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_Q;
                    }
                case "R":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_R;
                    }
                case "S":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_S;
                    }
                case "U":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_U;
                    }
                case "W":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_W;
                    }
                case "X":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_W;
                    }
                case "Y":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_Y;
                    }
                case "Z":
                    {
                        log.LogMethodExit(EntitlementType.Tickets);
                        return EntitlementType.License_Z;
                    }
                default:
                    {
                        log.LogMethodExit(EntitlementType.Bonus);
                        return EntitlementType.Bonus;
                    }
            }

        }

        public bool ConvertCreditsForTime(Card card, double credits, int salesTrxId, int salesLineId, bool conversionRequired, string remarks, ref string message, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(card, credits, salesTrxId, salesLineId, conversionRequired, remarks, message, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;

            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            double cardCredits, creditPlusCardBalance, timePerCredit, finalTimeValue;
            int expiryDays;
            int tokenRoundOffAmountTo = -1;

            try
            {
                if (credits > (card.credits + card.CreditPlusCardBalance + card.CreditPlusCredits))
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                if (credits >= card.credits)
                {
                    cardCredits = card.credits;
                    creditPlusCardBalance = credits - cardCredits;
                }
                else
                {
                    cardCredits = credits;
                    creditPlusCardBalance = 0;
                }

                if (!card.AddCreditsToCard(cardCredits * -1, SQLTrx, ref message, 0))
                {
                    //SQLTrx.Rollback();
                    //cnn.Close();

                    log.LogVariableState("message ", message);
                    //log.LogMethodExit(false);
                    //return false;
                    throw new Exception(message);
                }

                if (creditPlusCardBalance > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    creditPlus.deductCreditPlus(-1, card.card_id, creditPlusCardBalance, null, SQLTrx, -1, Utilities.ParafaitEnv.LoginID);
                }

                try
                {
                    timePerCredit = Convert.ToDouble(Utilities.getParafaitDefaults("TIME_IN_MINUTES_PER_CREDIT"));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to find valid value for timePerCredit", ex);
                    timePerCredit = 0;
                    log.LogVariableState("timePerCredit", timePerCredit);
                }

                if (timePerCredit <= 0)
                {
                    message = Utilities.MessageUtils.getMessage(1339);

                    log.LogVariableState("message ", message);
                    throw new Exception(message);
                }



                if (conversionRequired)
                {
                    finalTimeValue = Convert.ToInt32(credits * timePerCredit);
                    object tokenRoundoffObj = Utilities.executeScalar(@"SELECT TOP 1 LV.LookupValue
                                                                                      FROM Lookups L, LookupValues LV
                                                                                     WHERE L.LookupId = LV.LookupId
                                                                                       AND L.LookupName = 'ALOHA_ROUNDOFF_AMOUNT_TO'");
                    if (tokenRoundoffObj != null && tokenRoundoffObj != DBNull.Value && !String.IsNullOrEmpty(tokenRoundoffObj.ToString()))
                        tokenRoundOffAmountTo = Convert.ToInt32(tokenRoundoffObj);

                    if (tokenRoundOffAmountTo != -1 && tokenRoundOffAmountTo != 0)
                    {
                        finalTimeValue = Math.Round(finalTimeValue, tokenRoundOffAmountTo);
                    }
                    else
                    {
                        finalTimeValue = Math.Round(finalTimeValue, 0);
                    }
                }
                else
                {
                    finalTimeValue = Convert.ToInt32(credits);
                }

                try
                {
                    expiryDays = Convert.ToInt32(Utilities.getParafaitDefaults("CONVERTED_TIME_ENTITLEMENT_VALID_FOR_DAYS"));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to find valid value for expiryDays", ex);
                    expiryDays = 1;
                    log.LogVariableState("expiryDays", expiryDays);
                }

                Loyalty loyalty = new Loyalty(Utilities);
                //Create Time Entitlement
                loyalty.CreateGenericCreditPlusLine(card.card_id, "M", finalTimeValue, false, expiryDays, "N", Utilities.ParafaitEnv.LoginID, "Exchange Credits for Time", SQLTrx, null, salesTrxId, salesLineId);

                createTask(card.card_id, TaskProcs.EXCHANGECREDITFORTIME, finalTimeValue, -1, -credits, -1, -1, -1, -1, remarks, SQLTrx, -1, -1, -1, -1, salesTrxId);

                if (inSQLTrx == null)
                {
                    SQLTrx.Commit();
                    cnn.Close();
                }

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while exchanging credit for time.", ex);

                message = ex.Message;
                if (inSQLTrx == null)
                {
                    SQLTrx.Rollback();
                    cnn.Close();
                }

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool ConvertTimeForCredit(Card card, double time, bool conversionRequired, string remarks, ref string message, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(card, time, conversionRequired, remarks, message, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;

            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, card.card_id, false, false);
            if (accountBL.IsAccountUpdatedByOthers(card.last_update_time))
            {
                message = Utilities.MessageUtils.getMessage(354);

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            double cardTime, creditPlusTime, timePerCredit, finalCreditValue;
            int tokenRoundOffAmountTo = -1;
            try
            {
                Loyalty loyalty = new Loyalty(Utilities);

                if (time > (card.time + card.CreditPlusTime))
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                if (time >= card.CreditPlusTime)
                {
                    creditPlusTime = card.CreditPlusTime;
                    cardTime = time - creditPlusTime;
                }
                else
                {
                    creditPlusTime = time;
                    cardTime = 0;
                }

                if (creditPlusTime > 0)
                {
                    CreditPlus loadTimeCreditPlus = new CreditPlus(Utilities);
                    loadTimeCreditPlus.DeductGenericCreditPlus(card, "M", creditPlusTime, SQLTrx); //Deduct Time entitlement
                }

                if (cardTime > 0)
                {
                    string cmd = @"update cards set time = isnull(time,0) - @time,
                                                last_update_time = getdate(),
                                                LastUpdatedBy = @LastUpdatedBy
                                                where card_id = @card_id
                                                  and isnull(time, 0) - @time >= 0";
                    if (Utilities.executeNonQuery(cmd, SQLTrx,
                                                new SqlParameter("@card_id", card.card_id),
                                                new SqlParameter("@time", cardTime),
                                                new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID)) < 1)
                    {
                        message = Utilities.MessageUtils.getMessage(354);

                        log.LogVariableState("@card_id", card.card_id);
                        log.LogVariableState("@time", cardTime);
                        log.LogVariableState("message ", message);
                        log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                try
                {
                    timePerCredit = Convert.ToDouble(Utilities.getParafaitDefaults("TIME_IN_MINUTES_PER_CREDIT"));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to find valid value for timePerCredit", ex);
                    timePerCredit = 0;
                    log.LogVariableState("timePerCredit", timePerCredit);
                }

                if (timePerCredit <= 0)
                {
                    message = Utilities.MessageUtils.getMessage(1339);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }


                if (conversionRequired)
                {
                    finalCreditValue = Convert.ToDouble(time / timePerCredit);

                    object tokenRoundoffObj = Utilities.executeScalar(@"SELECT TOP 1 LV.LookupValue
                                                                                      FROM Lookups L, LookupValues LV
                                                                                     WHERE L.LookupId = LV.LookupId
                                                                                       AND L.LookupName = 'ALOHA_ROUNDOFF_AMOUNT_TO'");

                    if (tokenRoundoffObj != null && tokenRoundoffObj != DBNull.Value && !String.IsNullOrEmpty(tokenRoundoffObj.ToString()))
                    {
                        tokenRoundOffAmountTo = Convert.ToInt32(tokenRoundoffObj);
                    }

                    if (tokenRoundOffAmountTo != -1)
                    {
                        if (tokenRoundOffAmountTo == 0 || finalCreditValue % tokenRoundOffAmountTo == 0)
                        {
                            finalCreditValue = Convert.ToInt32(finalCreditValue);
                        }
                        else
                        {
                            finalCreditValue = Convert.ToInt32(finalCreditValue + tokenRoundOffAmountTo - (finalCreditValue % tokenRoundOffAmountTo));
                        }
                    }
                }
                else
                {
                    finalCreditValue = time;
                }

                if (!card.AddCreditsToCard(finalCreditValue, SQLTrx, ref message, 0))
                {
                    SQLTrx.Rollback();
                    cnn.Close();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                createTask(card.card_id, TaskProcs.EXCHANGETIMEFORCREDIT, finalCreditValue, -1, finalCreditValue, -1, -1, -1, -1, remarks, SQLTrx);

                if (inSQLTrx == null)
                {
                    SQLTrx.Commit();
                    cnn.Close();
                }

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting time to credit.", ex);

                message = ex.Message;
                if (inSQLTrx == null)
                {
                    SQLTrx.Rollback();
                    cnn.Close();
                }

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        public bool BalanceTransferTime(int sourceCardId, int destinationCardId, double time, string remarks, ref string message, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(sourceCardId, destinationCardId, time, remarks, message, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            try
            {
                double cardTime, creditPlusTime;
                int expiryDays;
                //bool isSourceCardTimeRunning = false;
                DateTime playStartTime;
                Loyalty loyalty = new Loyalty(Utilities);
                Card sourceCard = new Card(sourceCardId, Utilities.ParafaitEnv.LoginID, Utilities);

                if (time > (sourceCard.time + sourceCard.CreditPlusTime))
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                if (time >= sourceCard.CreditPlusTime)
                {
                    creditPlusTime = sourceCard.CreditPlusTime;
                    cardTime = time - creditPlusTime;
                }
                else
                {
                    creditPlusTime = time;
                    cardTime = 0;
                }

                if (creditPlusTime > 0)
                {
                    //isSourceCardTimeRunning = loyalty.IsCreditPlusTimeRunning(sourceCard.card_id, SQLTrx);
                    CreditPlus loadTimeCreditPlus = new CreditPlus(Utilities);
                    loadTimeCreditPlus.DeductGenericCreditPlus(sourceCard, "M", creditPlusTime, SQLTrx); //Deduct Time entitlement
                }

                if (cardTime > 0)
                {
                    string cmd = @"update cards set time = isnull(time,0) - @time,
                                                last_update_time = getdate(),
                                                LastUpdatedBy = @LastUpdatedBy
                                                where card_id = @card_id
                                                  and isnull(time, 0) - @time >= 0";
                    if (Utilities.executeNonQuery(cmd, SQLTrx,
                                                new SqlParameter("@card_id", sourceCard.card_id),
                                                new SqlParameter("@time", cardTime),
                                                new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID)) < 1)
                    {
                        message = Utilities.MessageUtils.getMessage(354);

                        log.LogVariableState("@card_id", sourceCard.card_id);
                        log.LogVariableState("@time", cardTime);
                        log.LogVariableState("message ", message);
                        log.LogVariableState("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                Card destCard = new Card(destinationCardId, Utilities.ParafaitEnv.LoginID, Utilities);

                try
                {
                    expiryDays = Convert.ToInt32(Utilities.getParafaitDefaults("CONVERTED_TIME_ENTITLEMENT_VALID_FOR_DAYS"));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to get valid value for expiryDays", ex);
                    expiryDays = 1;
                    log.LogVariableState("expiryDays", expiryDays);
                }

                //if (isSourceCardTimeRunning)
                //{
                //    if (loyalty.IsCreditPlusTimeRunning(destCard.card_id, SQLTrx))
                //        playStartTime = DateTime.MinValue;
                //    else
                //        playStartTime = Utilities.getServerTime();
                //}
                //else
                playStartTime = DateTime.MinValue;
                //Create Time Entitlement
                loyalty.CreateGenericCreditPlusLine(destCard.card_id, "M", time, false, expiryDays, "N", Utilities.ParafaitEnv.LoginID, "Balance Transfer of Time", SQLTrx, playStartTime);

                createTask(sourceCardId, TaskProcs.BALANCETRANSFERTIME, time, destinationCardId, -1, -1, -1, -1, -1, remarks, SQLTrx);

                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error oured when balancing transfer time.", ex);

                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        public bool PauseTimeEntitlement(int cardId, string remarks, ref string message, SqlTransaction inSQLTrx = null)
        {
            bool returnStatus = false;
            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            try
            {
                Card trxCard = new Card(cardId, Utilities.ParafaitEnv.LoginID, Utilities);
                AccountBL accountBLCreditPlus = new AccountBL(Utilities.ExecutionContext, trxCard.card_id, true, false);
                if (!accountBLCreditPlus.CardHasCreditPlus(CreditPlusType.TIME))
                {
                    message = Utilities.MessageUtils.getMessage(1841);
                    log.LogMethodExit("Card is not active.");
                    log.LogMethodExit("Fresh card is not loaded with credit plus so card is not active.");
                    return false;
                }
                if ((trxCard.time + trxCard.CreditPlusTime) > 0)
                {
                    Loyalty trxLoyalty = new Loyalty(Utilities);
                    bool isTimeRunning = trxLoyalty.IsCreditPlusTimeRunning(cardId, SQLTrx);
                    if (isTimeRunning)
                    {
                        if (!CheckTimePauseLimit(cardId, ref message))
                            return false;

                        AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, cardId, false, true, SQLTrx);
                        bool canPauseRunningTime = accountBL.CanPauseRunningCreditPlusTime(SQLTrx);
                        if (canPauseRunningTime)
                        {
                            CreditPlus trxCreditPlus = new CreditPlus(Utilities);
                            DataTable getTimeDetails = trxCreditPlus.getCreditPlusDetails(cardId, "M", SQLTrx);
                            if (getTimeDetails.Rows.Count == 0)
                            {
                                returnStatus = false;
                                message = Utilities.MessageUtils.getMessage(1386);
                                return returnStatus;

                            }
                            else
                            {
                                foreach (DataRow dr in getTimeDetails.Rows)
                                {
                                    if (dr["PlayStartTime"] != null && dr["PlayStartTime"] != DBNull.Value && Convert.ToDateTime(dr["PlayStartTime"]) != DateTime.MinValue && Convert.ToInt32(dr["CreditPlusBalance"]) > 0)
                                    {
                                        CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO = new CardCreditPlusPauseTimeLogDTO(-1,
                                                 Convert.ToInt32(dr["CardCreditPlusId"]), Convert.ToDateTime(dr["PlayStartTime"]), Utilities.getServerTime(),
                                                 Convert.ToDouble(dr["CreditPlusBalance"]), "Pause Time", Utilities.ParafaitEnv.POSMachine);
                                        CardCreditPlusPauseTimeLogBL cardCreditPlusPauseTimeLogBL = new CardCreditPlusPauseTimeLogBL(Utilities.ExecutionContext, cardCreditPlusPauseTimeLogDTO);
                                        cardCreditPlusPauseTimeLogBL.Save(SQLTrx);
                                        Utilities.executeNonQuery(@"UPDATE CardCreditPlus
                                                                   SET PlayStartTime = NULL,
                                                                       CreditPlusBalance = @CreditPlusBalance,
                                                                       lastUpdatedDate = getdate(),
                                                                       LastUpdatedBy = @loginId
                                                                 WHERE CardCreditPlusId = @CardCreditPlusId", SQLTrx,
                                                                     new SqlParameter("@CardCreditPlusId", dr["CardCreditPlusId"]),
                                                                     new SqlParameter("@CreditPlusBalance", dr["CreditPlusBalance"]),
                                                                     new SqlParameter("@loginId", Utilities.ParafaitEnv.LoginID));
                                    }
                                }
                                Card updateCard = new Card((int)cardId, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                                updateCard.updateCardTime(SQLTrx);
                                createTask(cardId, TaskProcs.PAUSETIMEENTITLEMENT, -1, -1, -1, -1, -1, -1, -1, remarks, SQLTrx);
                                returnStatus = true;
                            }
                        }
                        else
                        {
                            returnStatus = false;
                            message = Utilities.MessageUtils.getMessage(1706);
                            return returnStatus;
                        }
                    }
                    else
                    {
                        returnStatus = false;
                        message = Utilities.MessageUtils.getMessage(1386);
                        return returnStatus;
                    }
                }
                else
                {
                    returnStatus = false;
                    message = Utilities.MessageUtils.getMessage(1838);
                    return returnStatus;
                }
                if (inSQLTrx == null)
                    SQLTrx.Commit();

                return returnStatus;
            }
            catch (Exception ex)
            {
                if (inSQLTrx == null)
                    SQLTrx.Rollback();
                message = ex.Message;
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        public bool isTimePaused(int cardId, ref string message)
        {
            CardCPPauseTimeLogList isTimePausedLogList = new CardCPPauseTimeLogList();
            List<CardCreditPlusPauseTimeLogDTO> isTimePausedLogDTOList = new List<CardCreditPlusPauseTimeLogDTO>();
            isTimePausedLogDTOList = isTimePausedLogList.GetAllCardCPPauseTimeLogByCardId(cardId);
            if (isTimePausedLogDTOList != null)
            {
                object playStartTime = Utilities.executeScalar(@"SELECT PlayStartTime
                                                                           FROM CardCreditPlus
                                                                          WHERE CreditPlusType = 'M'
                                                                            AND card_id = @CardId
                                                                   ORDER BY PlayStartTime desc",
                                                                         new SqlParameter("@CardId", cardId));

                List<CardCreditPlusPauseTimeLogDTO> pauseTimeLogDTOFilteredList = new List<CardCreditPlusPauseTimeLogDTO>();
                //Get list of DTO for number of times Pause was performed in a day for the same card
                pauseTimeLogDTOFilteredList = isTimePausedLogDTOList.FindAll(x => x.PauseStartTime != DateTime.MinValue
                                                                              && x.PauseStartTime >= Utilities.getServerTime().Date.AddHours(6)
                                                                              && x.PauseStartTime < Utilities.getServerTime().Date.AddDays(1).AddHours(6));
                if (pauseTimeLogDTOFilteredList.Count > 0)
                {
                    bool found = false;
                    foreach (CardCreditPlusPauseTimeLogDTO pauseTimeLogDTOFilteredDTO in pauseTimeLogDTOFilteredList)
                    {
                        if (playStartTime != null && playStartTime != DBNull.Value)
                        {
                            if (DateTime.Compare(Convert.ToDateTime(playStartTime), pauseTimeLogDTOFilteredDTO.PauseStartTime) > 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                        return true;
                }
                else
                    return false;
            }
            return false;
        }

        public bool CheckTimePauseLimit(int cardId, ref string message)
        {
            string maxPauseTimeAllowed;
            maxPauseTimeAllowed = Utilities.getParafaitDefaults("MAX_ALLOWED_TIME_PAUSE");
            if (!String.IsNullOrEmpty(maxPauseTimeAllowed))
            {
                CardCPPauseTimeLogList pauseTimeLogList = new CardCPPauseTimeLogList();
                List<CardCreditPlusPauseTimeLogDTO> pauseTimeLogDTOList = new List<CardCreditPlusPauseTimeLogDTO>();
                pauseTimeLogDTOList = pauseTimeLogList.GetAllCardCPPauseTimeLogByCardId(cardId);
                if (pauseTimeLogDTOList != null)
                {
                    List<CardCreditPlusPauseTimeLogDTO> pauseTimeLogDTOFilteredList = new List<CardCreditPlusPauseTimeLogDTO>();
                    //Get list of DTO for number of times Pause was performed in a day for the same card
                    pauseTimeLogDTOFilteredList = pauseTimeLogDTOList.FindAll(x => x.PauseStartTime != DateTime.MinValue
                                                                                  && x.PauseStartTime >= Utilities.getServerTime().Date.AddHours(6)
                                                                                  && x.PauseStartTime < Utilities.getServerTime().Date.AddDays(1).AddHours(6));
                    if (pauseTimeLogDTOFilteredList.Count >= Convert.ToInt32(maxPauseTimeAllowed))
                    {
                        message = Utilities.MessageUtils.getMessage(1387);
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            else
                return true;
        }
        public bool BalanceTransfer(int SourceCardId, int DestinationCardId, decimal Credits, decimal Bonus, decimal Courtesy, decimal Tickets, string remarks, ref string message, SqlTransaction inSQLTrx = null, int transactionId = -1)
        {
            log.LogMethodEntry(SourceCardId, DestinationCardId, Credits, Bonus, Courtesy, Tickets, remarks, message, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            try
            {
                AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, SourceCardId, true, true, SQLTrx);
                AccountBL destAccountBL = new AccountBL(Utilities.ExecutionContext, DestinationCardId, true, true, SQLTrx);
                //Card trxCard = new Card(SourceCardId, Utilities.ParafaitEnv.LoginID, Utilities, SQLTrx);
                //if (Convert.ToDecimal(trxCard.credits) - Credits < 0)
                if (Convert.ToDecimal(accountBL.AccountDTO.TotalCreditsBalance) - Credits < 0)
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                if (Convert.ToDecimal(accountBL.AccountDTO.TotalBonusBalance) - Bonus < 0)
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }
                if (Convert.ToDecimal(accountBL.AccountDTO.TotalTicketsBalance) - Tickets < 0)
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                #region Staff card Transfer Limit check added on 25-Mar-17

                if (transferLimit > 0 || staffCreditLimit > 0 || gameCardCreditLimit > 0)
                {
                    if (Convert.ToDecimal(transferLimit) > 0)
                    {
                        if (accountBL.AccountDTO != null && accountBL.AccountDTO.TechnicianCard.Equals("Y"))
                        {
                            if (Credits != 0)
                            {
                                if (Credits > Convert.ToDecimal(transferLimit))
                                {
                                    message = Utilities.MessageUtils.getMessage(1165);
                                    TasksEventLog.logEvent("Parafait POS", 'D', accountBL.AccountDTO.TagNumber, "BalanceTransfer - Tech card exceeded transfer limit", "", 3);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                    }
                    if (staffCreditLimit > 0 || gameCardCreditLimit > 0)
                    {
                        //Card card = new Card(DestinationCardId, Utilities.ParafaitEnv.LoginID, Utilities);
                        if (destAccountBL.AccountDTO != null && staffCreditLimit > 0 && destAccountBL.AccountDTO.TechnicianCard.Equals("Y"))
                        {
                            if (Credits != 0)
                            {
                                if ((Credits + Convert.ToDecimal(destAccountBL.AccountDTO.TotalCreditsBalance)) > staffCreditLimit)
                                {
                                    message = Utilities.MessageUtils.getMessage(1164);
                                    TasksEventLog.logEvent("Parafait POS", 'D', destAccountBL.AccountDTO.TagNumber, "BalanceTransfer - Tech card exceeded credit limit", "", 3);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                        else if (destAccountBL.AccountDTO != null && gameCardCreditLimit > 0 && !destAccountBL.AccountDTO.TechnicianCard.Equals("Y"))
                        {
                            if (Credits != 0)
                            {
                                if ((Credits + Convert.ToDecimal(destAccountBL.AccountDTO.TotalCreditsBalance)) > gameCardCreditLimit)
                                {
                                    message = Utilities.MessageUtils.getMessage(1168);
                                    log.LogMethodExit(false);
                                    return false;
                                }
                            }
                        }
                    }
                }

                #endregion

                //AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, SourceCardId, false, false, SQLTrx);
                if (accountBL.IsAccountUpdatedByOthers(accountBL.AccountDTO.LastUpdateDate, SQLTrx))
                {
                    message = Utilities.MessageUtils.getMessage(354);

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                if (Credits > 0)
                {
                    entitlements.Add(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE), Credits);
                }
                if (Bonus > 0)
                {
                    entitlements.Add(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS), Bonus);
                }
                if (Tickets > 0)
                {
                    entitlements.Add(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET), Tickets);
                }

                // remove invalid credit plus lines for balance transfer
                if (transactionId == -1)
                {
                    accountBL.RemoveInvalidCreditPlusLines();
                }

                if (!TranferEntitlementBalance(accountBL, destAccountBL, entitlements, remarks, ref message, transactionId, -1, SQLTrx))
                {
                    if (inSQLTrx == null)
                        SQLTrx.Rollback();
                    message = Utilities.MessageUtils.getMessage(message);
                    return false;
                }




                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to Transfer Balance", ex);
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        public bool LinkChildCard(int ParentCardId, SqlTransaction SQLTrx, params int[] ChildCardId)
        {
            log.LogMethodEntry(ParentCardId, SQLTrx, ChildCardId);

            foreach (int childCardId in ChildCardId)
            {
                if (Utilities.executeScalar(@"select  1 from ParentChildCards where ParentCardId =@child or ChildCardId = @parent", SQLTrx,
                    new SqlParameter("@child", childCardId), new SqlParameter("@parent", ParentCardId)) != null)
                {
                    continue;
                }
                else if (Utilities.executeNonQuery(@"update ParentChildCards 
                                                set ActiveFlag = 1,
                                                    LastUpdatedDate = getdate(),
                                                    LastUpdatedby = @user
                                                where ParentCardId = @parent 
                                                and ChildCardId = @child",
                                                        SQLTrx,
                                                        new SqlParameter("@parent", ParentCardId),
                                                        new SqlParameter("@child", childCardId),
                                                        new SqlParameter("@user", Utilities.ParafaitEnv.LoginID)) == 0)
                {
                    Utilities.executeNonQuery(@"insert into ParentChildCards
                                                (ParentCardId, ChildCardId, ActiveFlag, CreationDate, CreatedBy, LastUpdatedDate, LastUpdatedby)
                                                    values 
                                                (@parent, @child, 1, getdate(), @user, getdate(), @user)",
                                                SQLTrx,
                                                new SqlParameter("@parent", ParentCardId),
                                                new SqlParameter("@child", childCardId),
                                                new SqlParameter("@user", Utilities.ParafaitEnv.LoginID));

                    log.LogVariableState("@parent", ParentCardId);
                    log.LogVariableState("@child", childCardId);
                    log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);

                }
            }

            log.LogMethodExit(true);
            return true;
        }

        public bool LinkChildCard(int ParentCardId, params int[] ChildCardId)
        {
            log.LogMethodEntry(ParentCardId, ChildCardId);

            bool returnValue = LinkChildCard(ParentCardId, null, ChildCardId);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public bool DeLinkChildCard(int ParentCardId, params int[] ChildCardId)
        {
            log.LogMethodEntry(ParentCardId, ChildCardId);

            foreach (int childCardId in ChildCardId)
            {
                Utilities.executeNonQuery(@"update ParentChildCards 
                                            set ActiveFlag = 0,
                                                LastUpdatedDate = getdate(),
                                                LastUpdatedby = @user
                                            where ParentCardId = @parent 
                                            and ChildCardId = @child",
                                                new SqlParameter("@parent", ParentCardId),
                                                new SqlParameter("@child", childCardId),
                                                new SqlParameter("@user", Utilities.ParafaitEnv.LoginID));


                log.LogVariableState("@parent", ParentCardId);
                log.LogVariableState("@child", childCardId);
                log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);

            }

            log.LogMethodExit(true);
            return true;
        }

        public bool SetChildSiteCode(Card card, int siteCode, ref string message)
        {
            log.LogMethodEntry(card, siteCode, message);

            try
            {
                createTask(card.card_id, TaskProcs.SETCHILDSITECODE, -1, -1, -1, -1, -1, siteCode, -1, "", null);

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to careate new Task", ex);
                message = ex.Message;

                log.LogMethodExit(false);
                return false;
            }
        }

        public void getCardDetails(Card CurrentCard, ref DataGridView dataGridViewCardDetails)
        {
            log.LogMethodEntry(CurrentCard, dataGridViewCardDetails);

            dataGridViewCardDetails.EnableHeadersVisualStyles = false;
            dataGridViewCardDetails.ColumnHeadersVisible = true;
            dataGridViewCardDetails.RowHeadersVisible = false;
            dataGridViewCardDetails.ReadOnly = true;
            dataGridViewCardDetails.AllowUserToAddRows = false;
            dataGridViewCardDetails.AutoSize = true;
            dataGridViewCardDetails.Height = 40;
            dataGridViewCardDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCardDetails.BorderStyle = BorderStyle.None;
            dataGridViewCardDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCardDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewCardDetails.Rows.Clear();
            dataGridViewCardDetails.Columns.Clear();

            if (CurrentCard == null)
            {
                log.LogMethodEntry(null);
                return;
            }


            dataGridViewCardDetails.Columns.Add("Card_Number", "Card Number");
            dataGridViewCardDetails.Columns.Add("Issue_Date", "Issue Date");
            dataGridViewCardDetails.Columns.Add("Credits", "Credits");
            dataGridViewCardDetails.Columns.Add("Courtesy", "Courtesy");
            dataGridViewCardDetails.Columns.Add("Bonus", "Bonus");
            dataGridViewCardDetails.Columns.Add("Time", "Time");
            dataGridViewCardDetails.Columns.Add("Tickets", Utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT);
            dataGridViewCardDetails.Columns.Add("Used_Credits", "Spent");

            log.LogVariableState("Card_Number", "Card Number");
            log.LogVariableState("Issue_Date", "Issue Date");
            log.LogVariableState("Credits", "Credits");
            log.LogVariableState("Courtesy", "Courtesy");
            log.LogVariableState("Bonus", "Bonus");
            log.LogVariableState("Time", "Time");
            log.LogVariableState("Tickets", Utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT);
            log.LogVariableState("Used_Credits", "Spent");

            for (int i = 0; i < dataGridViewCardDetails.Columns.Count; i++)
            {
                dataGridViewCardDetails.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewCardDetails.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dataGridViewCardDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dataGridViewCardDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dataGridViewCardDetails.DefaultCellStyle.BackColor = Color.White;
            dataGridViewCardDetails.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewCardDetails.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridViewCardDetails.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridViewCardDetails.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(Utilities.getFont(), System.Drawing.FontStyle.Bold);
            dataGridViewCardDetails.DefaultCellStyle.Font = Utilities.getFont();
            dataGridViewCardDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridViewCardDetails.Rows.Add();
            dataGridViewCardDetails.Rows[0].Cells[0].Value = CurrentCard.CardNumber;
            dataGridViewCardDetails.Rows[0].Cells[1].Value = CurrentCard.issue_date.ToString(Utilities.getDateTimeFormat());
            dataGridViewCardDetails.Rows[0].Cells[2].Value = (CurrentCard.credits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits).ToString(Utilities.getAmountFormat());
            dataGridViewCardDetails.Rows[0].Cells[3].Value = CurrentCard.courtesy.ToString(Utilities.getAmountFormat());
            dataGridViewCardDetails.Rows[0].Cells[4].Value = (CurrentCard.bonus + CurrentCard.CreditPlusBonus).ToString(Utilities.getAmountFormat());
            dataGridViewCardDetails.Rows[0].Cells[5].Value = (CurrentCard.time + CurrentCard.CreditPlusTime).ToString(Utilities.getAmountFormat());
            dataGridViewCardDetails.Rows[0].Cells[6].Value = (CurrentCard.ticket_count + CurrentCard.CreditPlusTickets).ToString(Utilities.getNumberFormat());
            dataGridViewCardDetails.Rows[0].Cells[7].Value = CurrentCard.credits_played.ToString(Utilities.getNumberFormat());

            dataGridViewCardDetails.Refresh();
            log.LogMethodExit(null);
        }

        void UpdateRedemptionTicketAllocation(int redemptionTicketAllocationId, int trxId, int lineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionTicketAllocationId, trxId, lineId, sqlTrx);

            SqlParameter[] sqlParam = new SqlParameter[3];
            sqlParam[0] = new SqlParameter("@trxId", trxId);
            sqlParam[1] = new SqlParameter("@lineId", lineId);
            sqlParam[2] = new SqlParameter("@redemptionTicketAllocationId", redemptionTicketAllocationId);

            Utilities.executeNonQuery("Update redemptionticketAllocation SET trxId = @trxId, trxLineId = @lineId where id = @redemptionTicketAllocationId ", sqlTrx, sqlParam);
            log.LogMethodExit();
        }

        bool ReverseLoadTickets(Card card, int orginalTrxHeaderId, double ticketsToReverse, string Remarks)
        {
            log.LogMethodEntry(card, orginalTrxHeaderId, ticketsToReverse, Remarks);
            bool retValue = false;
            SqlConnection cnn = Utilities.createConnection();
            SqlTransaction sqlTrx;
            SqlCommand cmd;
            sqlTrx = cnn.BeginTransaction();
            cmd = Utilities.getCommand(sqlTrx);
            try
            {
                if (orginalTrxHeaderId == -1) //old task transaction
                {
                    Loyalty Loyalty = new Loyalty(Utilities);
                    Loyalty.CreateGenericCreditPlusLine(card.card_id, "T", ticketsToReverse * -1, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Reverse LoadTickets: " + Remarks, sqlTrx, null);
                    createTask(card.card_id, TaskProcs.LOADTICKETS, ticketsToReverse * -1, -1, -1, -1, -1, -1, -1, "Reverse LoadTickets: " + Remarks, sqlTrx);
                    retValue = true;
                }
                else
                {
                    string message = "";
                    if (!TransactionUtils.reverseTransaction(orginalTrxHeaderId, -1, true, (Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : Utilities.ParafaitEnv.POSMachine), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.User_Id, Utilities.ParafaitEnv.LoginID, "Reverse LoadTickets: " + Remarks, ref message))
                    {
                        log.Error(message);
                        throw new Exception(message);
                    }
                    else
                    {
                        retValue = true;
                    }
                }
                if (retValue)
                {
                    sqlTrx.Commit();
                }
                else
                {
                    sqlTrx.Rollback();
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                sqlTrx.Rollback();
                cnn.Close();
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
        private bool ReverseLoadBonus(Card card, int orginalTrxHeaderId, double bonusValueToReverser, EntitlementType bonusType, string Remarks)
        {
            log.LogMethodEntry(card, orginalTrxHeaderId, bonusValueToReverser, bonusType, Remarks);
            bool retValue = false;
            string message = string.Empty;
            try
            {
                if (orginalTrxHeaderId == -1) //old task transaction
                {
                    retValue = (loadBonus(card, bonusValueToReverser, bonusType, true, -1, Remarks, ref message));

                }
                else
                {
                    retValue = TransactionUtils.reverseTransaction(orginalTrxHeaderId, -1, true, (Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : Utilities.ParafaitEnv.POSMachine), Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.User_Id, Utilities.ParafaitEnv.LoginID, "Reverse LoadBonus: " + Remarks, ref message);
                }
                if (retValue == false)
                {
                    log.Error(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        void TransferTickets(int sourceCardId, int destinationCardId, double transferTickets, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sourceCardId, destinationCardId, transferTickets, sqlTrx);
            Card sourceCard = new Card(sourceCardId, Utilities.ParafaitEnv.LoginID, Utilities, sqlTrx);
            Card destinationCard = new Card(destinationCardId, Utilities.ParafaitEnv.LoginID, Utilities, sqlTrx);
            double balanceTickets = transferTickets;
            SqlCommand cmd = Utilities.getCommand(sqlTrx);
            cmd.CommandText = @"update cards 
                                      set ticket_count = ISNULL(ticket_count,0) - @balanceTickets, lastUpdatedBy = @loginId, last_update_time = getdate()
                                     where card_id = @cardId";
            Loyalty loyalty = new Loyalty(Utilities);
            if ((sourceCard.ticket_count + sourceCard.CreditPlusTickets) < balanceTickets)
            {
                throw new Exception(Utilities.MessageUtils.getMessage(746));
            }
            if (sourceCard.ticket_count >= balanceTickets)
            {
                //Update cards 
                cmd.Parameters.Add(new SqlParameter("@balanceTickets", balanceTickets));
                cmd.Parameters.Add(new SqlParameter("@loginId", Utilities.ParafaitEnv.LoginID));
                cmd.Parameters.Add(new SqlParameter("@cardId", sourceCard.card_id));
                cmd.ExecuteNonQuery();
                loyalty.CreateGenericCreditPlusLine(destinationCard.card_id, "T", balanceTickets, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Balance Transfer", sqlTrx);
                balanceTickets = 0;
            }
            else
            {
                if (sourceCard.ticket_count >= 1)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@balanceTickets", sourceCard.ticket_count));
                    cmd.Parameters.Add(new SqlParameter("@loginId", Utilities.ParafaitEnv.LoginID));
                    cmd.Parameters.Add(new SqlParameter("@cardId", sourceCard.card_id));
                    cmd.ExecuteNonQuery();
                    loyalty.CreateGenericCreditPlusLine(destinationCard.card_id, "T", sourceCard.ticket_count, false, 0, "N", Utilities.ParafaitEnv.LoginID, "Balance Transfer", sqlTrx);
                    balanceTickets = balanceTickets - sourceCard.ticket_count;
                }
                if (balanceTickets > 0)
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    DataTable dtSourceCreditPlus = creditPlus.getCreditPlusDetails(sourceCard.card_id, "T", sqlTrx);
                    if (dtSourceCreditPlus.Rows.Count > 0)
                    {
                        foreach (DataRow sourceCreditPlusRow in dtSourceCreditPlus.Rows)
                        {
                            double creditPlusBalanceOnCPLine = Convert.ToDouble(sourceCreditPlusRow["CreditPlusBalance"]);
                            int cpLineId = Convert.ToInt32(sourceCreditPlusRow["CardCreditPlusId"]);
                            if (creditPlusBalanceOnCPLine > 0)
                            {
                                if (creditPlusBalanceOnCPLine >= balanceTickets && balanceTickets > 0)
                                {
                                    //update cards
                                    creditPlus.TransferCreditPlus(sourceCard.card_id, destinationCard.card_id, cpLineId, balanceTickets, sqlTrx);
                                    balanceTickets = 0;
                                    break;
                                }
                                else
                                {
                                    if (balanceTickets > 0)
                                    {
                                        //update cards
                                        creditPlus.TransferCreditPlus(sourceCard.card_id, destinationCard.card_id, cpLineId, creditPlusBalanceOnCPLine, sqlTrx);
                                        balanceTickets = balanceTickets - creditPlusBalanceOnCPLine;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (balanceTickets > 0)
                    {
                        throw new Exception(Utilities.MessageUtils.getMessage(746));
                    }
                }
            }
            log.LogMethodExit();
        }

        public bool UpdateTask(int taskId, decimal Credits, decimal Bonus, decimal Courtesy, decimal Tickets, string remarks, ref string message, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(taskId, Credits, Bonus, Courtesy, Tickets, remarks, message, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            try
            {
                Utilities.executeNonQuery(@"update tasks set credits = @credits, bonus = @bonus, courtesy = @courtesy, tickets = @tickets
                                            where task_id = @taskId", SQLTrx,
                                            new SqlParameter("@credits", Credits),
                                            new SqlParameter("@bonus", Bonus),
                                            new SqlParameter("@courtesy", Courtesy),
                                            new SqlParameter("@tickets", Tickets),
                                            new SqlParameter("@taskId", taskId));

                log.LogVariableState("@credits", Credits);
                log.LogVariableState("@bonus", Bonus);
                log.LogVariableState("@courtesy", Courtesy);
                log.LogVariableState("@tickets", Tickets);
                log.LogVariableState("@taskId", taskId);

                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to Transfer Balance", ex);
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        public bool BalanceTransferWithCreditPlus(int SourceCardId, List<int> DestinationCards, decimal Credits, decimal Bonus, decimal Courtesy, decimal Tickets,
            string remarks, ref string message, int transactionId, int transactionLineId, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(SourceCardId, DestinationCards, Credits, Bonus, Courtesy, Tickets, remarks, message, transactionId, transactionLineId, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;


            AccountBL parentCardBL = new AccountBL(Utilities.ExecutionContext, SourceCardId, true, true, SQLTrx);
            if (!parentCardBL.TransferCreditPlusAndGameEntitlementsForSplitProduct(DestinationCards, transactionId, transactionLineId, SQLTrx))
            {
                log.Error("Unable to Transfer Credit Plus and Games Balance", new Exception(message));
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            int creditsShare = (int)(Credits / (1 + DestinationCards.Count));
            int bonusShare = (int)(Bonus / (1 + DestinationCards.Count));
            int courtesyShare = (int)(Courtesy / (1 + DestinationCards.Count));
            int ticketsShare = (int)(Tickets / (1 + DestinationCards.Count));

            try
            {
                for (int i = 0; i < DestinationCards.Count; i++)
                {
                    if (!BalanceTransfer(SourceCardId, DestinationCards[i], creditsShare, bonusShare, courtesyShare, ticketsShare, remarks, ref message, SQLTrx, transactionId))
                    {
                        log.Error("Unable to Transfer Balance", new Exception(message));
                        if (inSQLTrx == null)
                            SQLTrx.Rollback();

                        log.LogVariableState("message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }



                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to Transfer Balance", ex);
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        /// <summary>
        /// Transfer entitlement amount from one card to another.
        /// </summary>
        /// <param name="parentCardBL"></param>
        /// <param name="destinationCardBL"></param>
        /// <param name="entitlementsToTransfer"></param>
        /// <param name="remarks"></param>
        /// <param name="message"></param>
        /// <param name="transactionId"></param>
        /// <param name="transactionLineId"></param>
        /// <param name="inSQLTrx"></param>
        public bool TranferEntitlementBalance(AccountBL parentCardBL, AccountBL destinationCardBL, Dictionary<string, decimal> entitlementsToTransfer,
            string remarks, ref string message, int transactionId, int transactionLineId, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(parentCardBL, destinationCardBL, entitlementsToTransfer, remarks, message, transactionId, transactionLineId, inSQLTrx);

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;
            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            try
            {
                if (!parentCardBL.TransferEntitlement(destinationCardBL.AccountDTO, transactionId, transactionLineId, entitlementsToTransfer, null, SQLTrx))
                {
                    log.Error("Unable to Transfer Credit Plus and Games Balance", new Exception(message));
                    if (inSQLTrx == null)
                        SQLTrx.Rollback();

                    log.LogVariableState("message ", message);
                    log.LogMethodExit(false);
                    return false;
                }

                int taskId = createTask(parentCardBL.AccountDTO.AccountId, TaskProcs.BALANCETRANSFER, -1, destinationCardBL.AccountDTO.AccountId, -1, -1, -1, -1, -1, remarks, SQLTrx, -1, -1, -1, -1, transactionId);
                decimal Credits = 0.0M, Bonus = 0.0M, Courtesy = 0.0M, Tickets = 0.0M;
                decimal cpCardBalance = 0.0M, cpGamePlayCredits = 0.0M, cpCounterItem = 0.0M, totalCredits = 0.0M, totalBonus = 0.0M, totalTickets = 0.0M;

                List<AccountCreditPlusDTO> destinationCreditPlusDTOList = null;
                destinationCreditPlusDTOList = destinationCardBL.AccountDTO.AccountCreditPlusDTOList;
                if (destinationCreditPlusDTOList != null && transactionId != -1)
                    destinationCreditPlusDTOList = destinationCreditPlusDTOList.Where(
                       x => (x.TransactionId == transactionId)).ToList();

                foreach (KeyValuePair<string, decimal> entitlement in entitlementsToTransfer)
                {
                    string entitlementType = entitlement.Key;
                    decimal entitlementTransferAmount = entitlement.Value;
                    if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE)))
                    {
                        Credits = entitlementTransferAmount;
                    }
                    else if (entitlementType.Equals("C"))
                    {
                        Courtesy = entitlementTransferAmount;
                    }
                    else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS)))
                    {

                        Bonus = entitlementTransferAmount;
                    }
                    else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET)))
                    {
                        Tickets = entitlementTransferAmount;
                    }
                }

                if (destinationCreditPlusDTOList != null)
                {
                    cpCardBalance = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.CARD_BALANCE) == true ?
                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.CARD_BALANCE).CreditPlusBalance) : 0);
                    cpGamePlayCredits = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT) == true ?
                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT).CreditPlusBalance) : 0);
                    cpCounterItem = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.COUNTER_ITEM) == true ?
                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.COUNTER_ITEM).CreditPlusBalance) : 0);
                    //totalCredits = Credits + cpCardBalance + cpGamePlayCredits + cpCounterItem;
                    totalBonus = Bonus + (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS) == true ?
                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS).CreditPlusBalance) : 0);
                    totalTickets = Tickets + (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.TICKET) == true ?
                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.TICKET).CreditPlusBalance) : 0);

                }

                remarks = remarks + "," + " Credits = " + Credits + " CreditPlus CardBalance =" + cpCardBalance + " CreditPlus GamePlayCredits = "
                    + cpGamePlayCredits + " CreditPlus Counter Items = " + cpCounterItem + " Bonus = " + totalBonus + " Tickets = " + totalTickets;


                Utilities.executeNonQuery(@"update tasks set credits = @credits, bonus = @bonus, courtesy = @courtesy, tickets = @tickets ,remarks = @remarks
                                            where task_id = @taskId", SQLTrx,
                                            new SqlParameter("@credits", Credits),
                                            new SqlParameter("@bonus", Bonus),
                                            new SqlParameter("@courtesy", Courtesy),
                                            new SqlParameter("@tickets", Tickets),
                                            new SqlParameter("@remarks", remarks),
                                            new SqlParameter("@taskId", taskId));
                message += " " + taskId.ToString();

                if (inSQLTrx == null)
                    SQLTrx.Commit();

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to Transfer Balance", ex);
                if (inSQLTrx == null)
                    SQLTrx.Rollback();

                message = ex.Message;

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                if (inSQLTrx == null)
                    cnn.Close();
            }
        }

        private void ConsolidateCardDiscountDetails(int oldCardId, int newCardId, SqlCommand consolidateCardDiscountCmd, int trxId, int trxlineId)
        {
            log.LogMethodEntry(oldCardId, newCardId, consolidateCardDiscountCmd, trxId, trxlineId);
            consolidateCardDiscountCmd.Parameters.Clear();
            consolidateCardDiscountCmd.Parameters.AddWithValue("@oldCardId", oldCardId);
            consolidateCardDiscountCmd.Parameters.AddWithValue("@newCardId", newCardId);
            consolidateCardDiscountCmd.Parameters.AddWithValue("@transactionId", trxId);
            consolidateCardDiscountCmd.Parameters.AddWithValue("@lineId", trxlineId);
            consolidateCardDiscountCmd.Parameters.AddWithValue("@loginId", Utilities.ExecutionContext.GetUserId());
            consolidateCardDiscountCmd.Parameters.AddWithValue("@siteId", Utilities.ExecutionContext.GetSiteId());
            consolidateCardDiscountCmd.ExecuteNonQuery();

            log.LogVariableState("@oldCardId", oldCardId);
            log.LogVariableState("@newCardId", newCardId);
            log.LogVariableState("@transactionId", trxId);
            log.LogVariableState("@lineId", trxlineId);
            log.LogVariableState("@loginId", Utilities.ExecutionContext.GetUserId());
            log.LogVariableState("@siteId", Utilities.ExecutionContext.GetSiteId());

            log.LogMethodExit();
        }
        private List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingSchedules(AccountDTO accountDTO, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1), inSQLTrx);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
            if (accountDTO != null)
            {
                List<int> subsriptionBillingCycleIdList = new List<int>();
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any()
                    && accountDTO.AccountCreditPlusDTOList.Exists(cp => cp.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId > -1).Select(cp => cp.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any()
                    && accountDTO.AccountGameDTOList.Exists(cg => cg.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountGameDTOList.Where(cg => cg.SubscriptionBillingScheduleId > -1).Select(cg => cg.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any()
                   && accountDTO.AccountDiscountDTOList.Exists(cd => cd.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountDiscountDTOList.Where(cd => cd.SubscriptionBillingScheduleId > -1).Select(cd => cd.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }

                if (subsriptionBillingCycleIdList != null && subsriptionBillingCycleIdList.Any())
                {
                    subsriptionBillingCycleIdList = subsriptionBillingCycleIdList.Distinct().ToList();
                    SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(Utilities.ExecutionContext);
                    subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOListById(subsriptionBillingCycleIdList, inSQLTrx);
                }
            }
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;
        }

        private void CanRefundTheCard(AccountDTO accountDTO, List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1), subscriptionBillingScheduleDTOList, inSQLTrx);
            if (accountDTO != null)
            {
                if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                {
                    List<int> subscriptionHeaderIdList = subscriptionBillingScheduleDTOList.Select(sbs => sbs.SubscriptionHeaderId).ToList();
                    if (subscriptionHeaderIdList != null && subscriptionHeaderIdList.Any())
                    {
                        subscriptionHeaderIdList = subscriptionHeaderIdList.Distinct().ToList();
                        for (int i = 0; i < subscriptionHeaderIdList.Count; i++)
                        {
                            SubscriptionHeaderBL subscriptionHeaderBL = GetSubscriptionHeaderBL(subscriptionHeaderIdList[i], inSQLTrx);
                            if (subscriptionHeaderBL.AccountHoldsActiveSubscriptionEntitlements(accountDTO, inSQLTrx))
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4004, accountDTO.TagNumber));
                                //Cannot proceed with refund task. &1 is linked with active subscription.
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private SubscriptionHeaderBL GetSubscriptionHeaderBL(int headerId, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry(headerId, inSQLTrx);
            SubscriptionHeaderBL subscriptionHeaderBL = null;
            if (subScriptionHeaderDictionary == null)
            {
                subScriptionHeaderDictionary = new Dictionary<int, SubscriptionHeaderBL>();
            }
            if (subScriptionHeaderDictionary.ContainsKey(headerId))
            {
                subscriptionHeaderBL = subScriptionHeaderDictionary[headerId];
            }
            else
            {
                subscriptionHeaderBL = new SubscriptionHeaderBL(Utilities.ExecutionContext, headerId, true, inSQLTrx);
                subScriptionHeaderDictionary.Add(headerId, subscriptionHeaderBL);
            }
            log.LogMethodExit();
            return subscriptionHeaderBL;
        }
    }
}
