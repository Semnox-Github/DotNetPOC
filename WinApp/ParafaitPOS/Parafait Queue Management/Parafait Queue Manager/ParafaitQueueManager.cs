/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - FormQueueManagement - manages the display of the queue
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;


namespace ParafaitQueueManagement
{

    public   class AuditManager
    {
        // Utilities parafaitUtility = new Utilities();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public    void logAction(string actionPerformed, string objectModified, int idOfObjectModified, string valueModified, string userUpdating)
        {
            log.LogMethodEntry(actionPerformed, objectModified, idOfObjectModified, valueModified, userUpdating);
            Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', objectModified, "", actionPerformed, 0, idOfObjectModified.ToString(), valueModified, userUpdating, Common.ParafaitEnv.POSMachine, null);
            log.LogMethodExit();
        }
    }


    


    public class QueueManager
    {
        GameMachineList gameMachinesList;
        PlayersList playersList;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Utilities parafaitUtility = new Utilities();
        int gamePlayPastTime;
        bool isDigitalTokenEnabled;
        /// <summary>
        /// Featches all the machines that are part of the queue system
        /// </summary>
        public QueueManager()
        {
            log.LogMethodEntry();
            gameMachinesList = new GameMachineList();
            gamePlayPastTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("GAMEPLAY_END_WAIT_TIME"));
            isDigitalTokenEnabled = (Common.Utilities.getParafaitDefaults("ENABLE_DIGITAL_TOKEN").CompareTo("Y") == 0);
            log.LogMethodExit();
        }
        public GameMachineList GetGameMachineList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameMachinesList);
            return gameMachinesList;
        }
        public GameMachine[] getMachineDetails()
        {
            log.LogMethodEntry();
            GameMachine[] machineDetails = gameMachinesList.GetAllGameMachines();
            log.LogMethodExit(machineDetails);
            return machineDetails;
        }
        public Player[] getPlayersList()
        {
            log.LogMethodEntry();
            Player[] playerListRet;
            if (playersList != null)
            {
                playerListRet = playersList.GetAllPlayers();
                log.LogMethodExit(playerListRet);
                return playerListRet;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        public string[] GetListOfGame()
        {
            log.LogMethodEntry();
            string[] listOfGames = gameMachinesList.GetAllGameMachinesName();
            log.LogMethodExit(listOfGames);
            return listOfGames;
        }
        public void PrintTokens(int playerIdentifier)
        {
            log.LogMethodEntry(playerIdentifier);
            playersList.PrintTokens(playerIdentifier, true);
            log.LogMethodExit();
        }
        public void Activate(int playerIdentifier)
        {
            log.LogMethodEntry(playerIdentifier);
            playersList.Activate(playerIdentifier);
            log.LogMethodExit();
        }
        public void UndoPrintTokens(int playerIdentifier, bool customerIntimationFlag)
        {
            log.LogMethodEntry(playerIdentifier, customerIntimationFlag);
            playersList.UndoPrintTokens(playerIdentifier, customerIntimationFlag);
            log.LogMethodExit();
        }
        public int AssignLane(int playerIdentifier, int laneNo, ref string retMessage)
        {
            log.LogMethodEntry(playerIdentifier, laneNo);
            int playerId = playersList.FindPlayer(playerIdentifier);
            Player currPlayer = playersList.GetPlayer(playerId);
            string gameNameOfPlayer = currPlayer.EntitledGameName;
            GameMachine newGameMachine = gameMachinesList.IdentifyGameMachineThruSerial(laneNo);
            if (newGameMachine == null)
            {
                retMessage = "Lane does not exists, please check";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-1);
                return -1;
            }
            if (newGameMachine.IsActive == false)
            {
                retMessage = "Lane is not active, cannot assign";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-2);
                return -2;
            }
            if (string.Compare(newGameMachine.GameNameOfMachine, gameNameOfPlayer) != 0)
            {
                if (newGameMachine.IsCompatibleWith(gameNameOfPlayer) == false)
                {
                    retMessage = "Lane is not compatible, please check the entitlement and lane you are trying to assign";
                    log.Debug("retMessage: " + retMessage);
                    log.LogMethodExit(-3);
                    return -3;
                }
            }
            currPlayer.ShiftPlayer(newGameMachine.GameMachineId, gameNameOfPlayer, Common.Utilities.getServerTime());
            log.LogMethodExit(0);
            return 0;
        }
        public void refreshPlayersOnGameMachine()
        {
            log.LogMethodEntry();
           // MessageBox.Show(DateTime.Now.ToString());
            gameMachinesList.refreshGameMachineList();
            gameMachinesList.ClearPlayers();
            try
            {
                SqlCommand cmdActivePlayers = Common.Utilities.getCommand();
                cmdActivePlayers.CommandText = @"select gmp.machine_id machine_id, 
                                                        gmp.play_date play_date,
                                                        gmp.gameplay_id gameplay_id, 
                                                        gmp.card_id card_id,
                                                        isnull(gplayinfo.play_time, 0) playTime,
                                                        c.card_number cardNumber,
                                                        datediff(mi,gmp.play_date,getdate()) datedifference, 
                                                        cu.customer_name customer_name,
                                                        isnull(gplayinfo.GameEndTime,0) GameEndTime,    
                                                        gplayinfo.IsPaused pauseStatus, 
                                                        isnull(gplayinfo.PauseStartTime,0) PauseStartTime,
                                                        isnull(gplayinfo.TotalPauseTime,0) TotalPauseTime,
                                                        cu.username userName
                                                   from machines mac, 
                                                        cards c, 
                                                        CustomerView(@PassPhrase) cu,
                                                        (select m.machine_id, m.machine_name, g.game_name, max(gmp.play_date) play_date
                                                              from game_profile gp, games g, machines m, gameplay gmp
                                                             where gp.game_profile_id = g.game_profile_id
                                                               and g.game_id = m.game_id
                                                               and dbo.GetGameProfileValue(g.game_Id, 'QUEUE_SETUP_REQUIRED') = '1'
                                                               and gmp.machine_id = m.machine_id
                                                               and gmp.play_date > dateadd(mi,@minutesAddParam,getdate())
                                                             group by m.machine_id, m.machine_name, g.game_name) gps,
                                                        (gameplay gmp left outer join gameplayinfo gplayinfo on gmp.gameplay_id = gplayinfo.gameplay_id)
                                                  where gps.play_date = gmp.play_date
                                                    and gps.machine_id = gmp.machine_id
                                                    and mac.machine_id = gmp.machine_id
                                                    and gmp.card_id = c.card_id
                                                    and cu.customer_id = c.customer_id";

                cmdActivePlayers.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));
                string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                string passPhrase = encryptedPassPhrase;
                cmdActivePlayers.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
                SqlDataAdapter daActivePlayers = new SqlDataAdapter(cmdActivePlayers);
                DataTable dtActivePlayers = new DataTable();
                daActivePlayers.Fill(dtActivePlayers);
                for (int i = 0; i < dtActivePlayers.Rows.Count; i++)
                {

                    Player playerOnMachine = new Player(-1, 
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["card_id"].ToString()),
                                                        dtActivePlayers.Rows[i]["cardNumber"].ToString(), 
                                                        dtActivePlayers.Rows[i]["customer_name"].ToString(),
                                                        "",
                                                        dtActivePlayers.Rows[i]["userName"].ToString(),
                                                        "",
                                                        DateTime.Parse("01/01/1900"),
                                                        DateTime.Parse("01/01/1900"),
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["playTime"]),
                                                        -1,
                                                        -1,
                                                        -1,
                                                        0,
                                                        "",
                                                        0);
                    playerOnMachine.SetupPlayerPlayTimes(
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["gameplay_id"].ToString()),            
                                                        Convert.ToDateTime(dtActivePlayers.Rows[i]["play_date"].ToString()),
                                                        Convert.ToDateTime(dtActivePlayers.Rows[i]["GameEndTime"].ToString()),
                                                        dtActivePlayers.Rows[i]["pauseStatus"].ToString(),
                                                        Convert.ToDateTime(dtActivePlayers.Rows[i]["PauseStartTime"].ToString()),
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["TotalPauseTime"].ToString()));

                    GameMachine playerGameMachine = gameMachinesList.IdentifyGameMachine(Convert.ToInt32(dtActivePlayers.Rows[i]["machine_id"].ToString()));
                    if (playerGameMachine != null)
                        playerGameMachine.BeingPlayedUpon(playerOnMachine);                                                        
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }
        public void activatePlayers(int totalActivePlayerPerMachine)
        {
            log.LogMethodEntry(totalActivePlayerPerMachine);
            int playerCountActivated;
            for (int i = 0; i < gameMachinesList.TotalEntries; i++)
            {
                int gameMachineId = gameMachinesList.RetrieveGameMachine(i).GameMachineId;
                log.LogVariableState("gameMachineId", gameMachineId);
                GameMachine currGameMachine = gameMachinesList.IdentifyGameMachine(gameMachineId);
                playerCountActivated = 0;
                for (int j = 0; (j < playersList.TotalEntries); j++)
                {
                    Player currentPlayer = playersList.GetPlayer(j);
                    log.LogVariableState("currentPlayer", currentPlayer);
                    if (currentPlayer.GameMachineAssigned == gameMachineId)
                    {
                        if (currGameMachine.IsActive == true)
                        {
                            if (currentPlayer.GroupWaitNumber == 0)
                            {
                                if (playerCountActivated < totalActivePlayerPerMachine)
                                {
                                    playerCountActivated++;
                                    if (currentPlayer.PrintCount == 0)
                                        playersList.PrintTokens(currentPlayer.QueueEntryId, false);
                                }
                                else
                                {
                                    if (currentPlayer.PrintCount != 0)
                                        playersList.UndoPrintTokens(currentPlayer.QueueEntryId, false);
                                }
                            }
                        }
                        else
                        {
                            if (currentPlayer.PrintCount != 0)
                                playersList.UndoPrintTokens(currentPlayer.QueueEntryId, false);
                        }

                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generates the customer queue
        /// </summary>
        public string generateCustomerQueue()
        {
            log.LogMethodEntry();
            int machineAssigned = 0;
            int waitTime = 0;
            int groupWaitTime = 0;
            int lastGroupWaitTime = 0;
            int groupWaitNumberLocal = 0;
            string custname = string.Empty;
            int totalBufferTimeToAdd = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_BUFFER_TIME")) + Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_SETUP_TIME"));
            int queueMaxEntries = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_MAX_ENTRIES"));

            int lastCustomerMachineAssigned = 0;
            string lastCustomerTeamName = string.Empty;
            string lastCustomerGameName = string.Empty;

            string errorMessage = string.Empty;

            try
            {
                SqlCommand cmdCustQueue = Common.Utilities.getCommand();
                cmdCustQueue.CommandText = @"select x.Custqueue_id customerQueueId,
                                                        x.play_request playrequest, 
                                                        dateadd(ss, isnull(x.advancedtime,0)*-1, x.play_request) playRequestAdvanced,
                                                        x.manual_override manual_override, 
                                                        x.manual_override_time, 
                                                        x.machine_id_assigned machine_assigned, 
                                                        g.game_id game_id, 
                                                        g.game_name game_name, 
                                                        x.promised_wait promised_wait, 
                                                        isnull(y.OptionalAttribute,0) playTime,
                                                        isnull(y.EntitlementType,'TIME') playType,
                                                        isnull(y.Quantity,0) playQuantity,
                                                        isnull(
                                                            (select MIN(DateAdd(ss,isnull(sx.advancedtime,0)*-1,sx.play_request))
                                                               from customerqueue sx, 
                                                                    CardGames sy, 
                                                                    games sg, 
                                                                    cards sz
                                                              where sx.card_game_id = sy.card_game_id
                                                                 and sx.play_request > DATEADD(mi,@minutesAddParam,getdate())
                                                                 and sz.card_id = sx.card_id
                                                                 and sz.valid_flag = 'Y'
                                                                 and sx.customer_group_name = x.customer_group_name
                                                                 --and sx.custqueue_id != x.custqueue_id
                                                                 and sy.game_id = sg.game_id
                                                                 and sg.game_id = g.game_id), 
                                                        dateadd(ss, isnull(x.advancedtime,0)*-1, x.play_request)) minPR,
                                                        isnull(manual_override_time, 
                                                                DATEADD(dd,10,getdate())) final_manual_override_time,
                                                        z.card_number cardNumber,
                                                        z.card_id card_id, 
                                                        q.customer_id customer_id, 
                                                        q.customer_name customer_name, 
                                                        x.customer_group_name teamName,
                                                        x.promised_wait promisedWait,
                                                        isnull(x.print_count,0) printCount,
                                                        q.username userName,
                                                        isnull(x.advancedtime,0) advancedTime,
                                                        isnull(x.customer_intimated,0) customerintimated
                                                from customerqueue x, 
                                                    CardGames y, 
                                                    games g, 
                                                    CustomerView(@PassPhrase) q,
                                                    cards z
                                               where x.card_game_id = y.card_game_id
                                                 and ((x.play_complete = 0) or (x.play_complete = 3))
                                                 and y.BalanceGames != 0
                                                 and x.play_request > DATEADD(mi,@minutesAddParam,getdate())
                                                 and z.valid_flag = 'Y'
                                                 and z.card_id = x.card_id
                                                 and z.customer_id = q.customer_id
                                                 and g.game_id = y.game_id
                                                order by final_manual_override_time, minPR, playRequestAdvanced";
                log.Debug("cmdCustQueue: " + cmdCustQueue);
                cmdCustQueue.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));
                string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                string passPhrase = encryptedPassPhrase;
                cmdCustQueue.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
                SqlDataAdapter daCustQueue = new SqlDataAdapter(cmdCustQueue);
                DataTable dtCustQueue = new DataTable();
                daCustQueue.Fill(dtCustQueue);

                playersList = new PlayersList(dtCustQueue.Rows.Count);
                GameMachine playerGameMachine;

                for (int i = 0; i < dtCustQueue.Rows.Count; i++)
                {
                    playerGameMachine = null;
                    DateTime manualOverrideTime;
                    int playTimeTotal = 0;
                    if (dtCustQueue.Rows[i]["manual_override_time"] == DBNull.Value)
                        manualOverrideTime = DateTime.Parse("01/01/1900");
                    else
                        manualOverrideTime = Convert.ToDateTime(dtCustQueue.Rows[i]["manual_override_time"].ToString());
                    if (dtCustQueue.Rows[i]["playType"].ToString().CompareTo("OVERS") == 0)
                        playTimeTotal = Convert.ToInt32(dtCustQueue.Rows[i]["playTime"].ToString());
                    else
                        playTimeTotal = Convert.ToInt32(dtCustQueue.Rows[i]["playQuantity"].ToString());
                    Player playerOnMachine = new Player(Convert.ToInt32(dtCustQueue.Rows[i]["customerQueueId"].ToString()), 
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["card_id"].ToString()),
                                                        dtCustQueue.Rows[i]["cardNumber"].ToString(),
                                                        dtCustQueue.Rows[i]["customer_name"].ToString(),
                                                        dtCustQueue.Rows[i]["teamName"].ToString(),
                                                        dtCustQueue.Rows[i]["userName"].ToString(),
                                                        dtCustQueue.Rows[i]["game_name"].ToString(),
                                                        Convert.ToDateTime(dtCustQueue.Rows[i]["playrequest"].ToString()),
                                                        manualOverrideTime,
                                                        playTimeTotal,
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["promisedWait"]),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["printCount"]),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["advancedTime"]),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["customerintimated"]),
                                                        dtCustQueue.Rows[i]["playType"].ToString(),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["playQuantity"]));

                    if ((dtCustQueue.Rows[i]["manual_override"] != DBNull.Value) && (string.Compare(dtCustQueue.Rows[i]["manual_override"].ToString(),"Y") == 0))
                    {
                        int machineToSearch = Convert.ToInt32(dtCustQueue.Rows[i]["machine_assigned"]);
                        playerGameMachine = gameMachinesList.IdentifyGameMachine(machineToSearch);
                        if ((playerGameMachine != null) && (playerGameMachine.IsActive == true))
                        {
                            Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(machineToSearch);
                            if (lastPlayerOnMachine == null)
                            {
                                if ((playerGameMachine.CurrentPlayer() != null) && (playerGameMachine.CurrentPlayer().GameTime > 0))
                                    waitTime = playerGameMachine.CurrentPlayer().GameTime;
                                else
                                    waitTime = 0;
                            }
                            else
                                waitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;
                            groupWaitTime = waitTime;
                            groupWaitNumberLocal = 0;
                        }
                        else
                        {
                            waitTime = (9999 * 60);
                            errorMessage = errorMessage + dtCustQueue.Rows[i]["customer_name"].ToString().ToUpper() + " was manually assigned to lane. Now lane is inactive, please assign to active lane" + "\n";
                        }
                    }
                    else if ((dtCustQueue.Rows[i]["teamName"] != DBNull.Value) && (string.Compare(lastCustomerTeamName, dtCustQueue.Rows[i]["teamName"].ToString()) == 0) &&
                            (dtCustQueue.Rows[i]["game_name"] != DBNull.Value) && (string.Compare(lastCustomerGameName, dtCustQueue.Rows[i]["game_name"].ToString()) == 0))
                    {
                        groupWaitTime = lastGroupWaitTime;
                        groupWaitNumberLocal = groupWaitNumberLocal + 1;
                        playerGameMachine = gameMachinesList.IdentifyGameMachine(lastCustomerMachineAssigned);
                        Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(lastCustomerMachineAssigned);
                        if (playerGameMachine != null)
                        {
                            if (lastPlayerOnMachine == null)
                            {
                                if ((playerGameMachine.CurrentPlayer() != null) && (playerGameMachine.CurrentPlayer().GameTime > 0))
                                    waitTime = playerGameMachine.CurrentPlayer().GameTime;
                                else
                                    waitTime = 0;
                            }
                            else
                                waitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;
                        }
                    }
                    else
                    {
                        bool teamNameValid = false;
                        if (dtCustQueue.Rows[i]["teamName"] != DBNull.Value)
                        {
                            SqlCommand cmdGamePlayerOfTeam = Common.Utilities.getCommand();
                            cmdGamePlayerOfTeam.CommandText = @"select gmp.machine_id machineId, gmp.play_date machine_id
                                                                  from gameplay gmp, customerqueue cq, machines m, games g
                                                                 where gmp.play_date > DATEADD(mi,@minutesAddParam,getdate())
                                                                   and gmp.card_id = cq.card_id
                                                                   and cq.customer_group_name = @teamName
                                                                   and gmp.machine_id = m.machine_id
                                                                   and m.game_id = g.game_id
                                                                   and g.game_name = @gameName
                                                                 order by gmp.play_date desc";
                            cmdGamePlayerOfTeam.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));                                                 
                            cmdGamePlayerOfTeam.Parameters.AddWithValue("teamName", dtCustQueue.Rows[i]["teamName"].ToString());
                            cmdGamePlayerOfTeam.Parameters.AddWithValue("gameName", dtCustQueue.Rows[i]["game_name"].ToString());
                            SqlDataAdapter daTeamDetailsQueue = new SqlDataAdapter(cmdGamePlayerOfTeam);
                            DataTable dtTeamDetailsQueue = new DataTable();
                            daTeamDetailsQueue.Fill(dtTeamDetailsQueue);

                            if (dtTeamDetailsQueue.Rows.Count > 0)
                            {
                                teamNameValid = true;
                                int machineToSearch = Convert.ToInt32(dtTeamDetailsQueue.Rows[0]["machineId"].ToString());
                                playerGameMachine = gameMachinesList.IdentifyGameMachine(machineToSearch);

                                if (playerGameMachine != null)
                                {
                                    groupWaitTime = 0;
                                    groupWaitNumberLocal = 0;
                                    Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(machineToSearch);
                                    if (lastPlayerOnMachine == null)
                                    {
                                        if ((playerGameMachine.CurrentPlayer() != null) && (playerGameMachine.CurrentPlayer().GameTime > 0))
                                            waitTime = playerGameMachine.CurrentPlayer().GameTime;
                                        else
                                            waitTime = 0;
                                    }
                                    else
                                    {
                                        waitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;

                                    }
                                }
                            }

                            if (playerGameMachine == null)
                            {
                                SqlCommand cmdCheckManualOverride = Common.Utilities.getCommand();
                                cmdCheckManualOverride.CommandText = @"select machine_id_assigned machineId
                                                                         from customerqueue custqueue
                                                                        where custqueue.play_request > DATEADD(mi,@minutesAddParam,getdate())
	                                                                      and custqueue.manual_override = 'Y'
                                                                          and custqueue.customer_group_name = @teamName
                                                                        order by custqueue.play_request desc";
                                cmdCheckManualOverride.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));                                                 
                                cmdCheckManualOverride.Parameters.AddWithValue("teamName", dtCustQueue.Rows[i]["teamName"].ToString());
                                SqlDataAdapter daCheckManualOverride = new SqlDataAdapter(cmdCheckManualOverride);
                                DataTable dtCheckManualOverride = new DataTable();
                                daCheckManualOverride.Fill(dtCheckManualOverride);

                                if (dtCheckManualOverride.Rows.Count > 0)
                                {
                                    teamNameValid = true;
                                    int machineToSearch = Convert.ToInt32(dtCheckManualOverride.Rows[0]["machineId"].ToString());
                                    playerGameMachine = gameMachinesList.IdentifyGameMachine(machineToSearch);
                                    if (playerGameMachine != null)
                                    {
                                        groupWaitTime = 0;
                                        groupWaitNumberLocal = 0;
                                        Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(machineToSearch);
                                        if (lastPlayerOnMachine == null)
                                        {
                                            if ((playerGameMachine.CurrentPlayer() != null) && (playerGameMachine.CurrentPlayer().GameTime > 0))
                                                waitTime = playerGameMachine.CurrentPlayer().GameTime;
                                            else
                                                waitTime = 0;
                                        }
                                        else
                                            waitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;
                                    }
                                }
                            }
                        }
                        if (teamNameValid == false)
                        {
                            if ((dtCustQueue.Rows[i]["machine_assigned"] != DBNull.Value) &&
                               ((isDigitalTokenEnabled == false) || ((isDigitalTokenEnabled == true) && (playerOnMachine.CustomerIntimated == true))))
                            {
                                int machineToSearch = Convert.ToInt32(dtCustQueue.Rows[i]["machine_assigned"].ToString());
                                playerGameMachine = gameMachinesList.IdentifyGameMachine(machineToSearch);
                                if (playerGameMachine != null)
                                {
                                    if (playerGameMachine.ActiveFlag.CompareTo("N") == 0)
                                    {
                                        waitTime = (9999*60);
                                        errorMessage = errorMessage + "Token printed but queue made inactive for " + dtCustQueue.Rows[i]["customer_name"].ToString().ToUpper() + "\n";
                                    }
                                    else
                                    {
                                        Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(machineToSearch);
                                        if (lastPlayerOnMachine == null)
                                        {
                                            if ((playerGameMachine.CurrentPlayer() != null) && (playerGameMachine.CurrentPlayer().GameTime > 0))
                                                waitTime = playerGameMachine.CurrentPlayer().GameTime;
                                            else
                                                waitTime = 0;
                                        }
                                        else
                                            waitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;
                                    }
                                }
                                else
                                {
                                    waitTime = (9999*60);
                                    errorMessage = errorMessage + "Token printed but queue made inactive for " + dtCustQueue.Rows[i]["customer_name"].ToString().ToUpper() + "\n";
                                }
                                groupWaitTime = waitTime;
                                groupWaitNumberLocal = 0;

                            }
                            else
                            {
                                waitTime = 9999;
                                shortestWaitMachine(dtCustQueue.Rows[i]["game_name"].ToString(), ref machineAssigned, ref waitTime);
                                playerGameMachine = gameMachinesList.IdentifyGameMachine(machineAssigned);
                                groupWaitTime = waitTime;
                                groupWaitNumberLocal = 0;
                            }
                        }

                    }
                    if (playerGameMachine == null)
                    {
                        errorMessage = errorMessage + "Failed to assign queue to " + dtCustQueue.Rows[i]["customer_name"].ToString().ToUpper() + Environment.NewLine + "Please Check whether the lanes associated with " +dtCustQueue.Rows[i]["game_name"].ToString() + " are active"; ;
                    }
                    else
                    {
                        if (playersList.TotalEntries >= queueMaxEntries)
                        {
                            log.LogMethodExit("Queue has maxed out ");
                            return "Queue has maxed out ";
                        }
                        else
                        {

                            playerOnMachine.GameMachineAssigned = playerGameMachine.GameMachineId;
                            playerOnMachine.GameMachineAssignedSerialNumber = playerGameMachine.GameMachineSerialNumber;
                            playerOnMachine.SetupWaitDetails(
                                                                Convert.ToInt32(dtCustQueue.Rows[i]["customer_id"]),
                                                                Convert.ToString(dtCustQueue.Rows[i]["TeamName"]),
                                                                (waitTime + totalBufferTimeToAdd), 
                                                                groupWaitTime,
                                                                groupWaitNumberLocal,
                                                                playerGameMachine.GameNameOfMachine,
                                                                playerGameMachine.LaneName);
                            playersList.AddPlayer(playerOnMachine);
                            lastCustomerMachineAssigned = playerGameMachine.GameMachineId; ;
                            lastCustomerTeamName = dtCustQueue.Rows[i]["TeamName"].ToString();
                            lastCustomerGameName = dtCustQueue.Rows[i]["game_name"].ToString();
                            lastGroupWaitTime = groupWaitTime;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit("GenerateCustomerQueue " + custname + Environment.NewLine + ex.Message);
                return "GenerateCustomerQueue " + custname+Environment.NewLine + ex.Message;
            }
            log.LogMethodExit(errorMessage);
            return errorMessage;
        }
        /// <summary>
        /// This event is fired when a card is issued to the customer
        /// </summary>
        public void OnTransactionEvent()
        {
            log.LogMethodEntry();
            int promiseWaitTime = 9999;
            int queueIdShortest = -1;
          //  MessageBox.Show(DateTime.Now.ToString());
            try
            {
                SqlCommand cmdTransaction = Common.Utilities.getCommand();
                cmdTransaction.CommandText = @"select c.card_id card_id, cg.card_game_id card_game_id, cg.last_update_date last_update_date,
                                                      g.game_name game_name, c.customer_id customerID,
                                                        isnull(cg.OptionalAttribute,0) playTime,
                                                        isnull(cg.EntitlementType,'TIME') playType,
                                                        isnull(cg.Quantity,0) playQuantity
                                                 from cardgames cg, cards c, games g, CustomerView(@PassPhrase) cu
                                                where cg.last_update_date > DATEADD(dd,-1,getdate())
                                                  and dbo.GetGameProfileValue(g.game_Id, 'QUEUE_SETUP_REQUIRED') = '1'
                                                  and cg.card_id = c.card_id
                                                  and g.game_id = cg.game_id
                                                  and c.customer_id = cu.customer_id
                                                  and c.valid_flag = 'Y'
                                                  and c.technician_card = 'N'
                                                  and isnull(cg.BalanceGames,0)!=0 
                                                  and not exists
                                                    (select cusub.card_game_id
                                                       from customerqueue cusub
                                                      where cg.card_game_id=cusub.card_game_id)";
                log.Debug("cmdTransaction: " + cmdTransaction);
                string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                string passPhrase = encryptedPassPhrase;
                cmdTransaction.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
                SqlDataAdapter daTransaction = new SqlDataAdapter(cmdTransaction);
                DataTable dtTransaction = new DataTable();
                daTransaction.Fill(dtTransaction);
                if (dtTransaction.Rows.Count > 0)
                {

                    SqlCommand cmdCustomerQueue = Common.Utilities.getCommand();

                    cmdCustomerQueue.CommandText = @"Insert into customerqueue (play_request, promised_wait,
                                                        manual_override, manual_override_time, machine_id_assigned, card_id,
                                                        card_game_id, customer_group_name, last_update_date, last_update_by, play_complete) 
                                                        values (@play_request, @promised_wait, 
                                                        @manual_override, @manual_override_time, @machine_id_assigned, @card_id,  
                                                        @card_game_id, null, @last_update_date, @last_update_by, 0)";

                    log.Debug("cmdCustomerQueue: " + cmdCustomerQueue);
                    SqlCommand cmdCustomerQueueCheck = Common.Utilities.getCommand();
                    cmdCustomerQueueCheck.CommandText = "select count(*) from customerqueue where card_game_id = @cardGameId";
                    for (int i = 0; i < dtTransaction.Rows.Count; i++)
                    {
                        int playTimeTotal = 0;
                        if (dtTransaction.Rows[i]["playType"].ToString().CompareTo("OVERS") == 0)
                            playTimeTotal = Convert.ToInt32(dtTransaction.Rows[i]["playTime"].ToString());
                        else
                            playTimeTotal = Convert.ToInt32(dtTransaction.Rows[i]["playQuantity"].ToString());
                        log.LogVariableState("playTimeTotal", playTimeTotal);
                        cmdCustomerQueueCheck.Parameters.Clear();
                        cmdCustomerQueueCheck.Parameters.AddWithValue("@cardGameId", dtTransaction.Rows[i]["card_game_id"]);
                        int countOfCardGameRows = (Int32) cmdCustomerQueueCheck.ExecuteScalar();
                        if (countOfCardGameRows == 0)
                        {
                            log.Debug("customer queue is greater than 0 for cardgameid: " + dtTransaction.Rows[i]["card_game_id"]);
                            cmdCustomerQueue.Parameters.Clear();
                            cmdCustomerQueue.Parameters.AddWithValue("@play_request", dtTransaction.Rows[i]["last_update_date"]);
                            shortestWaitMachine(dtTransaction.Rows[i]["game_name"].ToString(), ref queueIdShortest, ref promiseWaitTime);
                            cmdCustomerQueue.Parameters.AddWithValue("@promised_wait", promiseWaitTime);
                            cmdCustomerQueue.Parameters.AddWithValue("@manual_override", DBNull.Value);
                            cmdCustomerQueue.Parameters.AddWithValue("@manual_override_time", DBNull.Value);
                            cmdCustomerQueue.Parameters.AddWithValue("@machine_id_assigned", DBNull.Value);
                            //cmdCustomerQueue.Parameters.AddWithValue("@customer_group_name", dtTransaction.Rows[i]["group_name"]);
                            cmdCustomerQueue.Parameters.AddWithValue("@card_id", dtTransaction.Rows[i]["card_id"]);
                            cmdCustomerQueue.Parameters.AddWithValue("@card_game_id", dtTransaction.Rows[i]["card_game_id"]);
                            cmdCustomerQueue.Parameters.AddWithValue("@last_update_date", dtTransaction.Rows[i]["last_update_date"]);
                            cmdCustomerQueue.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                            cmdCustomerQueue.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
           // MessageBox.Show(DateTime.Now.ToString());
        }
        //This event is fired when the lane allocator manipulates a lane(takes it offline/online)
        public void updateActiveLanes(int machineIdPassed, char activeFlagPassed)
        {
            log.LogMethodEntry(machineIdPassed, activeFlagPassed);
            GameMachine playerGameMachine = gameMachinesList.IdentifyGameMachine(machineIdPassed);
            if (playerGameMachine != null)
                playerGameMachine.SetStatusOfMachine(activeFlagPassed);
            log.LogMethodExit();
        }         

        //This method is called when the user removes a player from the queue
        public void updateCustomerQueuePlayerRemoval(int customerQueueIdPassed)
        {
            log.LogMethodEntry(customerQueueIdPassed);
            playersList.RemovePlayer(customerQueueIdPassed);
            log.LogMethodExit();        
        }
        // This method is used when the user advances a player by x minutes
        public void updateCustomerAdvancedTime(int advancedTime, int customerQueueIdPassed)
        {
            log.LogMethodEntry(advancedTime, customerQueueIdPassed);
            playersList.AdvancePlayer(customerQueueIdPassed, advancedTime);
            log.LogMethodExit();
        }

        //This method is mainly used to determine the shortest wait time of a game queue
        public void shortestWaitMachine(string gameNameToSearch, ref int queueIdShortest, ref int waitTimeShortest)
        {
            log.LogMethodEntry(gameNameToSearch, queueIdShortest, waitTimeShortest);
            int waitTime = 999999;
            int tempWaitTime = 999999;
            GameMachine shortestWaitTimeGameMachine = null;
            try
            {
                for (int j = 0; j < gameMachinesList.TotalEntries; j++)
                {
                    bool gameMachineFound = false;
                    GameMachine currentGameMachine = gameMachinesList.RetrieveGameMachine(j);
                    if ((currentGameMachine != null) && (currentGameMachine.IsActive == true))
                    {
                        if (string.Compare(currentGameMachine.GameNameOfMachine, gameNameToSearch) == 0)
                            gameMachineFound = true;
                        else
                        {
                            if (currentGameMachine.IsCompatibleWith(gameNameToSearch) == true)
                                gameMachineFound = true;
                        }
                        if (gameMachineFound == true)
                        {
                            Player lastPlayerOnMachine = playersList.CheckLastPlayerOnMachine(currentGameMachine.GameMachineId);
                            if (lastPlayerOnMachine == null)
                            {
                                if ((currentGameMachine.CurrentPlayer() != null) && (currentGameMachine.CurrentPlayer().GameTime > 0))
                                    tempWaitTime = currentGameMachine.CurrentPlayer().GameTime;
                                else
                                    tempWaitTime = 0;
                            }
                            else
                                tempWaitTime = lastPlayerOnMachine.WaitTime + lastPlayerOnMachine.GamePlayTime;
                            if (tempWaitTime < waitTime)
                            {
                                waitTime = tempWaitTime;
                                shortestWaitTimeGameMachine = currentGameMachine;
                            }
                        }
                    }
                }
                if (shortestWaitTimeGameMachine != null)
                    queueIdShortest = shortestWaitTimeGameMachine.GameMachineId;
                else
                    queueIdShortest = -1;
                waitTimeShortest = waitTime;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw (ex);
            }
        }


        //This method is used when the user moves a player to the priority queue
        public int moveToPriority(int customerQueueIdPassed, string loginID, ref string errorMessage)
        {
            log.LogMethodEntry(customerQueueIdPassed, loginID);
            string gameNameToSearch = "PRIORITY";
            int machineAssigned = -1;
            int waitTimeShortest = 9999;
            int returnCode = 0;
            
            shortestWaitMachine(gameNameToSearch, ref  machineAssigned, ref waitTimeShortest);
            GameMachine playerGameMachine = gameMachinesList.IdentifyGameMachine(machineAssigned);

            if (machineAssigned == -1)
            {
                errorMessage = "Could not assign any machine";
                log.Debug(errorMessage);
                returnCode = -1;
            }
            else if (waitTimeShortest != 0)
            {
                errorMessage = "Queue is occupied, cannot make the move";
                log.Debug(errorMessage);
                returnCode = -2;
            }
            else
            {
                playersList.ShiftPlayer(customerQueueIdPassed, playerGameMachine.GameMachineId, playerGameMachine.GameNameOfMachine);
            }
            log.LogMethodExit(returnCode);
            return returnCode;
        }
        /// <summary>
        /// This event is fired when the lane allocator uses the pause feature
        /// </summary>
        /// <param name="gameplayID"></param>
        public int pause(int gameMachineIdPassed,  ref string errorMessage)
        {
            log.LogMethodEntry(gameMachineIdPassed);
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.PauseMachine(ref errorMessage);
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }
        // This event fires when the lane allocator unpauses the machine
        public int unPause(int gameMachineIdPassed, ref string errorMessage)
        {
            log.LogMethodEntry(gameMachineIdPassed);
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.RestartMachine(ref errorMessage);
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }
        public int GameEnd(int gameMachineIdPassed, ref string errorMessage)
        {
            log.LogMethodEntry(gameMachineIdPassed);
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.EndGame(ref errorMessage);
            log.LogMethodExit(statusCheck);
            return statusCheck;

        }
        public void updateGroupName(string newGroupName, int queueId)
        {
            log.LogMethodEntry(newGroupName, queueId);
            playersList.UpdateTeamName(queueId, newGroupName);
            log.LogMethodExit();
        }

    }
    public class Players
    {
        int gamePlayPastTime;
        // Utilities parafaitPlayersUtility=new Utilities();
        // AuditManager auditMgr;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Players()
        {
            log.LogMethodEntry();
            gamePlayPastTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("GAMEPLAY_END_WAIT_TIME"));
           // auditMgr = new AuditManager();
            Common.ParafaitEnv.Initialize();
            log.LogMethodExit();
        }

        public DataTable GetRemovedExpiredPlayers(DateTime fromDate, DateTime toDate, string cardNumber, string custName, bool isRemoved, bool isExpired)
        {
            log.LogMethodEntry(fromDate, toDate, cardNumber, custName, isRemoved, isExpired);
            SqlCommand cmdGetRemovedExpiredPlayers = Common.Utilities.getCommand();
            cmdGetRemovedExpiredPlayers.CommandText = @"select 'Removed' Status, cu.customer_name Name, c.card_number 'Card Number', cq.play_request 'Play Request', cq.custqueue_id 'QueueId'
                                   from customerqueue cq, cards c, CustomerView(@PassPhrase) cu
                                  where cq.play_complete = 2
                                    and cq.card_id = c.card_id
                                    and c.customer_id = cu.customer_id
                                    and c.valid_flag = 'Y'
                                    and cq.play_request between @fromDate and @toDate
                                    and c.card_number like UPPER('%' + @cardId + '%')
                                    and UPPER(cu.customer_name) like UPPER('%' + @customerName + '%')
                                    and cq.play_complete = @playCompleteFlag
                                  union
                                 select 'Expired' Status , cu.customer_name Name, c.card_number 'Card Number', cq.play_request 'Play Request', cq.custqueue_id 'QueueId'
                                   from customerqueue cq, cards c, CustomerView(@PassPhrase) cu, CardGames cg
                                  where cq.card_id = c.card_id
                                    and c.customer_id = cu.customer_id
                                    and cq.play_request between @fromDate and @toDate
                                    and cq.play_request <= DATEADD(mi,@minutesToAddParam,getdate())
                                    and c.card_number like UPPER('%' + @cardId + '%')
                                    and c.valid_flag = 'Y'
                                    and UPPER(cu.customer_name) like UPPER('%' + @customerName + '%')
                                    and cq.play_complete != 1
                                    and cq.card_game_id = cg.card_game_id
                                    and cg.BalanceGames >= @balanceGames";
            string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;
            cmdGetRemovedExpiredPlayers.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));

            cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@minutesToAddParam", (-1*gamePlayPastTime));
            cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@customerName", custName);
            if ((custName.CompareTo("") != 0) || (cardNumber.CompareTo("") != 0))
            {
                fromDate = Convert.ToDateTime((SqlDateTime.MinValue.ToString()));
                toDate = Convert.ToDateTime((SqlDateTime.MaxValue.ToString()));
            }
            cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@fromDate", fromDate);
            cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@toDate", toDate);
            cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@cardId", cardNumber);
            if (isRemoved == true)
                cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@playCompleteFlag", 2);
            else
                cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@playCompleteFlag", 1);

            if (isExpired == true)
                cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@balanceGames", 1);
            else
                cmdGetRemovedExpiredPlayers.Parameters.AddWithValue("@balanceGames", 9999);

            SqlDataAdapter daGetRemovedExpiredPlayers = new SqlDataAdapter(cmdGetRemovedExpiredPlayers);
            DataTable dtGetRemovedExpiredPlayers = new DataTable();
            daGetRemovedExpiredPlayers.Fill(dtGetRemovedExpiredPlayers);
            int countOfPlayers = dtGetRemovedExpiredPlayers.Rows.Count;

            log.LogMethodExit(dtGetRemovedExpiredPlayers);
            return dtGetRemovedExpiredPlayers;
            
        }
        public void ReInstate(int customerQueueId)
        {
            log.LogMethodEntry(customerQueueId);
            try
            {
                SqlCommand cmdReinstatePlayers = Common.Utilities.getCommand();
                cmdReinstatePlayers.CommandText = @"update customerqueue 
                                                        set play_request = getdate(),
                                                            last_update_by = @loginId,
                                                            last_update_date = getdate(),
                                                            play_complete = 3,
                                                            manual_override = null,
                                                            machine_id_assigned = null,
                                                            advancedtime = null,
                                                            print_count = null,
                                                            customer_intimated = null
                                                     where custqueue_id = @customerQueueId";
                cmdReinstatePlayers.Parameters.AddWithValue("@loginId", Common.ParafaitEnv.LoginID);
                cmdReinstatePlayers.Parameters.AddWithValue("@customerQueueId", customerQueueId);

                cmdReinstatePlayers.ExecuteNonQuery();
                //.logAction("ReInstate", "Customer", customerQueueId, DBNull.Value.ToString(), Common.ParafaitEnv.LoginID);
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "ReInstate", 0, customerQueueId.ToString(), DBNull.Value.ToString(), Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }
    }
}
