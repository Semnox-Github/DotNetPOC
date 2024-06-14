/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - GameMachine 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

namespace ParafaitQueueManagement
{
    public class GameMachine
    {
        private int machineId;
        private string machineSerialNumber;
        private string machineName;
        private string gameName;
        private bool isActive;
        private Player currentPlayer;
        private int serialNumber;
       // Utilities parafaitUtility = new Utilities();
        public AuditManager auditMgr;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //  ParafaitEnv parafaitEnv=Para

        public GameMachine(int machineIdPassed, string machineNamePassed, string gameNamePassed, string isActivePassed, string serialNumberPassed)
        {
            log.LogMethodEntry(machineIdPassed, machineNamePassed, gameNamePassed);
            Common.ParafaitEnv.Initialize();
            machineId = machineIdPassed;
            machineName = machineNamePassed;
            machineSerialNumber = serialNumberPassed;
            gameName = gameNamePassed;
            if (String.Compare(isActivePassed, "Y") == 0)
                isActive = true;
            else
                isActive = false;
            log.LogVariableState("isActive", isActive);
            currentPlayer = null;
            bool parseSerialNumStatus = int.TryParse(serialNumberPassed, out serialNumber);
            if (!parseSerialNumStatus)
                serialNumber = -1;
            log.LogVariableState("serialNumber", serialNumber);
            //auditMgr = new AuditManager();
            log.LogMethodExit();
        }
        public void BeingPlayedUpon(Player playerPassed)
        {
            log.LogMethodEntry(playerPassed);
            currentPlayer = playerPassed;
            log.LogMethodExit();
        }
        public Player CurrentPlayer()
        {
            log.LogMethodEntry();
            log.LogMethodExit(currentPlayer);
            return currentPlayer;
        }
        public void ClearCurrentPlayer()
        {
            log.LogMethodEntry();
            currentPlayer = null;
            log.LogMethodExit();
        }

