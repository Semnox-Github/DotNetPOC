/********************************************************************************************************************
 * Project Name - GameTransactionBL
 * Description  - GameTransactionBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************************************
 *2.150.2     12-Dec-2022      Mathew Ninan   GameTransactionBL as alternative to GamePlay class  
 *2.155.0     13-Jul-2023      Mathew Ninan   Tap Containment enhancement - This should allow time play 
 *                                            even if game play duration is not passed as long as tickets have 
 *                                            dispensed. This is only if ENABLE_OVERRIDE_GAMEPLAY_DURATION is set.
*********************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Game;
using Semnox.Parafait.Device.Biometric;
using Semnox.Parafait.Product;
using System.Threading.Tasks; 
using Semnox.Parafait.logger;
using Semnox.Parafait.Languages;
using EventLog = Semnox.Parafait.logger.EventLog;
//using Semnox.Parafait.Transaction.V2;

namespace Semnox.Parafait.ServerCore
{
    public class GameTransactionBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ExecutionContext GameExecutionContext;
        GameServerEnvironment GameServerEnvironment;
        GameServerEnvironment.GameServerPlayDTO GameServerPlayDTO;

        Machine GameMachine;
        public GameTransactionBL(ExecutionContext executionContext, int machineId)
        {
            log.LogMethodEntry(executionContext, machineId);
            GameExecutionContext = executionContext;
            GameServerEnvironment = new GameServerEnvironment(GameExecutionContext);
            GameMachine = new Machine(machineId, GameServerEnvironment);
            GameMachine.RefreshMachine();
            log.LogMethodExit(GameMachine);
        }

        public GameTransactionBL(ExecutionContext executionContext, int machineId,
                                 GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO)
            : this(executionContext, machineId)
        {
            log.LogMethodEntry(executionContext, machineId, gameServerPlayDTO);
            GameServerPlayDTO = new GameServerEnvironment.GameServerPlayDTO(machineId);
            GameServerPlayDTO = gameServerPlayDTO;
            log.LogMethodExit(GameMachine);
        }

        /// <summary>
        /// Play game wrapper
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gamePlayBuildDTO"></param>
        /// <returns></returns>
        public GamePlayDTO PlayGame(int accountId, GamePlayBuildDTO gamePlayBuildDTO, SqlTransaction inSqlTrx = null)
        {
            log.LogMethodEntry(accountId, gamePlayBuildDTO);
            string message = string.Empty;
            bool proceedWithGamePlay = false;
            if (string.IsNullOrWhiteSpace(gamePlayBuildDTO.GameplayType)
                || gamePlayBuildDTO.GameplayType == "R")
            {
                GameServerPlayDTO = ValidateGamePlay(gamePlayBuildDTO.GamePlayDTO.MachineId, accountId, inSqlTrx);
                if (gamePlayBuildDTO.GameServerPlayDTO != null && gamePlayBuildDTO.GameServerPlayDTO.PlayRequestTime != DateTime.MinValue)
                {
                    GameServerPlayDTO.PlayRequestTime = gamePlayBuildDTO.GameServerPlayDTO.PlayRequestTime;
                }
                if (GameMachine.OutOfService && GameServerPlayDTO.TechCard.Equals('Y') == false)
                {
                    string errorMessage = "Wait:No Service";
                    log.Error(GameMachine.machine_name + " out of service");
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new Exception(errorMessage);
                }
                switch (GameServerPlayDTO.GamePlayType)
                {
                    case GameServerEnvironment.VALIDCREDIT:
                    case GameServerEnvironment.VALIDBONUS:
                    case GameServerEnvironment.VALIDCREDITBONUS:
                    case GameServerEnvironment.VALIDCOURTESY:
                    case GameServerEnvironment.VALIDCREDITCOURTESY:
                    case GameServerEnvironment.TIMEPLAY:
                    case GameServerEnvironment.VALIDCARDGAME:
                        proceedWithGamePlay = true;
                        break;
                    case GameServerEnvironment.LOBALCREDIT:
                        message = "Low Balance, please recharge";
                        break;
                    case GameServerEnvironment.INVCARD:
                        message = "Invalid card, please check";
                        break;
                    case GameServerEnvironment.GAMETIMEMINGAP:
                        int GameTimeMinGap = GameServerEnvironment.MIN_SECONDS_BETWEEN_GAMETIME_PLAY;
                        message = "Wait for " + GameTimeMinGap + " seconds";
                        break;
                    case GameServerEnvironment.TECHPLAY:
                        proceedWithGamePlay = true;
                        message = "Technician Play";
                        break;
                    case GameServerEnvironment.MULTIPLESWIPE:
                        message = "Wait for 3 seconds";
                        break;
                    case GameServerEnvironment.NOTALLOWED:
                        message = "Card not allowed";
                        break;
                    case GameServerEnvironment.RESETTIMER:
                        message = "Sending off command";
                        break;
                    case GameServerEnvironment.NOENTITLEMENT:
                        message = "Card does not have balance to play this game, please check the card or recharge";
                        break;
                    default:
                        message = "Invalid response. Please retry";
                        break;
                }
            }
            else //other game play types to proceed with gameplay
            {
                proceedWithGamePlay = true;
            }
            if (proceedWithGamePlay && gamePlayBuildDTO.CommitGamePlay)
            {
                AccountDTO accountDTO = new AccountBL(GameExecutionContext, accountId, false, false, null).AccountDTO;
                long gamePlayId = -1;
                Utilities utilities = GetUtility(GameServerEnvironment.GameEnvExecutionContext);
                if (string.IsNullOrEmpty(gamePlayBuildDTO.GameplayType))
                {
                    gamePlayId = CreateGamePlay(gamePlayBuildDTO.GamePlayDTO.MachineId, accountDTO.TagNumber, GameServerPlayDTO, GameServerEnvironment, gamePlayBuildDTO.MultiGamePlayReference, gamePlayBuildDTO.GamePriceTierInfo, ref message, null, utilities);
                }
                else
                {
                    Machine machine = new Machine(GameExecutionContext, gamePlayBuildDTO.GamePlayDTO.MachineId);

                    switch (gamePlayBuildDTO.GameplayType)
                    {
                        case "R":
                            gamePlayId = CreateGamePlay(gamePlayBuildDTO.GamePlayDTO.MachineId, accountDTO.TagNumber, GameServerPlayDTO, GameServerEnvironment, gamePlayBuildDTO.MultiGamePlayReference, gamePlayBuildDTO.GamePriceTierInfo, ref message, null, utilities);
                            break;
                        case "M":
                            gamePlayId = CreateMiFareGamePlay(GameMachine, accountDTO.TagNumber, gamePlayBuildDTO.GamePlayDTO.PayoutCost.ToString(),
                                gamePlayBuildDTO.GamePlayDTO.Credits.ToString(), gamePlayBuildDTO.GamePlayDTO.Bonus.ToString(), gamePlayBuildDTO.GamePlayDTO.Courtesy.ToString(),
                                gamePlayBuildDTO.GamePlayDTO.CPCredits.ToString(), gamePlayBuildDTO.GamePlayDTO.ExternalSystemReference, gamePlayBuildDTO.GamePlayDTO.CPCardBalance.ToString(),
                                gamePlayBuildDTO.GameServerPlayDTO.RepeatPlayIndex.ToString(), gamePlayBuildDTO.GameServerPlayDTO.CardTechGames.ToString(), gamePlayBuildDTO.GamePlayDTO.CardGame.ToString(),
                                GameServerEnvironment, ref message, utilities);
                            break;
                        case "C":
                            gamePlayId = CreateCoinPusherGamePlay(GameMachine, accountDTO.TagNumber, GameServerEnvironment, gamePlayBuildDTO.MultiGamePlayReference, gamePlayBuildDTO.GamePriceTierInfo, ref message, utilities);
                            break;
                        case "F":
                            gamePlayId = CreateFreeModeGamePlay(GameMachine, accountDTO == null ? gamePlayBuildDTO.GamePlayDTO.CardNumber : accountDTO.TagNumber, gamePlayBuildDTO.GamePlayDTO.TicketMode, gamePlayBuildDTO.GamePlayDTO.TicketCount.ToString(),
                                gamePlayBuildDTO.GamePlayDTO.MultiGamePlayReference.ToString(), GameServerEnvironment, ref message, utilities);
                            break;
                        default:
                            log.Error("Game Play Type is not Valid");
                            break;
                    }
                }
                //GamePlayBL is using int for GamePlayID, this would create problem in HQ environment. CHECK
                GamePlayDTO gamePlayDTO = null;
                gamePlayDTO = new GamePlayBL(GameExecutionContext, Convert.ToInt32(gamePlayId)).GamePlayDTO;
                log.LogMethodExit(gamePlayDTO);
                return gamePlayDTO;
            }
            else if (proceedWithGamePlay)
            {
                log.LogMethodExit("GamePlay can be performed. CommitGameplay is false");
                return null;
            }
            else
            {
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        internal GameServerEnvironment.GameServerPlayDTO ValidateGamePlay(int machineId, int accountId, SqlTransaction SQLTrx = null)
        {
            AccountDTO accountDTO = new AccountBL(GameExecutionContext, accountId, false, false, SQLTrx).AccountDTO;
            if (accountDTO == null || (accountDTO != null && accountDTO.AccountId <= -1))
            {
                if (accountDTO == null)
                    accountDTO = new AccountDTO();
                accountDTO.TagNumber = "XYZCXYZC";
            }
            GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO = getGamePlayDetails(machineId, accountDTO.TagNumber, GameServerEnvironment, SQLTrx);
            return gameServerPlayDTO;
        }

        internal GameServerEnvironment.GameServerPlayDTO GetGamePlayDetails(int machineId, int accountId, SqlTransaction SQLTrx = null)
        {
            AccountDTO accountDTO = new AccountBL(GameExecutionContext, accountId, false, false, SQLTrx).AccountDTO;
            if (accountDTO == null || (accountDTO != null && accountDTO.AccountId <= -1))
            {
                if (accountDTO == null)
                    accountDTO = new AccountDTO();
                accountDTO.TagNumber = "XYZCXYZC";
            }
            GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO = getGamePlayDetails(machineId, accountDTO.TagNumber, GameServerEnvironment, SQLTrx);
            return gameServerPlayDTO;
        }

        public GameServerEnvironment.GameServerPlayDTO getGamePlayDetails(int MachineId, string card_number, GameServerEnvironment GameServerEnvironment, SqlTransaction trx = null)
        {
            log.LogMethodEntry(MachineId, card_number, GameServerEnvironment, trx);
            log.LogMethodExit("Game Server Play DTO");
            return getGamePlayDetails(MachineId, card_number, GameServerEnvironment, null, -1, -1, trx);
        }

        public GameServerEnvironment.GameServerPlayDTO getGamePlayDetails(int MachineId, string card_number, GameServerEnvironment GameServerEnvironment, Guid? multiGamePlayReference, int multiGamePlayCount, int gamePriceTierId, SqlTransaction trx = null)
        {
            log.LogMethodEntry(MachineId, card_number, GameServerEnvironment);
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
            GameServerEnvironment.GameServerPlayDTO GamePlayStats = new GameServerEnvironment.GameServerPlayDTO(MachineId);
            bool repeat_play = false;

            Utilities Utilities = GetUtility(GameServerEnvironment.GameEnvExecutionContext);
            //MachineContainerDTO machineContainerDTO = MachineContainerList.GetMachineContainerDTO(GameExecutionContext.GetSiteId(), machineId);

            //Build machine object
            Machine GameMachine = new Machine(MachineId, GameServerEnvironment);
            GameMachine.RefreshMachine();

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

                    if (GameServerEnvironment.CHECK_FOR_CARD_EXCEPTIONS == "Y") // check this only if configuration is set to do it
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
                        cmd.Parameters.AddWithValue("@Machine_Id", MachineId);
                        log.LogVariableState("@card_number", card_number);
                        log.LogVariableState("@Machine_Id", MachineId);
                        if (cmd.ExecuteScalar() == null)
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.NOTALLOWED;
                            //message += "; not allowed for this card type";
                            //log.LogVariableState("Message", message);
                            log.LogMethodExit(null);
                            return GamePlayStats;
                        }
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Exception check");
                    } // end exception check
                    else
                    {
                        cmd.Parameters.AddWithValue("@card_number", card_number);
                        cmd.Parameters.AddWithValue("@Machine_Id", MachineId);
                        log.LogVariableState("@card_number", card_number);
                        log.LogVariableState("@Machine_Id", MachineId);
                    }

                    // check for last play time by same card on same machine
                    // it should be older than MIN_SECONDS_BETWEEN_REPEAT_PLAY secs
                    additionalMessages.AppendLine();
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Last game play query check for card: " + card_number + " and machine: " + GameMachine.machine_name);
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
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Last game play query check for card: " + card_number + " and machine: " + GameMachine.machine_name);
                    if (cardId != null)
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Last game play query check for machine: " + GameMachine.machine_name + " in last 10 minutes.");
                        cmd.CommandText = @"select play_date, card_id, getdate() sysdate
                                from gameplay g
                                where g.machine_id = @Machine_Id
                                and g.play_date > dateadd(MI, -10, getdate())
                                order by g.play_date desc";

                        SqlDataAdapter daRepeat = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        daRepeat.Fill(dt);

                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Last game play query check for machine: " + GameMachine.machine_name + " in last 10 minutes.");
                        DateTime lastPlayDate = Convert.ToDateTime(dt.Rows[0][0]);
                        DateTime sysDate = Convert.ToDateTime(dt.Rows[0][2]);

                        if (sysDate < lastPlayDate.AddSeconds(GameMachine.RepeatPlayDelay))
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.MULTIPLESWIPE;
                            //log.LogVariableState("Message", message);
                            log.LogMethodExit(null);
                            return GamePlayStats;
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

                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Main query to get Card, Machine and Gameplay attributes for card: " + card_number + " and machine: " + GameMachine.machine_name);
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
                                   (select max(gameplay_id)  
                                      from gameplay 
                                     where notes='Time Play'
                                       and card_id = c.card_id
                                       and play_date > DATEADD(HH, 6, 
                                                                      DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 
                                                                      then getdate() 
                                                                      else getdate() - 1 end)), 0))) Last_Timeplay_GpId, 
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

                    cmd.Parameters.AddWithValue("@roamingAllowed", Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS);
                    cmd.Parameters.AddWithValue("@siteId", Utilities.ParafaitEnv.SiteId);
                    log.LogVariableState("@roamingAllowed", Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS);
                    log.LogVariableState("@siteId", Utilities.ParafaitEnv.SiteId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(DT);
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Main query to get Card, Machine and Gameplay attributes for card: " + card_number + " and machine: " + GameMachine.machine_name);
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
                        Utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
                        Card card = new Card(GamePlayStats.CardId, "", Utilities);
                        TransactionUtils trxUtils = new TransactionUtils(Utilities);
                        DataTable dtProduct = trxUtils.getProductDetails(GamePlayStats.ProductId, card);
                        string productType = dtProduct.Rows[0]["Product_Type"].ToString();
                        if (productType == "CHECK-IN" || productType == "CHECK-OUT")
                        {
                            if (dtProduct.Rows[0]["CheckInFacilityId"] == DBNull.Value)
                            {
                                GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                GamePlayStats.GameplayMessage = Utilities.MessageUtils.getMessage(225);
                                //log.LogVariableState("Message", message);
                                log.LogMethodExit(null);
                                return GamePlayStats;
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
                                        GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                        GamePlayStats.GameplayMessage = "Check in Pending";
                                        log.LogVariableState("Message", "Check in status is pending . Cannot check in");
                                        log.Error("Check in status is pending . Cannot check in");
                                        log.LogMethodExit();
                                        return GamePlayStats;
                                    }
                                    else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDIN)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                        GamePlayStats.GameplayMessage = "Already Checked";
                                        log.LogVariableState("Message", "Already Checked In. Cannot check in again");
                                        log.Error("Already Checked In. Cannot check in again");
                                        log.LogMethodExit();
                                        return GamePlayStats;
                                    }
                                    else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDOUT)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                        GamePlayStats.GameplayMessage = "Already Checked out";
                                        log.LogVariableState("Message", "Already Checked out");
                                        log.Error("Already Checked out");
                                        log.LogMethodExit();
                                        return GamePlayStats;
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
                                FacilityDTO facilityDTO = new FacilityBL(Utilities.ExecutionContext, Convert.ToInt32(dtProduct.Rows[0]["CheckInFacilityId"])).FacilityDTO;
                                CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext);
                                int totalCapacity = facilityDTO.Capacity == null ? 0 : Convert.ToInt32(facilityDTO.Capacity);
                                int totalCheckedIn = checkInBL.GetTotalCheckedInForFacility(facilityDTO.FacilityId, null);
                                if (totalCapacity > 0 && totalCapacity < (totalCheckedIn + 1))
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                    GamePlayStats.GameplayMessage = Utilities.MessageUtils.getMessage(11);
                                    log.LogVariableState("Message", Utilities.MessageUtils.getMessage(11));
                                    log.LogMethodExit(null);
                                    return GamePlayStats;
                                }
                                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMERTYPE") == "N"
                                    && card.customerDTO == null)
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                    GamePlayStats.GameplayMessage = "Customer not registered";
                                    log.LogVariableState("Message", "Check-in not possible as customer is not registered");
                                    log.LogMethodExit(null);
                                    return GamePlayStats;
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
                                            GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                            GamePlayStats.GameplayMessage = "Check in Pending";
                                            log.LogVariableState("Message", "Check in status is pending . Cannot check in");
                                            log.Error("Check in status is pending . Cannot check in");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return GamePlayStats;
                                        }
                                        else if (checkInDetailDTO.Status == CheckInStatus.CHECKEDOUT)
                                        {
                                            GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                            GamePlayStats.GameplayMessage = "Already Checked out";
                                            log.LogVariableState("Message", "Already Checked out");
                                            log.Error("Already Checked out");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return GamePlayStats;
                                        }
                                        else if (checkInDetailDTO.Status == CheckInStatus.ORDERED)
                                        {
                                            GamePlayStats.GamePlayType = GameServerEnvironment.ERROR;
                                            GamePlayStats.GameplayMessage = "Check in Pending";
                                            log.LogVariableState("Message", "Check in Pending");
                                            log.Error("Already Checked out");
                                            log.LogVariableState("Message", "Not Checked In" + card.CardNumber);
                                            log.LogMethodExit();
                                            return GamePlayStats;
                                        }

                                        CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, filteredCheckInDetailDTOList[0].CheckInId);
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

                if (multiGamePlayCount > 0)
                {
                    GameMachine.GameplayMultiplier = multiGamePlayCount; //Update multiplier property with GamePlay count
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - Update: Multplay Game updated: " + multiGamePlayCount + " and machine: " + GameMachine.machine_name);
                }

                //Game Price Tier Logic to derive the updated price.
                //Change to Machine Container at later stage. Machine Container will have Game Price Tier DTO
                if (gamePriceTierId > -1)
                {
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Game Price Tier Info update: " + gamePriceTierId + " and machine: " + GameMachine.machine_name);
                    GamePriceTierDTO gamePriceTierDTO = new GamePriceTierBL(Utilities.ExecutionContext, gamePriceTierId, trx).GamePriceTierDTO;
                    if (gamePriceTierDTO != null && gamePriceTierDTO.GamePriceTierId > -1)
                    {
                        gamePrice = gamePriceTierDTO.PlayCredits;
                        vipGamePrice = gamePriceTierDTO.VipPlayCredits;
                    }
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Game Price Tier Info update: GamePrice: " + gamePrice + ", Vip Price: " + vipGamePrice + " and machine: " + GameMachine.machine_name);
                }

                GamePlayStats.TechCard = Convert.ToChar(DT.Rows[0]["technician_card"]);

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Creditplus initialized for card: " + card_number + " and machine: " + GameMachine.machine_name);
                CreditPlus creditPlus = new CreditPlus(Utilities);
                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Creditplus initialized for card: " + card_number + " and machine: " + GameMachine.machine_name);
                if (GamePlayStats.TechCard.Equals('N')) // user card
                {
                    decimal CardCreditPlusCardBalance = 0;
                    decimal CardCreditPlusCredits = 0;
                    decimal CardCreditPlusBonus = 0;
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Check Creditplus for gameplay for card: " + card_number + " and machine: " + GameMachine.machine_name);
                    creditPlus.getCreditPlusForGamePlay(GamePlayStats.CardId, GameMachine.MachineId, ref CardCreditPlusCardBalance, ref CardCreditPlusCredits, ref CardCreditPlusBonus, ref creditPlusTicketAllowed, trx);
                    GamePlayStats.CardCreditPlusCardBalance = CardCreditPlusCardBalance;
                    GamePlayStats.CardCreditPlusCredits = CardCreditPlusCredits;
                    GamePlayStats.CardCreditPlusBonus = CardCreditPlusBonus;
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Check Creditplus for gameplay for card: " + card_number + " and machine: " + GameMachine.machine_name);
                    GamePlayStats.CardCreditPlusCardBalance = Math.Max(GamePlayStats.CardCreditPlusCardBalance, 0);
                    GamePlayStats.CardCreditPlusCredits = Math.Max(GamePlayStats.CardCreditPlusCredits, 0);
                    GamePlayStats.CardCreditPlusBonus = Math.Max(GamePlayStats.CardCreditPlusBonus, 0);
                    GamePlayStats.CardCreditPlusCredits = Math.Round(GamePlayStats.CardCredits + GamePlayStats.CardCreditPlusCredits + GamePlayStats.CardCreditPlusCardBalance, 4, MidpointRounding.AwayFromZero);
                    GamePlayStats.CardCreditPlusBonus = Math.Round(GamePlayStats.CardCreditPlusBonus + GamePlayStats.CardBonus, 4, MidpointRounding.AwayFromZero);
                    int cardCreditPlusTimeId = -1;
                    GamePlayStats.CardCreditPlusTime = Math.Round(creditPlus.getCreditPlusTimeForGamePlay(GamePlayStats.CardId, GameMachine.MachineId, ref cardCreditPlusTimeId, ref creditPlusTicketAllowed, ref CreditPlusPlayStartTime, trx), 4, MidpointRounding.AwayFromZero);
                    GamePlayStats.CardCreditPlusTimeId = cardCreditPlusTimeId;
                }

                decimal promotion_credits = 0;
                decimal promotion_vip_credits = 0;
                int promotionDetailId = -1;

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get Promotion Details for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                int PromotionId = -1;
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
                                                ref PromotionId,
                                                ref promotionDetailId,
                                                Utilities);
                GamePlayStats.PromotionId = PromotionId;
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
                //message += "; Post Promo Credits Required: " + GamePlayStats.CreditsRequired.ToString() + Environment.NewLine;

                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get Discounted Credits method call for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                GamePlayStats.CreditsRequired = Discounts.getDiscountedCredits(GamePlayStats.CardId, DT.Rows[0]["game_id"], Convert.ToDecimal(DT.Rows[0]["credits_played"] == DBNull.Value ? 0 : DT.Rows[0]["credits_played"]), GamePlayStats.CreditsRequired, Utilities);
                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Get Discounted Credits method call for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                //message += "; Post Discounts Credits Required: " + GamePlayStats.CreditsRequired.ToString();

                if (repeat_play)
                {
                    GamePlayStats.CreditsRequired = Math.Round(GamePlayStats.CreditsRequired * (1 - Convert.ToDecimal(DT.Rows[0]["repeat_play_discount"]) / 100), 4, MidpointRounding.AwayFromZero);
                    //message += "; Repeat Play Credits Required:" + GamePlayStats.CreditsRequired.ToString();
                }

                // game play multiplier change on 9-jun-2017
                GamePlayStats.CreditsRequired = GamePlayStats.CreditsRequired * GameMachine.GameplayMultiplier;
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
                                //Tap Containment change
                                int gpTicketCount = -1;
                                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_OVERRIDE_GAMEPLAY_DURATION") == "Y")
                                {
                                    if (DT.Rows[0]["Last_Timeplay_GpId"] != DBNull.Value)
                                    {
                                        object ticketCount = Utilities.executeScalar(@"select ticket_count
                                                                                     from gameplay
                                                                                    where gameplay_id = @gameplayId",
                                                                                     new SqlParameter("@gameplayId", DT.Rows[0]["Last_Timeplay_GpId"]));
                                        if (ticketCount != null && ticketCount != DBNull.Value)
                                            gpTicketCount = Convert.ToInt32(ticketCount);
                                    }
                                }
                                DateTime lastPlayedTime = Convert.ToDateTime(DT.Rows[0]["last_played_time"]);
                                if (sysDate <= lastPlayedTime.AddSeconds(GameMachine.ConsecutiveTimePlayDelay)
                                    && gpTicketCount <= 0)
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.GAMETIMEMINGAP;
                                }
                            }

                            if (GamePlayStats.GamePlayType != GameServerEnvironment.GAMETIMEMINGAP)
                            {
                                if (DT.Rows[0]["time_allowed"].ToString() == "N")
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                                }
                                else
                                {
                                    if (promo_time_allowed == "N")
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.TIMEPLAY;

                                        // find the balance time left on card. used to display to user
                                        TimeSpan balanceTime = startTime.AddMinutes((double)GamePlayStats.CardTime) - sysDate;
                                        GamePlayStats.CardTime = balanceTime.Days * 24 * 60 + balanceTime.Hours * 60 + balanceTime.Minutes;
                                    }
                                }
                            }
                        }
                        else
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                        }
                    }
                    else
                    {
                        GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                    }

                    if (GamePlayStats.GamePlayType == GameServerEnvironment.LOBALCREDIT && GamePlayStats.CardCreditPlusTimeId != -1) //credit plus time card
                    {
                        // check last played time, and make sure it is older than min time gap needed

                        if (DT.Rows[0]["last_played_time"] != DBNull.Value)
                        {
                            //Tap Containment change
                            int gpTicketCount = -1;
                            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_OVERRIDE_GAMEPLAY_DURATION") == "Y")
                            {
                                if (DT.Rows[0]["Last_Timeplay_GpId"] != DBNull.Value)
                                {
                                    object ticketCount = Utilities.executeScalar(@"select ticket_count
                                                                                     from gameplay
                                                                                    where gameplay_id = @gameplayId",
                                                                                 new SqlParameter("@gameplayId", DT.Rows[0]["Last_Timeplay_GpId"]));
                                    if (ticketCount != null && ticketCount != DBNull.Value)
                                        gpTicketCount = Convert.ToInt32(ticketCount);
                                }
                            }
                            DateTime lastPlayedTime = Convert.ToDateTime(DT.Rows[0]["last_played_time"]);
                            if (sysDate <= lastPlayedTime.AddSeconds(GameMachine.ConsecutiveTimePlayDelay)
                                && gpTicketCount <= 0)
                            {
                                GamePlayStats.GamePlayType = GameServerEnvironment.GAMETIMEMINGAP;
                            }
                        }

                        if (GamePlayStats.GamePlayType != GameServerEnvironment.GAMETIMEMINGAP)
                        {
                            if (DT.Rows[0]["time_allowed"].ToString() == "N")
                            {
                                GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                            }
                            else
                            {
                                if (promo_time_allowed == "N")
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.TIMEPLAY;
                                    GamePlayStats.CreditsRequired = promotion_credits * GameMachine.GameplayMultiplier; //13-Feb-2017: added to get promotion price for time play
                                    GamePlayStats.CardTime = GamePlayStats.CardCreditPlusTime;
                                }
                            }
                        }
                    }

                    // check for credit play if game time has expired or it was not a gametime card
                    if (GamePlayStats.GamePlayType == GameServerEnvironment.LOBALCREDIT) // check for specific games loaded on card
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        List<int> cardGameIdList = new List<int>();
                        CardGames cardGames = new CardGames(Utilities);
                        GamePlayStats.CardGames = cardGames.getCardGames(GamePlayStats.CardId, Convert.ToInt32(DT.Rows[0]["game_id"]), Convert.ToInt32(DT.Rows[0]["game_profile_id"]), ref cardGameTicketAllowed, ref cardGameIdList, trx);
                        GamePlayStats.CardGameIdList = cardGameIdList;
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));

                        if (GamePlayStats.CardGames >= GameMachine.GameplayMultiplier)
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCARDGAME;
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
                                GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCOURTESY; // courtesy available. play.
                            }
                            else
                                if (GamePlayStats.CardCourtesy > 0) // if courtesy is there try to use it with combination of credits and bonus
                            {
                                if (GamePlayStats.CreditsRequired <= GamePlayStats.CardCourtesy + GamePlayStats.CardCreditPlusCredits + GamePlayStats.CardCreditPlusBonus)
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDITCOURTESY; // courtesy+credits+bonus available. play.
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
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
                                GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                            }
                            else // required credit available
                            {
                                if (GameServerEnvironment.CONSUME_CREDITS_BEFORE_BONUS == "Y")
                                {
                                    if (GamePlayStats.CardCreditPlusCredits >= GamePlayStats.CreditsRequired)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDIT;
                                    }
                                    else
                                        if (GamePlayStats.CardCreditPlusCredits > 0)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDITBONUS;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDBONUS;
                                    }
                                }
                                else
                                {
                                    if (GamePlayStats.CardCreditPlusBonus >= GamePlayStats.CreditsRequired)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDBONUS;
                                    }
                                    else
                                        if (GamePlayStats.CardCreditPlusBonus > 0)
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDITBONUS;
                                    }
                                    else
                                    {
                                        GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDIT;
                                    }
                                }
                            }
                        }
                    }// end regular credit play
                }
                else // technician card
                {
                    if (DT.Rows[0]["timer_reset_card"].ToString() == "Y")
                        GamePlayStats.GamePlayType = GameServerEnvironment.RESETTIMER;
                    else
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Technician Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        List<int> CardGameIdList = new List<int>();
                        CardGames cardGames = new CardGames(Utilities);
                        GamePlayStats.CardGames = GamePlayStats.CardTechGames + cardGames.getCardGames(GamePlayStats.CardId, Convert.ToInt32(DT.Rows[0]["game_id"]), Convert.ToInt32(DT.Rows[0]["game_profile_id"]), ref cardGameTicketAllowed, ref CardGameIdList, trx);
                        GamePlayStats.CardGameIdList = CardGameIdList;
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Technician Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        if (GamePlayStats.CardGames > 0)
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.TECHPLAY;
                        }
                        else
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.LOBALCREDIT;
                            GamePlayStats.CardCreditPlusCredits = 0;
                            GamePlayStats.CardCreditPlusBonus = 0;
                        }
                    }
                }

                if (GamePlayStats.GamePlayType == GameServerEnvironment.LOBALCREDIT
                    && GameServerEnvironment.READER_HARDWARE_VERSION >= 1.5)
                {
                    int CardCreditPlusTimeId = -1;
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Check Creditplus Time for gameplay for card: " + card_number + " and machine: " + GameMachine.machine_name);
                    creditPlus.getCreditPlusTimeForGamePlay(GamePlayStats.CardId, -1, ref CardCreditPlusTimeId, ref creditPlusTicketAllowed, ref CreditPlusPlayStartTime, trx);
                    additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Check Creditplus Time for gameplay for card: " + card_number + " and machine: " + GameMachine.machine_name);
                    if (CardCreditPlusTimeId > 0)
                    {
                        GamePlayStats.GamePlayType = GameServerEnvironment.NOENTITLEMENT;
                    }
                    else
                    {
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        CardGames cardGames = new CardGames(Utilities);
                        int cardGameId = 0;
                        int count = cardGames.getCardGames(GamePlayStats.CardId, -1, -1, ref cardGameTicketAllowed, ref cardGameId, trx);
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Card Games check for card: " + card_number + " and Game: " + Convert.ToInt32(DT.Rows[0]["game_id"]));
                        if (count > 0)
                        {
                            GamePlayStats.GamePlayType = GameServerEnvironment.NOENTITLEMENT;
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
                                    GamePlayStats.GamePlayType = GameServerEnvironment.GAMETIMEPACKAGEEXPIRED;
                                }
                            }
                        }
                    }
                }

                if (!setConsumedValues(GameServerEnvironment, GamePlayStats))
                {
                    //log.LogVariableState("Message", message);
                    log.LogMethodExit(GamePlayStats);
                    return GamePlayStats;
                }
            }
            else // card is invalid
            {
                GamePlayStats.GamePlayType = GameServerEnvironment.INVCARD;
                //message += "; Invalid card or machine address";
                //log.LogVariableState("Message", message);
                log.LogMethodExit(GamePlayStats);
                return GamePlayStats;
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
                    case GameServerEnvironment.VALIDCOURTESY:
                    case GameServerEnvironment.VALIDCREDITCOURTESY:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_courtesy"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            break;
                        }
                    case GameServerEnvironment.TIMEPLAY:
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
                    case GameServerEnvironment.VALIDCREDITBONUS:
                    case GameServerEnvironment.VALIDCREDIT:
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
                    case GameServerEnvironment.VALIDCARDGAME:
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
                    case GameServerEnvironment.VALIDBONUS:
                        {
                            if (DT.Rows[0]["ticket_allowed_on_bonus"].ToString() != "Y")
                            {
                                //GamePlayStats.TicketMode = "N";
                                GamePlayStats.TicketEligibility = false;
                            }
                            break;
                        }
                    case GameServerEnvironment.TECHPLAY:
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

            //message += additionalMessages.ToString();

            GamePlayStats.ValidGamePlay = true;
            GamePlayStats.AdditionalMessage = additionalMessages.ToString();
            log.LogVariableState("Message", additionalMessages.ToString());
            log.LogMethodExit(GamePlayStats);
            return GamePlayStats;
        }

        internal bool setConsumedValues(GameServerEnvironment GameServerEnvironment, GameServerEnvironment.GameServerPlayDTO GamePlayStats)
        {
            log.LogMethodEntry(GameServerEnvironment, GamePlayStats);
            Machine GameMachine = new Machine(GamePlayStats.CurrentMachineId, GameServerEnvironment);
            GameMachine.RefreshMachine();
            switch (GamePlayStats.GamePlayType)
            {
                case GameServerEnvironment.VALIDCOURTESY:
                    {
                        GamePlayStats.ConsumedValues.CardCourtesy = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Courtesy Play";
                        break;
                    }
                case GameServerEnvironment.VALIDCARDGAME:
                    {
                        if (!GameServerEnvironment.ZERO_PRICE_CARDGAME_PLAY)
                            GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Card Game Play";
                        break;
                    }
                case GameServerEnvironment.VALIDCREDIT:
                    {
                        GamePlayStats.ConsumedValues.CardCredits = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Credit Play";
                        break;
                    }
                case GameServerEnvironment.VALIDCREDITBONUS:
                    {
                        if (GameServerEnvironment.CONSUME_CREDITS_BEFORE_BONUS == "Y")
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
                case GameServerEnvironment.VALIDCREDITCOURTESY:
                    {
                        GamePlayStats.ConsumedValues.CardCourtesy = GamePlayStats.CardCourtesy;

                        if (GameServerEnvironment.CONSUME_CREDITS_BEFORE_BONUS == "Y")
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
                case GameServerEnvironment.VALIDBONUS:
                    {
                        GamePlayStats.ConsumedValues.CardBonus = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Bonus Play";
                        break;
                    }
                case GameServerEnvironment.TIMEPLAY:
                    {
                        if (!GameServerEnvironment.ZERO_PRICE_GAMETIME_PLAY)
                            GamePlayStats.ConsumedValues.CardTime = GamePlayStats.CreditsRequired;
                        GamePlayStats.GameplayMessage = "Time Play";
                        break;
                    }
                case GameServerEnvironment.TECHPLAY:
                    {
                        if (!GameServerEnvironment.ZERO_PRICE_CARDGAME_PLAY)
                            GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;
                        GamePlayStats.ConsumedValues.TechGame = (GameMachine == null ? 1 : GameMachine.GameplayMultiplier);
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

            if (GamePlayStats.GamePlayType != GameServerEnvironment.VALIDCARDGAME // game play using regular balance, not loaded games
                && GamePlayStats.GamePlayType != GameServerEnvironment.TECHPLAY)
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

        public static long CreateGamePlay(int MachineId, string card_number, GameServerEnvironment.GameServerPlayDTO GamePlayStats, GameServerEnvironment GameServerEnvironment, ref string message, SqlTransaction inSQLTrx = null, Utilities Utilities = null)
        {
            log.LogMethodEntry(MachineId, card_number, GamePlayStats, GameServerEnvironment, message, inSQLTrx, Utilities);
            return CreateGamePlay(MachineId, card_number, GamePlayStats, GameServerEnvironment, null, string.Empty, ref message, inSQLTrx, Utilities);
        }

        public static long CreateGamePlay(int MachineId, string card_number, GameServerEnvironment.GameServerPlayDTO GamePlayStats, GameServerEnvironment GameServerEnvironment, Guid? MultiGamePlayReference, string GamePriceTierInfo, ref string message, SqlTransaction inSQLTrx = null, Utilities Utilities = null)
        {
            log.LogMethodEntry(MachineId, card_number, GamePlayStats, GameServerEnvironment, message);
            StringBuilder additionalMessages = new StringBuilder();
            DateTime loggingStartTime = DateTime.Now;
            Machine GameMachine = new Machine(MachineId, GameServerEnvironment);
            GameMachine.RefreshMachine();
            //SqlCommand cmd = new SqlCommand();
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

                    if (GamePlayStats.GamePlayType != GameServerEnvironment.VALIDCARDGAME) // game play using regular balance, not loaded games
                    {
                        CreditPlus creditPlus = new CreditPlus(Utilities);
                        if (GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0)
                        {
                            try
                            {
                                double cardCreditPlusCardBalance = 0;
                                double CardCreditPlusGameplayCredits = 0;
                                additionalMessages.AppendLine();
                                additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Deduct Creditplus Credits for gameplay for card: " + card_number + " and machine: " + MachineId);
                                creditPlus.deductCreditPlusOnGamePlay(card_number, MachineId, (double)GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits,
                                                                      0,
                                                                      ref cardCreditPlusCardBalance,
                                                                      ref CardCreditPlusGameplayCredits, SQLTrx);
                                GamePlayStats.ConsumedValues.CardCreditPlusCardBalance = cardCreditPlusCardBalance;
                                GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits = CardCreditPlusGameplayCredits;
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
                            cmd.CommandText = "update CardCreditPlus set PlayStartTime = getdate(), LastupdatedDate = getdate() where CardCreditPlusId = @id and PlayStartTime is null";
                            cmd.Parameters.AddWithValue("@id", GamePlayStats.CardCreditPlusTimeId);
                            cmd.ExecuteNonQuery();

                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Start Time for CreditPlus record for card: " + card_number + " and machine: " + MachineId);
                            log.LogVariableState("@id", GamePlayStats.CardCreditPlusTimeId);
                        }

                        int cardId = -1;
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Get card id for card: " + card_number);
                        object objCardId = Utilities.executeScalar(@"SELECT card_id from cards
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
                            int multiplier = (GameMachine == null ? 1 : GameMachine.GameplayMultiplier);
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
                        if (GameMachine != null)
                            x = GameMachine.GameplayMultiplier;

                        for (int i = 0; i < x; i++)
                        {
                            int cardGameId = -1;
                            if (i < GamePlayStats.CardGameIdList.Count)
                                cardGameId = GamePlayStats.CardGameIdList[i];

                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Create Game Play record for card: " + card_number + " and machine: " + MachineId);
                            gamePlayId = CreateGamePlayRecord(MachineId, card_number, GamePlayStats.ConsumedValues.CardCredits / x, GamePlayStats.ConsumedValues.CardCourtesy / x,
                                                                GamePlayStats.ConsumedValues.CardBonus / x, GamePlayStats.ConsumedValues.CardTime / x, GamePlayStats.ConsumedValues.CardGame / x,
                                                                (decimal)GamePlayStats.ConsumedValues.CardCreditPlusCardBalance / x, (decimal)GamePlayStats.ConsumedValues.CardCreditPlusGameplayCredits / x,
                                                                GamePlayStats.ConsumedValues.CardCreditPlusBonus / x, (GamePlayStats.TicketEligibility ? GamePlayStats.TicketMode : "N"), 0, GamePlayStats.GameplayMessage, cardGameId, GamePlayStats.PromotionId, GamePlayStats.PlayRequestTime, SQLTrx, GameServerEnvironment, Utilities, MultiGamePlayReference, GamePriceTierInfo);
                            additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - END: Create Game Play record for card: " + card_number + " and machine: " + MachineId);
                        }

                        message += "; Game Play record created";
                        additionalMessages.AppendLine();
                        additionalMessages.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - BEGIN: Create Game play Transaction for card: " + card_number + " and machine: " + MachineId);
                        CreateGamePlayTransaction(gamePlayId, GameServerEnvironment, GamePlayStats, SQLTrx, Utilities);
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
                        if (GameServerEnvironment.ALLOW_LOYALTY_ON_GAMEPLAY == "Y")
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

        static void CreateGamePlayTransaction(long gamePlayId, GameServerEnvironment GameServerEnvironment, GameServerEnvironment.GameServerPlayDTO gameplayStats, SqlTransaction SQLTrx, Utilities Utilities)
        {
            log.LogMethodEntry(gamePlayId, GameServerEnvironment, gameplayStats, SQLTrx, Utilities);
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
            Utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            Utilities.ParafaitEnv.User_Id = Utilities.ParafaitEnv.ExternalPOSUserId;
            Semnox.Parafait.Transaction.Transaction Trx = new Semnox.Parafait.Transaction.Transaction(Utilities);
            string message = "";

            Card card = new Card(gameplayStats.CardId, "", Utilities);
            TransactionUtils trxUtils = new TransactionUtils(Utilities);
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
                            //log.LogMethodExit(null, "Throwing Application Exception- " + Utilities.MessageUtils.getMessage(225));
                            //throw new ApplicationException(Utilities.MessageUtils.getMessage(225));
                            string errorMessage = MessageContainerList.GetMessage(GameServerEnvironment.GameEnvExecutionContext, 225);
                            log.LogMethodExit(null, "Throwing Application Exception- " + errorMessage);
                            throw new ApplicationException(errorMessage);
                        }
                        CustomerDTO customerDTO;
                        if (card.customerDTO == null)
                        {
                            customerDTO = card.customerDTO = new CustomerDTO();
                            if (ParafaitDefaultContainerList.GetParafaitDefault(GameServerEnvironment.GameEnvExecutionContext, "CUSTOMERTYPE") != "N")
                            {
                                customerDTO.CustomerType = CustomerType.UNREGISTERED;
                            }
                            else
                            {
                                //log.LogMethodExit(null, "Throwing Application Exception- " + GameServerEnvironment.Utilities.MessageUtils.getMessage(471));
                                //throw new ApplicationException(GameServerEnvironment.Utilities.MessageUtils.getMessage(471));
                                string errorMessage = MessageContainerList.GetMessage(GameServerEnvironment.GameEnvExecutionContext, 471);
                                log.LogMethodExit(null, "Throwing Application Exception- " + errorMessage);
                                throw new ApplicationException(errorMessage);
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
                        CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(GameServerEnvironment.GameEnvExecutionContext);
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
                                        ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(GameServerEnvironment.GameEnvExecutionContext);
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInDetailDTO.CheckInId, new List<CheckInDetailDTO> { checkInDetailDTO }, SQLTrx);
                                        t.Wait();
                                    }
                                    log.Debug("UpdateCheckInStatus to Checked In ");
                                    List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> tagIssuedSearchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, GameServerEnvironment.GameEnvExecutionContext.GetSiteId().ToString()));
                                    tagIssuedSearchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, card.card_id.ToString()));
                                    NotificationTagIssuedListBL notificationTagIssuedListBL = new NotificationTagIssuedListBL(GameServerEnvironment.GameEnvExecutionContext);
                                    List<NotificationTagIssuedDTO> notificationTagIssuedListDTO = notificationTagIssuedListBL.GetAllNotificationTagIssuedDTOList(tagIssuedSearchParameters);
                                    log.LogVariableState("notificationTagIssuedListDTO", notificationTagIssuedListDTO);
                                    NotificationTagIssuedDTO notificationTagIssuedDTO = null;
                                    if (notificationTagIssuedListDTO != null && notificationTagIssuedListDTO.Any()
                                                        && notificationTagIssuedListDTO.Exists(tag => tag.StartDate == DateTime.MinValue))
                                    {
                                        notificationTagIssuedListDTO = notificationTagIssuedListDTO.OrderByDescending(x => x.IssueDate).ToList(); // get latest record 
                                        notificationTagIssuedDTO = notificationTagIssuedListDTO.Where(tag => tag.StartDate == DateTime.MinValue
                                                                                           && (tag.ExpiryDate == DateTime.MinValue || tag.ExpiryDate > Utilities.getServerTime())).FirstOrDefault();//check
                                        log.LogVariableState("notificationTagIssuedDTO", notificationTagIssuedDTO);
                                        NotificationTagIssuedBL notificationTagIssuedBL = new NotificationTagIssuedBL(GameServerEnvironment.GameEnvExecutionContext, notificationTagIssuedDTO);
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
                            CheckInDTO checkInDTO = new CheckInDTO(-1, customerDTO.Id, Utilities.getServerTime(),
                                                                       null, null, null, card.card_id, -1, -1,
                                                                       Convert.ToInt32(dtProduct.Rows[0]["CheckInFacilityId"]),
                                                                       -1, null, customerDTO, true);

                            int allowedTimeInMinutes = Convert.ToInt32(dtProduct.Rows[0]["Time"] == DBNull.Value ? 0 : dtProduct.Rows[0]["Time"]);
                            string autoCheckOut = dtProduct.Rows[0]["AutoCheckOut"].ToString();
                            CheckInDetailDTO checkInDetailDTO = new CheckInDetailDTO(-1, -1, customerDTO.FirstName, card.card_id, null, null, null, null, 0, null, null,
                                                                                      null,
                                                                                      autoCheckOut == "Y" ? Utilities.getServerTime().AddMinutes(allowedTimeInMinutes) : (DateTime?)null,
                                                                                      -1, null, -1, -1, Utilities.getServerTime(), CheckInStatus.CHECKEDIN, true);//c
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
                            string errorMessage = MessageContainerList.GetMessage(GameServerEnvironment.GameEnvExecutionContext, 225);
                            log.LogMethodExit(null, "Throwing Application Exception- " + errorMessage);
                            throw new ApplicationException(errorMessage);
                            //log.LogMethodExit(null, "Throwing Application Exception- " + GameServerEnvironment.Utilities.MessageUtils.getMessage(225));
                            //throw new ApplicationException(GameServerEnvironment.Utilities.MessageUtils.getMessage(225));
                        }
                        List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> checkInSearchParams = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CARD_ID, card.card_id.ToString()));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
                        checkInSearchParams.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS, CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN)));
                        CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(GameServerEnvironment.GameEnvExecutionContext);
                        List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(checkInSearchParams);
                        log.LogVariableState("checkInDetailDTOList", checkInDetailDTOList);
                        if (checkInDetailDTOList != null && checkInDetailDTOList.Any()
                                 && checkInDetailDTOList.Exists(x => x.CheckInTime != null &&
                                  (x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime())))//c
                        {
                            List<CheckInDetailDTO> filteredCheckInDetailDTOList = checkInDetailDTOList.
                                                                                  Where(x => x.CheckInTime != null && (x.CheckOutTime == null || x.CheckOutTime > Utilities.getServerTime()))//c
                                                                                 .ToList();
                            if (filteredCheckInDetailDTOList != null && filteredCheckInDetailDTOList.Any())
                            {
                                CheckInBL checkInBL = new CheckInBL(GameServerEnvironment.GameEnvExecutionContext, filteredCheckInDetailDTOList[0].CheckInId);
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
                if (!Transaction.createTransactionGamePlayRecord(gamePlayId, Trx.Trx_id, Trx.TrxLines.Count, GameServerEnvironment, SQLTrx, ref message, Utilities))
                    throw new ApplicationException(message);
                PaymentModeList paymentModeListBL = new PaymentModeList(GameServerEnvironment.GameEnvExecutionContext);
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                if (paymentModeDTOList != null)
                {
                    TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                    debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                    debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                    debitTrxPaymentDTO.TransactionId = Trx.Trx_id;
                    debitTrxPaymentDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                    debitTrxPaymentDTO.Amount = Trx.Net_Transaction_Amount;
                    debitTrxPaymentDTO.CardId = (new Card(GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD, "", Utilities)).card_id;
                    debitTrxPaymentDTO.CardEntitlementType = "C";
                    TransactionPaymentsBL debitTrxPaymentBL = new TransactionPaymentsBL(GameServerEnvironment.GameEnvExecutionContext, debitTrxPaymentDTO);
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
        /// <param name="GameServerEnvironment">ServerStatic Object</param>
        /// <param name="message">Message</param>
        static void UpdateTicketEaterGamePlay(string CardNumber, Machine CurrentMachine, int TicketCount, SqlTransaction SQLTrx, GameServerEnvironment GameServerEnvironment, ref string message, Utilities Utilities)
        {
            log.LogMethodEntry(CardNumber, CurrentMachine, TicketCount, SQLTrx, GameServerEnvironment, message);
            //SqlCommand cmd;
            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@cardNumber", CardNumber);
            queryParams[1] = new SqlParameter("@tickets", TicketCount);
            queryParams[2] = new SqlParameter("@machine_id", CurrentMachine.MachineId);

            int queryResult = Utilities.executeNonQuery(@"update gameplay set ticket_count = ticket_count + @tickets
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
                    queryResult = Utilities.executeNonQuery(@"update gameplay set ticket_count = ticket_count + @tickets
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
                                     "T", TicketCount, "Ticket Eater log", -1, -1, DateTime.Now, SQLTrx, GameServerEnvironment, Utilities);
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Override Method to Create Gameplay record.
        /// </summary>
        /// <param name="MachineId">Machine Id</param>
        /// <param name="card_number">Tag Number</param>
        /// <param name="credits">Credits Entitlement</param>
        /// <param name="courtesy">Courtesy</param>
        /// <param name="bonus">Bonus</param>
        /// <param name="time">Time</para>
        /// <param name="CardGame">Card Game</param>
        /// <param name="CPCardBalance">Card Balance</param>
        /// <param name="CPCredits">Creditplus Credits</param>
        /// <param name="CPBonus">Creditplus Bonus</param>
        /// <param name="TicketMode">Ticket Mode - Paper or e-Ticket</param>
        /// <param name="TicketCount">Ticket count</param>
        /// <param name="Notes">Notes</param>
        /// <param name="cardGameId">Card Game Id</param>
        /// <param name="PromotionId">Promotion Id</param>
        /// <param name="PlayRequestTime">Play requested time</param>
        /// <param name="SQLTrx">SQL Transaction</param>
        /// <param name="GameServerEnvironment">Game Server Environment object</param>
        /// <param name="Utilities">Utilities object</param>
        /// <returns>Game Play Id</returns>
        public static long CreateGamePlayRecord(int MachineId, string card_number,
                                                 decimal credits, decimal courtesy,
                                                 decimal bonus, decimal time,
                                                 decimal CardGame, decimal CPCardBalance, decimal CPCredits, decimal CPBonus,
                                                 string TicketMode, int TicketCount, string Notes,
                                                 int cardGameId, int PromotionId, DateTime PlayRequestTime,
                                                 SqlTransaction SQLTrx, GameServerEnvironment GameServerEnvironment, Utilities Utilities)
        {
            log.LogMethodEntry(MachineId, card_number, credits, courtesy, bonus, time, CardGame, CPCardBalance,
                                        CPCredits, CPBonus, TicketMode, TicketCount, Notes, cardGameId, PromotionId, PlayRequestTime,
                                        SQLTrx, GameServerEnvironment, Utilities);
            return CreateGamePlayRecord(MachineId, card_number, credits, courtesy, bonus, time, CardGame, CPCardBalance,
                                        CPCredits, CPBonus, TicketMode, TicketCount, Notes, cardGameId, PromotionId, PlayRequestTime,
                                        SQLTrx, GameServerEnvironment, Utilities, null, null);
        }

        /// <summary>
        /// Method to create Game Play in DB
        /// </summary>
        /// <param name="MachineId">Machine Id</param>
        /// <param name="card_number">Tag Number</param>
        /// <param name="credits">Credits Entitlement</param>
        /// <param name="courtesy">Courtesy</param>
        /// <param name="bonus">Bonus</param>
        /// <param name="time">Time</para>
        /// <param name="CardGame">Card Game</param>
        /// <param name="CPCardBalance">Card Balance</param>
        /// <param name="CPCredits">Creditplus Credits</param>
        /// <param name="CPBonus">Creditplus Bonus</param>
        /// <param name="TicketMode">Ticket Mode - Paper or e-Ticket</param>
        /// <param name="TicketCount">Ticket count</param>
        /// <param name="Notes">Notes</param>
        /// <param name="cardGameId">Card Game Id</param>
        /// <param name="PromotionId">Promotion Id</param>
        /// <param name="PlayRequestTime">Play requested time</param>
        /// <param name="SQLTrx">SQL Transaction</param>
        /// <param name="GameServerEnvironment">Game Server Environment object</param>
        /// <param name="Utilities">Utilities object</param>
        /// <param name="MultiGamePlayReference">Multiple Gameplay common reference</param>
        /// <param name="GamePriceTierInfo">Game Price Tier Info in format - Price | No. Of Gameplays</param>
        /// <returns>Game Play Id</returns>
        public static long CreateGamePlayRecord(int MachineId, string card_number,
                                                 decimal credits, decimal courtesy,
                                                 decimal bonus, decimal time,
                                                 decimal CardGame, decimal CPCardBalance, decimal CPCredits, decimal CPBonus,
                                                 string TicketMode, int TicketCount, string Notes,
                                                 int cardGameId, int PromotionId, DateTime PlayRequestTime,
                                                 SqlTransaction SQLTrx, GameServerEnvironment GameServerEnvironment, 
                                                 Utilities Utilities, Guid? MultiGamePlayReference, string GamePriceTierInfo)
        {
            log.LogMethodEntry(MachineId, card_number, credits, courtesy, bonus, time, CardGame, CPCardBalance, CPCredits, CPBonus, TicketMode, TicketCount, Notes, cardGameId, PromotionId, SQLTrx, GameServerEnvironment);
            SqlParameter[] queryParams = new SqlParameter[19];
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
            queryParams[16] = new SqlParameter("@siteId", Utilities.ParafaitEnv.SiteId == -1 ? DBNull.Value : (object)Utilities.ParafaitEnv.SiteId);
            queryParams[17] = new SqlParameter("@multiGamePlayReference", MultiGamePlayReference == null ? DBNull.Value : (object)MultiGamePlayReference);
            queryParams[18] = new SqlParameter("@gamePriceTierInfo", GamePriceTierInfo == null ? DBNull.Value : (object)GamePriceTierInfo);
            //SqlCommand cmd;

            object o = Utilities.executeScalar(@"insert into gameplay 
                                                                (machine_id, card_id,
                                                                credits, courtesy, bonus,
                                                                time, CardGame, CPCardBalance, CPCredits, CPBonus, 
                                                                play_date, notes, ticket_mode, Ticket_Count, CardGameId, PromotionId, PlayRequestTime,site_id,PayoutCost,
                                                                MultiGamePlayReference, GamePriceTierInfo) 
                                                               select TOP 1 @machine_id, c1.card_id, 
                                                                @credits, @courtesy, @bonus,
                                                                @time, @CardGame, @CPCardBalance, @CPCredits, @CPBonus, 
                                                                getdate(), @notes, @ticket_mode, @TicketCount, @cardGameId, @promotionId, @playRequestTime,@siteId,
                                                                isnull(m.PayoutCost, (select top 1 convert(float, isnull(default_value, m.PayoutCost)) ticketCost 
	                                                                                   from parafait_defaults 
	                                                                                  where default_value_name = 'TICKET_COST')),
                                                                @multiGamePlayReference, @gamePriceTierInfo
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

        /// <summary>
        /// Method to update Ticket count in Gameplay and in Cards (for e-tickets)
        /// </summary>
        /// <param name="CardId">Tapped Card instance</param>
        /// <param name="TicketCount">Count of tickets to be updated</param>
        /// <returns>GamePlayDTO</returns>
        public GamePlayDTO UpdateTicketCount(int CardId, int TicketCount)
        {
            log.LogMethodEntry(CardId, TicketCount);
            GamePlayDTO TicketGamePlayDTO = null;
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
            if (GameMachine.MachineId > -1)
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, GameMachine.MachineId.ToString()));
            if (CardId > -1)
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, CardId.ToString()));

            GamePlayListBL gamePlayBL = new GamePlayListBL(GameExecutionContext);
            List<GamePlayDTO> gamePlayList = gamePlayBL.GetGamePlayDTOListWithTagNumber(searchParameters, null);
            if (gamePlayList != null && gamePlayList.Count > 0)
            {
                TicketGamePlayDTO = gamePlayList[0];
            }
            //else
            //{
            //    log.Error("No Gameplay to update ticket count");
            //    throw new Exception("No Gameplay record available to update ticket count");
            //}
            if (TicketCount <= 0)
            {
                log.Error("Ticket count is 0, nothing to update.");
                return TicketGamePlayDTO;
            }
            AccountDTO GameAccountDTO = null;
            if (CardId > -1)
            {
                GameAccountDTO = new AccountBL(GameExecutionContext, CardId, false, false, null).AccountDTO;
                if (GameAccountDTO == null || (GameAccountDTO != null && GameAccountDTO.AccountId == -1))
                {
                    log.Error("Card Number is Invalid");
                    throw new Exception("Card Number is Invalid");
                }
            }
            string message = string.Empty;
            try
            {
                Utilities utilities = GetUtility(GameServerEnvironment.GameEnvExecutionContext);
                this.UpdateTickets(GameMachine, GameAccountDTO == null ? "" : GameAccountDTO.TagNumber, TicketCount, GameServerEnvironment, ref message, utilities);
                if (TicketGamePlayDTO != null)
                {
                    TicketGamePlayDTO = new GamePlayBL(GameExecutionContext, TicketGamePlayDTO.GameplayId, null).GamePlayDTO;
                }
                //TicketGamePlayDTO = new GamePlayBL(GameExecutionContext, TicketGamePlayDTO.GameplayId, null).GamePlayDTO;
                log.LogMethodExit(TicketGamePlayDTO);
                return TicketGamePlayDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        internal int UpdateTickets(Machine CurrentMachine, string card_number, int ticket_count, GameServerEnvironment GameServerEnvironment, ref string message, Utilities Utilities)
        {
            log.LogMethodEntry(CurrentMachine, card_number, ticket_count, GameServerEnvironment, message, Utilities);
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
                    using (SqlConnection cnn = Utilities.createConnection())
                    {
                        SqlTransaction SQLTrx = cnn.BeginTransaction();
                        log.LogVariableState("@cardNumber", card_number);
                        log.LogVariableState("@tickets", ticket_count);

                        object objCardId = Utilities.executeScalar(@"SELECT card_id from cards
                                                                                    where card_number = @cardNumber
                                                                                    and valid_flag = 'Y'",
                                                                                    SQLTrx,
                                                                                new SqlParameter("@cardNumber", card_number));
                        int nrecs = 0;
                        try
                        {
                            if (objCardId == null) //card not found
                            {
                                DataTable dtCardDetails = Utilities.executeDataTable(@"select top 1 card_number, c.card_id 
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
                                    nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(dtCardDetails.Rows[0]["card_id"]), ticket_count, SQLTrx, GameServerEnvironment, Utilities);
                                }
                                else if (dtCardDetails != null && dtCardDetails.Rows.Count == 0)
                                {
                                    //Check if Machine object has Reference machine, search for card using Reference machine id and associate card number
                                    if (CurrentMachine.GameplayMachineId != CurrentMachine.MachineId)
                                    {
                                        dtCardDetails = Utilities.executeDataTable(@"select top 1 card_number, c.card_id 
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
                                            nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(dtCardDetails.Rows[0]["card_id"]), ticket_count, SQLTrx, GameServerEnvironment, Utilities);
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
                                nrecs = UpdateTicketsCreditPlus(Convert.ToInt32(objCardId), ticket_count, SQLTrx, GameServerEnvironment, Utilities);
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
                            UpdateTicketEaterGamePlay(card_number, CurrentMachine, ticket_count, SQLTrx, GameServerEnvironment, ref message, Utilities);
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
                            UpdateTicketEaterGamePlay(card_number, CurrentMachine, ticket_count, null, GameServerEnvironment, ref message, Utilities);
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
                        object cardNumber = Utilities.executeScalar(@"select card_number 
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
                        if (cardNumber == null || (cardNumber != null && cardNumber.ToString() != (card_number.StartsWith("FFFFFFFF") ? GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD : card_number)))
                        {
                            long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId, card_number.StartsWith("FFFFFFFF") ? GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD : card_number, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                   card_number.StartsWith("FFFFFFFF") ? "T" : "E", 0, "Slot Reader", -1, -1, DateTime.Now, null, GameServerEnvironment, Utilities);
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
                    using (SqlConnection cnn = Utilities.createConnection())
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
                                        //GameServerEnvironment.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.GameplayMachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(), null);
                                        EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                              GameExecutionContext.POSMachineName, message, message,
                                                             "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.GameplayMachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(),
                                                              string.Empty, GameExecutionContext.GetSiteId(), false);
                                        SaveEventLog(eventLogDTO);
                                        log.LogVariableState("Message", message);
                                        log.LogMethodExit(0);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    message = "Game Play not found for this machine";
                                    //cmd.Connection.Close();
                                    //GameServerEnvironment.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(), null);
                                    EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                            GameExecutionContext.POSMachineName, message, message,
                                                           "TICKETS", 1, "MachineId, CardId, TicketCount", CurrentMachine.GameplayMachineId.ToString() + ", " + card_number + ", " + ticket_count.ToString(),
                                                            string.Empty, GameExecutionContext.GetSiteId(), false);
                                    SaveEventLog(eventLogDTO);
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
                                //GameServerEnvironment.Utilities.EventLog.logEvent("Parafait Server", 'W', "MaxTicketsPerGamePlay", "Max Tickets Reached", "TICKETS", 1, "MachineId, CardId, MaxTicketsPerGamePlay", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.MaxTicketsPerGamePlay.ToString(), null);
                                EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                             GameExecutionContext.POSMachineName, "MaxTicketsPerGamePlay", "Max Tickets Reached",
                                                            "TICKETS", 1, "MachineId, CardId, MaxTicketsPerGamePlay", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.MaxTicketsPerGamePlay.ToString(),
                                                             string.Empty, GameExecutionContext.GetSiteId(), false);
                                SaveEventLog(eventLogDTO);
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
                                                                                      GameServerEnvironment, ref message, Utilities))
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
                                object productId = Utilities.executeScalar("select top 1 inv.ProductId from inventory inv, product invP where" +
                                    " inv.locationId = @locationId and ISNULL(inv.Quantity, 0) != 0 and inv.ProductId = invP.ProductId and invp.IsActive = 'Y' " +
                                    "order by ISNULL(inv.LotId, 0)", new SqlParameter("@locationId", CurrentMachine.InventoryLocationId));

                                log.LogVariableState("@locationId", CurrentMachine.InventoryLocationId);
                                if (productId == null)
                                {
                                    message += " No inventory product found for location.";
                                    //cmd.Connection.Close();
                                    //GameServerEnvironment.Utilities.EventLog.logEvent("Parafait Server", 'W', message, message, "TICKETS", 1, "MachineId, CardId, InvLocId, TicketCount", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.InventoryLocationId.ToString() + ", " + ticket_count.ToString(), null);
                                    EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                             GameExecutionContext.POSMachineName, message, message,
                                                            "TICKETS", 1, "MachineId, CardId, InvLocId, TicketCount", CurrentMachine.MachineId.ToString() + ", " + card_number + ", " + CurrentMachine.InventoryLocationId.ToString() + ", " + ticket_count.ToString(),
                                                             string.Empty, GameExecutionContext.GetSiteId(), false);
                                    SaveEventLog(eventLogDTO);
                                    log.LogVariableState("Message", message);
                                    log.LogMethodExit(1);
                                    return 1;
                                }

                                try
                                {
                                    Inventory.AdjustInventory(Inventory.AdjustmentTypes.Payout,
                                                                        Utilities,
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
                            else if (GamePlay.Rows[0]["ticket_mode"].ToString() == "E" || CurrentMachine.ForceRedeemToCard || GameServerEnvironment.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD == "Y")
                            {
                                SqlTransaction SQLTrx = cmd.Connection.BeginTransaction();
                                cmd.Transaction = SQLTrx;
                                try
                                {
                                    UpdateTicketsCreditPlus(Convert.ToInt32(GamePlay.Rows[0]["card_id"]), ticket_count, SQLTrx, GameServerEnvironment, Utilities);
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
                                if (CurrentMachine.ForceRedeemToCard || GameServerEnvironment.AUTO_UPDATE_PHYSICAL_TICKETS_ON_CARD == "Y")
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
        /// <param name="GameServerEnvironment">ServerStatic object</param>
        /// <returns>returns records updated</returns>
        public static int UpdateTicketsCreditPlus(int cardId, int ticketCount, SqlTransaction sqlTransaction, GameServerEnvironment GameServerEnvironment, Utilities Utilities)
        {
            log.LogMethodEntry(cardId, ticketCount, GameServerEnvironment, Utilities);
            int updatedRecs;
            updatedRecs = Utilities.executeNonQuery(@"update CardCreditPlus set CreditPlus = CreditPlus + @tickets, 
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
                                                                  , new SqlParameter("@extendOnReload", GameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD)
                                                                  , new SqlParameter("@ExpireDays", GameServerEnvironment.GAMEPLAY_TICKETS_EXPIRY_DAYS));

            log.LogVariableState("@card_id", cardId);
            log.LogVariableState("@tickets", ticketCount);
            log.LogVariableState("@ExpireDays", GameServerEnvironment.GAMEPLAY_TICKETS_EXPIRY_DAYS);
            log.LogVariableState("@extendOnReload", GameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD);

            try
            {
                Loyalty loyalty = new Loyalty(Utilities);
                if (updatedRecs == 0)
                {
                    loyalty.CreateGenericCreditPlusLine(Convert.ToInt32(cardId),
                                                        "T", ticketCount, false,
                                                        GameServerEnvironment.GAMEPLAY_TICKETS_EXPIRY_DAYS,
                                                        GameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD,
                                                        "ServerCore", "Gameplay Tickets", sqlTransaction);
                    if (GameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD.Equals("Y"))
                        loyalty.ExtendOnReload(Convert.ToInt32(cardId), "T", sqlTransaction);
                }
                else
                {
                    Card updateCard = new Card(cardId, "", Utilities, sqlTransaction);
                    updateCard.updateCardTime(sqlTransaction);
                    log.LogVariableState("Card Updated for Credit plus update scenario: ", updateCard.card_id);
                }
                if (GameServerEnvironment.AUTO_EXTEND_GAMEPLAY_TICKETS_ON_RELOAD.Equals("Y"))
                    loyalty.ExtendOnReload(Convert.ToInt32(cardId), "T", sqlTransaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(updatedRecs);
            return updatedRecs;
        }

        public static void CreateGamePlayInfo(int MachineId, decimal playTime, SqlTransaction SQLTrx, Utilities Utilities)
        {
            log.LogMethodEntry(MachineId, playTime, SQLTrx, Utilities);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            // fetch latest game play record on this machine, on the same business day
            Utilities.executeNonQuery(@"insert into gameplayinfo (gameplay_id, isPaused, play_time, last_update_by, last_update_date)
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
                                                       new SqlParameter("@login", Utilities.ParafaitEnv.LoginID),
                                                       new SqlParameter("@playTime", playTime));

            cmd.Parameters.AddWithValue("@machine_id", MachineId);
            cmd.Parameters.AddWithValue("@login", Utilities.ParafaitEnv.LoginID);
            cmd.Parameters.AddWithValue("@playTime", playTime);
            cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@playTime", playTime);
            log.LogMethodExit(null);
        }

        public void CreateGamePlayInfoOnTime(int MachineId, decimal playTime, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(MachineId, playTime, SQLTrx);
            Utilities utilities = GetUtility(GameExecutionContext);
            CreateGamePlayInfo(MachineId, playTime, SQLTrx, utilities);
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", GameExecutionContext.GetUserId());
            log.LogVariableState("@playTime", playTime);
            log.LogMethodExit(null);
        }

        public void CreateGamePlayInfoOnCardGame(int MachineId, int CardGameId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(MachineId, CardGameId, SQLTrx);
            Utilities utilities = GetUtility(GameExecutionContext);
            CreateGamePlayInfo(MachineId, CardGameId, SQLTrx, utilities);

            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", GameExecutionContext.GetUserId());
            log.LogVariableState("@cgId", CardGameId);
            log.LogMethodExit(null);
        }

        public static void CreateGamePlayInfo(int MachineId, int CardGameId, SqlTransaction SQLTrx, Utilities Utilities)
        {
            log.LogMethodEntry(MachineId, CardGameId, SQLTrx, Utilities);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            // fetch latest game play record on this machine, on the same business day
            cmd.CommandText = @"insert into gameplayinfo (gameplay_id, isPaused, play_time, last_update_by, last_update_date)
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
                                                                            and gg.play_date > DATEADD(HH, 6, DATEADD(dd, DATEDIFF(dd, 0, (case when datepart(HH, getdate()) > 6 then getdate() else getdate() - 1 end)), 0)))";

            cmd.Parameters.AddWithValue("@machine_id", MachineId);
            cmd.Parameters.AddWithValue("@login", Utilities.ParafaitEnv.LoginID);
            cmd.Parameters.AddWithValue("@cgId", CardGameId);
            cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", MachineId);
            log.LogVariableState("@login", Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@cgId", CardGameId);
            log.LogMethodExit(null);
        }

        public long CreateMiFareGamePlay(Machine CurrentMachine,
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
                                               GameServerEnvironment GameServerEnvironment,
                                               ref string Message,
                                               Utilities Utilities)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, Price, Credits, Bonus, Courtesy, CreditPlusCredits, DiscountPercentage, CardBalance, RecordId, CardGamePlay, CardGameBalance, GameServerEnvironment, Message, Utilities);
            //ServerStatic.GlobalGamePlayStats GamePlayStats = new ServerStatic.GlobalGamePlayStats(CurrentMachine);
            GameServerEnvironment.GameServerPlayDTO GamePlayStats = new GameServerEnvironment.GameServerPlayDTO(CurrentMachine.MachineId);
            using (SqlConnection cnn = Utilities.createConnection())
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
                        if (GameServerEnvironment.CREATE_FF_GAMEPLAY_IF_CARD_NOT_FOUND)
                        {
                            CardNumber = GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD;
                            cmd.Parameters["@cardNumber"].Value = CardNumber;
                            daCard.SelectCommand = cmd;
                            daCard.Fill(dtCard);
                        }

                        if (dtCard.Rows.Count == 0) // card not found
                        {
                            //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Card not found", "MIFARE-GAMEPLAY", 3);
                            EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "E", GameExecutionContext.GetUserId(),
                                                                     GameExecutionContext.POSMachineName, GamePlayData, "Card not found",
                                                                    "MIFARE-GAMEPLAY", 3, string.Empty, string.Empty,
                                                                     string.Empty, GameExecutionContext.GetSiteId(), false);
                            SaveEventLog(eventLogDTO);
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
                                CardGames cardGames = new CardGames(Utilities);
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
                                if (!GameServerEnvironment.ZERO_PRICE_CARDGAME_PLAY)
                                    GamePlayStats.ConsumedValues.CardGame = GamePlayStats.CreditsRequired;

                                GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCARDGAME;
                            }
                            else
                            {
                                //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Card not found", "MIFARE-GAMEPLAY", 3);
                                EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "E", GameServerEnvironment.GameEnvExecutionContext.GetUserId(),
                                                                     GameServerEnvironment.GameEnvExecutionContext.POSMachineName, GamePlayData, "Card not found",
                                                                    "MIFARE-GAMEPLAY", 3, string.Empty, string.Empty,
                                                                     string.Empty, GameServerEnvironment.GameEnvExecutionContext.GetSiteId(), false);
                                SaveEventLog(eventLogDTO);
                                cmd.Connection.Close();
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }
                        else
                        {
                            decimal cardCredits = 0;
                            decimal cardBonus = 0;
                            decimal cardCourtesy = 0;
                            decimal cardCreditPlusCardBalanceAndGPCredits = 0;
                            decimal.TryParse(Credits, out cardCredits);
                            GamePlayStats.ConsumedValues.CardCredits = cardCredits / 100;

                            decimal.TryParse(Bonus, out cardBonus);
                            GamePlayStats.ConsumedValues.CardBonus = cardBonus / 100;

                            decimal.TryParse(Courtesy, out cardCourtesy);
                            GamePlayStats.ConsumedValues.CardCourtesy = cardCourtesy / 100;

                            decimal.TryParse(CreditPlusCredits, out cardCreditPlusCardBalanceAndGPCredits);
                            GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits = cardCreditPlusCardBalanceAndGPCredits / 100;

                            if (GamePlayStats.ConsumedValues.CardCourtesy > 0)
                            {
                                if (GamePlayStats.ConsumedValues.CardCredits > 0 || GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0 || GamePlayStats.ConsumedValues.CardBonus > 0)
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDITCOURTESY;
                                    GamePlayStats.GameplayMessage = "Credit, Bonus and Courtesy Play";
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCOURTESY;
                                    GamePlayStats.GameplayMessage = "Courtesy Play";
                                }
                            }
                            else if (GamePlayStats.ConsumedValues.CardBonus > 0)
                            {
                                if (GamePlayStats.ConsumedValues.CardCredits > 0 || GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits > 0)
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDITBONUS;
                                    GamePlayStats.GameplayMessage = "Credit and Bonus Play";
                                }
                                else
                                {
                                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDBONUS;
                                    GamePlayStats.GameplayMessage = "Bonus Play";
                                }
                            }
                            else
                            {
                                GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDIT;
                                GamePlayStats.GameplayMessage = "Credit Play";
                            }

                            decimal price = 0;
                            decimal.TryParse(Price, out price);
                            if (price / 100 != GamePlayStats.ConsumedValues.CardCredits + GamePlayStats.ConsumedValues.CardBonus + GamePlayStats.ConsumedValues.CardCourtesy + GamePlayStats.ConsumedValues.CardCreditPlusCardBalanceAndGPCredits)
                            {
                                //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData, "Price does not match total consumed", "MIFARE-GAMEPLAY", 3);
                                EventLogDTO eventLogDTO = new EventLogDTO(-1, "ParafaitServer", ServerDateTime.Now, "E", GameExecutionContext.GetUserId(),
                                                     GameExecutionContext.POSMachineName, GamePlayData, "Price does not match total consumed",
                                                    "MIFARE-GAMEPLAY", 3, string.Empty, string.Empty,
                                                     string.Empty, GameExecutionContext.GetSiteId(), false);
                                SaveEventLog(eventLogDTO);
                                cmd.Connection.Close();
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(-1);
                                return -1;
                            }
                        }
                    } // tech card
                    else
                    {
                        GamePlayStats.GamePlayType = GameServerEnvironment.TECHPLAY;
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
                        //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData + "-" + ReaderRecordId.ToString(), Message, "MIFARE-GAMEPLAY", 3);
                        EventLogDTO eventLogDTO = new EventLogDTO(-1, "ParafaitServer", ServerDateTime.Now, "E", GameExecutionContext.GetUserId(),
                                                                     GameExecutionContext.POSMachineName, GamePlayData + "-" + ReaderRecordId.ToString(), Message,
                                                                    "MIFARE-GAMEPLAY", 3, string.Empty, string.Empty,
                                                                     string.Empty, GameExecutionContext.GetSiteId(), false);
                        SaveEventLog(eventLogDTO);
                        cmd.Connection.Close();
                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(0);
                        return 0;
                    }

                    long gamePlayId = CreateGamePlay(CurrentMachine.MachineId, CardNumber, GamePlayStats, GameServerEnvironment, ref Message);

                    if (gamePlayId < 0)
                    {
                        //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'W', GamePlayData, Message, "MIFARE-GAMEPLAY", 3);
                        EventLogDTO eventLogDTO = new EventLogDTO(-1, "ParafaitServer", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                                     GameExecutionContext.POSMachineName, GamePlayData, Message,
                                                                    "MIFARE-GAMEPLAY", 3, string.Empty, string.Empty,
                                                                     string.Empty, GameExecutionContext.GetSiteId(), false);
                        SaveEventLog(eventLogDTO);
                    }
                    else
                    {
                        cmd.CommandText = @"insert into gameplayinfo (gameplay_id, ReaderRecordId, GamePlayData, last_update_by, last_update_date)
                                        values (@gameplay_id, @ReaderRecordId, @GamePlayData, @last_update_by, getdate())";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                        cmd.Parameters.AddWithValue("@ReaderRecordId", ReaderRecordId);
                        cmd.Parameters.AddWithValue("@last_update_by", Utilities.ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@GamePlayData", GamePlayData);
                        cmd.ExecuteNonQuery();
                        log.LogVariableState("@gameplay_id", gamePlayId);
                        log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                        log.LogVariableState("@last_update_by", Utilities.ParafaitEnv.LoginID);
                        log.LogVariableState("@GamePlayData", GamePlayData);
                    }

                    cmd.Connection.Close();
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(gamePlayId);
                    return gamePlayId;
                }
            }
        }

        /// <summary>
        /// Override Method to create Coin Pusher Game Play
        /// </summary>
        /// <param name="CurrentMachine">Machine Object</param>
        /// <param name="CardNumber">Tag Number</param>
        /// <param name="GameServerEnvironment">Game Server Environment</param>
        /// <param name="Message">Message</param>
        /// <param name="Utilities">Utilities Object</param>
        /// <returns>Game Play Id</returns>
        public long CreateCoinPusherGamePlay(Machine CurrentMachine, string CardNumber, GameServerEnvironment GameServerEnvironment, ref string Message, Utilities Utilities)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, GameServerEnvironment, Message, Utilities);
            return CreateCoinPusherGamePlay(CurrentMachine, CardNumber, GameServerEnvironment, null, string.Empty, ref Message, Utilities);
        }

        /// <summary>
        /// Method to create Coin Pusher Game Play
        /// </summary>
        /// <param name="CurrentMachine">Machine Object</param>
        /// <param name="CardNumber">Tag Number</param>
        /// <param name="GameServerEnvironment">Game Server Environment</param>
        /// <param name="MultiGamePlayReference">Multi Game Play Reference</param>
        /// <param name="GamePriceTierInfo">Game Price Tier Info in format - Price | No. Of Gameplays</param>
        /// <param name="Message">Message</param>
        /// <param name="Utilities">Utilities Object</param>
        /// <returns>Game Play Id</returns>
        public long CreateCoinPusherGamePlay(Machine CurrentMachine, string CardNumber, GameServerEnvironment GameServerEnvironment, Guid? MultiGamePlayReference, string GamePriceTierInfo, ref string Message, Utilities Utilities)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, GameServerEnvironment, Message, Utilities);
            if (CardNumber.StartsWith("FFFFFFFF"))
                CardNumber = GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD;
            long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId, CardNumber, CurrentMachine.play_credits, 0, 0, 0, 0, 0, 0, 0, "T", 0, "Coin Pusher", -1, -1, DateTime.Now, null, GameServerEnvironment, Utilities, MultiGamePlayReference, GamePriceTierInfo);
            if (gamePlayId > 0)
                Message = "Coin pusher gameplay with " + CurrentMachine.play_credits.ToString("N2") + " credits created for card: " + GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD;
            else
                Message = "Unable to create coin pusher gameplay for card: " + GameServerEnvironment.TOKEN_MACHINE_GAMEPLAY_CARD;

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
        /// <param name="GameServerEnvironment"></param>
        /// <returns>Parent card number if exists else child card number</returns>
        public static ParentChildCardsDTO getParentCard(string CardNumber, GameServerEnvironment GameServerEnvironment)
        {
            log.LogMethodEntry(CardNumber, GameServerEnvironment);
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
            Customer.Accounts.AccountDTO childCardAccountDTO = new Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, CardNumber, false, false).AccountDTO;
            if (childCardAccountDTO == null || (childCardAccountDTO != null && childCardAccountDTO.AccountId <= -1))
            {
                log.LogMethodExit(CardNumber);
                return (new ParentChildCardsDTO());
            }
            ParentChildCardsListBL gpParentCardsListBL = new ParentChildCardsListBL(GameServerEnvironment.GameEnvExecutionContext);
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
                Customer.Accounts.AccountDTO parentCardAccountDTO = new Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, gpParentCardsListDTO[0].ParentCardId, false, false).AccountDTO;
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
        public string ProcessEntitlementForChildCard(int accountId)
        {
            log.LogMethodEntry(accountId);
            List<AccountRelationshipDTO> accountRelationshipDTOList = new List<AccountRelationshipDTO>();
            List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID, accountId.ToString()));
            searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, 1.ToString()));
            AccountRelationshipListBL accountRelationshipListBL = new AccountRelationshipListBL(GameExecutionContext);
            accountRelationshipDTOList = accountRelationshipListBL.GetAccountRelationshipDTOList(searchParameters);
            string accountNumber = ProcessEntitlementForChildCard(accountRelationshipDTOList.FirstOrDefault(), GameServerEnvironment, GameServerPlayDTO);
            log.LogMethodExit(accountNumber);
            return accountNumber;
        }

        /// <summary>
        /// Process Entitlement for child card from parent card if applicable
        /// </summary>
        /// <param name="accountRelationshipDTO">parentChildCard DTO of child card tapped on reader</param>
        /// <param name="gamePlayStats">gameplayStats of parent card if applicable</param>
        /// <returns>card number of child or parent</returns>
        public string ProcessEntitlementForChildCard(AccountRelationshipDTO accountRelationshipDTO, GameServerEnvironment GameServerEnvironment, GameServerEnvironment.GameServerPlayDTO gamePlayStats)
        {
            Customer.Accounts.AccountDTO parentCardAccountDTO = new Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.AccountId, false, false).AccountDTO;
            Semnox.Parafait.Customer.Accounts.AccountDTO childCardAccountDTO = new Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.RelatedAccountId, false, false).AccountDTO;
            //Compute GameServerPlayDTO for Parent card
            gamePlayStats = getGamePlayDetails(GameMachine.MachineId, parentCardAccountDTO.TagNumber, GameServerEnvironment);
            if (gamePlayStats.GamePlayType == GameServerEnvironment.VALIDCARDGAME
                || gamePlayStats.GamePlayType == GameServerEnvironment.TECHPLAY)
            {
                log.LogVariableState("Daily Limit not specified for card. Return Parent card: ", parentCardAccountDTO);
                return parentCardAccountDTO.TagNumber;
            }
            getParentChildCardEntitlementBalance(accountRelationshipDTO, GameServerEnvironment, gamePlayStats.CreditsRequired);
            return childCardAccountDTO.TagNumber;
        }

        /// <summary>
        /// Transfer entitlement from parent to child card
        /// </summary>
        /// <param name="accountRelationshipDTO">parent child card DTO</param>
        /// <param name="GameServerEnvironment">Game Server Static</param>
        /// <param name="valueRequiredInChildCard">Value required in case daily percentage not defined</param>
        /// <returns>true if successful</returns>
        private bool getParentChildCardEntitlementBalance(AccountRelationshipDTO accountRelationshipDTO, GameServerEnvironment GameServerEnvironment, decimal valueRequiredInChildCard)
        {
            log.LogMethodEntry(accountRelationshipDTO, GameServerEnvironment, valueRequiredInChildCard);
            Utilities Utilities = GetUtility(GameServerEnvironment.GameEnvExecutionContext);
            Semnox.Parafait.Customer.Accounts.AccountDTO parentAccountDTO = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.AccountId, true, true).AccountDTO;
            decimal parentCardBalance =
                Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance == null ? 0 : parentAccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance);
            log.LogVariableState("Parent card balance: ", parentCardBalance);
            decimal parentBonusBalance =
                Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.TotalBonusBalance == null ? 0 : parentAccountDTO.AccountSummaryDTO.TotalBonusBalance);
            log.LogVariableState("Parent Bonus balance: ", parentBonusBalance);
            Semnox.Parafait.Customer.Accounts.AccountDTO childAccountDTO = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.RelatedAccountId, true, true).AccountDTO;
            if (accountRelationshipDTO.DailyLimitPercentage == null || accountRelationshipDTO.DailyLimitPercentage == 0)
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
                    Semnox.Parafait.Customer.Accounts.AccountBL parentAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.AccountId, true, true);
                    Semnox.Parafait.Customer.Accounts.AccountBL childAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, childAccountDTO.AccountId, true, true);
                    TaskProcs taskProcs = new TaskProcs(Utilities);
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
                DateTime serverTime = Utilities.getServerTime();
                int businessStartTime;
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(GameServerEnvironment.GameEnvExecutionContext,
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
                valueToTransfer = parentCardBalance * Convert.ToInt32(accountRelationshipDTO.DailyLimitPercentage) / 100;
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
                valueToTransfer = Decimal.Round(valueToTransfer, Utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                log.LogVariableState("Required balance to transfer from parent to child card: ", valueToTransfer);
                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                entitlements.Add(Semnox.Parafait.Customer.Accounts.CreditPlusTypeConverter.ToString(Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE), valueToTransfer);
                Semnox.Parafait.Customer.Accounts.AccountBL parentAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, accountRelationshipDTO.AccountId, true, true);
                Semnox.Parafait.Customer.Accounts.AccountBL childAccountBL = new Semnox.Parafait.Customer.Accounts.AccountBL(GameServerEnvironment.GameEnvExecutionContext, childAccountDTO.AccountId, true, true);
                TaskProcs taskProcs = new TaskProcs(Utilities);
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

        public void UpdateGameplayStatus(int machineId, bool isSuccess)
        {
            log.LogMethodEntry(machineId, isSuccess);
            Utilities Utilities = GetUtility(GameExecutionContext);
            SqlCommand cmd = Utilities.getCommand();

            // fetch latest game play record on this machine, on the same business day
            Utilities.executeNonQuery(@"insert into gameplayinfo (gameplay_id, GameEndTime, Status, last_update_by, last_update_date)
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
                                                    new SqlParameter("@machine_id", machineId),
                                                    new SqlParameter("@login", Utilities.ParafaitEnv.LoginID),
                                                    new SqlParameter("@status", (isSuccess ? "SUCCESS" : "FAILED")));

            cmd.Parameters.AddWithValue("@machine_id", machineId);
            cmd.Parameters.AddWithValue("@login", Utilities.ParafaitEnv.LoginID);
            cmd.Parameters.AddWithValue("@status", (isSuccess ? "SUCCESS" : "FAILED"));
            cmd.ExecuteNonQuery();
            log.LogVariableState("@machine_id", machineId);
            log.LogVariableState("@login", Utilities.ParafaitEnv.LoginID);
            log.LogVariableState("@status", (isSuccess ? "SUCCESS" : "FAILED"));
            log.LogMethodExit(null);
        }

        public bool ValidateFingerPrint(int accountId,
                                               byte[] Template)
        {

            log.LogMethodEntry(accountId, Template);
            Utilities utilities = GetUtility(GameExecutionContext);
            AccountBL accountBL = new AccountBL(GameExecutionContext, accountId, false, false);
            string CardNumber = accountBL.AccountDTO.TagNumber;
            DataTable dtFP = utilities.executeDataTable(@"select cfp.Template, cfp.FPSalt
                                                                from cards cc, CustomerFingerPrint cfp
                                                                where cfp.CardId = cc.card_Id
                                                                and cc.card_number = @cardNumber
                                                                and cc.valid_flag = 'Y'
                                                                and cfp.ActiveFlag = 1",
                                                            new SqlParameter("@cardNumber", CardNumber));
            log.LogVariableState("@cardNumber", CardNumber);
            if (dtFP.Rows.Count == 0)
            {
                string FPSalt = utilities.GenerateRandomCardNumber(10);

                byte[] encryptedTemplate = /*Semnox.Parafait.EncryptionUtils.EncryptionAES.*/Encryption.Encrypt(Template, Encryption.getKey(FPSalt));
                utilities.executeNonQuery(@"insert into CustomerFingerPrint
                                                        (CardId, Template, ActiveFlag, Source, LastUpdatedDate, LastUpdatedBy, FPSalt)
                                                        select card_id, @template, 1, @source, getdate(), @user, @FPSalt
                                                        from cards
                                                        where card_number = @cardNumber
                                                        and valid_flag = 'Y'",
                                                     new SqlParameter("@cardNumber", CardNumber),
                                                     new SqlParameter("@template", encryptedTemplate),
                                                     new SqlParameter("@source", GameMachine.machine_name),
                                                     new SqlParameter("@user", "Server"),
                                                     new SqlParameter("@FPSalt", FPSalt));
                log.LogVariableState("@cardNumber", CardNumber);
                log.LogVariableState("@template", encryptedTemplate);
                log.LogVariableState("@source", GameMachine.machine_name);
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

                bool returnvalue = GameMachine.fingerPrintReader.Verify(userFingerPrintDetailDTOList, Template);
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }
        }

        public long CreateFreeModeGamePlay(Machine CurrentMachine,
                                               string CardNumber,
                                               string TicketMode,
                                               string Tickets,
                                               string Counter,
                                               GameServerEnvironment GameServerEnvironment,
                                               ref string Message,
                                               Utilities Utilities)
        {
            log.LogMethodEntry(CurrentMachine, CardNumber, TicketMode, Tickets, Counter, GameServerEnvironment, Message);
            if (System.Text.RegularExpressions.Regex.Matches(CardNumber, "0").Count >= 8)
            {
                log.LogVariableState("Message", Message);
                log.LogMethodExit(-1);
                return -1;
            }


            //ServerStatic.GlobalGamePlayStats GamePlayStats = new ServerStatic.GlobalGamePlayStats();
            GameServerEnvironment.GameServerPlayDTO GamePlayStats = new GameServerEnvironment.GameServerPlayDTO();
            using (SqlConnection cnn = Utilities.createConnection())
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
                        Card card = new Card(CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
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
                            //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'E', GamePlayData + "-" + ReaderRecordId.ToString(), Message, "FREE-GAMEPLAY", 3);
                            EventLogDTO eventLogDTO = new EventLogDTO(-1, "ParafaitServer", ServerDateTime.Now, "E", GameExecutionContext.GetUserId(),
                                                              GameExecutionContext.POSMachineName, GamePlayData + "-" + ReaderRecordId.ToString(), Message,
                                                             "FREE-GAMEPLAY", 3, string.Empty, string.Empty, string.Empty, GameExecutionContext.GetSiteId(), false);
                            SaveEventLog(eventLogDTO);
                            SQLTrx.Rollback();
                            cmd.Connection.Close();
                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(0);
                            return 0;
                        }
                    }

                    GamePlayStats.GamePlayType = GameServerEnvironment.VALIDCREDIT;
                    GamePlayStats.GameplayMessage = "Free Play";
                    int ticketCount = 0;
                    Int32.TryParse(Tickets, out ticketCount);

                    long gamePlayId = CreateGamePlayRecord(CurrentMachine.MachineId,
                                                            CardNumber, 0, 0, 0, 0, 0, 0, 0, 0,
                                                            TicketMode, ticketCount,
                                                            GamePlayStats.GameplayMessage, -1, -1,
                                                            GamePlayStats.PlayRequestTime, SQLTrx, GameServerEnvironment, Utilities);

                    if (gamePlayId < 0)
                    {
                        SQLTrx.Rollback();
                        EventLogDTO eventLogDTO = new EventLogDTO(-1, "Parafait Server", ServerDateTime.Now, "W", GameExecutionContext.GetUserId(),
                                                  GameExecutionContext.POSMachineName, GamePlayData, Message,
                                                  "FREE-GAMEPLAY", 3, string.Empty, string.Empty, string.Empty, GameExecutionContext.GetSiteId(), false);
                        SaveEventLog(eventLogDTO);
                        //GameServerEnvironment.Utilities.EventLog.logEvent("ParafaitServer", 'W', GamePlayData, Message, "FREE-GAMEPLAY", 3);
                    }
                    else
                    {
                        cmd.CommandText = @"insert into gameplayinfo (gameplay_id, ReaderRecordId, GamePlayData, last_update_by, last_update_date)
                                        values (@gameplay_id, @ReaderRecordId, @GamePlayData, @last_update_by, getdate())";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                        cmd.Parameters.AddWithValue("@ReaderRecordId", ReaderRecordId);
                        cmd.Parameters.AddWithValue("@last_update_by", Utilities.ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@GamePlayData", GamePlayData);
                        cmd.ExecuteNonQuery();
                        log.LogVariableState("@gameplay_id", gamePlayId);
                        log.LogVariableState("@ReaderRecordId", ReaderRecordId);
                        log.LogVariableState("@last_update_by", Utilities.ParafaitEnv.LoginID);
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

        private void SaveEventLog(EventLogDTO eventLogDTO)
        {
            log.LogMethodEntry(eventLogDTO);
            EventLog eventLog = new EventLog(GameExecutionContext, eventLogDTO);
            eventLog.Save();
            log.LogMethodExit();
        }

        private Utilities GetUtility(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            utilities.ExecutionContext.SetMachineId(executionContext.MachineId);
            utilities.ExecutionContext.SetPosMachineGuid(executionContext.PosMachineGuid);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return utilities;
        }
    }
}
