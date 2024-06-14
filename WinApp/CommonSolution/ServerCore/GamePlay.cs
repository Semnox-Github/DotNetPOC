/**********************************************************************************************************************
 * Project Name - GamePlay
 * Description  - GamePlay class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 **********************************************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value  
 *2.60.2      18-May-2019      Mathew Ninan   Record Gameplay using Machine Id even if Reference
 *                                            Machine exists. Tickets are logged against Machine
 *2.70.0      28-Jun-2019      Mathew Ninan   Moved Check-in specific logic to 3 Tier.    
 *2.70.2      18-Nov-2019      Jinto Thomas   Inactive inventory product count should not decrease on game play
 *2.70.3      14-Feb-2020      Lakshminarayana Modified: Creating unregistered customer during check-in process
 *            02-Jun-2020      Mathew Ninan   Added support for Slot reader ticket updates 
 *2.80.0      25-Feb-2020      Indrajeet K    Moved the Encrypt, Decrypt & getKey Method to Encryption.cs
 *            10-Sep-2020      Indrajeet Kumar Moved Fingerprint scan to Biometric reference 
 *2.100.0     16-Nov-2020      Mathew Ninan   Child Card won't be replaced by parent card. Child Card will have 
 *                                            required entitlement loaded instead of changing card to parent card
 *2.130.0     09-Sep-2021      Mathew Ninan   Added logic for additional debug, Parent-child account management
 *2.130.3     21-Jan-2022      Mathew Ninan   Load gameplay id to TransactionLineGamePlayMapping instead of Trx_lines 
 *2.130.3     29-Mar-2022      Mathew Ninan   Handling Paused status for Check-in.
 *2.140.0     09-Sep-2021      Girish Kundar  Modified: Check In/Check out changes
 *2.140.0     08-Dec-2021      Mathew Ninan   Added logic to handle Ticket Eligibility v/s Ticket mode. GamePlayStats
 *                                            has additional property for this to identify if ticket is allowed and 
 *                                            also mode of ticket
**********************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Device.Biometric;
using Semnox.Parafait.Product;
using System.Threading.Tasks;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ServerCore
{
    public static class GamePlay
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void getGamePlayDetails(Machine Machine, string card_number, ServerStatic.GlobalGamePlayStats GamePlayStats, ServerStatic ServerStatic, ref string message, SqlTransaction trx = null)
        {
            log.LogMethodEntry(Machine, card_number, GamePlayStats, ServerStatic, message);
            string promo_bonus_allowed = "Y";
            string promo_courtesy_allowed = "Y";
            string promo_time_allowed = "Y";
            string promo_ticket_allowed = "Y";
            StringBuilder additionalMessages = new StringBuilder();
            DateTime loggingStartTime = DateTime.Now;
            int promoThemeNumber = -1;
            bool cardGameTicketAllowed = true;
            bool creditPlusTicketAllowed = true;
            DateTime CreditPlusPlayStartTime = DateTime.Now;

            bool repeat_play = false;

            Utilities Utilities = ServerStatic.Utilities;
            DataTable DT = new DataTable();
            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (trx == null)
                    {
                        cmd.Connection = cnn;
                    }
                    else
                    {
                        cmd.Connection = trx.Connection;
                        cmd.Transaction = trx;
                    }

                    if (ServerStatic.CHECK_FOR_CARD_EXCEPTIONS == "Y") // check this only if configuration is set to do it
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Card Exception check");
                        // check if card is in exception list and this machine is allowed for it
                        cmd.CommandText = @"select top 1 1
                                     where not exists
                                        (select 1 from
                                        CardTypeRule ct, games g, machines m
                                        where g.game_id = m.game_id
                                        and (g.game_profile_id = ct.game_profile_id or g.game_id = ct.game_id)
                                        and m.machine_id = @Machine_Id
                                        and ct.active = 'Y')
                                    OR Exists (
                                        select 1 from
                                        CardTypeRule ct, games g, machines m, cards c left join customers cu on c.customer_id = cu.customer_id
                                        where c.Card_Number = @card_number
                                        and c.valid_flag = 'Y'
                                        and ct.active = 'Y'
                                        and ct.disAllowed = 'N'
                                        and (g.game_profile_id = ct.game_profile_id or g.game_id = ct.game_id)
                                        and m.machine_id = @Machine_Id
                                        and ct.MembershipId = cu.MembershipID)";
                        cmd.Parameters.AddWithValue("@card_number", card_number);
                        cmd.Parameters.AddWithValue("@Machine_Id", Machine.MachineId);
                        log.LogVariableState("@card_number", card_number);
                        log.LogVariableState("@Machine_Id", Machine.MachineId);
                        if (cmd.ExecuteScalar() == null)
                        {
                            GamePlayStats.GamePlayType = ServerStatic.NOTALLOWED;
                            message += "; not allowed for this card type";
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(null);
                            return;
                        }
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Exception check");
                    } // end exception check
                    else
                    {
                        cmd.Parameters.AddWithValue("@card_number", card_number);
                        cmd.Parameters.AddWithValue("@Machine_Id", Machine.MachineId);
                        log.LogVariableState("@card_number", card_number);
                        log.LogVariableState("@Machine_Id", Machine.MachineId);
                    }

                    // check for last play time by same card on same machine
                    // it should be older than MIN_SECONDS_BETWEEN_REPEAT_PLAY secs
                    additionalMessages.AppendLine();
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Last game play query check for card: " + card_number + " and machine: " + Machine.machine_name);
                    cmd.CommandText = @"select g.card_id 
                                from gameplay g, cards c 
                               where g.gameplay_id = (select max(gameplay_id) 
                                                      from gameplay 
                                                     where machine_id = @Machine_Id)
                               and g.card_id = c.card_id
                                and c.card_number = @card_number 
                                and g.play_date > dateadd(MI, -10, getdate())
                                and c.valid_flag = 'Y'";
                    object cardId = cmd.ExecuteScalar();
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Last game play query check for card: " + card_number + " and machine: " + Machine.machine_name);
                    if (cardId != null)
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Last game play query check for machine: " + Machine.machine_name + " in last 10 minutes.");
                        cmd.CommandText = @"select play_date, card_id, getdate() sysdate
                                from gameplay g
                                where g.machine_id = @Machine_Id
                                and g.play_date > dateadd(MI, -10, getdate())
                                order by g.play_date desc";

                        SqlDataAdapter daRepeat = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        daRepeat.Fill(dt);

                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Last game play query check for machine: " + Machine.machine_name + " in last 10 minutes.");
                        DateTime lastPlayDate = Convert.ToDateTime(dt.Rows[0][0]);
                        DateTime sysDate = Convert.ToDateTime(dt.Rows[0][2]);

                        if (sysDate < lastPlayDate.AddSeconds(Machine.RepeatPlayDelay))
                        {
                            GamePlayStats.GamePlayType = ServerStatic.MULTIPLESWIPE;
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(null);
                            return;
                        }

                        // if same card is used within 10 minutes of last gameplay on the machine by the same card, it is a repeat play
                        if (sysDate < lastPlayDate.AddMinutes(10))
                            repeat_play = true;

                        GamePlayStats.RepeatPlayIndex = 1;
                        foreach (DataRow sqr in dt.Rows)
                        {
                            if (sqr[1].Equals(cardId))
                                GamePlayStats.RepeatPlayIndex++;
                            else
                                break;
                        }
                    }

                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Main query to get Card, Machine and Gameplay attributes for card: " + card_number + " and machine: " + Machine.machine_name);
                    cmd.CommandText = @"select c.card_id, c.credits, c.courtesy, c.bonus, 
                                   g.play_credits game_play_credits, gp.play_credits profile_play_credits, 
                                   g.vip_play_credits vip_play_credits, gp.vip_play_credits profile_vip_play_credits, 
                                   gp.credit_allowed, gp.bonus_allowed, gp.courtesy_allowed, gp.time_allowed,  
                                   c.vip_customer, c.real_ticket_mode, c.ticket_allowed as card_ticket_allowed, 
                                   m.ticket_allowed as machine_ticket_allowed, 
                                   gp.ticket_allowed_on_credit, 
                                   gp.ticket_allowed_on_courtesy, 
                                   gp.ticket_allowed_on_bonus, 
                                   gp.ticket_allowed_on_time, 
                                   g.game_id, 
                                   g.game_profile_id, 
                                   isnull(c.technician_card, 'N') technician_card,
                                   c.timer_reset_card, 
                                   c.tech_games, 
                                   c.time, 
                                   c.start_time, 
                                   (select max(play_date) 
                                      from gameplay 
                                     where notes='Time Play'
                                       and card_id = c.card_id) last_played_time, 
                                   m.ticket_mode, 
                                   c.credits_played, 
                                   isnull(g.repeat_play_discount, 0) repeat_play_discount, 
                                   getdate() sysdate,
                                   --c.cardTypeId, 
                                   cu.membershipId,
                                   g.ProductId
                               from games g, game_profile gp, 
                                   machines m, cards c left join customers cu on cu.customer_id = c.customer_id
                                   where c.card_number = @card_number 
                                   and g.game_profile_id = gp.game_profile_id 
                                   and c.valid_flag = 'Y' 
                                   and (c.ExpiryDate is null or c.ExpiryDate >= getdate())
                                   and g.game_id = m.game_id 
                                   and (c.site_id is null or c.site_id = @siteId or @roamingAllowed = 'Y')
                                   and m.machine_id = @Machine_Id";

                    cmd.Parameters.AddWithValue("@roamingAllowed", ServerStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS);
                    cmd.Parameters.AddWithValue("@siteId", ServerStatic.Utilities.ParafaitEnv.SiteId);
                    log.LogVariableState("@roamingAllowed", ServerStatic.Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS);
                    log.LogVariableState("@siteId", ServerStatic.Utilities.ParafaitEnv.SiteId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(DT);
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Main query to get Card, Machine and Gameplay attributes for card: " + card_number + " and machine: " + Machine.machine_name);
                }
            }

            if (DT.Rows.Count > 0)
            {
                DateTime sysDate = Convert.ToDateTime(DT.Rows[0]["sysdate"]);
                if (DT.Rows[0]["credits"] != DBNull.Value)
                    GamePlayStats.CardCredits = Convert.ToDecimal(DT.Rows[0]["credits"]);
                if (DT.Rows[0]["courtesy"] != DBNull.Value)
                    GamePlayStats.CardCourtesy = Math.Round(Convert.ToDecimal(DT.Rows[0]["courtesy"]), 4, MidpointRounding.AwayFromZero);
                if (DT.Rows[0]["bonus"] != DBNull.Value)
                    GamePlayStats.CardBonus = Convert.ToDecimal(DT.Rows[0]["bonus"]);
                if (DT.Rows[0]["time"] != DBNull.Value)
                    GamePlayStats.CardTime = Convert.ToDecimal(DT.Rows[0]["time"]);
                if (DT.Rows[0]["tech_games"] != DBNull.Value)
                    GamePlayStats.CardTechGames = Convert.ToInt32(DT.Rows[0]["tech_games"]);

                GamePlayStats.CardId = Convert.ToInt32(DT.Rows[0]["card_id"]);
                //if (DT.Rows[0]["cardTypeId"] != DBNull.Value)
                //    GamePlayStats.CardTypeId = Convert.ToInt32(DT.Rows[0]["cardTypeId"]); 
                if (DT.Rows[0]["membershipId"] != DBNull.Value && GamePlayStats.MembershipId <= -1) //If Membership id is assigned, dont consider current card membership value
                    GamePlayStats.MembershipId = Convert.ToInt32(DT.Rows[0]["membershipId"]);

                if (DT.Rows[0]["ProductId"] != DBNull.Value)
                    GamePlayStats.ProductId = Convert.ToInt32(DT.Rows[0]["ProductId"]);

                if (GamePlayStats.ProductId != -1)
                {
                    if (Convert.ToChar(DT.Rows[0]["technician_card"]).Equals('N'))
                    {
                        string ipAddress = "";
                        try
                        {
                            ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("MachineName", Environment.MachineName);
                            log.Error("Error while calculating the ip address", ex);
                        }
                        ServerStatic.Utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
                        Card card = new Card(GamePlayStats.CardId, "", ServerStatic.Utilities);
                        TransactionUtils trxUtils = new TransactionUtils(ServerStatic.Utilities);
                        DataTable dtProduct = trxUtils.getProductDetails(GamePlayStats.ProductId, card);
                        string productType = dtProduct.Rows[0]["Product_Type"].ToString();
                        if (productType == "CHECK-IN" || productType == "CHECK-OUT")
                        {
                            if (dtProduct.Rows[0]["CheckInFacilityId"] == DBNull.Value)
                            {
                                GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                GamePlayStats.GameplayMessage = ServerStatic.Utilities.MessageUtils.getMessage(225);
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(null);
                                return;
                            }

                            //////Check-in moved to 3 tier
                            ////List<KeyValuePair<CheckInDTO.SearchByParameters, string>> checkInSearchParams = new List<KeyValuePair<CheckInDTO.SearchByParameters, string>>();
                            ////checkInSearchParams.Add(new KeyValuePair<CheckInDTO.SearchByParameters, string>(CheckInDTO.SearchByParameters.CARD_ID, card.card_id.ToString()));
                            ////checkInSearchParams.Add(new KeyValuePair<CheckInDTO.SearchByParameters, string>(CheckInDTO.SearchByParameters.IS_ACTIVE, "1"));
                            ////CheckInListBL checkInListBL = new CheckInListBL(Utilities.ExecutionContext);
                            ////List<CheckInDTO> checkInDTOList = checkInListBL.GetCheckInDTOList(checkInSearchParams, true, true);
                            ////checkInDTOList = checkInDTOList.OrderByDescending(x => x.CheckInTime).ToList();
                            //log.LogVariableState("@cardId", card.card_id);
                            //log.LogVariableState("CheckINDTOList", checkInDTOList);
                            //if (productType == "CHECK-IN" && (checkInDTOList != null && checkInDTOList.Count > 0
                            //                                  && checkInDTOList[0].CheckInDetailDTOList.Any(x => x.Status == CheckInStatus.CHECKEDIN && (x.CheckOutTime == null ||
                            //                                                 x.CheckOutTime > Utilities.getServerTime()))
                            //                                 )
                            //   )
                            //{
                            //    GamePlayStats.GamePlayType = ServerStatic.ERROR;
                            //    GamePlayStats.GameplayMessage = "Already Checked";
                            //    log.LogVariableState("Message", message);
                            //    log.LogMethodExit(null);
                            //    return;
                            //}
                            List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> checkInSearchParams = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
                            checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CARD_ID, card.card_id.ToString()));
                            checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
                            CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(Utilities.ExecutionContext);
                            List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(checkInSearchParams);
                            log.LogVariableState("checkInDetailDTOList", checkInDetailDTOList);
                            if (productType == "CHECK-IN" &&
                                            checkInDetailDTOList != null && checkInDetailDTOList.Any())
                            {
                                checkInDetailDTOList = checkInDetailDTOList.OrderByDescending(x => x.CheckInDetailId).ToList();
                                List<CheckInDetailDTO> filteredCheckInDetailDTOList = checkInDetailDTOList.Where(x => x.Status != CheckInStatus.CHECKEDOUT && (x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime())).ToList();
                                if (filteredCheckInDetailDTOList != null && filteredCheckInDetailDTOList.Any())
                                {
                                    CheckInDetailDTO checkInDetailDTO = checkInDetailDTOList.FirstOrDefault();
                                    if (checkInDetailDTO.Status == CheckInStatus.PENDING)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                        GamePlayStats.GameplayMessage = "Check in Pending";
                                        log.LogVariableState("Message", "Check in status is pending . Cannot check in");
                                        log.Error("Check in status is pending . Cannot check in");
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDIN)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                        GamePlayStats.GameplayMessage = "Already Checked";
                                        log.LogVariableState("Message", "Already Checked In. Cannot check in again");
                                        log.Error("Already Checked In. Cannot check in again");
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDOUT)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                        GamePlayStats.GameplayMessage = "Already Checked out";
                                        log.LogVariableState("Message", "Already Checked out");
                                        log.Error("Already Checked out");
                                        log.LogMethodExit();
                                        return;
                                    }
                                    //Comment below block if PAUSED status should be allowed to check-in
                                    else if (checkInDetailDTO.Status == CheckInStatus.PAUSED)
                                    {
                                        //GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                        //GamePlayStats.GameplayMessage = "Please UnPause";
                                        log.Debug("Current state is Paused. Proceeding with Check-in");
                                    }
                                }
                            }

                            if (productType == "CHECK-IN")
                            {
                                FacilityDTO facilityDTO = new FacilityBL(ServerStatic.Utilities.ExecutionContext, Convert.ToInt32(dtProduct.Rows[0]["CheckInFacilityId"])).FacilityDTO;
                                CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext);
                                int totalCapacity = facilityDTO.Capacity == null ? 0 : Convert.ToInt32(facilityDTO.Capacity);
                                int totalCheckedIn = checkInBL.GetTotalCheckedInForFacility(facilityDTO.FacilityId, null);
                                if (totalCapacity > 0 && totalCapacity < (totalCheckedIn + 1))
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                    GamePlayStats.GameplayMessage = ServerStatic.Utilities.MessageUtils.getMessage(11);
                                    log.LogVariableState("Message", ServerStatic.Utilities.MessageUtils.getMessage(11));
                                    log.LogMethodExit(null);
                                    return;
                                }
                                if (ParafaitDefaultContainerList.GetParafaitDefault(ServerStatic.Utilities.ExecutionContext, "CUSTOMERTYPE") == "N"
                                    && card.customerDTO == null)
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                    GamePlayStats.GameplayMessage = "Customer not registered";
                                    log.LogVariableState("Message", "Check-in not possible as customer is not registered");
                                    log.LogMethodExit(null);
                                    return;
                                }
                            }
                            //In case of check-out, if there is no corresponding check-in, continue with 
                            //entitlement and charge if available else it would stop as LOW Balance
                            if (productType == "CHECK-OUT")
                            {
                                //if (checkInId == null)
                                if (checkInDetailDTOList != null && checkInDetailDTOList.Any()
                                    && checkInDetailDTOList.Exists(x => x.Status != CheckInStatus.CHECKEDOUT
                                                            && (x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime())))
                                {
                                    checkInDetailDTOList = checkInDetailDTOList.OrderByDescending(x => x.CheckInDetailId).ToList();
                                    List<CheckInDetailDTO> filteredCheckInDetailDTOList = checkInDetailDTOList.Where(x => x.Status != CheckInStatus.CHECKEDOUT
                                                                                          && (x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime()))
                                                                                         .ToList();
                                    if (filteredCheckInDetailDTOList != null && filteredCheckInDetailDTOList.Any())
                                    {
                                        CheckInDetailDTO checkInDetailDTO = checkInDetailDTOList.FirstOrDefault();
                                        if (checkInDetailDTO.Status == CheckInStatus.PENDING)
                                        {
                                            GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                            GamePlayStats.GameplayMessage = "Check in Pending";
                                            log.LogVariableState("Message", "Check in status is pending . Cannot check in");
                                            log.Error("Check in status is pending . Cannot check in");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDOUT)
                                        {
                                            GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                            GamePlayStats.GameplayMessage = "Already Checked out";
                                            log.LogVariableState("Message", "Already Checked out");
                                            log.Error("Already Checked out");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (checkInDetailDTO.Status == CheckInStatus.ORDERED)
                                        {
                                            GamePlayStats.GamePlayType = ServerStatic.ERROR;
                                            GamePlayStats.GameplayMessage = "Check in Pending";
                                            log.LogVariableState("Message", "Check in Pending");
                                            log.Error("Already Checked out");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return;
                                        }

                                        CheckInBL checkInBL = new CheckInBL(ServerStatic.Utilities.ExecutionContext, filteredCheckInDetailDTOList[0].CheckInId);
                                        //checkOut.BaseTimeForPrice = Convert.ToInt32(dtProduct.Rows[0]["time"] == DBNull.Value ? 0 : dtProduct.Rows[0]["time"]);
                                        decimal effectivePrice = checkInBL.GetCheckOutPrice(GamePlayStats.ProductId, filteredCheckInDetailDTOList);
                                        DT.Rows[0]["game_play_credits"] = effectivePrice;
                                    }
                                }
                                else
                                {
                                    log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                }
                                //if (filteredCheckInDetailDTOList != null)
                                //{
                                //    CheckInBL checkInBL = new CheckInBL(ServerStatic.Utilities.ExecutionContext, checkInDTOList[0]);
                                //    //checkOut.BaseTimeForPrice = Convert.ToInt32(dtProduct.Rows[0]["time"] == DBNull.Value ? 0 : dtProduct.Rows[0]["time"]);
                                //    decimal effectivePrice = checkInBL.GetCheckOutPrice(GamePlayStats.ProductId, checkInDTOList[0].CheckInDetailDTOList);
                                //    DT.Rows[0]["game_play_credits"] = effectivePrice;
                                //}
                            }
                        }
                    }
                }

                decimal gamePrice = 0;
                decimal vipGamePrice = 0;
                // fetch game play credits from game or profile
                if (DT.Rows[0]["game_play_credits"] == DBNull.Value)
                {
                    if (DT.Rows[0]["profile_play_credits"] != DBNull.Value)
                        gamePrice = Convert.ToDecimal(DT.Rows[0]["profile_play_credits"]);
                }
                else
                {
                    gamePrice = Convert.ToDecimal(DT.Rows[0]["game_play_credits"]);
                }

                if (DT.Rows[0]["vip_play_credits"] == DBNull.Value)
                {
                    if (DT.Rows[0]["profile_vip_play_credits"] != DBNull.Value)
                        vipGamePrice = Convert.ToDecimal(DT.Rows[0]["profile_vip_play_credits"]);
                }
                else
                {
                    vipGamePrice = Convert.ToDecimal(DT.Rows[0]["vip_play_credits"]);
                }

                GamePlayStats.TechCard = Convert.ToChar(DT.Rows[0]["technician_card"]);

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Creditplus initialized for card: " + card_number + " and machine: " + Machine.machine_name);
                CreditPlus creditPlus = new CreditPlus(Utilities);
                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Creditplus initialized for card: " + card_number + " and machine: " + Machine.machine_name);
                if (GamePlayStats.TechCard.Equals('N')) // user card
                {
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Check Creditplus for gameplay for card: " + card_number + " and machine: " + Machine.machine_name);
                    creditPlus.getCreditPlusForGamePlay(GamePlayStats.CardId, Machine.MachineId, ref GamePlayStats.CardCreditPlusCardBalance, ref GamePlayStats.CardCreditPlusCredits, ref GamePlayStats.CardCreditPlusBonus, ref creditPlusTicketAllowed);
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Check Creditplus for gameplay for card: " + card_number + " and machine: " + Machine.machine_name);
                    GamePlayStats.CardCreditPlusCardBalance = Math.Max(GamePlayStats.CardCreditPlusCardBalance, 0);
                    GamePlayStats.CardCreditPlusCredits = Math.Max(GamePlayStats.CardCreditPlusCredits, 0);
                    GamePlayStats.CardCreditPlusBonus = Math.Max(GamePlayStats.CardCreditPlusBonus, 0);
                    GamePlayStats.CardCreditPlusCredits = Math.Round(GamePlayStats.CardCredits + GamePlayStats.CardCreditPlusCredits + GamePlayStats.CardCreditPlusCardBalance, 4, MidpointRounding.AwayFromZero);
                    GamePlayStats.CardCreditPlusBonus = Math.Round(GamePlayStats.CardCreditPlusBonus + GamePlayStats.CardBonus, 4, MidpointRounding.AwayFromZero);
                    GamePlayStats.CardCreditPlusTime = Math.Round(creditPlus.getCreditPlusTimeForGamePlay(GamePlayStats.CardId, Machine.MachineId, ref GamePlayStats.CardCreditPlusTimeId, ref creditPlusTicketAllowed, ref CreditPlusPlayStartTime), 4, MidpointRounding.AwayFromZero);
                }

                decimal promotion_credits = 0;
                decimal promotion_vip_credits = 0;
                int promotionDetailId = -1;

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get Promotion Details for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                Promotions.getPromotionDetails(GamePlayStats.MembershipId,
                                                Convert.ToInt32(DT.Rows[0]["game_id"]),
                                                Convert.ToInt32(DT.Rows[0]["game_profile_id"]),
                                                gamePrice,
                                                vipGamePrice,
                                                ref promotion_credits,
                                                ref promotion_vip_credits,
                                                ref promo_bonus_allowed,
                                                ref promo_courtesy_allowed,
                                                ref promo_time_allowed,
                                                ref promo_ticket_allowed,
                                                ref promoThemeNumber,
                                                ref promoThemeNumber,
                                                ref GamePlayStats.PromotionId,
                                                ref promotionDetailId,
                                                Utilities);

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Get Promotion Details for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                //if VIP Customer is set in GamplayStats then use that. Applicable in parent child card scenario
                if (string.IsNullOrWhiteSpace(GamePlayStats.Vip_Customer))
                    GamePlayStats.Vip_Customer = DT.Rows[0]["vip_customer"].ToString();
                if (GamePlayStats.Vip_Customer == null || GamePlayStats.Vip_Customer == "N") // not vip customer
                {
                    GamePlayStats.CreditsRequired = promotion_credits;
                }
                else
                {
                    GamePlayStats.CreditsRequired = promotion_vip_credits;
                }
                message += "; Post Promo Credits Required: " + GamePlayStats.CreditsRequired.ToString() + Environment.NewLine;

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get Discounted Credits method call for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                GamePlayStats.CreditsRequired = Discounts.getDiscountedCredits(GamePlayStats.CardId, DT.Rows[0]["game_id"], Convert.ToDecimal(DT.Rows[0]["credits_played"] == DBNull.Value ? 0 : DT.Rows[0]["credits_played"]), GamePlayStats.CreditsRequired, Utilities);
                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Get Discounted Credits method call for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                message += "; Post Discounts Credits Required: " + GamePlayStats.CreditsRequired.ToString();

                if (repeat_play)
                {
                    GamePlayStats.CreditsRequired = Math.Round(GamePlayStats.CreditsRequired * (1 - Convert.ToDecimal(DT.Rows[0]["repeat_play_discount"]) / 100), 4, MidpointRounding.AwayFromZero);
                    message += "; Repeat Play Credits Required:" + GamePlayStats.CreditsRequired.ToString();
                }

                // game play multiplier change on 9-jun-2017
                GamePlayStats.CreditsRequired = GamePlayStats.CreditsRequired * Machine.GameplayMultiplier;
                //end change

                if (GamePlayStats.TechCard.Equals('N')) // user card
                {
                    if (GamePlayStats.CardTime > 0) //game time card
                    {
                        DateTime startTime;
                        if (DT.Rows[0]["start_time"] == DBNull.Value)
                            startTime = sysDate;
                        else
                            startTime = Convert.ToDateTime(DT.Rows[0]["start_time"]);

                        if (startTime.AddMinutes((double)GamePlayStats.CardTime) >= sysDate) // has pending time
                        {
                            // check last played time, and make sure it is older than min time gap needed

                            if (DT.Rows[0]["last_played_time"] != DBNull.Value)
                            {
                                DateTime lastPlayedTime = Convert.ToDateTime(DT.Rows[0]["last_played_time"]);
                                if (sysDate <= lastPlayedTime.AddSeconds(Machine.ConsecutiveTimePlayDelay))
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.GAMETIMEMINGAP;
                                }
                            }

                            if (GamePlayStats.GamePlayType != ServerStatic.GAMETIMEMINGAP)
                            {
                                if (DT.Rows[0]["time_allowed"].ToString() == "N")
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                                }
                                else
                                {
                                    if (promo_time_allowed == "N")
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.TIMEPLAY;

                                        // find the balance time left on card. used to display to user
                                        TimeSpan balanceTime = startTime.AddMinutes((double)GamePlayStats.CardTime) - sysDate;
                                        GamePlayStats.CardTime = balanceTime.Days * 24 * 60 + balanceTime.Hours * 60 + balanceTime.Minutes;
                                    }
                                }
                            }
                        }
                        else
                        {
                            GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                        }
                    }
                    else
                    {
                        GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                    }

                    if (GamePlayStats.GamePlayType == ServerStatic.LOBALCREDIT && GamePlayStats.CardCreditPlusTimeId != -1) //credit plus time card
                    {
                        // check last played time, and make sure it is older than min time gap needed

                        if (DT.Rows[0]["last_played_time"] != DBNull.Value)
                        {
                            DateTime lastPlayedTime = Convert.ToDateTime(DT.Rows[0]["last_played_time"]);
                            if (sysDate <= lastPlayedTime.AddSeconds(Machine.ConsecutiveTimePlayDelay))
                            {
                                GamePlayStats.GamePlayType = ServerStatic.GAMETIMEMINGAP;
                            }
                        }

                        if (GamePlayStats.GamePlayType != ServerStatic.GAMETIMEMINGAP)
                        {
                            if (DT.Rows[0]["time_allowed"].ToString() == "N")
                            {
                                GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                            }
                            else
                            {
                                if (promo_time_allowed == "N")
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.TIMEPLAY;
                                    GamePlayStats.CreditsRequired = promotion_credits * Machine.GameplayMultiplier; //13-Feb-2017: added to get promotion price for time play
                                    GamePlayStats.CardTime = GamePlayStats.CardCreditPlusTime;
                                }
                            }
                        }
                    }

                    // check for credit play if game time has expired or it was not a gametime card
                    if (GamePlayStats.GamePlayType == ServerStatic.LOBALCREDIT) // check for specific games loaded on card
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        CardGames cardGames = new CardGames(Utilities);
                        GamePlayStats.CardGames = cardGames.getCardGames(GamePlayStats.CardId, Convert.ToInt32(DT.Rows[0]["game_id"]), Convert.ToInt32(DT.Rows[0]["game_profile_id"]), ref cardGameTicketAllowed, ref GamePlayStats.CardGameIdList);
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));

                        if (GamePlayStats.CardGames >= Machine.GameplayMultiplier)
                        {
                            GamePlayStats.GamePlayType = ServerStatic.VALIDCARDGAME;
                        }
                        else
                        {
                            GamePlayStats.CardGames = 0;
                            GamePlayStats.GamePlayType = "CREDITPLAY";
                        }
                    }

                    if (GamePlayStats.GamePlayType == "CREDITPLAY") // regular credit play
                    {
                        if (DT.Rows[0]["credit_allowed"].ToString() == "N")
                        {
                            GamePlayStats.CardCreditPlusCredits = 0;
                            GamePlayStats.CardCredits = 0;
                            GamePlayStats.CardCreditPlusCardBalance = 0;
                        }

                        if (promo_bonus_allowed == "N" || DT.Rows[0]["bonus_allowed"].ToString() == "N")
                        {
                            GamePlayStats.CardCreditPlusBonus = 0;
                            GamePlayStats.CardBonus = 0;
                        }

                        string courtesy_allowed = DT.Rows[0]["courtesy_allowed"].ToString();
                        if (courtesy_allowed == "Y") // if machine level courtesy is allowed, check for promo courtesy
                            courtesy_allowed = promo_courtesy_allowed;

                        if (courtesy_allowed == "Y") // check if courtesy allowed on the game
                        {
                            if (GamePlayStats.CreditsRequired <= GamePlayStats.CardCourtesy) // if allowed, check for courtesy balance
                            {
                                GamePlayStats.GamePlayType = ServerStatic.VALIDCOURTESY; // courtesy available. play.
                            }
                            else
                                if (GamePlayStats.CardCourtesy > 0) // if courtesy is there try to use it with combination of credits and bonus
                            {
                                if (GamePlayStats.CreditsRequired <= GamePlayStats.CardCourtesy + GamePlayStats.CardCreditPlusCredits + GamePlayStats.CardCreditPlusBonus)
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.VALIDCREDITCOURTESY; // courtesy+credits+bonus available. play.
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                                }
                            }
                            else
                            {
                                GamePlayStats.GamePlayType = "CREDITPLAY";
                            }
                        }
                        else
                        {
                            GamePlayStats.GamePlayType = "CREDITPLAY";
                        }

                        if (GamePlayStats.GamePlayType == "CREDITPLAY") // credit play
                        {
                            if (GamePlayStats.CreditsRequired > GamePlayStats.CardCreditPlusCredits + GamePlayStats.CardCreditPlusBonus) // more credit required than available
                            {
                                GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                            }
                            else // required credit available
                            {
                                if (ServerStatic.CONSUME_CREDITS_BEFORE_BONUS == "Y")
                                {
                                    if (GamePlayStats.CardCreditPlusCredits >= GamePlayStats.CreditsRequired)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDCREDIT;
                                    }
                                    else
                                        if (GamePlayStats.CardCreditPlusCredits > 0)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDCREDITBONUS;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDBONUS;
                                    }
                                }
                                else
                                {
                                    if (GamePlayStats.CardCreditPlusBonus >= GamePlayStats.CreditsRequired)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDBONUS;
                                    }
                                    else
                                        if (GamePlayStats.CardCreditPlusBonus > 0)
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDCREDITBONUS;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = ServerStatic.VALIDCREDIT;
                                    }
                                }
                            }
                        }
                    }// end regular credit play
                }
                else // technician card
                {
                    if (DT.Rows[0]["timer_reset_card"].ToString() == "Y")
                        GamePlayStats.GamePlayType = ServerStatic.RESETTIMER;
                    else
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Technician Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        CardGames cardGames = new CardGames(Utilities);
                        GamePlayStats.CardGames = GamePlayStats.CardTechGames + cardGames.getCardGames(GamePlayStats.CardId, Convert.ToInt32(DT.Rows[0]["game_id"]), Convert.ToInt32(DT.Rows[0]["game_profile_id"]), ref cardGameTicketAllowed, ref GamePlayStats.CardGameIdList);
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Technician Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        if (GamePlayStats.CardGames > 0)
                        {
                            GamePlayStats.GamePlayType = ServerStatic.TECHPLAY;
                        }
                        else
                        {
                            GamePlayStats.GamePlayType = ServerStatic.LOBALCREDIT;
                            GamePlayStats.CardCreditPlusCredits = 0;
                            GamePlayStats.CardCreditPlusBonus = 0;
                        }
                    }
                }

                if (GamePlayStats.GamePlayType == ServerStatic.LOBALCREDIT
                    && ServerStatic.READER_HARDWARE_VERSION >= 1.5)
                {
                    int CardCreditPlusTimeId = -1;
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Check Creditplus Time for gameplay for card: " + card_number + " and machine: " + Machine.machine_name);
                    creditPlus.getCreditPlusTimeForGamePlay(GamePlayStats.CardId, -1, ref CardCreditPlusTimeId, ref creditPlusTicketAllowed, ref CreditPlusPlayStartTime);
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Check Creditplus Time for gameplay for card: " + card_number + " and machine: " + Machine.machine_name);
                    if (CardCreditPlusTimeId > 0)
                    {
                        GamePlayStats.GamePlayType = ServerStatic.NOENTITLEMENT;
                    }
                    else
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        CardGames cardGames = new CardGames(Utilities);
                        int cardGameId = 0;
                        int count = cardGames.getCardGames(GamePlayStats.CardId, -1, -1, ref cardGameTicketAllowed, ref cardGameId);
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        if (count > 0)
                        {
                            GamePlayStats.GamePlayType = ServerStatic.NOENTITLEMENT;
                        }
                        else
                        {
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Last Time play check for card: " + card_number);
                            object o = Utilities.executeScalar(@"select top 1 play_date 
                                                                from gameplay g 
                                                                where Notes = 'Time Play'
                                                                and g.card_id = @cardId 
                                                                order by play_date desc",
                                                             new SqlParameter("@cardId", GamePlayStats.CardId));
                            log.LogVariableState("@cardId", GamePlayStats.CardId);
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Last Time play check for card: " + card_number);
                            if (o != null)
                            {
                                if ((sysDate - Convert.ToDateTime(o)).TotalMinutes < 10)
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.GAMETIMEPACKAGEEXPIRED;
                                }
                            }
                        }
                    }
                }

                if (!setConsumedValues(ServerStatic, GamePlayStats))
                {
                    log.LogVariableState("Message", message);
                    log.LogMethodExit(null);
                    return;
                }
            }
            else // card is invalid
            {
                GamePlayStats.GamePlayType = ServerStatic.INVCARD;
                message += "; Invalid card or machine address";
                log.LogVariableState("Message", message);
                log.LogMethodExit(null);
                return;
            }

            GamePlayStats.TicketMode = "E";
            if (DT.Rows[0]["card_ticket_allowed"].ToString() != "Y"
                || DT.Rows[0]["machine_ticket_allowed"].ToString() != "Y"
                || promo_ticket_allowed != "Y")
            {
                //GamePlayStats.TicketMode = "N";
                GamePlayStats.TicketEligibility = false;
            }
            else
            {
                switch (GamePlayStats.GamePlayType)
                {
                    case ServerStatic.VALIDCOURTESY:
                    case ServerStatic.VALIDCREDITCOURTESY:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_courtesy"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            break;
                        }
                    case ServerStatic.TIMEPLAY:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_time"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            else
                            {
                                //GamePlayStats.TicketMode = creditPlusTicketAllowed ? "Y" : "N";
                                GamePlayStats.TicketEligibility = creditPlusTicketAllowed ? true : false;
                            }
                            break;
                        }
                    case ServerStatic.VALIDCREDITBONUS:
                    case ServerStatic.VALIDCREDIT:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_credit"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            else
                            {
                                //GamePlayStats.TicketMode = creditPlusTicketAllowed ? "Y" : "N";
                                GamePlayStats.TicketEligibility = creditPlusTicketAllowed ? true : false;
                            }
                            break;
                        }
                    case ServerStatic.VALIDCARDGAME:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_credit"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            else
                            {
                                //GamePlayStats.TicketMode = cardGameTicketAllowed ? "Y" : "N";
                                GamePlayStats.TicketEligibility = cardGameTicketAllowed ? true : false;
                            }
                            break;
                        }
                    case ServerStatic.VALIDBONUS:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_bonus"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            break;
                        }
                    case ServerStatic.TECHPLAY:
                        {
                            //GamePlayStats.TicketMode = "Y";
                            GamePlayStats.TicketEligibility = true;
                            break;
                        }
                    default:
                        {
                            //GamePlayStats.TicketMode = "N";
                            GamePlayStats.TicketEligibility = false;
                            break;
                        }
                }
            }

            //if (GamePlayStats.TicketMode != "N") // if ticket allowed, determine the physical or e-ticket mode
            //{
            if (DT.Rows[0]["ticket_mode"].ToString() == "D")
            {
                if (DT.Rows[0]["real_ticket_mode"].ToString() == "Y")
                    GamePlayStats.TicketMode = "T"; // real ticket
                else
                    GamePlayStats.TicketMode = "E"; // e ticket
            }
            else
            {
                GamePlayStats.TicketMode = DT.Rows[0]["ticket_mode"].ToString();
            }
            //}

            message += additionalMessages.ToString();

            GamePlayStats.ValidGamePlay = true;
            log.LogVariableState("Message", message);
            log.LogMethodExit(null);
        }

        static bool setConsumedValues(ServerStatic ServerStatic, ServerStatic.GlobalGamePlayStats GamePlayStats)
        {
            log.LogMethodEntry(ServerStatic, GamePlayStats);
            switch (GamePlayStats.GamePlayType)
            {
                case ServerStatic.VALIDCOURTESY:
                    {
                        GamePlayStats.ConsumedValues.CardCourtesy = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Courtesy Play";
                        break;
                    }
                case ServerStatic.VALIDCARDGAME:
                    {
                        if (!ServerStatic.ZERO_PRICE_CARDGAME_PLAY)
                            GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Card Game Play";
                        break;
                    }
                case ServerStatic.VALIDCREDIT:
                    {
                        GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Credit Play";
                        break;
                    }
                case ServerStatic.VALIDCREDITBONUS:
                    {
                        if (ServerStatic.CONSUME_CREDITS_BEFORE_BONUS == "Y")
                        {
                            GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.CardCreditPlusCredits;
                            GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardCredits;
                        }
                        else
                        {
                            GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.CardCreditPlusBonus;
                            GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardBonus;
                        }
                        GamePlayStats.GameplayMessage = "Credit and Bonus Play";
                        break;
                    }
                case ServerStatic.VALIDCREDITCOURTESY:
                    {
                        GamePlayStats.ConsumedValues.CardCourtesy = GamePlayStats.CardCourtesy;

                        if (ServerStatic.CONSUME_CREDITS_BEFORE_BONUS == "Y")
                        {
                            GamePlayStats.ConsumedValues.CardCredits = (GamePlayStats.CreditsRequired > GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.CardCreditPlusCredits) ? GamePlayStats.CardCreditPlusCredits : GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardCourtesy;
                            GamePlayStats.ConsumedValues.CardBonus = (GamePlayStats.CreditsRequired > GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.CardCreditPlusCredits) ? GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardCredits - GamePlayStats.ConsumedValues.CardCourtesy : 0;
                        }
                        else
                        {
                            GamePlayStats.ConsumedValues.CardBonus = (GamePlayStats.CreditsRequired > GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.CardCreditPlusBonus) ? GamePlayStats.CardCreditPlusBonus : GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardCourtesy;
                            GamePlayStats.ConsumedValues.CardCredits = (GamePlayStats.CreditsRequired > GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.CardCreditPlusBonus) ? GamePlayStats.CreditsRequired - GamePlayStats.ConsumedValues.CardBonus - GamePlayStats.ConsumedValues.CardCourtesy : 0;
                        }
                        GamePlayStats.GameplayMessage = "Courtesy, Credit and Bonus Play";
                        break;
                    }
                case ServerStatic.VALIDBONUS:
                    {
                        GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Bonus Play";
                        break;
                    }
                case ServerStatic.TIMEPLAY:
                    {
                        if (!ServerStatic.ZERO_PRICE_GAMETIME_PLAY)
                            GamePlayStats.ConsumedValues.CardTime = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Time Play";
                        break;
                    }
                case ServerStatic.TECHPLAY:
                    {
                        if (!ServerStatic.ZERO_PRICE_CARDGAME_PLAY)
                            GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;
                        GamePlayStats.ConsumedValues.TechGame = (GamePlayStats.CurrentMachine == null ? 1 : GamePlayStats.CurrentMachine.GameplayMultiplier);
                        GamePlayStats.GameplayMessage = "Technician Play";
                        break;
                    }
                default:
                    {
                        GamePlayStats.GameplayMessage = GamePlayStats.GamePlayType;
                        log.LogMethodExit(false);
                        return false;
                    }
            }

            if (GamePlayStats.GamePlayType != ServerStatic.VALIDCARDGAME // game play using regular balance, not loaded games
                && GamePlayStats.GamePlayType != ServerStatic.TECHPLAY)
            {
                decimal cardCreditPlusCredits = GamePlayStats.CardCreditPlusCredits - GamePlayStats.CardCredits;
                decimal totalUsedCreditPlusCredits = 0;

                if (GamePlayStats.ConsumedValues.CardCredits > cardCreditPlusCredits) // credit plus and credits is used
                {
                    totalUsedCreditPlusCredits = cardCreditPlusCredits;
                    GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.ConsumedValues.CardCredits - cardCreditPlusCredits;
                }
                else
                {
                    totalUsedCreditPlusCredits = GamePlayStats.ConsumedValues.CardCredits;
                    GamePlayStats.ConsumedValues.CardCredits = 0;
                }
                GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits = totalUsedCreditPlusCredits;

                decimal cardCreditPlusBonus = GamePlayStats.CardCreditPlusBonus - GamePlayStats.CardBonus;
                decimal usedCreditPlusBonus = 0;
                if (GamePlayStats.ConsumedValues.CardBonus > cardCreditPlusBonus) // credit plus bonus and bonus is used
                {
                    usedCreditPlusBonus = cardCreditPlusBonus;
                    GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.ConsumedValues.CardBonus - cardCreditPlusBonus;
                }
                else
                {
                    usedCreditPlusBonus = GamePlayStats.ConsumedValues.CardBonus;
                    GamePlayStats.ConsumedValues.CardBonus = 0;
                }

                GamePlayStats.ConsumedValues.CardCreditPlusBonus = usedCreditPlusBonus;
            }
            log.LogMethodExit(true);
            return true;
        }

        public static long CreateGamePlay(int MachineId, string card_number, ServerStatic.GlobalGamePlayStats GamePlayStats, ServerStatic ServerStatic, ref string message, SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(MachineId, card_number, GamePlayStats, ServerStatic, message);
            StringBuilder additionalMessages = new StringBuilder();
            DateTime loggingStartTime = DateTime.Now;
            //SqlCommand cmd = new SqlCommand();
            Utilities Utilities = ServerStatic.Utilities;
            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn; //Utilities.createConnection(); // get new connection to ensure no conflict with static connection object
                    SqlTransaction SQLTrx;
                    if (inSQLTrx == null)
                        SQLTrx = cmd.Connection.BeginTransaction();
                    else
                    {
                        cmd.Connection = inSQLTrx.Connection;
                        SQLTrx = inSQLTrx;
                    }

                    cmd.Transaction = SQLTrx;

                    if (GamePlayStats.GamePlayType != ServerStatic.VALIDCARDGAME) // game play using regular balance, not loaded games
                    {
                        CreditPlus creditPlus = new CreditPlus(Utilities);
                        if (GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0)
                        {
                            try
                            {
                                additionalMessages.AppendLine();
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Deduct Creditplus Credits for gameplay for card: " + card_number + " and machine: " + MachineId);
                                creditPlus.deductCreditPlusOnGamePlay(card_number, MachineId, (double)GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits,
                                                                      0,
                                                                      ref GamePlayStats.ConsumedValues.CardCreditPlusCardBalance,
                                                                      ref GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits, SQLTrx);
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Deduct Creditplus Credits for gameplay for card: " + card_number + " and machine: " + MachineId);
                            }
                            catch (Exception ex)
                            {
                                message += ";deduct CreditPlus Credits: " + ex.Message;
                                if (inSQLTrx == null && SQLTrx != null)
                                    SQLTrx.Rollback();
                                //cmd.Connection.Close();
                                log.Error("Error occured on deducting credit plus on game play", ex);
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }

                        if (GamePlayStats.ConsumedValues.CardCreditPlusBonus > 0)
                        {
                            try
                            {
                                double x = 0;
                                double y = 0;
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Deduct Creditplus Bonus for gameplay for card: " + card_number + " and machine: " + MachineId);
                                creditPlus.deductCreditPlusOnGamePlay(card_number, MachineId, 0, (double)GamePlayStats.ConsumedValues.CardCreditPlusBonus, ref x, ref y, SQLTrx);
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Deduct Creditplus Bonus for gameplay for card: " + card_number + " and machine: " + MachineId);
                            }
                            catch (Exception ex)
                            {
                                message += ";deduct CreditPlus Bonus: " + ex.Message;
                                if (inSQLTrx == null && SQLTrx != null)
                                    SQLTrx.Rollback();
                                //cmd.Connection.Close();
                                log.Error("Error occured on deducting credit plus on game play", ex);
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }

                        if (GamePlayStats.CardCreditPlusTimeId > 0)
                        {
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Start Time for CreditPlus record for card: " + card_number + " and machine: " + MachineId);
                            cmd.CommandText = "update CardCreditPlus set PlayStartTime = getdate() where CardCreditPlusId = @id and PlayStartTime is null";
                            cmd.Parameters.AddWithValue("@id", GamePlayStats.CardCreditPlusTimeId);
                            cmd.ExecuteNonQuery();

                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Start Time for CreditPlus record for card: " + card_number + " and machine: " + MachineId);
                            log.LogVariableState("@id", GamePlayStats.CardCreditPlusTimeId);
                        }

                        int cardId = -1;
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get card id for card: " + card_number);
                        object objCardId = ServerStatic.Utilities.executeScalar(@"SELECT card_id from cards
                                                                                    where card_number = @cardNumber
                                                                                    and valid_flag = 'Y'",
                                                                                    SQLTrx,
                                                                                new SqlParameter("@cardNumber", card_number));
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - End: Get card id for card: " + card_number);
                        if (objCardId != null && objCardId != DBNull.Value)
                            cardId = Convert.ToInt32(objCardId);

                        cmd.CommandText = "update cards set credits = case when credits - @credits <= 0 then 0 else credits - @credits end, " +
                                                           "courtesy = case when courtesy - @courtesy <= 0 then 0 else courtesy - @courtesy end, " +
                                                           "bonus = case when bonus - @bonus <= 0 then 0 else bonus - @bonus end, " +
                                                           "tech_games = case when tech_games - @tech_games <= 0 then 0 else tech_games - @tech_games end, " +
                                                           "start_time = isnull(start_time, getdate()), " +
                                                           "last_played_time = getdate(), " +
                                                           "credits_played = isnull(credits_played, 0) + @credits_played, " +
                                                           "last_update_time = getdate() " +
                                                       " where card_id = @cardId";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@credits", GamePlayStats.ConsumedValues.CardCredits);
                        cmd.Parameters.AddWithValue("@courtesy", GamePlayStats.ConsumedValues.CardCourtesy);
                        cmd.Parameters.AddWithValue("@bonus", GamePlayStats.ConsumedValues.CardBonus);
                        cmd.Parameters.AddWithValue("@tech_games", (GamePlayStats.CardTechGames <= 0 ? 0 : GamePlayStats.ConsumedValues.TechGame));
                        cmd.Parameters.AddWithValue("@credits_played", GamePlayStats.ConsumedValues.CardCredits
                                                                     + GamePlayStats.ConsumedValues.CardBonus
                                                                     + GamePlayStats.ConsumedValues.CardTime
                                                                     + (decimal)GamePlayStats.ConsumedValues.CardCreditPlusCardBalance
                                                                     + (decimal)GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits
                                                                     + GamePlayStats.ConsumedValues.CardCreditPlusBonus);
                        //  cmd.Parameters.AddWithValue("@card_number", card_number);
                        cmd.Parameters.AddWithValue("@cardId", cardId);
                        log.LogVariableState("@credits", GamePlayStats.ConsumedValues.CardCredits);
                        log.LogVariableState("@courtesy", GamePlayStats.ConsumedValues.CardCourtesy);
                        log.LogVariableState("@bonus", GamePlayStats.ConsumedValues.CardBonus);
                        log.LogVariableState("@tech_games", (GamePlayStats.CardTechGames <= 0 ? 0 : GamePlayStats.ConsumedValues.TechGame));
                        log.LogVariableState("@credits_played", GamePlayStats.ConsumedValues.CardCredits
                                                                     + GamePlayStats.ConsumedValues.CardBonus
                                                                     + GamePlayStats.ConsumedValues.CardTime
                                                                     + (decimal)GamePlayStats.ConsumedValues.CardCreditPlusCardBalance
                                                                     + (decimal)GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits
                                                                     + GamePlayStats.ConsumedValues.CardCreditPlusBonus);
                        //  log.LogVariableState("@card_number", card_number);
                        log.LogVariableState("@cardId", cardId);
                        int nrecs;
                        try
                        {
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Update card details for card: " + card_number);
                            nrecs = cmd.ExecuteNonQuery();
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Update card details for card: " + card_number);

                            if (GamePlayStats.ConsumedValues.TechGame > 0 && GamePlayStats.CardTechGames < GamePlayStats.ConsumedValues.TechGame) // technician card with no tech games balance but has card games
                            {
                                CardGames cardGames = new CardGames(Utilities);
                                int diff = GamePlayStats.ConsumedValues.TechGame - GamePlayStats.CardTechGames;
                                for (int i = 0; i < diff; i++)
                                {
                                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Update card game for technician card: " + card_number);
                                    cardGames.deductGameCount(GamePlayStats.CardGameIdList[i], SQLTrx);
                                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Update card game for technician card: " + card_number);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            message += ";Card Balance: " + ex.Message;
                            if (inSQLTrx == null && SQLTrx != null)
                                SQLTrx.Rollback();
                            //cmd.Connection.Close();
                            log.Error("Error occured while executing the update query", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(-1);
                            return -1;
                        }

                        if (nrecs == 1)
                        {
                            message += "; Card balance reduced by game play credit";
                        }
                        else if (nrecs == 0)
                        {
                            message += "; There was an error reducing card credits by game play credit. Card not found";
                        }
                        else
                        {
                            message += "; More than 1 card found while reducing card credit by game play credit";
                        }
                    }
                    else
                    {
                        cmd.CommandText = "update cards set start_time = isnull(start_time, getdate()), " +
                                                            "last_played_time = getdate(), " +
                                                            "credits_played = isnull(credits_played, 0) + @credits_played, " +
                                                            "last_update_time = getdate() " +
                                                        " where card_number = @card_number and valid_flag = 'Y'";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@credits_played", GamePlayStats.ConsumedValues.CardGame);
                        cmd.Parameters.AddWithValue("@card_number", card_number);
                        log.LogVariableState("@credits_played", GamePlayStats.ConsumedValues.CardGame);
                        log.LogVariableState("@card_number", card_number);

                        try
                        {
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Update card details in card games condition for card: " + card_number);
                            cmd.ExecuteNonQuery();
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Update card details in card games condition for card: " + card_number);
                        }
                        catch (Exception ex)
                        {
                            message += ex.Message;
                            if (inSQLTrx == null && SQLTrx != null)
                                SQLTrx.Rollback();
                            //cmd.Connection.Close();
                            log.Error("Error occured while executing the Query", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(-1);
                            return -1;
                        }

                        try
                        {
                            CardGames cardGames = new CardGames(Utilities);
                            int multiplier = (GamePlayStats.CurrentMachine == null ? 1 : GamePlayStats.CurrentMachine.GameplayMultiplier);
                            for (int i = 0; i < multiplier; i++)
                            {
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Update card game for card: " + card_number);
                                cardGames.deductGameCount(GamePlayStats.CardGameIdList[i], SQLTrx);
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Update card game for card: " + card_number);
                            }
                        }
                        catch (Exception ex)
                        {
                            message += ";Card game: " + ex.Message;
                            if (inSQLTrx == null && SQLTrx != null)
                                SQLTrx.Rollback();
                            //cmd.Connection.Close();
                            log.Error("Error while deducting game count", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(-1);
                            return -1;
                        }
                    }

                    long gamePlayId = 0;
                    try
                    {
                        int x = 1;
                        if (GamePlayStats.CurrentMachine != null)
                            x = GamePlayStats.CurrentMachine.GameplayMultiplier;

                        for (int i = 0; i < x; i++)
                        {
                            int cardGameId = -1;
                            if (i < GamePlayStats.CardGameIdList.Count)
                                cardGameId = GamePlayStats.CardGameIdList[i];

                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Create Game Play record for card: " + card_number + " and machine: " + MachineId);
                            gamePlayId = CreateGamePlayRecord(MachineId, card_number, GamePlayStats.ConsumedValues.CardCredits / x, GamePlayStats.ConsumedValues.CardCourtesy / x,
                                                                GamePlayStats.ConsumedValues.CardBonus / x, GamePlayStats.ConsumedValues.CardTime / x, GamePlayStats.ConsumedValues.CardGame / x,
                                                                (decimal)GamePlayStats.ConsumedValues.CardCreditPlusCardBalance / x, (decimal)GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits / x,
                                                                GamePlayStats.ConsumedValues.CardCreditPlusBonus / x, (GamePlayStats.TicketEligibility ? GamePlayStats.TicketMode : "N"), 0, GamePlayStats.GameplayMessage, cardGameId, GamePlayStats.PromotionId, GamePlayStats.PlayRequestTime, SQLTrx, ServerStatic);
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Create Game Play record for card: " + card_number + " and machine: " + MachineId);
                        }

                        message += "; Game Play record created";
                        additionalMessages.AppendLine();
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Create Game play Transaction for card: " + card_number + " and machine: " + MachineId);
                        CreateGamePlayTransaction(gamePlayId, ServerStatic, GamePlayStats, SQLTrx);
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Create Game play Transaction  for card: " + card_number + " and machine: " + MachineId);
                    }
                    catch (Exception ex)
                    {
                        message += "; " + ex.Message;
                        if (inSQLTrx == null && SQLTrx != null)
                            SQLTrx.Rollback();
                        //cmd.Connection.Close();
                        log.Error("Error while creating the game play record", ex);
                        log.LogVariableState("Message", message);
                        log.LogMethodExit(-1);
                        return -1;
                    }

                    try
                    {
                        if (ServerStatic.ALLOW_LOYALTY_ON_GAMEPLAY == "Y")
                        {
                            try
                            {
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Execute Loyalty on Gameplay for card: " + card_number + " and machine: " + MachineId);
                                Loyalty loyalty = new Loyalty(Utilities);
                                Utilities.ParafaitEnv.User_Id = Utilities.ParafaitEnv.ExternalPOSUserId;
                                loyalty.LoyaltyOnGamePlay(gamePlayId, "Y", SQLTrx);
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Execute Loyalty on Gameplay for card: " + card_number + " and machine: " + MachineId);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error in loyalty on game play", ex);
                                message += "; Loyalty: " + ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        message += "; " + ex.Message;
                        if (inSQLTrx == null && SQLTrx != null)
                            SQLTrx.Rollback();
                        //cmd.Connection.Close();
                        log.Error("Error in server static allow loyalty on game play", ex);
                        log.LogVariableState("Message", message);
                        log.LogMethodExit(-1);
                        return -1;
                    }

                    if (inSQLTrx == null && SQLTrx != null)
                        SQLTrx.Commit();
                    //cmd.Connection.Close();
                    //cmd.Dispose();
                    if ((DateTime.Now - GamePlayStats.PlayRequestTime).TotalSeconds >= 5) //greater than 5 seconds
                    {
                        message += additionalMessages.ToString();
                    }
                    log.LogVariableState("Message", message);
                    log.LogMethodExit(gamePlayId);
                    return gamePlayId;
                }
            }
        }

        public static List<GamePlayDTO> CreateCreditCardGamePlay(int machineId, 
                                                                 CreditCardDetailDTO creditCardDetailDTO, 
                                                                 int noOfGamePlays, 
                                                                 int productId, 
                                                                 ServerStatic _ServerStatic,
                                                                 SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry(machineId, creditCardDetailDTO, noOfGamePlays, productId,
                _ServerStatic, inSQLTrx);
            List<GamePlayDTO> result = new List<GamePlayDTO>();
            string cardNumber = _ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;
            Semnox.Parafait.Transaction.Transaction Trx = new Semnox.Parafait.Transaction.Transaction(_ServerStatic.Utilities);
            int ret = 0;
            string message = "";
            List<decimal> priceList = new List<decimal>();
            decimal remainingAmount = creditCardDetailDTO.AmountAuthorized;
            string maskedCreditCardNumber = string.Empty.PadLeft(creditCardDetailDTO.PartialPan.Length - 4, 'X') + creditCardDetailDTO.PartialPan.Substring(creditCardDetailDTO.PartialPan.Length - 4); ;
            string acqRefData = "AID:" + creditCardDetailDTO.AID + "|ARC:" + creditCardDetailDTO.ARC + "|IAD:" + creditCardDetailDTO.IAD + "|TSI:" + creditCardDetailDTO.TSI + "|TID:" + creditCardDetailDTO.TID + "|CVM:"+ creditCardDetailDTO.CVM + "|CardTokenA:" + creditCardDetailDTO.CardTokenA + "|CardTokenB:" + creditCardDetailDTO.CardTokenB;
            DateTime creditCardTransactionDate;
            try
            {
                creditCardTransactionDate = Convert.ToDateTime(creditCardDetailDTO.TransactionTime);
            }
            catch (Exception)
            {
                creditCardTransactionDate = ServerDateTime.Now;
            }
            if((ServerDateTime.Now - creditCardTransactionDate).TotalDays > 1)
            {
                creditCardTransactionDate = ServerDateTime.Now;
            }
            for (int i = 0; i < noOfGamePlays; i++)
            {
                decimal price = creditCardDetailDTO.AmountAuthorized / noOfGamePlays;
                remainingAmount -= price;
                priceList.Add(price);
            }
            priceList[noOfGamePlays - 1] = priceList[noOfGamePlays - 1] + remainingAmount;
            for (int i = 0; i < noOfGamePlays; i++)
            {
                long gamePlayId = CreateGamePlayRecord(machineId, cardNumber, priceList[i], 0, 0, 0, 0, 0, 0, 0, "T", 0, "Credit Card " + maskedCreditCardNumber, -1, -1, DateTime.Now, inSQLTrx, _ServerStatic);
                if (gamePlayId <= -1)
                {
                    string errorMessage = "An error occurred while attempting to create the gameplay";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new Exception();
                }
                GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler();
                GamePlayDTO gamePlayDTO = gamePlayDataHandler.GetGamePlayDTO(Convert.ToInt32(gamePlayId), inSQLTrx);
                result.Add(gamePlayDTO);
                Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine = new Semnox.Parafait.Transaction.Transaction.TransactionLine();
                Card card = new Card(cardNumber, "", _ServerStatic.Utilities, inSQLTrx);
                ret = Trx.createTransactionLine(card,
                                                productId,
                                                (double)priceList[i], 1, ref message, trxLine);
                if (ret != 0)
                {
                    string errorMessage = "An error occurred while attempting to create the transaction line.";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new Exception();
                }
            }

            ret = Trx.SaveOrder(ref message, inSQLTrx);
            if (ret != 0)
            {
                log.LogMethodExit(null, "Throwing Application exception-" + message);
                throw new ApplicationException(message);
            }

            LookupValuesContainerDTO lookupValuesContainerDTO = LookupsContainerList.GetLookupValuesContainerDTOOrDefault(_ServerStatic.Utilities.ExecutionContext.GetSiteId(), "CREDIT_CARD_GAME_PLAY_ATTRIBUTES", "PaymentModeGuid");
            if (lookupValuesContainerDTO == null)
            {
                string errorMessage = "Credit card game play payment mode is not defined.";
                log.LogMethodExit(null, "Throwing Application exception-" + errorMessage);
                throw new ApplicationException(errorMessage);
            }
            List<PaymentModeDTO> paymentModeDTOList = null;
            try
            {
                PaymentModeList paymentModeListBL = new PaymentModeList(_ServerStatic.Utilities.ExecutionContext);
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, _ServerStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_GUID, lookupValuesContainerDTO.Description));
                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the payment mode", ex);
            }
            
            if (paymentModeDTOList == null)
            {
                string errorMessage = "Credit card game play payment mode is not valid.";
                log.LogMethodExit(null, "Throwing Application exception-" + errorMessage);
                throw new ApplicationException(errorMessage);
            }
            
            CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(Trx.Trx_id, creditCardDetailDTO.AmountAuthorized, "CREDIT CARD PAYMENT", _ServerStatic.Utilities);

            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO(responseID: -1, invoiceNo : ccRequestPGWDTO.RequestID.ToString(), tokenID: string.Empty, recordNo: string.Empty, dSIXReturnCode: string.Empty, statusID : -1,
                                textResponse: string.Empty, acctNo: maskedCreditCardNumber, cardType: string.Empty, tranCode: "SALE", refNo: creditCardDetailDTO.AuthorizationCode, purchase: creditCardDetailDTO.AmountAuthorized.ToString(),
                                authorize: creditCardDetailDTO.AmountAuthorized.ToString(), transactionDatetime: creditCardTransactionDate, authCode: creditCardDetailDTO.AuthId, processData: string.Empty, responseOrigin: string.Empty, userTraceData: creditCardDetailDTO.CardType, captureStatus: creditCardDetailDTO.Channel, acqRefData: acqRefData,
                                tipAmount : string.Empty, customerCopy: string.Empty, merchantCopy: string.Empty, customerCardProfileId: string.Empty);
            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, _ServerStatic.Utilities.ExecutionContext);
            cCTransactionsPGWBL.Save();
            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, Trx.Trx_id, paymentModeDTOList[0].PaymentModeId, Convert.ToDouble(creditCardDetailDTO.AmountAuthorized),
                                                                              maskedCreditCardNumber, string.Empty, creditCardDetailDTO.CardType, string.Empty, string.Empty, -1, string.Empty, -1, -1, "", "", false, _ServerStatic.Utilities.ExecutionContext.GetSiteId(), ccTransactionsPGWDTO.ResponseID, "", ServerDateTime.Now,
                                                                             _ServerStatic.Utilities.ExecutionContext.GetUserId(), -1, null, 0, -1, _ServerStatic.Utilities.ExecutionContext.POSMachineName, -1, string.Empty, null, null, null, true);
            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
            Trx.TransactionPaymentsDTOList.Add(trxPaymentDTO);

            ret = Trx.SaveOrder(ref message, inSQLTrx);

            if (ret != 0)
            {
                log.LogMethodExit(null, "Throwing Application exception-" + message);
                throw new ApplicationException(message);
            }
            for (int i = 0; i < noOfGamePlays; i++)
            {
                if (!Transaction.createTransactionGamePlayRecord(result[i].GameplayId, Trx.Trx_id, i + 1, _ServerStatic, inSQLTrx, ref message))
                    throw new ApplicationException(message);
            }
            if (!Trx.CompleteTransaction(inSQLTrx, ref message))
            {
                log.LogMethodExit(null, "Throwing Application exception-Unable to Complete transaction.");
                throw new ApplicationException("Unable to Complete transaction.");
            }
            log.LogMethodExit(result);
            return result;
        }

        

        private static CCRequestPGWDTO CreateCCRequestPGW(int transactionId,
                                                          decimal amount,
                                                          string transactionType,
                                                          Utilities utilities)
        {
            log.LogMethodEntry(transactionId, transactionType);
            CCRequestPGWDTO cCRequestPGWDTO = new CCRequestPGWDTO();
            cCRequestPGWDTO.InvoiceNo = transactionId.ToString();
            cCRequestPGWDTO.POSAmount = amount.ToString("#,##0.000");
            cCRequestPGWDTO.TransactionType = transactionType;
            cCRequestPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS, utilities);
            cCRequestPGWDTO.MerchantID = utilities.ParafaitEnv.POSMachine;
            cCRequestPGWDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_INITIATED.ToString();

            CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestPGWDTO);
            cCRequestPGWBL.Save();
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

        private static int GetCCStatusPGWStatusId(string status, Utilities utilities)
        {
            log.LogMethodEntry(status);
            int returnValue = -1;
            CCStatusPGWListBL cCStatusPGWListBL = new CCStatusPGWListBL();
            List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>(CCStatusPGWDTO.SearchByParameters.STATUS_MESSAGE, status));
            searchParameters.Add(new KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>(CCStatusPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<CCStatusPGWDTO> cCStatusPGWDTOList = cCStatusPGWListBL.GetCCStatusPGWDTOList(searchParameters);
            if (cCStatusPGWDTOList != null && cCStatusPGWDTOList.Count == 1)
            {
                returnValue = cCStatusPGWDTOList[0].StatusId;
            }
            else
            {
                log.LogMethodExit(null, "Throwing Payment Gateway Exception Status: " + status + " doesn't exist. Please check the configuration.");
                throw new PaymentGatewayException("Status: " + status + " doesn't exist. Please check the configuration.");
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        static void CreateGamePlayTransaction(long gamePlayId, ServerStatic serverStatic, ServerStatic.GlobalGamePlayStats gameplayStats, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(gamePlayId, serverStatic, gameplayStats, SQLTrx);
            if (gameplayStats.ProductId == -1)
            {
                log.LogMethodExit(null);
                return;
            }


            if (!gameplayStats.TechCard.Equals('N'))
            {
                log.LogMethodExit(null);
                return;
            }


            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch (Exception ex)
            {
                log.LogVariableState("MachineName", Environment.MachineName);
                log.Error("Error while calculating the ip address", ex);
            }
            serverStatic.Utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            serverStatic.Utilities.ParafaitEnv.User_Id = serverStatic.Utilities.ParafaitEnv.ExternalPOSUserId;
            Semnox.Parafait.Transaction.Transaction Trx = new Semnox.Parafait.Transaction.Transaction(serverStatic.Utilities);
            string message = "";

            Card card = new Card(gameplayStats.CardId, "", serverStatic.Utilities);
            TransactionUtils trxUtils = new TransactionUtils(serverStatic.Utilities);
            DataTable dtProduct = trxUtils.getProductDetails(gameplayStats.ProductId, card);
            int ret = 0;
            Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine;
            switch (dtProduct.Rows[0]["Product_Type"].ToString())
            {
                case "GAMEPLAYTRXPRODUCT":
                    {
                        trxLine = new Semnox.Parafait.Transaction.Transaction.TransactionLine();
                        ret = Trx.createTransactionLine(card,
                                                        gameplayStats.ProductId,
                                                        gameplayStats.ConsumedValues.CardCreditPlusCardBalance
                                                        + gameplayStats.ConsumedValues.CardCreditPlusGameplayCredits
                                                        + (double)gameplayStats.ConsumedValues.CardCredits, 1, ref message, trxLine);
                        //if (ret == 0)
                        //    Trx.TrxLines[0].GameplayId = gamePlayId;
                    }
                    break;
                case "CHECK-IN":
                    {
                        if (dtProduct.Rows[0]["CheckInFacilityId"] == DBNull.Value)
                        {
                            log.LogMethodExit(null, "Throwing Application Exception- " + serverStatic.Utilities.MessageUtils.getMessage(225));
                            throw new ApplicationException(serverStatic.Utilities.MessageUtils.getMessage(225));
                        }
                        CustomerDTO customerDTO;
                        if (card.customerDTO == null)
                        {
                            customerDTO = card.customerDTO = new CustomerDTO();
                            if (ParafaitDefaultContainerList.GetParafaitDefault(serverStatic.Utilities.ExecutionContext, "CUSTOMERTYPE") != "N")
                            {
                                customerDTO.CustomerType = CustomerType.UNREGISTERED;
                            }
                            else
                            {
                                log.LogMethodExit(null, "Throwing Application Exception- " + serverStatic.Utilities.MessageUtils.getMessage(471));
                                throw new ApplicationException(serverStatic.Utilities.MessageUtils.getMessage(471));
                            }
                            customerDTO.FirstName = "Check-In customer " + card.CardNumber;
                        }
                        else
                        {
                            customerDTO = card.customerDTO;
                        }
                        string statusList = CheckInStatusConverter.ToString(CheckInStatus.ORDERED) + "," + CheckInStatusConverter.ToString(CheckInStatus.PAUSED);
                        List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> checkInSearchParams = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CARD_ID, card.card_id.ToString()));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
                        //checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS, CheckInStatusConverter.ToString(CheckInStatus.ORDERED)));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS_LIST, statusList));
                        CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(serverStatic.Utilities.ExecutionContext);
                        List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(checkInSearchParams, SQLTrx);
                        log.LogVariableState("checkInDetailDTOList", checkInDetailDTOList);
                        if (checkInDetailDTOList != null && checkInDetailDTOList.Any())
                        {
                            try
                            {
                                checkInDetailDTOList = checkInDetailDTOList.OrderByDescending(x => x.CheckInDetailId).ToList();
                                CheckInDetailDTO checkInDetailDTO = checkInDetailDTOList.FirstOrDefault();
                                if (checkInDetailDTO.CheckInTrxId > -1 && checkInDetailDTO.CheckInId > -1)
                                {
                                    checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                    using (NoSynchronizationContextScope.Enter())
                                    {
                                        ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(serverStatic.Utilities.ExecutionContext);
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInDetailDTO.CheckInId, new List<CheckInDetailDTO> { checkInDetailDTO }, SQLTrx);
                                        t.Wait();
                                    }
                                    log.Debug("UpdateCheckInStatus to Checked In ");
                                    List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, serverStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, card.card_id.ToString()));
                                    NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(serverStatic.Utilities.ExecutionContext);
                                    List<NotificationTagIssuedDTO> notificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters);
                                    log.LogVariableState("notificationTagIssuedListDTO", notificationTagIssuedListDTO);
                                    NotificationTagIssuedDTO notificationTagIssuedDTO = null;
                                    if (notificationTagIssuedListDTO != null && notificationTagIssuedListDTO.Any()
                                                        && notificationTagIssuedListDTO.Exists(tag => tag.StartDate == DateTime.MinValue))
                                    {
                                        notificationTagIssuedListDTO = notificationTagIssuedListDTO.OrderByDescending(x => x.IssueDate).ToList(); // get latest record 
                                        notificationTagIssuedDTO = notificationTagIssuedListDTO.Where(tag => tag.StartDate == DateTime.MinValue
                                                                                           && (tag.ExpiryDate == DateTime.MinValue || tag.ExpiryDate > serverStatic.Utilities.getServerTime())).FirstOrDefault();
                                        log.LogVariableState("notificationTagIssuedDTO", notificationTagIssuedDTO);
                                        NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(serverStatic.Utilities.ExecutionContext, notificationTagIssuedDTO);
                                        notificationTagIssuedBL.UpdateStartTime();
                                    }
                                    log.LogMethodExit("Transaction is already created. returning ");
                                    return; // Do not create transaction record
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                log.LogMethodExit("Throwing Application exception-Unable to Complete transaction.");
                                throw new ApplicationException("Unable to Complete transaction.");
                            }
                        }
                        // Should we create new check in and detail record? or update the existing one to trx
                        // The deduct balance should be done
                        else
                        {
                            CheckInDTO checkInDTO = new CheckInDTO(-1, customerDTO.Id, serverStatic.Utilities.getServerTime(),
                                                                       null, null, null, card.card_id, -1, -1,
                                                                       Convert.ToInt32(dtProduct.Rows[0]["CheckInFacilityId"]),
                                                                       -1, null, customerDTO, true);

                            int allowedTimeInMinutes = Convert.ToInt32(dtProduct.Rows[0]["Time"] == DBNull.Value ? 0 : dtProduct.Rows[0]["Time"]);
                            string autoCheckOut = dtProduct.Rows[0]["AutoCheckOut"].ToString();
                            CheckInDetailDTO checkInDetailDTO = new CheckInDetailDTO(-1, -1, customerDTO.FirstName, card.card_id, null, null, null, null, 0, null, null,
                                                                                      null,
                                                                                      autoCheckOut == "Y" ? serverStatic.Utilities.getServerTime().AddMinutes(allowedTimeInMinutes) : (DateTime?)null,
                                                                                      -1, null, -1, -1, serverStatic.Utilities.getServerTime(), CheckInStatus.CHECKEDIN, true);
                            checkInDTO.CheckInDetailDTOList.Add(checkInDetailDTO);
                            ret = Trx.createTransactionLine(card, gameplayStats.ProductId, checkInDTO, checkInDetailDTO, (double)gameplayStats.CreditsRequired, 1, ref message, null);
                            //if (ret == 0)
                            //    Trx.TrxLines[0].GameplayId = gamePlayId;
                        }
                    }
                    break;
                case "CHECK-OUT":
                    {
                        if (dtProduct.Rows[0]["CheckInFacilityId"] == DBNull.Value)
                        {
                            log.LogMethodExit(null, "Throwing Application Exception- " + serverStatic.Utilities.MessageUtils.getMessage(225));
                            throw new ApplicationException(serverStatic.Utilities.MessageUtils.getMessage(225));
                        }
                        List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> checkInSearchParams = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CARD_ID, card.card_id.ToString()));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS, CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN)));
                        CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(serverStatic.Utilities.ExecutionContext);
                        List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(checkInSearchParams);
                        log.LogVariableState("checkInDetailDTOList", checkInDetailDTOList);
                        if (checkInDetailDTOList != null && checkInDetailDTOList.Any()
                                 && checkInDetailDTOList.Exists(x => x.CheckInTime != null &&
                                  (x.CheckOutTime == null || x.CheckOutTime > serverStatic.Utilities.getServerTime())))
                        {
                            List<CheckInDetailDTO> filteredCheckInDetailDTOList = checkInDetailDTOList.
                                                                                  Where(x => x.CheckInTime != null && (x.CheckOutTime == null || x.CheckOutTime > serverStatic.Utilities.getServerTime()))
                                                                                 .ToList();
                            if (filteredCheckInDetailDTOList != null && filteredCheckInDetailDTOList.Any())
                            {
                                CheckInBL checkInBL = new CheckInBL(serverStatic.Utilities.ExecutionContext, filteredCheckInDetailDTOList[0].CheckInId);
                                decimal effectivePrice = checkInBL.GetCheckOutPrice(gameplayStats.ProductId, filteredCheckInDetailDTOList);
                                ret = 0;
                                for (int i = 0; i < filteredCheckInDetailDTOList.Count; i++)
                                {
                                    // The create transaction line will take care of updating status of check in and check in details records
                                    ret = Trx.createTransactionLine(card, gameplayStats.ProductId, checkInBL.CheckInDTO, filteredCheckInDetailDTOList[i], (double)effectivePrice, 1, ref message, null);
                                    //if (ret == 0)
                                    //{
                                    //    Trx.TrxLines[i].GameplayId = gamePlayId;
                                    //}
                                }
                            }
                        }
                        else
                        {
                            log.LogMethodExit(null, "Throwing Application Exception-Customer not checked in ");
                            throw new ApplicationException("Customer not checked in");
                        }
                    }
                    break;
                default:
                    {
                        log.LogMethodExit(null);
                        return;
                    }
            }

            if (ret == 0)
            {
                //trxLine.GameplayId = gamePlayId;
                ret = Trx.SaveOrder(ref message, SQLTrx);

                if (ret != 0)
                {
                    log.LogMethodExit(null, "Throwing Application exception-" + message);
                    throw new ApplicationException(message);
                }

                //Create Trx Line GamePlay mapping record
                if (!Transaction.createTransactionGamePlayRecord(gamePlayId, Trx.Trx_id, Trx.TrxLines.Count, serverStatic, SQLTrx, ref message))
                    throw new ApplicationException(message);
                PaymentModeList paymentModeListBL = new PaymentModeList(serverStatic.Utilities.ExecutionContext);
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                if (paymentModeDTOList != null)
                {
                    TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                    debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                    debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                    debitTrxPaymentDTO.TransactionId = Trx.Trx_id;
                    debitTrxPaymentDTO.PosMachine = serverStatic.Utilities.ParafaitEnv.POSMachine;
                    debitTrxPaymentDTO.Amount = Trx.Net_Transaction_Amount;
                    debitTrxPaymentDTO.CardId = (new Card(serverStatic.TOKEN_MACHINE_GAMEPLAY_CARD, "", serverStatic.Utilities)).card_id;
                    debitTrxPaymentDTO.CardEntitlementType = "C";
                    TransactionPaymentsBL debitTrxPaymentBL = new TransactionPaymentsBL(serverStatic.Utilities.ExecutionContext, debitTrxPaymentDTO);
                    debitTrxPaymentBL.Save(SQLTrx);
                }


                if (!Trx.CompleteTransaction(SQLTrx, ref message))
                {
                    log.LogMethodExit(null, "Throwing Application exception-Unable to Complete transaction.");
                    throw new ApplicationException("Unable to Complete transaction.");
                }
            }
            else
            {
                log.LogMethodExit(null, "Throwing Application exception-" + message);
                throw new ApplicationException(message);
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Method to update ticket count for Ticket Eater machine. First check with Machine id.
        /// If Reference Machine exists, then check with Reference Machine for update
        /// </summary>
        /// <param name="CardNumber">Card Number</param>
        /// <param name="CurrentMachine">Machine Object</param>
        /// <param name="TicketCount">Ticket Count</param>
        /// <param name="SQLTrx">SQL Transaction</param>
        /// <param name="ServerStatic">ServerStatic Object</param>
        /// <param name="message">Message</param>
        static void UpdateTicketEaterGamePlay(string CardNumber, Machine CurrentMachine, int TicketCount, SqlTransaction SQLTrx, ServerStatic ServerStatic, ref string message)
        {
            log.LogMethodEntry(CardNumber, CurrentMachine, TicketCount, SQLTrx, ServerStatic, message);
            //SqlCommand cmd;
            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@cardNumber", CardNumber);
            queryParams[1] = new SqlParameter("@tickets", TicketCount);
            queryParams[2] = new SqlParameter("@machine_id", CurrentMachine.MachineId);

            int queryResult = ServerStatic.Utilities.executeNonQuery(@"update gameplay set ticket_count = ticket_count + @tickets
                                                                        where gameplay_id = (select max(gameplay_id)
                                                                                          from gameplay g, cards c
                                                                                        where c.card_id = g.card_id
                                                                                          and c.card_number = @cardNumber
                                                                                          and c.valid_flag = 'Y'
                                                                                          and machine_id = @machine_id
                                                                                          and play_date > dateadd(MI, -3, getdate()))",
                                                                       SQLTrx, queryParams);
            log.LogVariableState("@cardNumber", CardNumber);
            log.LogVariableState("@tickets", TicketCount);
            log.LogVariableState("@machine_id", CurrentMachine.MachineId);
            log.LogVariableState("Message", message);
            if (queryResult == 0)
            {
                if (CurrentMachine.MachineId != CurrentMachine.GameplayMachineId)
                {
                    queryParams = new SqlParameter[3];
                    queryParams[0] = new SqlParameter("@cardNumber", CardNumber);
                    queryParams[1] = new SqlParameter("@tickets", TicketCount);
                    queryParams[2] = new SqlParameter("@machine_id", CurrentMachine.GameplayMachineId);
                    queryResult = ServerStatic.Utilities.executeNonQuery(@"update gameplay set ticket_count = ticket_count + @tickets
                                                                        where gameplay_id = (select max(gameplay_id)
                                                                                          from gameplay g, cards c
                                                                                        where c.card_id = g.card_id
                                                                                          and c.card_number = @cardNumber
                                                                                          and c.valid_flag = 'Y'
                                                                                          and machine_id = @machine_id
                                                                                          and play_date > dateadd(MI, -3, getdate()))",
                                                                           SQLTrx, queryParams);
                }
                if (queryResult == 0) //If not found in Reference or actual machine, create gameplay against Machineid
                {
                    CreateGamePlayRecord(CurrentMachine.MachineId, CardNumber, 0, 0, 0, 0, 0,
                                     0, 0, 0,
                                     "T", TicketCount, "Ticket Eater log", -1, -1, DateTime.Now, SQLTrx, ServerStatic);
                }
            }
            log.LogMethodExit(null);
        }

        public static long CreateGamePlayRecord(int MachineId, string card_number,
                                                 decimal credits, decimal courtesy,
                                                 decimal bonus, decimal time,
                                                 decimal CardGame, decimal CPCardBalance, decimal CPCredits, decimal CPBonus,
                                                 string TicketMode, int TicketCount, string Notes,
                                                 int cardGameId, int PromotionId, DateTime PlayRequestTime,
                                                 SqlTransaction SQLTrx, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(MachineId, card_number, credits, courtesy, bonus, time, CardGame, CPCardBalance, CPCredits, CPBonus, TicketMode, TicketCount, Notes, cardGameId, PromotionId, SQLTrx, ServerStatic);
            SqlParameter[] queryParams = new SqlParameter[17];
            queryParams[0] = new SqlParameter("@credits", credits);
            queryParams[1] = new SqlParameter("@courtesy", courtesy);
            queryParams[2] = new SqlParameter("@bonus", bonus);
            queryParams[3] = new SqlParameter("@time", time);
            queryParams[4] = new SqlParameter("@card_number", card_number);
            queryParams[5] = new SqlParameter("@machine_id", MachineId);
            queryParams[6] = new SqlParameter("@notes", Notes);
            queryParams[7] = new SqlParameter("@ticket_mode", TicketMode);
            queryParams[8] = new SqlParameter("@TicketCount", TicketCount);
            queryParams[9] = new SqlParameter("@CardGame", CardGame);
            queryParams[10] = new SqlParameter("@CPCardBalance", CPCardBalance);
            queryParams[11] = new SqlParameter("@CPCredits", CPCredits);
            queryParams[12] = new SqlParameter("@CPBonus", CPBonus);
            queryParams[13] = new SqlParameter("@CardGameId", cardGameId <= 0 ? DBNull.Value : (object)cardGameId);
            queryParams[14] = new SqlParameter("@promotionId", PromotionId <= 0 ? DBNull.Value : (object)PromotionId);
            queryParams[15] = new SqlParameter("@playRequestTime", PlayRequestTime == DateTime.MinValue ? DBNull.Value : (object)PlayRequestTime);
            queryParams[16] = new SqlParameter("@siteId", ServerStatic.Utilities.ParafaitEnv.SiteId == -1 ? DBNull.Value : (object)ServerStatic.Utilities.ParafaitEnv.SiteId);
            //SqlCommand cmd;

            object o = ServerStatic.Utilities.executeScalar(@"insert into gameplay 
                                                                (machine_id, card_id,
                                                                credits, courtesy, bonus,
                                                                time, CardGame, CPCardBalance, CPCredits, CPBonus, 
                                                                play_date, notes, ticket_mode, Ticket_Count, CardGameId, PromotionId, PlayRequestTime,site_id,PayoutCost) 
                                                               select TOP 1 @machine_id, c1.card_id, 
                                                                @credits, @courtesy, @bonus,
                                                                @time, @CardGame, @CPCardBalance, @CPCredits, @CPBonus, 
                                                                getdate(), @notes, @ticket_mode, @TicketCount, @cardGameId, @promotionId, @playRequestTime,@siteId,
                                                                isnull(m.PayoutCost, (select top 1 convert(float, isnull(default_value, m.PayoutCost)) ticketCost 
	                                                                                   from parafait_defaults 
	                                                                                  where default_value_name = 'TICKET_COST'))
                                                                 from cards c1, machines m 
                                                                where card_id = (isnull( (select max(c2.card_id)
										                                                    from cards c2
										                                                    where c2.valid_flag = 'Y' 
										                                                      and c2.card_number = @card_number), 
									                                                      (select max(c3.card_id)
										                                                    from cards c3
										                                                    where c3.card_number = @card_number)
							                                                            )
							                                                    )
                                                                and m.machine_id = @machine_id;
                                                                select @@identity", SQLTrx, queryParams);

            //SqlCommand cmd;
            //if (SQLTrx == null)
            //    cmd = ServerStatic.Utilities.getCommand(ServerStatic.Utilities.createConnection());
            //else
            //    cmd = ServerStatic.Utilities.getCommand(SQLTrx);

            //cmd.CommandText = @"insert into gameplay 
            //                    (machine_id, card_id,
            //                    credits, courtesy, bonus,
            //                    time, CardGame, CPCardBalance, CPCredits, CPBonus, 
            //                    play_date, notes, ticket_mode, Ticket_Count, CardGameId, PromotionId, PlayRequestTime, PayoutCost) 
            //                   select TOP 1 @machine_id, c1.card_id, 
            //                    @credits, @courtesy, @bonus,
            //                    @time, @CardGame, @CPCardBalance, @CPCredits, @CPBonus, 
            //                    getdate(), @notes, @ticket_mode, @TicketCount, @cardGameId, @promotionId, @playRequestTime,
            //                    isnull(m.PayoutCost, (select top 1 convert(float, isnull(default_value, m.PayoutCost)) ticketCost 
            //                                           from parafait_defaults 
            //                                          where default_value_name = 'TICKET_COST'))
            //                     from cards c1, machines m 
            //                    where card_id = (select max(c2.card_id)
            //                                       from cards c2
            //                                      where c2.card_number = @card_number)
            //                    and m.machine_id = @machine_id;
            //                    select @@identity";

            //cmd.Parameters.AddWithValue("@credits", credits);
            //cmd.Parameters.AddWithValue("@courtesy", courtesy);
            //cmd.Parameters.AddWithValue("@bonus", bonus);
            //cmd.Parameters.AddWithValue("@time", time);
            //cmd.Parameters.AddWithValue("@card_number", card_number);
            //cmd.Parameters.AddWithValue("@machine_id", MachineId);
            //cmd.Parameters.AddWithValue("@notes", Notes);
            //cmd.Parameters.AddWithValue("@ticket_mode", TicketMode);
            //cmd.Parameters.AddWithValue("@TicketCount", TicketCount);
            //cmd.Parameters.AddWithValue("@CardGame", CardGame);
            //cmd.Parameters.AddWithValue("@CPCardBalance", CPCardBalance);
            //cmd.Parameters.AddWithValue("@CPCredits", CPCredits);
            //cmd.Parameters.AddWithValue("@CPBonus", CPBonus);
            //cmd.Parameters.AddWithValue("@CardGameId", cardGameId <= 0 ? DBNull.Value : (object)cardGameId);
            //cmd.Parameters.AddWithValue("@promotionId", PromotionId <= 0 ? DBNull.Value : (object)PromotionId);
            //cmd.Parameters.AddWithValue("@playRequestTime", PlayRequestTime == DateTime.MinValue ? DBNull.Value : (object)PlayRequestTime);


            log.LogVariableState("@credits", credits);
            log.LogVariableState("@courtesy", courtesy);
            log.LogVariableState("@bonus", bonus);
            log.LogVariableState("@time", time);
            log.LogVariableState("@card_number", card_number);
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@notes", Notes);
            log.LogVariableState("@ticket_mode", TicketMode);
            log.LogVariableState("@TicketCount", TicketCount);
            log.LogVariableState("@CardGame", CardGame);
            log.LogVariableState("@CPCardBalance", CPCardBalance);
            log.LogVariableState("@CPCredits", CPCredits);
            log.LogVariableState("@CPBonus", CPBonus);
            log.LogVariableState("@CardGameId", cardGameId <= 0 ? DBNull.Value : (object)cardGameId);
            log.LogVariableState("@promotionId", PromotionId <= 0 ? DBNull.Value : (object)PromotionId);
            log.LogVariableState("@playRequestTime", PlayRequestTime == DateTime.MinValue ? DBNull.Value : (object)PlayRequestTime);
            //object o = cmd.ExecuteScalar();

            //if (SQLTrx == null)
            //    cmd.Connection.Close();

            if (o != DBNull.Value)
            {
                log.LogMethodExit(Convert.ToInt32(o));
                return Convert.ToInt32(o);
            }

            else
            {
                log.LogMethodExit(-1);
                return -1;
            }

        }

        public static int UpdateTickets(Machine CurrentMachine, string card_number, int ticket_count, ServerStatic ServerStatic, ref string message)
        {
            log.LogMethodEntry(CurrentMachine, card_number, ticket_count, ServerStatic, message);
            try
            {
                if (ticket_count <= 0)
                {
                    log.LogVariableState("Message", message);
                    log.LogMethodExit(0);
                    return 0;
                }

                string ticketMultiplierStr = CurrentMachine.Configuration.getValue("TICKET_MULTIPLIER");
                int ticketMultiplier = 1;
                if (Int32.TryParse(ticketMultiplierStr, out ticketMultiplier))
                {
                    if (ticketMultiplier > 1)
                    {
                        ticket_count = ticket_count * ticketMultiplier;
                        log.LogVariableState("ticketMultiplier", ticketMultiplier);
                        log.LogVariableState("ticket_count", ticket_count);
                    }
                }

                if (CurrentMachine.TicketEater == "Y" && CurrentMachine.TokenRedemption != "Y") // ticket eater
                {
                    using (SqlConnection cnn = ServerStatic.Utilities.createConnection())
                    {
                        SqlTransaction SQLTrx = cnn.BeginTransaction();
                        log.LogVariableState("@cardNumber", card_number);
                        log.LogVariableState("@tickets", ticket_count);

                        object objCardId = ServerStatic.Utilities.executeScalar(@"SELECT card_id from cards
                                                                                    where card_number = @cardNumber
                                                                                    and valid_flag = 'Y'",
                                                                                    SQLTrx,
                                                                                new SqlParameter("@cardNumber", card_number));
                        int nrecs = 0;
                        try
                        {
                            if (objCardId == null) //card not found
                            {
                                DataTable dtCardDetails = ServerStatic.Utilities.executeDataTable(@"select top 1 card_number, c.card_id 
                                                                                                    from cards c, gameplay g 
                                                                                                    where g.machine_id = @machineId 
                                                                                                    and g.card_id = c.card_id
                                                                                                    and c.valid_flag = 'Y' 
                                                                                                    order by play_date desc"
                                                                                                    , SQLTrx
                                                                                                    , new SqlParameter("@machineId", CurrentMachine.MachineId));
                                log.LogVariableState("@machineId", CurrentMachine.MachineId);
                                if (dtCardDetails != null && dtCardDetails.Rows.Count > 0)
                                {
                                    message = "Ticket eater card " + card_number + " not found. Changed to " + dtCardDetails.Rows[0]["card_number"].ToString();
                                    card_number = dtCardDetails.Rows[0]["card_number"].ToString();
                                    nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(dtCardDetails.Rows[0]["card_id"]), ticket_count, SQLTrx, ServerStatic);
                                }
                                else if (dtCardDetails != null && dtCardDetails.Rows.Count == 0)
                                {
                                    //Check if Machine object has Reference machine, search for card using Reference machine id and associate card number
                                    if (CurrentMachine.GameplayMachineId != CurrentMachine.MachineId)
                                    {
                                        dtCardDetails = ServerStatic.Utilities.executeDataTable(@"select top 1 card_number, c.card_id 
                                                                                                    from cards c, gameplay g 
                                                                                                    where g.machine_id = @machineId 
                                                                                                    and g.card_id = c.card_id
                                                                                                    and c.valid_flag = 'Y' 
                                                                                                    order by play_date desc"
                                                                                                            , SQLTrx
                                                                                                            , new SqlParameter("@machineId", CurrentMachine.GameplayMachineId));
                                        log.LogVariableState("@machineId", CurrentMachine.GameplayMachineId);
                                        if (dtCardDetails != null && dtCardDetails.Rows.Count > 0)
                                        {
                                            message = "Ticket eater card " + card_number + " not found. Changed to " + dtCardDetails.Rows[0]["card_number"].ToString();
                                            card_number = dtCardDetails.Rows[0]["card_number"].ToString();
                                            nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(dtCardDetails.Rows[0]["card_id"]), ticket_count, SQLTrx, ServerStatic);
                                        }
                                        else
                                            message = "Ticket eater card " + card_number + " not found. No previous card found in gameplay.";
                                    }
                                    else
                                        message = "Ticket eater card " + card_number + " not found. No previous card found in gameplay.";
                                }
                            }
                            else
                            {
                                //Load tickets to Credit Plus. Method handles auto extend or specific expiry
                                nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(objCardId), ticket_count, SQLTrx, ServerStatic);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (SQLTrx != null)
                                SQLTrx.Rollback();
                            message = ex.Message;
                            log.Error("Error in executing the update query", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(0);
                            return 1;
                        }

                        if (nrecs == 1)
                        {
                            message = "Ticket eater ticket count updated on card " + card_number;
                        }
                        else if (nrecs > 1)
                        {
                            message = "More than 1 card found while updating ticket count from ticket eater";
                        }

                        try
                        {
                            ServerCore.GamePlay.UpdateTicketEaterGamePlay(card_number, CurrentMachine, ticket_count, SQLTrx, ServerStatic, ref message);
                        }
                        catch (Exception ex)
                        {
                            if (SQLTrx != null)
                                SQLTrx.Rollback();
                            message = ex.Message;
                            log.Error("Error in Update ticket eater game play", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(-1);
                            return -1;
                        }

                        if (SQLTrx != null)
                            SQLTrx.Commit();

                        log.LogVariableState("Message", message);
                        log.LogMethodExit(0);
                        return 0;
                    }
                }
                else
                {
                    if (CurrentMachine.TicketEater == "Y") // ATM Machine
                    {
                        log.Debug("CurrentMachine.TicketEater == Y");
                        log.LogVariableState("ticket_count", ticket_count);
                        try
                        {
                            ServerCore.GamePlay.UpdateTicketEaterGamePlay(card_number, CurrentMachine, ticket_count, null, ServerStatic, ref message);
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                            log.Error("Error in Update ticket eater game play", ex);
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(-1);
                            return -1;
                        }
                    }
                    //Logic to handle Slot reader. In case of Slot reader, specific card should have Game Play
                    //for the business day. If not, create Gameplay for passed card number
                    //In case Card Number is FFFFFFFFFF, then its physical ticket mode.
                    //In case of other cards, it's e-Ticket mode and card should be updated with e-Ticket count
                    if (CurrentMachine.Configuration.getValue("SLOT_READER") == "1")
                    {
                        object cardNumber = ServerStatic.Utilities.executeScalar(@"select card_number 
                                                                from cards c, gameplay g 
                                                                where g.machine_id = @machineId 
                                                                and g.card_id = c.card_id
                                                                and g.gameplay_id = (select max(gameplay_id)
                                                                                     from gameplay gg
                                                                                     where gg.machine_id = @machineId
                                                                                     and gg.play_date > DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 then getdate() else getdate() - 1 end)), 0))
                                                                                    )"
                                                        , new SqlParameter("@machineId", CurrentMachine.MachineId)
                                                     );
                        //If no game play or if game play and card number is not for same card
                        if (cardNumber == null || (cardNumber != null && cardNumber.ToString() != (card_number.StartsWith("FFFFFFFF") ? ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD : card_number)))
                        {
                            long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId, card_number.StartsWith("FFFFFFFF") ? ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD : card_number, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                   card_number.StartsWith("FFFFFFFF") ? "T" : "E", 0, "Slot Reader", -1, -1, DateTime.Now, null, ServerStatic);
                            if (gamePlayId >= 0)
                                message += "Slot Reader gameplay created for card: " + card_number + ";gameplayId: " + gamePlayId;
                            else
                            {
                                message += "Unable to create Slot Reader gameplay for card in UpdateTickets: " + card_number + ";ticket Count: " + ticket_count;
                                log.Error("Error in Update ticket method while creating Slot Reader gameplay for card: " + card_number);
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }
                    }
                    //End Slot reader change
                    using (SqlConnection cnn = ServerStatic.Utilities.createConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cnn;
                            // fetch latest game play record on this machine, on the same business day
                            cmd.CommandText = @"select g.card_id, g.gameplay_id, g.ticket_mode, isnull(g.ticket_count, 0) ticket_count, c.technician_card 
                                                from gameplay g, cards c
                                                where c.card_id = g.card_id
                                                and gameplay_id = (select max(gameplay_id)
                                                                     from gameplay gg
                                                                     where gg.machine_id = @machine_id
                                                                     and gg.play_date > DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 then getdate() else getdate() - 1 end)), 0)))";
                            //                                                             and (@coinPusher = 0 or (gg.card_id = c.card_id and c.card_number = @coinPusherCard)))";

                            cmd.Parameters.AddWithValue("@machine_id", CurrentMachine.MachineId);
                            log.LogVariableState("@machine_id", CurrentMachine.MachineId);
                            DataTable GamePlay = new DataTable();
                            SqlDataAdapter daGamePlay = new SqlDataAdapter(cmd);
                            daGamePlay.Fill(GamePlay);
                            daGamePlay.Dispose();

                            if (GamePlay.Rows.Count == 0)
                            {
                                //Check if Reference Machine Exists, search for gameplay on Reference Machine
                                //This is primarily to handle dual ticket dispenser machines
                                if (CurrentMachine.MachineId != CurrentMachine.GameplayMachineId)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@machine_id", CurrentMachine.GameplayMachineId);
                                    GamePlay = new DataTable();
                                    daGamePlay = new SqlDataAdapter(cmd);
                                    daGamePlay.Fill(GamePlay);
                                    daGamePlay.Dispose();
                                    if (GamePlay.Rows.Count == 0)
                                    {
                                        message = "Game Play not found for this machine";
                                        //cmd.Connection.Close();
                                        ServerStatic.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.GameplayMachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(), null);
                                        log.LogVariableState("Message", message);
                                        log.LogMethodExit(0);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    message = "Game Play not found for this machine";
                                    //cmd.Connection.Close();
                                    ServerStatic.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(), null);
                                    log.LogVariableState("Message", message);
                                    log.LogMethodExit(0);
                                    return 0;
                                }
                            }

                            if (GamePlay.Rows[0]["ticket_mode"].ToString() == "N")
                            {
                                message = "Ticket not allowed for this game play";
                                //cmd.Connection.Close();
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(0);
                                return 0;
                            }

                            int currentTickets = Convert.ToInt32(GamePlay.Rows[0]["ticket_count"]);
                            if (currentTickets + ticket_count > CurrentMachine.MaxTicketsPerGamePlay)
                                ticket_count = CurrentMachine.MaxTicketsPerGamePlay - currentTickets;

                            if (ticket_count <= 0)
                            {
                                ServerStatic.Utilities.EventLog.logEvent("Parafait Server", 'W', "MaxTicketsPerGamePlay", "Max Tickets Reached", "TICKETS", 1, "MachineId, CardId, MaxTicketsPerGamePlay", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.MaxTicketsPerGamePlay.ToString(), null);
                                message = "Max tickets reached for this game play";
                                //cmd.Connection.Close();
                                log.LogVariableState("Message", message);
                                log.LogMethodExit(1);
                                return 1;
                            }

                            if (CurrentMachine.TokenRedemption == "Y" && CurrentMachine.TokenPrice != 0)
                            {
                                // convert to credits in E ticket mode or if machine is a note acceptor (ticketEater should be set)
                                if ((GamePlay.Rows[0]["ticket_mode"].ToString() == "E" || CurrentMachine.TicketEater == "Y" || CurrentMachine.ForceRedeemToCard)
                                    && GamePlay.Rows[0]["technician_card"].ToString() != "Y")
                                {
                                    if (Transaction.createGamePlayTokenOutTransaction(Convert.ToInt64(GamePlay.Rows[0]["gameplay_id"]),
                                                                                      Convert.ToInt32(GamePlay.Rows[0]["card_id"]),
                                                                                      CurrentMachine.RedeemTokenTo,
                                                                                      ticket_count,
                                                                                      CurrentMachine.TokenPrice,
                                                                                      CurrentMachine.TicketEater,
                                                                                      ServerStatic, ref message))
                                    {
                                        message = "Token Redemption machine. Game Play bonus transaction created";
                                    }
                                    else
                                    {
                                        //cmd.Connection.Close();
                                        log.LogVariableState("Message", message);
                                        log.LogMethodExit(1);
                                        return 1;
                                    }
                                }
                            }

                            if (CurrentMachine.InventoryLocationId != -1)
                            {
                                message = "Linked inventory location. No ticket update on card. Reducing inventory.";
                                // object productId = ServerStatic.Utilities.executeScalar("select top 1 productId from inventory where locationId = @locationId",
                                //                                          new SqlParameter("@locationId", CurrentMachine.InventoryLocationId));
                                object productId = ServerStatic.Utilities.executeScalar("select top 1 inv.ProductId from inventory inv, product invP where" +
                                    " inv.locationId = @locationId and ISNULL(inv.Quantity, 0) != 0 and inv.ProductId = invP.ProductId and invp.IsActive = 'Y' " +
                                    "order by ISNULL(inv.LotId, 0)", new SqlParameter("@locationId", CurrentMachine.InventoryLocationId));

                                log.LogVariableState("@locationId", CurrentMachine.InventoryLocationId);
                                if (productId == null)
                                {
                                    message += " No inventory product found for location.";
                                    //cmd.Connection.Close();
                                    ServerStatic.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, InvLocId, TicketCount", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.InventoryLocationId.ToString() + ", " + ticket_count.ToString(), null);
                                    log.LogVariableState("Message", message);
                                    log.LogMethodExit(1);
                                    return 1;
                                }

                                try
                                {
                                    Inventory.AdjustInventory(Inventory.AdjustmentTypes.Payout,
                                                                        ServerStatic.Utilities,
                                                                        CurrentMachine.InventoryLocationId,
                                                                        Convert.ToInt32(productId),
                                                                        -1,
                                                                        "ParafaitServer",
                                                                        "Payout from machine: " + CurrentMachine.machine_name);
                                }
                                catch (Exception ex)
                                {
                                    message = ex.Message;
                                    //cmd.Connection.Close();
                                    log.Error("Error occured in adjust inventory", ex);
                                    log.LogVariableState("Message", message);
                                    log.LogMethodExit(1);
                                    return 1;
                                }
                            }
                            else if (CurrentMachine.TokenRedemption == "Y" && GamePlay.Rows[0]["technician_card"].ToString() != "Y")
                            {
                                message = "Token redemption machine. no ticket update on card";
                            }
                            else if (GamePlay.Rows[0]["ticket_mode"].ToString() == "E" || CurrentMachine.ForceRedeemToCard || ServerStatic.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD == "Y")
                            {
                                SqlTransaction SQLTrx = cmd.Connection.BeginTransaction();
                                cmd.Transaction = SQLTrx;
                                try
                                {
                                    UpdateTicketsCreditPlus(Convert.ToInt32(GamePlay.Rows[0]["card_id"]), ticket_count, SQLTrx, ServerStatic);
                                }
                                catch (Exception ex)
                                {
                                    SQLTrx.Rollback();
                                    message = ex.Message;
                                    log.Error("Error in executing query", ex);
                                    log.LogVariableState("Message", message);
                                    log.LogMethodExit(1);
                                    return 1;
                                }
                                SQLTrx.Commit();
                            }
                            else
                            {
                                message = "Real ticket mode and no auto update of tickets to cards";
                            }

                            if (CurrentMachine.TicketEater != "Y") // not ATM Machine
                            {
                                string ticketMode = GamePlay.Rows[0]["ticket_mode"].ToString();
                                if (CurrentMachine.ForceRedeemToCard || ServerStatic.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD == "Y")
                                    ticketMode = "E";

                                cmd.CommandText = @"update gameplay set ticket_count = isnull(ticket_count, 0) + @tickets,
                                                                ticket_mode = @ticketmode
                                                            where gameplay_id = @gameplay_id";

                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@gameplay_id", GamePlay.Rows[0]["gameplay_id"]);
                                cmd.Parameters.AddWithValue("@tickets", ticket_count);
                                cmd.Parameters.AddWithValue("@ticketMode", ticketMode);
                                cmd.ExecuteNonQuery();
                                log.LogVariableState("@gameplay_id", GamePlay.Rows[0]["gameplay_id"]);
                                log.LogVariableState("@tickets", ticket_count);
                                log.LogVariableState("@ticketMode", ticketMode);

                            }
                            log.LogVariableState("Message", message);
                            log.LogMethodExit(0);
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error("Exception occured while updating the tickets", ex);
                log.LogVariableState("Message", message);
                log.LogMethodExit(-1);
                return -1;
            }

        }

        /// <summary>
        /// updateTicketCreditPlus is for loading tickets to cardCreditPlus
        /// </summary>
        /// <param name="cardId">Card Id</param>
        /// <param name="ticketCount">Ticket count to be updated</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// <param name="ServerStatic">ServerStatic object</param>
        /// <returns>returns records updated</returns>
        public static int UpdateTicketsCreditPlus(int cardId, int ticketCount, SqlTransaction sqlTransaction, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(cardId, ticketCount, ServerStatic);
            int updatedRecs;
            updatedRecs = ServerStatic.Utilities.executeNonQuery(@"update CardCreditPlus set CreditPlus = CreditPlus + @tickets, 
                                                                              CreditPlusBalance = CreditPlusBalance + @tickets,
                                                                              LastupdatedDate = getdate()
                                                                       where CardCreditPlusId = (select top 1 CardCreditPlusId 
                                                                                                    from CardCreditPlus 
                                                                                                   where card_id = @card_id
                                                                                                    and CreditPlusType = 'T'
                                                                                                    and remarks = 'Gameplay Tickets'
                                                                                                    and ExtendOnReload = @extendOnReload
							                                                                        and (
									                                                                        (PeriodTo IS NOT NULL AND PeriodTo = DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0)) + @ExpireDays)
									                                                                        OR
									                                                                        (PeriodTo IS NULL AND CreationDate BETWEEN CASE WHEN datepart(HH, getdate()) between 0 and 5
																				                                                                        THEN DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate() - 1), 0))
																				                                                                        ELSE DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0))
																				                                                                        END
																		                                                                           AND CASE WHEN datepart(HH, getdate()) between 0 and 5
																					                                                                        THEN DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0))
																					                                                                        ELSE DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, getdate() + 1), 0))
																					                                                                    END
									                                                                        )
								                                                                        )
                                                                                                    order by CreationDate desc)"
                                                                  , sqlTransaction
                                                                  , new SqlParameter("@card_id", cardId)
                                                                  , new SqlParameter("@tickets", ticketCount)
                                                                  , new SqlParameter("@extendOnReload", ServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD)
                                                                  , new SqlParameter("@ExpireDays", ServerStatic.GAMEPLAY_TICKETS_EXPIRY_DAYS));

            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@tickets", ticketCount);
            log.LogVariableState("@ExpireDays", ServerStatic.GAMEPLAY_TICKETS_EXPIRY_DAYS);
            log.LogVariableState("@extendOnReload", ServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD);

            try
            {
                Loyalty loyalty = new Loyalty(ServerStatic.Utilities);
                if (updatedRecs == 0)
                {
                    loyalty.CreateGenericCreditPlusLine(Convert.ToInt32(cardId),
                                                        "T", ticketCount, false,
                                                        ServerStatic.GAMEPLAY_TICKETS_EXPIRY_DAYS,
                                                        ServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD,
                                                        "ServerCore", "Gameplay Tickets", sqlTransaction);
                    if (ServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD.Equals("Y"))
                        loyalty.ExtendOnReload(Convert.ToInt32(cardId), "T", sqlTransaction);
                }
                else
                {
                    Card updateCard = new Card(cardId, "", ServerStatic.Utilities, sqlTransaction);
                    updateCard.updateCardTime(sqlTransaction);
                    log.LogVariableState("Card Updated for Credit plus update scenario: ", updateCard.card_id);
                }
                if (ServerStatic.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD.Equals("Y"))
                    loyalty.ExtendOnReload(Convert.ToInt32(cardId), "T", sqlTransaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(updatedRecs);
            return updatedRecs;
        }

        public static void CreateGamePlayInfo(int MachineId, decimal playTime, SqlTransaction SQLTrx, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(MachineId, playTime, SQLTrx, ServerStatic);
            //SqlCommand cmd = ServerStatic.Utilities.getCommand(SQLTrx);

            // fetch latest game play record on this machine, on the same business day
            ServerStatic.Utilities.executeNonQuery(@"insert into gameplayinfo (gameplay_id, isPaused, play_time, last_update_by, last_update_date)
                                                    select g.gameplay_id, 'N', 
                                                        @playTime, 
                                                        @login, getdate()
                                                    from gameplay g
                                                    where gameplay_id = (select max(gameplay_id)
                                                                            from gameplay gg
                                                                            where gg.machine_id = @machine_id
                                                                            and gg.play_date > DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 then getdate() else getdate() - 1 end)), 0)))",
                                                       SQLTrx,
                                                       new SqlParameter("@machine_id", MachineId),
                                                       new SqlParameter("@login", ServerStatic.Utilities.ParafaitEnv.LoginID),
                                                       new SqlParameter("@playTime", playTime));

            //cmd.Parameters.AddWithValue("@machine_id", MachineId);
            //cmd.Parameters.AddWithValue("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            //cmd.Parameters.AddWithValue("@playTime", playTime);
            //cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@playTime", playTime);
            log.LogMethodExit(null);
        }

        public static void CreateGamePlayInfo(int MachineId, int CardGameId, SqlTransaction SQLTrx, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(MachineId, CardGameId, SQLTrx, ServerStatic);
            //SqlCommand cmd = ServerStatic.Utilities.getCommand(SQLTrx);

            // fetch latest game play record on this machine, on the same business day
            ServerStatic.Utilities.executeNonQuery(@"insert into gameplayinfo (gameplay_id, isPaused, play_time, last_update_by, last_update_date)
                                                    select g.gameplay_id, 'N', 
                                                        (select case EntitlementType 
                                                                when 'OVERS' then (case when isnumeric(OptionalAttribute) = 1 then convert(int, OptionalAttribute) else 0 end) 
                                                                else Quantity end
                                                        from cardgames 
                                                        where card_game_id = @cgId), 
                                                        @login, getdate()
                                                    from gameplay g
                                                    where gameplay_id = (select max(gameplay_id)
                                                                            from gameplay gg
                                                                            where gg.machine_id = @machine_id
                                                                            and gg.play_date > DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 then getdate() else getdate() - 1 end)), 0)))",
                                                       SQLTrx,
                                                       new SqlParameter("@machine_id", MachineId),
                                                       new SqlParameter("@login", ServerStatic.Utilities.ParafaitEnv.LoginID),
                                                       new SqlParameter("@cgId", CardGameId));

            //cmd.Parameters.AddWithValue("@machine_id", MachineId);
            //cmd.Parameters.AddWithValue("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            //cmd.Parameters.AddWithValue("@cgId", CardGameId);
            //cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@cgId", CardGameId);
            log.LogMethodExit(null);
        }

        public static long CreateMiFareGamePlay(Machine CurrentMachine,
                                               string CardNumber,
                                               string Price,
                                               string Credits,
                                               string Bonus,
                                               string Courtesy,
                                               string CreditPlusCredits,
                                               string DiscountPercentage,
                                               string CardBalance,
                                               string RecordId,
                                               string CardGamePlay,
                                               string CardGameBalance,
                                               ServerStatic ServerStatic,
                                               ref string Message)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, Price, Credits, Bonus, Courtesy, CreditPlusCredits, DiscountPercentage, CardBalance, RecordId, CardGamePlay, CardGameBalance, ServerStatic, Message);
            ServerStatic.GlobalGamePlayStats GamePlayStats = new ServerStatic.GlobalGamePlayStats(CurrentMachine);

            using (SqlConnection cnn = ServerStatic.Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandText = @"select top 1 technician_card, card_id
                                        from cards 
                                        where card_number = @cardNumber 
                                        order by card_id desc";
                    cmd.Parameters.Add(new SqlParameter("@cardNumber", CardNumber));
                    log.LogVariableState("@cardNumber", CardNumber);
                    SqlDataAdapter daCard = new SqlDataAdapter(cmd);
                    DataTable dtCard = new DataTable();
                    daCard.Fill(dtCard);

                    string GamePlayData = string.Join("-", new string[] { CardNumber, CardBalance, Price, Credits, Bonus, Courtesy, CreditPlusCredits, DiscountPercentage, CardGamePlay, CardGameBalance });

                    string TechCard;
                    object CardId;
                    if (dtCard.Rows.Count == 0) // card not found
                    {
                        if (ServerStatic.CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND)
                        {
                            CardNumber = ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;
                            cmd.Parameters["@cardNumber"].Value = CardNumber;
                            daCard.SelectCommand = cmd;
                            daCard.Fill(dtCard);
                        }

                        if (dtCard.Rows.Count == 0) // card not found
                        {
                            ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Card not found", "MIFARE-GAMEPLAY", 3);
                            cmd.Connection.Close();
                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(-1);
                            return -1;
                        }
                        else
                        {
                            CardId = dtCard.Rows[0]["card_id"];
                            TechCard = "N";
                        }
                    }
                    else
                    {
                        CardId = dtCard.Rows[0]["card_id"];
                        TechCard = dtCard.Rows[0]["technician_card"].ToString();
                    }

                    if (TechCard == "N") // regular card
                    {
                        if (CardGamePlay == "1") // card game play
                        {
                            cmd.CommandText = @"select g.play_credits game_play_credits, gp.play_credits profile_play_credits, 
                                               g.game_id, 
                                               g.game_profile_id
                                           from games g, game_profile gp, 
                                               machines m
                                               where g.game_profile_id = gp.game_profile_id 
                                               and g.game_id = m.game_id 
                                               and m.machine_id = @Machine_Id";

                            cmd.Parameters.AddWithValue("@Machine_Id", CurrentMachine.MachineId);
                            log.LogVariableState("@Machine_Id", CurrentMachine.MachineId);
                            DataTable DT = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(DT);

                            if (DT.Rows.Count > 0)
                            {
                                CardGames cardGames = new CardGames(ServerStatic.Utilities);
                                int cardGameId = -1;
                                bool cardGameTicketAllowed = true;
                                GamePlayStats.CardGames = cardGames.getCardGames((int)CardId, Convert.ToInt32(DT.Rows[0]["game_id"]), Convert.ToInt32(DT.Rows[0]["game_profile_id"]), ref cardGameTicketAllowed, ref cardGameId);

                                if (GamePlayStats.CardGames > 0)
                                {
                                    GamePlayStats.CardGameIdList.Add(cardGameId);
                                }
                                GamePlayStats.GameplayMessage = "Card Game Play";
                                if (DT.Rows[0]["game_play_credits"] == DBNull.Value)
                                {
                                    if (DT.Rows[0]["profile_play_credits"] != DBNull.Value)
                                        GamePlayStats.CreditsRequired = Convert.ToDecimal(DT.Rows[0]["profile_play_credits"]);
                                }
                                else
                                {
                                    GamePlayStats.CreditsRequired = Convert.ToDecimal(DT.Rows[0]["game_play_credits"]);
                                }
                                if (!ServerStatic.ZERO_PRICE_CARDGAME_PLAY)
                                    GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;

                                GamePlayStats.GamePlayType = ServerStatic.VALIDCARDGAME;
                            }
                            else
                            {
                                ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Card not found", "MIFARE-GAMEPLAY", 3);
                                cmd.Connection.Close();
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }
                        else
                        {
                            decimal.TryParse(Credits, out GamePlayStats.ConsumedValues.CardCredits);
                            GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.ConsumedValues.CardCredits / 100;

                            decimal.TryParse(Bonus, out GamePlayStats.ConsumedValues.CardBonus);
                            GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.ConsumedValues.CardBonus / 100;

                            decimal.TryParse(Courtesy, out GamePlayStats.ConsumedValues.CardCourtesy);
                            GamePlayStats.ConsumedValues.CardCourtesy = GamePlayStats.ConsumedValues.CardCourtesy / 100;

                            decimal.TryParse(CreditPlusCredits, out GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits);
                            GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits = GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits / 100;

                            if (GamePlayStats.ConsumedValues.CardCourtesy > 0)
                            {
                                if (GamePlayStats.ConsumedValues.CardCredits > 0 || GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0 || GamePlayStats.ConsumedValues.CardBonus > 0)
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.VALIDCREDITCOURTESY;
                                    GamePlayStats.GameplayMessage = "Credit, Bonus and Courtesy Play";
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.VALIDCOURTESY;
                                    GamePlayStats.GameplayMessage = "Courtesy Play";
                                }
                            }
                            else if (GamePlayStats.ConsumedValues.CardBonus > 0)
                            {
                                if (GamePlayStats.ConsumedValues.CardCredits > 0 || GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0)
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.VALIDCREDITBONUS;
                                    GamePlayStats.GameplayMessage = "Credit and Bonus Play";
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = ServerStatic.VALIDBONUS;
                                    GamePlayStats.GameplayMessage = "Bonus Play";
                                }
                            }
                            else
                            {
                                GamePlayStats.GamePlayType = ServerStatic.VALIDCREDIT;
                                GamePlayStats.GameplayMessage = "Credit Play";
                            }

                            decimal price = 0;
                            decimal.TryParse(Price, out price);
                            if (price / 100 != GamePlayStats.ConsumedValues.CardCredits + GamePlayStats.ConsumedValues.CardBonus + GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits)
                            {
                                ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Price does not match total consumed", "MIFARE-GAMEPLAY", 3);
                                cmd.Connection.Close();
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }
                    } // tech card
                    else
                    {
                        GamePlayStats.GamePlayType = ServerStatic.TECHPLAY;
                        GamePlayStats.ConsumedValues.TechGame = 1;
                        GamePlayStats.GameplayMessage = "Tech Play";
                    }

                    int ReaderRecordId = 0;
                    Int32.TryParse(RecordId, out ReaderRecordId);

                    cmd.CommandText = @"select top 1 1
                                from GamePlayInfo i, GamePlay g
                                where g.gameplay_id = i.gameplay_id
                                and g.machine_id = @machineId
                                and i.ReaderRecordId = @ReaderRecordId
                                and g.card_id = @cardId
                                and i.GamePlayData = @GamePlayData";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@machineId", CurrentMachine.MachineId));
                    cmd.Parameters.Add(new SqlParameter("@cardId", CardId));
                    cmd.Parameters.Add(new SqlParameter("@ReaderRecordId", ReaderRecordId));
                    cmd.Parameters.Add(new SqlParameter("@GamePlayData", GamePlayData));
                    log.LogVariableState("@machineId", CurrentMachine.MachineId);
                    log.LogVariableState("@cardId", CardId);
                    log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                    log.LogVariableState("@GamePlayData", GamePlayData);
                    if (cmd.ExecuteScalar() != null)
                    {
                        Message = "Duplicate Gameplay: " + CurrentMachine.machine_name;
                        ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData + "-" + ReaderRecordId.ToString(), Message, "MIFARE-GAMEPLAY", 3);
                        cmd.Connection.Close();
                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(0);
                        return 0;
                    }

                    long gamePlayId = CreateGamePlay(CurrentMachine.MachineId, CardNumber, GamePlayStats, ServerStatic, ref Message);

                    if (gamePlayId < 0)
                        ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'W', GamePlayData, Message, "MIFARE-GAMEPLAY", 3);
                    else
                    {
                        cmd.CommandText = @"insert into gameplayinfo (gameplay_id, ReaderRecordId, GamePlayData, last_update_by, last_update_date)
                                        values (@gameplay_id, @ReaderRecordId, @GamePlayData, @last_update_by, getdate())";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                        cmd.Parameters.AddWithValue("@ReaderRecordId", ReaderRecordId);
                        cmd.Parameters.AddWithValue("@last_update_by", ServerStatic.Utilities.ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@GamePlayData", GamePlayData);
                        cmd.ExecuteNonQuery();
                        log.LogVariableState("@gameplay_id", gamePlayId);
                        log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                        log.LogVariableState("@last_update_by", ServerStatic.Utilities.ParafaitEnv.LoginID);
                        log.LogVariableState("@GamePlayData", GamePlayData);
                    }

                    cmd.Connection.Close();
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(gamePlayId);
                    return gamePlayId;
                }
            }

        }

        public static long CreateCoinPusherGamePlay(Machine CurrentMachine, string CardNumber, ServerStatic ServerStatic, ref string Message)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, ServerStatic, Message);
            if (CardNumber.StartsWith("FFFFFFFF"))
                CardNumber = ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;
            long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId, CardNumber, CurrentMachine.play_credits, 0, 0, 0, 0, 0, 0, 0, "T", 0, "Coin Pusher", -1, -1, DateTime.Now, null, ServerStatic);
            if (gamePlayId > 0)
                Message = "Coin pusher gameplay with " + CurrentMachine.play_credits.ToString("N2") + " credits created for card: " + ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;
            else
                Message = "Unable to create coin pusher gameplay for card: " + ServerStatic.TOKEN_MACHINE_GAMEPLAY_CARD;

            log.LogVariableState("Message", Message);
            log.LogMethodExit(gamePlayId);
            return gamePlayId;
        }

        public static string GetCardType(string CardNumber, ServerStatic ServerStatic, out int CardId, out string CustomerName)
        {
            log.LogMethodEntry(CardNumber, ServerStatic);
            DataTable dt = ServerStatic.Utilities.executeDataTable(@"select card_id, vip_customer, technician_card, 
                                                                	   cu.customer_name + case when (isnull(cu.last_name, '')) = '' then '' else ' ' + cu.last_name end customer
                                                                    from cards c left outer join CustomerView(@PassPhrase) cu
                                                                        on c.customer_id = cu.customer_id
                                                                    where card_number = @card_number 
                                                                    and valid_flag = 'Y'", new SqlParameter("@card_number", CardNumber), new SqlParameter("@PassPhrase", Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(ServerStatic.Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
            log.LogVariableState("@card_number", CardNumber);
            if (dt.Rows.Count > 0)
            {
                CardId = Convert.ToInt32(dt.Rows[0]["card_id"]);
                CustomerName = dt.Rows[0]["customer"].ToString();
                string returnvalue = (dt.Rows[0]["technician_card"].ToString() == "Y" ? "TECH" : "NORMAL");
                log.LogMethodExit(returnvalue);
                return (returnvalue);
            }
            else
            {
                CardId = -1;
                CustomerName = "";
                log.LogMethodExit("NOTFOUND");
                return "NOTFOUND";
            }

        }

        /// <summary>
        /// Get Parent Card number for given card number.
        /// If no parent found, return same card number
        /// </summary>
        /// <param name="CardNumber">Child card number</param>
        /// <param name="ServerStatic"></param>
        /// <returns>Parent card number if exists else child card number</returns>
        public static ParentChildCardsDTO getParentCard(string CardNumber, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(CardNumber, ServerStatic);
            //object o = ServerStatic.Utilities.executeScalar(@"select cp.Card_number 
            //                                                    from cards cc, cards cp, parentChildCards pc 
            //                                                    where pc.ChildCardId = cc.card_Id
            //                                                    and cc.card_number = @cardNumber
            //                                                    and cc.valid_flag = 'Y'
            //                                                    and cp.valid_flag = 'Y'
            //                                                    and pc.parentCardId = cp.card_id
            //                                                    and pc.ActiveFlag = 1",
            //                                                new SqlParameter("@cardNumber", CardNumber));
            //log.LogVariableState("@cardNumber", CardNumber);
            Customer.Accounts.AccountDTO childCardAccountDTO = new Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, CardNumber, false, false).AccountDTO;
            if (childCardAccountDTO == null || (childCardAccountDTO != null && childCardAccountDTO.AccountId <= -1))
            {
                log.LogMethodExit(CardNumber);
                return (new ParentChildCardsDTO());
            }
            ParentChildCardsListBL gpParentCardsListBL = new ParentChildCardsListBL(ServerStatic.Utilities.ExecutionContext);
            List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.CHILD_CARD_ID, childCardAccountDTO.AccountId.ToString()));
            searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            List<ParentChildCardsDTO> gpParentCardsListDTO = gpParentCardsListBL.GetParentChildCardsDTOList(searchParameters);
            if (gpParentCardsListDTO == null || (gpParentCardsListDTO != null && gpParentCardsListDTO.Count == 0))
            {
                log.LogMethodExit(CardNumber);
                return (new ParentChildCardsDTO());
            }
            else
            {
                Customer.Accounts.AccountDTO parentCardAccountDTO = new Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, gpParentCardsListDTO[0].ParentCardId, false, false).AccountDTO;
                if (parentCardAccountDTO == null || (parentCardAccountDTO != null && parentCardAccountDTO.AccountId <= -1))
                {
                    log.LogMethodExit(CardNumber);
                    return (new ParentChildCardsDTO());
                }
                log.LogMethodExit(gpParentCardsListDTO);
                return gpParentCardsListDTO[0];
            }
        }

        /// <summary>
        /// Process Entitlement for child card from parent card if applicable
        /// </summary>
        /// <param name="parentChildCardsDTO">parentChildCard DTO of child card tapped on reader</param>
        /// <param name="gamePlayStats">gameplayStats of parent card if applicable</param>
        /// <returns>card number of child or parent</returns>
        public static string ProcessEntitlementForChildCard(ParentChildCardsDTO parentChildCardsDTO, ServerStatic ServerStatic, ServerStatic.GlobalGamePlayStats gamePlayStats)
        {
            Customer.Accounts.AccountDTO parentCardAccountDTO = new Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardsDTO.ParentCardId, false, false).AccountDTO;
            Semnox.Parafait.Customer.Accounts.AccountDTO childCardAccountDTO = new Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardsDTO.ChildCardId, false, false).AccountDTO;
            if (gamePlayStats.GamePlayType == ServerStatic.VALIDCARDGAME
                || gamePlayStats.GamePlayType == ServerStatic.TECHPLAY)
            {
                log.LogVariableState("Daily Limit not specified for card. Return Parent card: ", parentCardAccountDTO);
                return parentCardAccountDTO.TagNumber;
            }
            getParentChildCardEntitlementBalance(parentChildCardsDTO, ServerStatic, gamePlayStats.CreditsRequired);
            return childCardAccountDTO.TagNumber;
        }

        /// <summary>
        /// Transfer entitlement from parent to child card
        /// </summary>
        /// <param name="parentChildCardDTO">parent child card DTO</param>
        /// <param name="ServerStatic">Game Server Static</param>
        /// <param name="valueRequiredInChildCard">Value required in case daily percentage not defined</param>
        /// <returns>true if successful</returns>
        private static bool getParentChildCardEntitlementBalance(ParentChildCardsDTO parentChildCardDTO, ServerStatic ServerStatic, decimal valueRequiredInChildCard)
        {
            log.LogMethodEntry(parentChildCardDTO, ServerStatic, valueRequiredInChildCard);
            Semnox.Parafait.Customer.Accounts.AccountDTO parentAccountDTO = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true).AccountDTO;
            decimal parentCardBalance =
                Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance == null ? 0 : parentAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance);
            log.LogVariableState("Parent card balance: ", parentCardBalance);
            decimal parentBonusBalance =
                Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.TotalBonusBalance == null ? 0 : parentAccountDTO.AccountSummaryDTO.TotalBonusBalance);
            log.LogVariableState("Parent Bonus balance: ", parentBonusBalance);
            Semnox.Parafait.Customer.Accounts.AccountDTO childAccountDTO = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardDTO.ChildCardId, true, true).AccountDTO;
            if (parentChildCardDTO.DailyLimitPercentage == null || parentChildCardDTO.DailyLimitPercentage == 0)
            {
                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                decimal childCardBalance =
                    Convert.ToDecimal(childAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance == null ? 0 : childAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance);
                childCardBalance +=
                    Convert.ToDecimal(childAccountDTO.AccountSummaryDTO.TotalBonusBalance == null ? 0 : childAccountDTO.AccountSummaryDTO.TotalBonusBalance);
                log.LogVariableState("Child Card Balance: ", childCardBalance);
                if (parentBonusBalance >= Convert.ToDecimal(valueRequiredInChildCard)
                    && childCardBalance < Convert.ToDecimal(valueRequiredInChildCard))
                {
                    entitlements.Add(Semnox.Parafait.Customer.Accounts.CreditPlusTypeConverter.ToString(Semnox.Parafait.Customer.Accounts.CreditPlusType.GAME_PLAY_BONUS), Convert.ToDecimal(valueRequiredInChildCard));
                }
                else if (parentCardBalance >= Convert.ToDecimal(valueRequiredInChildCard)
                    && childCardBalance < Convert.ToDecimal(valueRequiredInChildCard))
                {
                    entitlements.Add(Semnox.Parafait.Customer.Accounts.CreditPlusTypeConverter.ToString(Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE), Convert.ToDecimal(valueRequiredInChildCard));
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
                if (entitlements.Count > 0)
                {
                    Semnox.Parafait.Customer.Accounts.AccountBL parentAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true);
                    Semnox.Parafait.Customer.Accounts.AccountBL childAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, childAccountDTO.AccountId, true, true);
                    TaskProcs taskProcs = new TaskProcs(ServerStatic.Utilities);
                    string message = string.Empty;
                    if (taskProcs.TranferEntitlementBalance(parentAccountBL, childAccountBL, entitlements, "Parent to child card transfer", ref message, -1, -1))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.Error("failure in transferring entitlements from Parent to Child card: " + entitlements + message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            else //Daily Limit Percentage is available
            {
                DateTime serverTime = ServerStatic.Utilities.getServerTime();
                int businessStartTime;
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(ServerStatic.Utilities.ExecutionContext,
                                "BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
                {
                    businessStartTime = 6;
                }
                if (serverTime.Hour < businessStartTime)
                {
                    serverTime = serverTime.AddDays(-1).Date;
                }
                else
                {
                    serverTime = serverTime.Date;
                }
                //Check if Child Account was loaded with balance for the day
                decimal valueToTransfer = 0;
                valueToTransfer = parentCardBalance * Convert.ToInt32(parentChildCardDTO.DailyLimitPercentage) / 100;
                log.LogVariableState("Value to be transferred from parent to child based on Daily Limit Perc: ", valueToTransfer);
                if (childAccountDTO.AccountCreditPlusDTOList != null
                    && childAccountDTO.AccountCreditPlusDTOList.Exists(x => x.CreationDate != null
                                                                       && x.CreationDate > serverTime.AddHours(6)
                                                                       && x.CreationDate < serverTime.AddDays(1).AddHours(6)
                                                                       && x.SourceCreditPlusId > -1
                                                                       && (x.CreditPlusType == Semnox.Parafait.Customer.Accounts.CreditPlusType.GAME_PLAY_BONUS
                                                                          || x.CreditPlusType == Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE)
                                                                       && x.IsActive)
                   )
                {
                    decimal childAccountBalance = Convert.ToDecimal(childAccountDTO.AccountCreditPlusDTOList.Where(x => x.CreationDate != null
                                                                       && x.CreationDate > serverTime.AddHours(6)
                                                                       && x.CreationDate < serverTime.AddDays(1).AddHours(6)
                                                                       && x.SourceCreditPlusId > -1
                                                                       && (x.CreditPlusType == Semnox.Parafait.Customer.Accounts.CreditPlusType.GAME_PLAY_BONUS
                                                                          || x.CreditPlusType == Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE)
                                                                       && x.IsActive).Sum(x => x.CreditPlus));
                    log.LogVariableState("Total CreditPlus loaded to Child Account for the business day: ", childAccountBalance);
                    if (childAccountBalance > 0 && valueToTransfer > childAccountBalance)
                    {
                        valueToTransfer = valueToTransfer - childAccountBalance;
                    }
                    else
                    {
                        log.LogVariableState("Value to transfer for the day is already loaded to child card", valueToTransfer);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                valueToTransfer = Decimal.Round(valueToTransfer, ServerStatic.Utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                log.LogVariableState("Required balance to transfer from parent to child card: ", valueToTransfer);
                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                entitlements.Add(Semnox.Parafait.Customer.Accounts.CreditPlusTypeConverter.ToString(Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE), valueToTransfer);
                Semnox.Parafait.Customer.Accounts.AccountBL parentAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true);
                Semnox.Parafait.Customer.Accounts.AccountBL childAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(ServerStatic.Utilities.ExecutionContext, childAccountDTO.AccountId, true, true);
                TaskProcs taskProcs = new TaskProcs(ServerStatic.Utilities);
                string message = string.Empty;
                if (taskProcs.TranferEntitlementBalance(parentAccountBL, childAccountBL, entitlements, "Parent to child card transfer", ref message, -1, -1))
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.Error("failure in transferring entitlements from Parent to Child card: " + entitlements + message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public static void UpdateGameplayStatus(Machine Machine, bool isSuccess, ServerStatic ServerStatic)
        {
            log.LogMethodEntry(Machine, isSuccess, ServerStatic);
            //SqlCommand cmd = ServerStatic.Utilities.getCommand();

            // fetch latest game play record on this machine, on the same business day
            ServerStatic.Utilities.executeNonQuery(@"insert into gameplayinfo (gameplay_id, GameEndTime, Status, last_update_by, last_update_date)
                                                    select g.gameplay_id, getdate(), 
                                                        @status, 
                                                        @login, getdate()
                                                    from gameplay g
                                                    where gameplay_id = (select max(gameplay_id)
                                                                            from gameplay gg
                                                                            where gg.machine_id = @machine_id
                                                                            and gg.play_date > DATEADD(HH, 6, 
                                                                                                       DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 
                                                                                                                                         then getdate() 
                                                                                                                                         else getdate() - 1 
                                                                                                                                     end)), 0)))",
                                                    new SqlParameter("@machine_id", Machine.MachineId),
                                                    new SqlParameter("@login", ServerStatic.Utilities.ParafaitEnv.LoginID),
                                                    new SqlParameter("@status", (isSuccess ? "SUCCESS" : "FAILED")));

            //cmd.Parameters.AddWithValue("@machine_id", Machine.MachineId);
            //cmd.Parameters.AddWithValue("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            //cmd.Parameters.AddWithValue("@status", (isSuccess ? "SUCCESS" : "FAILED"));
            //cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", Machine.MachineId);
            log.LogVariableState("@login", ServerStatic.Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@status", (isSuccess ? "SUCCESS" : "FAILED"));
            log.LogMethodExit(null);
        }

        public static bool ValidateFingerPrint(string CardNumber,
                                               byte[] Template,
                                               Machine CurrentMachine,
                                               ServerStatic ServerStatic,
                                               Semnox.Parafait.Device.Biometric.FingerPrintReader fpReader)
        {
            log.LogMethodEntry(CardNumber, Template, CurrentMachine, ServerStatic, fpReader);
            DataTable dtFP = ServerStatic.Utilities.executeDataTable(@"select cfp.Template, cfp.FPSalt
                                                                from cards cc, CustomerFingerPrint cfp
                                                                where cfp.CardId = cc.card_Id
                                                                and cc.card_number = @cardNumber
                                                                and cc.valid_flag = 'Y'
                                                                and cfp.ActiveFlag = 1",
                                                            new SqlParameter("@cardNumber", CardNumber));
            log.LogVariableState("@cardNumber", CardNumber);
            if (dtFP.Rows.Count == 0)
            {
                string FPSalt = ServerStatic.Utilities.GenerateRandomCardNumber(10);

                byte[] encryptedTemplate = /*Semnox.Parafait.EncryptionUtils.EncryptionAES.*/Encryption.Encrypt(Template, Encryption.getKey(FPSalt));
                ServerStatic.Utilities.executeNonQuery(@"insert into CustomerFingerPrint
                                                        (CardId, Template, ActiveFlag, Source, LastUpdatedDate, LastUpdatedBy, FPSalt)
                                                        select card_id, @template, 1, @source, getdate(), @user, @FPSalt
                                                        from cards
                                                        where card_number = @cardNumber
                                                        and valid_flag = 'Y'",
                                                     new SqlParameter("@cardNumber", CardNumber),
                                                     new SqlParameter("@template", encryptedTemplate),
                                                     new SqlParameter("@source", CurrentMachine.machine_name),
                                                     new SqlParameter("@user", "Server"),
                                                     new SqlParameter("@FPSalt", FPSalt));
                log.LogVariableState("@cardNumber", CardNumber);
                log.LogVariableState("@template", encryptedTemplate);
                log.LogVariableState("@source", CurrentMachine.machine_name);
                log.LogVariableState("@user", "Server");
                log.LogVariableState("@FPSalt", FPSalt);

                log.LogMethodExit(true);
                return true;
            }
            else
            {
                byte[] baseTemplate = /*Semnox.Parafait.EncryptionUtils.EncryptionAES.*/Encryption.Decrypt((byte[])dtFP.Rows[0]["Template"], Encryption.getKey(dtFP.Rows[0]["FPSalt"].ToString()));
                UserFingerPrintDetailDTO userFingerPrintDetailDTO = new UserFingerPrintDetailDTO(-1, 1, baseTemplate);

                List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList = new List<UserFingerPrintDetailDTO>();
                userFingerPrintDetailDTOList.Add(userFingerPrintDetailDTO);

                bool returnvalue = fpReader.Verify(userFingerPrintDetailDTOList, Template);
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }
        }

        public static long CreateFreeModeGamePlay(Machine CurrentMachine,
                                               string CardNumber,
                                               string TicketMode,
                                               string Tickets,
                                               string Counter,
                                               ServerStatic ServerStatic,
                                               ref string Message)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, TicketMode, Tickets, Counter, ServerStatic, Message);
            if (System.Text.RegularExpressions.Regex.Matches(CardNumber, "0").Count >= 8)
            {
                log.LogVariableState("Message", Message);
                log.LogMethodExit(-1);
                return -1;
            }


            ServerStatic.GlobalGamePlayStats GamePlayStats = new ServerStatic.GlobalGamePlayStats();

            using (SqlConnection cnn = ServerStatic.Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandText = @"select top 1 card_id
                                        from cards 
                                        where card_number = @cardNumber 
                                        order by card_id desc";
                    cmd.Parameters.Add(new SqlParameter("@cardNumber", CardNumber));
                    log.LogVariableState("@cardNumber", CardNumber);
                    SqlDataAdapter daCard = new SqlDataAdapter(cmd);
                    DataTable dtCard = new DataTable();
                    daCard.Fill(dtCard);

                    string GamePlayData = string.Join("-", new string[] { CardNumber, TicketMode, Tickets, Counter });

                    object CardId;
                    SqlTransaction SQLTrx = cnn.BeginTransaction();

                    int ReaderRecordId = 0;
                    Int32.TryParse(Counter, out ReaderRecordId);

                    cmd.Transaction = SQLTrx;
                    if (dtCard.Rows.Count == 0) // card not found
                    {
                        Card card = new Card(CardNumber, ServerStatic.Utilities.ParafaitEnv.LoginID, ServerStatic.Utilities);
                        card.createCard(SQLTrx);
                        CardId = card.card_id;
                        cmd.CommandText = "update cards set valid_flag = 'N' where card_id = @cardId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@cardId", CardId);
                        cmd.ExecuteNonQuery();
                        log.LogVariableState("@cardId", CardId);
                    }
                    else
                    {
                        CardId = dtCard.Rows[0]["card_id"];

                        cmd.CommandText = @"select top 1 1
                                            from GamePlayInfo i, GamePlay g
                                            where g.gameplay_id = i.gameplay_id
                                            and g.machine_id = @machineId
                                            and i.ReaderRecordId = @ReaderRecordId
                                            and g.card_id = @cardId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@machineId", CurrentMachine.MachineId));
                        cmd.Parameters.Add(new SqlParameter("@cardId", CardId));
                        cmd.Parameters.Add(new SqlParameter("@ReaderRecordId", ReaderRecordId));
                        log.LogVariableState("@machineId", CurrentMachine.MachineId);
                        log.LogVariableState("@cardId", CardId);
                        log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                        if (cmd.ExecuteScalar() != null)
                        {
                            Message = "Duplicate Gameplay: " + CurrentMachine.machine_name;
                            ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData + "-" + ReaderRecordId.ToString(), Message, "FREE-GAMEPLAY", 3);
                            SQLTrx.Rollback();
                            cmd.Connection.Close();
                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(0);
                            return 0;
                        }
                    }

                    GamePlayStats.GamePlayType = ServerStatic.VALIDCREDIT;
                    GamePlayStats.GameplayMessage = "Free Play";
                    int ticketCount = 0;
                    Int32.TryParse(Tickets, out ticketCount);

                    long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId,
                                                            CardNumber, 0, 0, 0, 0, 0, 0, 0, 0,
                                                            TicketMode, ticketCount,
                                                            GamePlayStats.GameplayMessage, -1, -1,
                                                            GamePlayStats.PlayRequestTime, SQLTrx, ServerStatic);

                    if (gamePlayId < 0)
                    {
                        SQLTrx.Rollback();
                        ServerStatic.Utilities.EventLog.logEvent("ParafaitServer", 'W', GamePlayData, Message, "FREE-GAMEPLAY", 3);
                    }
                    else
                    {
                        cmd.CommandText = @"insert into gameplayinfo (gameplay_id, ReaderRecordId, GamePlayData, last_update_by, last_update_date)
                                        values (@gameplay_id, @ReaderRecordId, @GamePlayData, @last_update_by, getdate())";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                        cmd.Parameters.AddWithValue("@ReaderRecordId", ReaderRecordId);
                        cmd.Parameters.AddWithValue("@last_update_by", ServerStatic.Utilities.ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@GamePlayData", GamePlayData);
                        cmd.ExecuteNonQuery();
                        log.LogVariableState("@gameplay_id", gamePlayId);
                        log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                        log.LogVariableState("@last_update_by", ServerStatic.Utilities.ParafaitEnv.LoginID);
                        log.LogVariableState("@GamePlayData", GamePlayData);
                        SQLTrx.Commit();
                        Message = "Free play created";
                    }

                    cmd.Connection.Close();
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(gamePlayId);
                    return gamePlayId;
                }
            }

        }
    }
}