        public void SetStatusOfMachine(char machineStatusFlag)
        {
            log.LogMethodEntry(machineStatusFlag);
            string userName = Common.ParafaitEnv.LoginID;
            try
            {
                if (machineStatusFlag == 'Y')
                    isActive = true;
                else
                    isActive = false;
                SqlCommand cmdMachineStatus = Common.Utilities.getCommand();
                cmdMachineStatus.CommandText = @"update machines 
                                                    set active_flag=@activeflagstatus,
                                                        last_updated_user=@last_updated_user,
                                                        last_updated_date=@last_updated_date 
                                                  where machine_id=@machine_id";
                cmdMachineStatus.Parameters.AddWithValue("@activeflagstatus", machineStatusFlag);
                cmdMachineStatus.Parameters.AddWithValue("@machine_id", machineId);
                cmdMachineStatus.Parameters.AddWithValue("@last_updated_user", userName);
                cmdMachineStatus.Parameters.AddWithValue("@last_updated_date", Common.Utilities.getServerTime());

                log.LogVariableState("@activeflagstatus", machineStatusFlag);
                log.LogVariableState("@machine_id", machineId);
                log.LogVariableState("@last_updated_user", userName);
                cmdMachineStatus.ExecuteNonQuery();
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Machine", "", "Lane Activate", 0, machineId.ToString(), machineStatusFlag.ToString(), userName, Common.ParafaitEnv.POSMachine, null);
                //auditMgr.logAction("Lane Activate", "Machine", machineId, machineStatusFlag.ToString(), userName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            log.LogMethodExit();
        }

        public bool IsCompatibleWith(string gameNamePassed)
        {
            log.LogMethodEntry(gameNamePassed);
           // System.Windows.Forms.MessageBox.Show(gameName, gameNamePassed);
            SqlCommand cmdFetchCompatibilities = Common.Utilities.getCommand();
            cmdFetchCompatibilities.CommandText = @"select lookupvalue, description 
                            from LookupValues lv, lookups l
                            where lv.lookupid = l.lookupid
                            and l.lookupName = 'QUEUE_COMPATIBILITY'
                            and lookupvalue = @currentGameMachine 
                            and description = @toCheckGameMachine";
            cmdFetchCompatibilities.Parameters.AddWithValue("currentGameMachine", gameName);
            cmdFetchCompatibilities.Parameters.AddWithValue("toCheckGameMachine", gameNamePassed);
            SqlDataAdapter daGameCompatibility = new SqlDataAdapter(cmdFetchCompatibilities);
            DataTable dtGameCompatibility = new DataTable();
            daGameCompatibility.Fill(dtGameCompatibility);
            if (dtGameCompatibility.Rows.Count == 1)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        public int PauseMachine(ref string retMessage)
        {
            log.LogMethodEntry();
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = Common.ParafaitEnv.LoginID;
            int statusCheck;

            if (currentPlayer == null)
            {
                retMessage = "No player on the game machine";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-1);
                return -1;
            }
            if (gameplayID == -1)
            {
                retMessage = "No game active on the game machine";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-1);
                return -1;
            }
            try
            {
                SqlCommand cmdcheckGamePlayIdExists = Common.Utilities.getCommand();
                cmdcheckGamePlayIdExists.CommandText = @"select isnull(isPaused,'N') PauseCheck, PauseStartTime , GameEndTime
                                                           from gameplayinfo 
                                                          where gameplay_id=@gameplayid";
                cmdcheckGamePlayIdExists.Parameters.AddWithValue("@gameplayid", gameplayID);
                log.LogVariableState("@gameplayid", gameplayID);
                SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdcheckGamePlayIdExists);
                DataTable dtGamePlayInfo = new DataTable();
                daGamePlayInfo.Fill(dtGamePlayInfo);
                if (dtGamePlayInfo.Rows.Count == 1)
                {
                    if (dtGamePlayInfo.Rows[0]["GameEndTime"] != DBNull.Value)
                    {
                        retMessage = "Game on the machine has ended, cannot be paused";
                        log.Debug("retMessage: " + retMessage);
                        log.LogMethodExit(-1);
                        return -1;
                    }
                    else
                    {
                        string isPausedStatus = dtGamePlayInfo.Rows[0]["PauseCheck"].ToString();
                        if (string.Compare(isPausedStatus, "N") == 0)
                        {
                            SqlCommand cmdPause = Common.Utilities.getCommand();
                            cmdPause.CommandText = @"update gameplayinfo 
                                                    set IsPaused='Y', 
                                                        PauseStartTime=getdate(), 
                                                        last_update_date=@last_update_date,
                                                        last_update_by=@last_update_by 
                                                  where gameplay_id=@gameplay_id";
                            cmdPause.Parameters.AddWithValue("@gameplay_id", gameplayID);
                            cmdPause.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                            cmdPause.Parameters.AddWithValue("@last_update_by", loginId);

                            log.LogVariableState("@gameplay_id", gameplayID);
                            log.LogVariableState("@last_update_by", loginId);
                            cmdPause.ExecuteNonQuery();
                            //auditMgr.logAction("Pause", "Game Play", gameplayID, "Y", Common.ParafaitEnv.LoginID);
                            Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Game Play", "", "Pause", 0, gameplayID.ToString(), "Y", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                            statusCheck = 0;
                        }
                        else
                        {
                            retMessage = "Already Paused, attempting to pause again..";
                            log.Debug("retMessage: " + retMessage);
                            statusCheck = 1;
                        }
                    }
                }
                else
                {
                    if (dtGamePlayInfo.Rows.Count == 0)
                    {
                        SqlCommand cmdPause = Common.Utilities.getCommand();
                        cmdPause.CommandText = @"insert into gameplayinfo 
                                                    (gameplay_id, IsPaused, PauseStartTime, last_update_date, last_update_by) 
                                                values (@gameplayid, 'Y', getdate(), @last_update_date, @last_update_by)";
                        cmdPause.Parameters.AddWithValue("@gameplayid", gameplayID);
                        cmdPause.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                        cmdPause.Parameters.AddWithValue("@last_update_by", loginId);
                        log.LogVariableState("@gameplay_id", gameplayID);
                        log.LogVariableState("@last_update_by", loginId);
                        cmdPause.ExecuteNonQuery();
                       // auditMgr.logAction("Pause", "Game Play", gameplayID, "Y", Common.ParafaitEnv.LoginID);
                        Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Game Play", "", "Pause", 0, gameplayID.ToString(), "Y", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Multiple game play info lines";
                        log.Debug("retMessage: " + retMessage);
                        statusCheck = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception: " + ex.Message;
                log.Error(retMessage);
                statusCheck = -1;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }
        public int RestartMachine(ref string retMessage)
        {
            log.LogMethodEntry();
            int statusCheck = 0;
            TimeSpan diff = new TimeSpan();
            int totalPauseTime;
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = Common.ParafaitEnv.LoginID;

            if (currentPlayer == null)
            {
                retMessage = "No player on the game machine";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-1);
                return -1;
            }
            if (gameplayID == -1)
            {
                retMessage = "No game active on the game machine";
                log.Debug("retMessage: " + retMessage);
                log.LogMethodExit(-1);
                return -1;
            }

            try
            {
                DateTime pauseStartTime;
                SqlCommand cmdgetPauseStartTime = Common.Utilities.getCommand();
                cmdgetPauseStartTime.CommandText = "select isnull(isPaused,'N') PauseCheck, PauseStartTime, GameEndTime from gameplayinfo where gameplay_id=@gameplay_id";
                cmdgetPauseStartTime.Parameters.AddWithValue("@gameplay_id", gameplayID);
                SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdgetPauseStartTime);
                DataTable dtGamePlayInfo = new DataTable();
                daGamePlayInfo.Fill(dtGamePlayInfo);
                if (dtGamePlayInfo.Rows.Count == 1)
                {
                    if (dtGamePlayInfo.Rows[0]["GameEndTime"] != DBNull.Value)
                    {
                        retMessage = "Game on the machine has ended, cannot be restarted";
                        log.Debug("retMessage: " + retMessage);
                        statusCheck = -1;
                    }
                    else
                    {
                        if (string.Compare(dtGamePlayInfo.Rows[0]["PauseCheck"].ToString(), "Y") == 0)
                        {
                            pauseStartTime = Convert.ToDateTime(dtGamePlayInfo.Rows[0]["PauseStartTime"]);
                            diff = Common.Utilities.getServerTime().Subtract(pauseStartTime);
                            totalPauseTime = Convert.ToInt32(diff.TotalSeconds);
                            SqlCommand cmdunPause = Common.Utilities.getCommand();
                            cmdunPause.CommandText = @"update gameplayinfo 
                                                      set IsPaused='N',
                                                          PauseStartTime=NULL,
                                                          TotalPauseTime=isnull(TotalPauseTime,0)+@totalpausetime,
                                                          last_update_by = @last_update_by,
                                                          last_update_date = @last_update_date
                                                    where gameplay_id=@gameplay_id";
                            cmdunPause.Parameters.AddWithValue("@gameplay_id", gameplayID);
                            cmdunPause.Parameters.AddWithValue("@totalpausetime", totalPauseTime);
                            cmdunPause.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                            cmdunPause.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                            log.LogVariableState("@gameplay_id", gameplayID);
                            log.LogVariableState("@totalpausetime", totalPauseTime);
                            log.LogVariableState("@last_update_by", loginId);
                            //auditMgr.logAction("Restart", "Game Play", gameplayID, "Y",Common.ParafaitEnv.LoginID);
                            Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Game Play", "", "Restart", 0, gameplayID.ToString(), "Y", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                            statusCheck = 0;
                        }
                        else
                        {
                            retMessage = "Not paused, attempting to restart..";
                            log.Debug("retMessage: " + retMessage);
                            statusCheck = 1;
                        }
                    }
                }
                else if (dtGamePlayInfo.Rows.Count == 0)
                {
                    retMessage = "No game play info lines, cannot unpause";
                    log.Debug("retMessage: " + retMessage);
                    statusCheck = -1;
                }
                else
                {
                    retMessage = "Multiple game play info lines";
                    log.Debug("retMessage: " + retMessage);
                    statusCheck = -1;
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception: " + ex.Message;
                log.Error(retMessage);
                statusCheck = -1;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }
        public int EndGame(ref string retMessage)
        {
            log.LogMethodEntry();
            int statusCheck = 0;
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = Common.ParafaitEnv.LoginID;

            try
            {
                SqlCommand cmdcheckGamePlayIdExists = Common.Utilities.getCommand();
                cmdcheckGamePlayIdExists.CommandText = "select GameEndTime from gameplayinfo where gameplay_id=@gameplayid";
                cmdcheckGamePlayIdExists.Parameters.AddWithValue("@gameplayid", gameplayID);
                SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdcheckGamePlayIdExists);
                DataTable dtGamePlayInfo = new DataTable();
                daGamePlayInfo.Fill(dtGamePlayInfo);
                if (dtGamePlayInfo.Rows.Count == 1)
                {
                    string GameEndTime = dtGamePlayInfo.Rows[0]["GameEndTime"].ToString();
                    if (string.Compare(GameEndTime, "") == 0)
                    {
                        SqlCommand cmdGameEnd = Common.Utilities.getCommand();
                        cmdGameEnd.CommandText = @"update gameplayinfo 
                                                      set GameEndTime = getDate(), 
                                                          last_update_by = @last_update_by,
                                                          last_update_date = @last_update_date
                                                    where gameplay_id=@gameplay_id";
                        cmdGameEnd.Parameters.AddWithValue("@gameplay_id", gameplayID);
                        cmdGameEnd.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                        cmdGameEnd.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                        log.LogVariableState("@gameplay_id", gameplayID);
                        log.LogVariableState("@last_update_by", loginId);
                        cmdGameEnd.ExecuteNonQuery();
                      //  auditMgr.logAction("Game End", "Game Play", gameplayID, "Y", Common.ParafaitEnv.LoginID);
                        Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Game Play", "", "Game End", 0, gameplayID.ToString(), "Y", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Already game ended, attempting to end again..";
                        log.Debug("retMessage: " + retMessage);
                        statusCheck = 1;
                    }
                }
                else
                {
                    if (dtGamePlayInfo.Rows.Count == 0)
                    {
                        SqlCommand cmdGameEnd = Common.Utilities.getCommand();
                        cmdGameEnd.CommandText = @"insert into gameplayinfo 
                                                    (gameplay_id, GameEndTime, last_update_by, last_update_date) values (@gameplayid, getdate(), @last_update_by, @last_update_date)";
                        cmdGameEnd.Parameters.AddWithValue("@gameplayid", gameplayID);
                        cmdGameEnd.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                        cmdGameEnd.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                        log.LogVariableState("@gameplay_id", gameplayID);
                        log.LogVariableState("@last_update_by", loginId);
                        cmdGameEnd.ExecuteNonQuery();
                        //auditMgr.logAction("Game End", "Game Play", gameplayID, "Y", Common.ParafaitEnv.LoginID);
                        Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Game Play", "", "Game End", 0, gameplayID.ToString(), "Y", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Multiple game play info lines";
                        log.Debug("retMessage: " + retMessage);
                        statusCheck = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception" + ex.Message;
                log.Error(retMessage);
                statusCheck = -1;
                throw ex;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }
        public int GameMachineId
        {
            get { return machineId; }
            set { machineId = value; }
        }
        public string GameMachineSerialNumber
        {
            get { return machineSerialNumber; }
            set { machineSerialNumber = value; }
        }
        public string GameNameOfMachine
        {
            get { return gameName; }
            set { gameName = value; }
        }
        public string MachineName
        {
            get { return machineName; }
        }
        public int SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        public bool IsActive
        {
            get { return isActive; }
        }
        public string ActiveFlag
        {
            get { if (isActive) return "Y"; else return "N"; }
            set { if (String.Compare(value, "Y") == 0) isActive = true; else isActive = false; }
        }
        public string MacPlayer
        {
            get { if (currentPlayer != null) return "Y"; else return "N"; }
        }
        public string CustomerName
        {
            get { if (currentPlayer != null) { if (currentPlayer.TeamName.CompareTo("") != 0) return "* " + currentPlayer.Name; else return currentPlayer.Name; } else return null; }
        }
        public string UserName
        {
            get { if (currentPlayer != null) { if (currentPlayer.TeamName.CompareTo("") != 0) return "* " + currentPlayer.UserName; else return currentPlayer.UserName; } else return null; }

        }
        public string UniqueID
        {
            get
            {
                if (currentPlayer != null) return currentPlayer.CardNumber;
                else return null;
            }
        }
        public string PlayDate
        {

            get { if (currentPlayer != null) return currentPlayer.PlayDate.ToString(); else return null; }
        }
        public int TimeLeftMins
        {
            get { if (currentPlayer != null) return (currentPlayer.GameTimeRemaining/60); else return 0; }
            set { currentPlayer.GameTimeRemaining = value; }
        }
        public int TimeLeft
        {
            get { if (currentPlayer != null) return currentPlayer.GameTimeRemaining; else return 0; }
            set { currentPlayer.GameTimeRemaining = value; }
        }
        public int CardID
        {
            get { if (currentPlayer != null) return currentPlayer.CardId; else return 0; }
        }
        public int QueueEntry
        {
            get { if (currentPlayer != null) return currentPlayer.QueueEntryId; else return 0; }
        }
        public string TeamName
        {
            get { if (currentPlayer != null) return currentPlayer.TeamName; else return null; }
        }
        public string LaneName
        {
            get { return machineName; }
            set { machineName = value; }
        }
        public string PauseStatus
        {
            get
            {
                if (currentPlayer != null)
                {
                    if (currentPlayer.IsPaused == true)
                        return "Y";
                }
                return "N";
            }
        }
        public int GamePlayId
        {
            get { if (currentPlayer != null) return currentPlayer.GamePlayId; else return 0; }
        }
    }

}
