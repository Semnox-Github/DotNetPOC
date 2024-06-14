﻿// Class to manage the queue. 
// The functionality has been divided into two files 
//      - ParafaitQueueManager.cs - which manages the queue 
//      - FormQueueManagement.cs - which manages the display of the queue

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using ParafaitUtils;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

namespace ParafaitQueueManagement
{
    public static class AuditManager
    {
        public static void logAction(string actionPerformed, string objectModified, int idOfObjectModified, string valueModified, string userUpdating)
        {
            ParafaitUtils.EventLog.logEvent("QUEUE MGMT", 'M', objectModified, "", actionPerformed, 0, idOfObjectModified.ToString(), valueModified, userUpdating, ParafaitEnv.POSMachine, null);
        }
    }


    public class Player
    {
        private int cardId;
        private int customerId;
        private string cardNumber;
        private int playerIdentifier;
        private string name;
        private string teamName;
        private DateTime queueEntryTime;
        private DateTime manualOverrideTime;
        private int promisedWaitTime;
        private int machineAssignedInQueue;
        private int gamePlayId;
        private string gameName;
        private string machineName;
        private string userName;
        private int advancedTime;
        private int gamePlayTime;
        private DateTime gameStartTime;
        private DateTime gameEndTime;
        private bool pauseStatus;
        private DateTime pauseStartTime;
        private int totalPauseTime;
        private int playTimeLeft;
        private int waitTime;
        private int waitTimeOfGroup;
        private int waitNumberWithinGroup;
        private int printCount;
        
        public Player()
        {
            cardId = -1;            
        }

        public Player(int customerQueueId, ref string retMessage)
        {
            SqlCommand cmdFetchPlayerDetails = ParafaitUtils.Utilities.getCommand();
            cmdFetchPlayerDetails.CommandText = @"select    x.card_number cardNumber, 
                                                            x.card_id cardId
	                                                        x.issue_date cardIssueDate, 
	                                                        y.customer_name customerFirstName, 
	                                                        y.middle_name customerMiddleName, 
	                                                        y.last_name customerLastName,
                                                            z.play_request playRequestTime;
	                                                        z.advancedtime advancedTime,
	                                                        z.customer_group_name teamName,
	                                                        isnull(z.machine_id_assigned, -1) machineAssigned,
	                                                        z.promised_wait promisedWaitTime,
                                                            z.print_count printCount
                                                    from    cards x, 
	                                                        customers y,
	                                                        customerqueue z
                                                   where    x.customer_id = y.customer_id
                                                     and    x.valid_flag = 'Y'  
                                                     and    x.card_id = z.card_id
                                                     and    z.Custqueue_id = @customerQueueId";
            cmdFetchPlayerDetails.Parameters.AddWithValue("customerQueueId", customerQueueId);
            SqlDataAdapter daPlayerDetails = new SqlDataAdapter(cmdFetchPlayerDetails);
            DataTable dtPlayerDetails = new DataTable();
            daPlayerDetails.Fill(dtPlayerDetails);
            if (dtPlayerDetails.Rows.Count == 0)
            {
                cardId = -1;
                retMessage = "No such customer id exists";
            }
            else
            {
                if (dtPlayerDetails.Rows.Count > 1)
                {
                    cardId = -1;
                    retMessage = "Multiple entries of customer id exists";
                }
                else
                {
                    cardId = Convert.ToInt32(dtPlayerDetails.Rows[0]["cardId"].ToString());
                    cardNumber = dtPlayerDetails.Rows[0]["cardNumber"].ToString();
                    playerIdentifier = customerQueueId;
                    name = dtPlayerDetails.Rows[0]["customerFirstName"].ToString() + " " + dtPlayerDetails.Rows[0]["customerMiddleName"].ToString() + " " + dtPlayerDetails.Rows[0]["customerLastName"].ToString();
                    teamName = dtPlayerDetails.Rows[0]["teamName"].ToString();
                    queueEntryTime = Convert.ToDateTime(dtPlayerDetails.Rows[0]["playRequestTime"]);
                    promisedWaitTime = Convert.ToInt32(dtPlayerDetails.Rows[0]["promisedWaitTime"].ToString());
                    advancedTime = Convert.ToInt32(dtPlayerDetails.Rows[0]["advancedTime"].ToString());
                    machineAssignedInQueue = Convert.ToInt32(dtPlayerDetails.Rows[0]["machineAssigned"].ToString());
                    printCount = Convert.ToInt32(dtPlayerDetails.Rows[0]["printCount"].ToString());
                }

            }
        }
        public Player(int customerQueueId, int cardIdOfPlayer, string cardNumberOfPlayer, string nameOfPlayer, string teamNameOfPlayer, string userNameOfPlayer, DateTime playerQueueEntryTime, 
                        DateTime playerManualOverrideTime, int playerGamePlayTime, int playerPromisedWaitTime, int printCountOfPlayer)
        {
            playerIdentifier = customerQueueId;
            cardId = cardIdOfPlayer;
            cardNumber = cardNumberOfPlayer;
            name = nameOfPlayer;
            teamName = teamNameOfPlayer;
            queueEntryTime = playerQueueEntryTime;
            promisedWaitTime = playerPromisedWaitTime;
            gamePlayTime = playerGamePlayTime;
            manualOverrideTime = playerManualOverrideTime;
            DateTime emptyDateTime = DateTime.Parse("01/01/1900");
            gameEndTime = emptyDateTime;
            pauseStatus = false;
            printCount = printCountOfPlayer;
            machineAssignedInQueue = -1;
            gamePlayId = -1;
            userName = userNameOfPlayer;

        }
        public void SetupPlayerPlayTimes(int playerGamePlayId, DateTime playerGameStartTime, DateTime playerGameEndTime, string playerPauseStatus, DateTime playerPauseStartTime, int playerTotalPauseTime)
        {
            TimeSpan pauseDiff = new TimeSpan();
            DateTime emptyDateTime = DateTime.Parse("01/01/1900");
            gamePlayId = playerGamePlayId;
            gameStartTime = playerGameStartTime;
            gameEndTime = playerGameEndTime;            
            if (string.Compare(playerPauseStatus, "Y") == 0)
                pauseStatus = true;
            else
                pauseStatus = false;
            pauseStartTime = playerPauseStartTime;
            totalPauseTime = playerTotalPauseTime;

            if (DateTime.Compare(gameEndTime, emptyDateTime) != 0)
                playTimeLeft = Convert.ToInt32(gameEndTime.Subtract(ParafaitUtils.Utilities.getServerTime()).TotalSeconds);
            else
            {
                if (pauseStatus == true)
                {
                    pauseDiff = pauseStartTime.Subtract(Convert.ToDateTime(gameStartTime));
                    playTimeLeft = gamePlayTime - Convert.ToInt32(pauseDiff.TotalSeconds) + totalPauseTime;
                }
                else
                {
                    pauseDiff = ParafaitUtils.Utilities.getServerTime().Subtract(Convert.ToDateTime(gameStartTime));
                    int finalDiff = Convert.ToInt32(pauseDiff.TotalSeconds) - totalPauseTime;
                    playTimeLeft = gamePlayTime - finalDiff;
                }
            }
        }
        public void SetupWaitDetails(int customerIdOfPlayer, string teamNameOfPlayer, int waitTimeForPlayer, int waitTimeForPlayerGroup, int groupWaitNumberOfPlayer, string gameNameOfPlayerMachine, string machineNameOfPlayerMachine)
        {
            customerId = customerIdOfPlayer;
            teamName = teamNameOfPlayer;
            waitTime = waitTimeForPlayer;
            waitTimeOfGroup = waitTimeForPlayerGroup;
            waitNumberWithinGroup = groupWaitNumberOfPlayer;
            gameName = gameNameOfPlayerMachine; ;
            machineName = machineNameOfPlayerMachine;
            
        }
        public int RemoveFromQueue()
        {
            try
            {
                SqlCommand cmd = ParafaitUtils.Utilities.getCommand();
                cmd.CommandText = @"update customerqueue set play_complete=2, last_update_by=@last_update_by,
                                    last_update_date=@last_update_date where custqueue_id=@customerQueueId";
                cmd.Parameters.AddWithValue("@customerQueueId", playerIdentifier);
                cmd.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                cmd.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                cmd.ExecuteNonQuery();
                AuditManager.logAction("Remove Customer", "Customer", playerIdentifier, "", ParafaitUtils.ParafaitEnv.LoginID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        public int UpdateTeamName(string teamNamePassed)
        {
            try
            {
                SqlCommand cmdUpdateGroupname = ParafaitUtils.Utilities.getCommand();
                cmdUpdateGroupname.CommandText = @"update   customerqueue set 
                                                            customer_group_name = @newgroupname,
                                                            last_update_by=@last_update_by,
                                                            last_update_date=@last_update_date                                                            
                                                    where custqueue_id = @queueId";
                cmdUpdateGroupname.Parameters.AddWithValue("@newgroupname", teamNamePassed);
                cmdUpdateGroupname.Parameters.AddWithValue("@queueId", playerIdentifier);
                cmdUpdateGroupname.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                cmdUpdateGroupname.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                cmdUpdateGroupname.ExecuteNonQuery();
                teamName = teamNamePassed;
                AuditManager.logAction("Team Name", "Customer", playerIdentifier, teamName, ParafaitEnv.LoginID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        public int AdvancePlayer(int timeToAdvance)
        {
            string loginId = ParafaitUtils.ParafaitEnv.LoginID;

            try
            {
                SqlCommand cmdAdvanceTime = ParafaitUtils.Utilities.getCommand();
                cmdAdvanceTime.CommandText = @"update   customerqueue 
                                                  set   advancedtime=(@advancedtime+isnull(advancedtime,0)),
                                                        last_update_date=@last_update_date,
                                                        last_update_by=@last_update_by 
                                                where   custqueue_id=@customerQueueId";
                cmdAdvanceTime.Parameters.AddWithValue("@advancedtime", (timeToAdvance*60));
                cmdAdvanceTime.Parameters.AddWithValue("@customerQueueId", playerIdentifier);
                cmdAdvanceTime.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                cmdAdvanceTime.Parameters.AddWithValue("@last_update_by", loginId);
                cmdAdvanceTime.ExecuteNonQuery();
                advancedTime += (timeToAdvance * 60);
                AuditManager.logAction("Advance Customer", "Customer", playerIdentifier, advancedTime.ToString(), loginId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        public int ShiftPlayer(int machineIdPassed, string gameNamePassed, DateTime manualOverrideTime)
        {
            string loginId = ParafaitUtils.ParafaitEnv.LoginID;
            try
            {
                SqlCommand cmdUpdateManual = ParafaitUtils.Utilities.getCommand();
                cmdUpdateManual.CommandText = @"update customerqueue set manual_override=@manual_override,
                    manual_override_time=@manual_override_time,machine_id_assigned=@machine_id_assigned,
                    last_update_date=@last_update_date,last_update_by=@last_update_by
                    where Custqueue_id=@customeQueueId";
                cmdUpdateManual.Parameters.AddWithValue("@manual_override", "Y");
                cmdUpdateManual.Parameters.AddWithValue("@manual_override_time", manualOverrideTime);
                cmdUpdateManual.Parameters.AddWithValue("@machine_id_assigned", machineIdPassed);
                cmdUpdateManual.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                cmdUpdateManual.Parameters.AddWithValue("@last_update_by", loginId);
                cmdUpdateManual.Parameters.AddWithValue("@customeQueueId", playerIdentifier);
                cmdUpdateManual.ExecuteNonQuery();
                AuditManager.logAction("Move", "Customer", playerIdentifier, gameNamePassed, loginId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        public void PrintToken()
        {
            PrintDocument tokenPrintDoc = new PrintDocument();
            tokenPrintDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            tokenPrintDoc.DocumentName = "Token Receipt - " + name;
         //   tokenPrintDoc.PrinterSettings.PrinterName = ParafaitUtils.Utilities.getParafaitDefaults("PDF_WRITER_PRINTER");
            tokenPrintDoc.DefaultPageSettings.PaperSize = new PaperSize("custom", 300, 400);

            try
            {
                tokenPrintDoc.Print();
                SqlCommand cmdUpdatePrintAttempts = ParafaitUtils.Utilities.getCommand();
                cmdUpdatePrintAttempts.CommandText = @"update   customerqueue set print_count = isnull(print_count, 0) + 1,
                                                                last_update_by=@last_update_by,
                                                                last_update_date=@last_update_date
                                                        where custqueue_id = @queueId";
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@queueId", playerIdentifier);
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                cmdUpdatePrintAttempts.ExecuteNonQuery();
                AuditManager.logAction("Print Token", "Customer", playerIdentifier, "", ParafaitEnv.LoginID);
                if (printCount == 0)
                {
                    SqlCommand cmdUpdateMachineId = ParafaitUtils.Utilities.getCommand();
                    cmdUpdateMachineId.CommandText = @"update customerqueue set machine_id_assigned=@machine_id_assigned,
                        last_update_date=@last_update_date,last_update_by=@last_update_by
                        where Custqueue_id=@customeQueueId";
                    cmdUpdateMachineId.Parameters.AddWithValue("@machine_id_assigned", GameMachineAssigned);
                    cmdUpdateMachineId.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                    cmdUpdateMachineId.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                    cmdUpdateMachineId.Parameters.AddWithValue("@customeQueueId", playerIdentifier);
                    cmdUpdateMachineId.ExecuteNonQuery();
                    AuditManager.logAction("Move", "Customer", playerIdentifier, gameName, ParafaitEnv.LoginID);
                }
                printCount += 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font printfont;
            string sitename = ParafaitEnv.SiteName;
            
            string curDate = "Date: " + ParafaitUtils.Utilities.getServerTime().ToString(ParafaitUtils.Utilities.getParafaitDefaults("DATETIME_FORMAT"));
            string custName = "Customer Name: " + name;
            string TeamName = "Team Name: " + teamName;
            string LaneNo = "Lane No: " + machineAssignedInQueue;
            string LaneName = "Lane Name: " + machineName;
            string tokenNo = "TNO: " + playerIdentifier.ToString();
            string playDate = "Entry Time: " + queueEntryTime.ToString(ParafaitUtils.Utilities.getParafaitDefaults("DATETIME_FORMAT")); ;
            string footerMsg = "Thank You";
            string measureString = string.Empty;
            SizeF stringsize = new SizeF();
            printfont = new Font("Times New Roman", 10);

            float leftmargin = e.MarginBounds.Left;
            float topmargin = e.MarginBounds.Top;
            float rightmargin = e.MarginBounds.Right;
            int recwidth = e.PageBounds.Width;
            float left = e.PageBounds.Left;
            int xposwidth = 10;
            float right = e.MarginBounds.Right;
            float center = (left + right) / 2;
            float ypos = 20;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            Point ulcorner = new Point(100, 100);

            stringsize = e.Graphics.MeasureString(sitename, printfont);
            e.Graphics.DrawString(sitename, printfont, Brushes.Black, new Rectangle((int)left, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(curDate, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(curDate, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(custName, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(custName, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(TeamName, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(TeamName, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(LaneNo, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(LaneNo, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(LaneName, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(LaneName, printfont, Brushes.Black, new Rectangle((int)left+xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
            ypos = ypos + 25;
            stringsize = e.Graphics.MeasureString(tokenNo, printfont);
            stringFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(tokenNo, new Font("Times New Roman", 16), Brushes.Black, new Rectangle((int)left, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
            ypos = ypos + 35;
            stringsize = e.Graphics.MeasureString(playDate, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(playDate, printfont, Brushes.Black, new Rectangle((int)left+xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
            ypos = ypos + 40;
            stringsize = e.Graphics.MeasureString(footerMsg, printfont);
            stringFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(footerMsg, printfont, Brushes.Black, new Rectangle((int)left, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
        }

        public int GameMachineAssigned
        {
            get { return machineAssignedInQueue; }
            set { machineAssignedInQueue = value; }
        }
        public int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }
        public int DisplayWaitTime
        {
            get { return (waitTime/60); }
        }
        public int GameTime
        {
            get { return (playTimeLeft); }
            set { playTimeLeft = value; }
        }
        public int GamePlayTime
        {
            get { return gamePlayTime; }
        }
        public int DisplayGamePlayTime
        {
            get { return (gamePlayTime/60); }
        }
        public string Name
        {
            get { return name; }
        }
        public string TeamName
        {
            get { return teamName; }
        }
        public string UserName
        {
            get { return userName; }
        }
        public DateTime PlayDate
        {
            get { return gameStartTime; }
        }
        public int GameTimeRemaining
        {
            get { return (playTimeLeft); }
            set { playTimeLeft = 0; }
        }
        public bool IsPaused
        {
            get { return pauseStatus; }
        }
        public int CardId
        {
            get { return cardId; }
        }
        public string CardNumber
        {
            get { return cardNumber; }
        }
        public int CustomerId
        {
            get { return customerId; }
        }
        public int QueueEntryId
        {
            get { return playerIdentifier; }
        }
        public int GroupWaitTime
        {
            get { return (waitTimeOfGroup/60); }
        }
        public int GroupWaitNumber
        {
            get { return waitNumberWithinGroup; }
        }
        public int PrintCount
        {
            get { return printCount; }
        }
        public DateTime QueueEntryTime
        {
            get { return queueEntryTime; }
        }
        public string GameName
        {
            get { return gameName; }
        }
        public int GamePlayId
        {
            get { return gamePlayId; }
        }
        public string MachineName
        {
            get { return machineName; }
        }
    }
    
    public class GameMachine
    {
        private int machineId;
        private string machineName;
        private string gameName;
        private bool isActive;
        private Player currentPlayer;
        private int serialNumber;

        public GameMachine(int machineIdPassed, string machineNamePassed, string gameNamePassed, string isActivePassed, string serialNumberPassed)
        {
            machineId = machineIdPassed;
            machineName = machineNamePassed;
            gameName = gameNamePassed;
            if (String.Compare(isActivePassed, "Y") == 0)
                isActive = true;
            else
                isActive = false;
            currentPlayer = null;
            bool parseSerialNumStatus = int.TryParse(serialNumberPassed, out serialNumber);
            if (!parseSerialNumStatus)
                serialNumber = -1;
        }
        public void BeingPlayedUpon(Player playerPassed)
        {
            currentPlayer = playerPassed;
        }
        public Player CurrentPlayer()
        {
            return currentPlayer;
        }
        public void ClearCurrentPlayer()
        {
            currentPlayer = null;
        }

        public void SetStatusOfMachine(char machineStatusFlag)
        {
            string userName = ParafaitUtils.ParafaitEnv.LoginID;
            try
            {
                if (machineStatusFlag == 'Y')
                    isActive = true;
                else
                    isActive = false;
                SqlCommand cmdMachineStatus = ParafaitUtils.Utilities.getCommand();
                cmdMachineStatus.CommandText = @"update machines 
                                                    set active_flag=@activeflagstatus,
                                                        last_updated_user=@last_updated_user,
                                                        last_updated_date=@last_updated_date 
                                                  where machine_id=@machine_id";
                cmdMachineStatus.Parameters.AddWithValue("@activeflagstatus", machineStatusFlag);
                cmdMachineStatus.Parameters.AddWithValue("@machine_id", machineId);
                cmdMachineStatus.Parameters.AddWithValue("@last_updated_user", userName);
                cmdMachineStatus.Parameters.AddWithValue("@last_updated_date", ParafaitUtils.Utilities.getServerTime());
                cmdMachineStatus.ExecuteNonQuery();
                AuditManager.logAction("Lane Activate", "Machine", machineId, machineStatusFlag.ToString(), userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool IsCompatibleWith(string gameNamePassed)
        {
            SqlCommand cmdFetchCompatibilities = ParafaitUtils.Utilities.getCommand();
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
                return true;
            else
                return false;
        }
        public int PauseMachine(ref string retMessage)
        {
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = ParafaitUtils.ParafaitEnv.LoginID;
            int statusCheck;

            if (currentPlayer == null)
            {
                retMessage = "No player on the game machine";
                return -1;
            }
            if (gameplayID == -1)
            {
                retMessage = "No game active on the game machine";
                return -1;
            }
            try
            {
                SqlCommand cmdcheckGamePlayIdExists = ParafaitUtils.Utilities.getCommand();
                cmdcheckGamePlayIdExists.CommandText = @"select isnull(isPaused,'N') PauseCheck, PauseStartTime , GameEndTime
                                                           from gameplayinfo 
                                                          where gameplay_id=@gameplayid";
                cmdcheckGamePlayIdExists.Parameters.AddWithValue("@gameplayid", gameplayID);
                SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdcheckGamePlayIdExists);
                DataTable dtGamePlayInfo = new DataTable();
                daGamePlayInfo.Fill(dtGamePlayInfo);
                if (dtGamePlayInfo.Rows.Count == 1)
                {
                    if (dtGamePlayInfo.Rows[0]["GameEndTime"] != DBNull.Value)
                    {
                        retMessage = "Game on the machine has ended, cannot be paused";
                        return -1;
                    }
                    else
                    {
                        string isPausedStatus = dtGamePlayInfo.Rows[0]["PauseCheck"].ToString();
                        if (string.Compare(isPausedStatus, "N") == 0)
                        {
                            SqlCommand cmdPause = ParafaitUtils.Utilities.getCommand();
                            cmdPause.CommandText = @"update gameplayinfo 
                                                    set IsPaused='Y', 
                                                        PauseStartTime=getdate(), 
                                                        last_update_date=@last_update_date,
                                                        last_update_by=@last_update_by 
                                                  where gameplay_id=@gameplay_id";
                            cmdPause.Parameters.AddWithValue("@gameplay_id", gameplayID);
                            cmdPause.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                            cmdPause.Parameters.AddWithValue("@last_update_by", loginId);

                            cmdPause.ExecuteNonQuery();
                            AuditManager.logAction("Pause", "Game Play", gameplayID, "Y", ParafaitEnv.LoginID);
                            statusCheck = 0;
                        }
                        else
                        {
                            retMessage = "Already Paused, attempting to pause again..";
                            statusCheck = 1;
                        }
                    }
                }
                else
                {
                    if (dtGamePlayInfo.Rows.Count == 0)
                    {
                        SqlCommand cmdPause = ParafaitUtils.Utilities.getCommand();
                        cmdPause.CommandText = @"insert into gameplayinfo 
                                                    (gameplay_id, IsPaused, PauseStartTime, last_update_date, last_update_by) 
                                                values (@gameplayid, 'Y', getdate(), @last_update_date, @last_update_by)";
                        cmdPause.Parameters.AddWithValue("@gameplayid", gameplayID);
                        cmdPause.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                        cmdPause.Parameters.AddWithValue("@last_update_by", loginId);
                        cmdPause.ExecuteNonQuery();
                        AuditManager.logAction("Pause", "Game Play", gameplayID, "Y", ParafaitEnv.LoginID);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Multiple game play info lines";
                        statusCheck = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception: " + ex.Message;
                statusCheck = -1;
            }
            return statusCheck;
        }
        public int RestartMachine(ref string retMessage)
        {
            int statusCheck = 0;
            TimeSpan diff = new TimeSpan();
            int totalPauseTime;
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = ParafaitUtils.ParafaitEnv.LoginID;

            if (currentPlayer == null)
            {
                retMessage = "No player on the game machine";
                return -1;
            }
            if (gameplayID == -1)
            {
                retMessage = "No game active on the game machine";
                return -1;
            }

            try
            {
                DateTime pauseStartTime;
                SqlCommand cmdgetPauseStartTime = ParafaitUtils.Utilities.getCommand();
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
                        statusCheck = -1;
                    }
                    else
                    {
                        if (string.Compare(dtGamePlayInfo.Rows[0]["PauseCheck"].ToString(), "Y") == 0)
                        {
                            pauseStartTime = Convert.ToDateTime(dtGamePlayInfo.Rows[0]["PauseStartTime"]);
                            diff = ParafaitUtils.Utilities.getServerTime().Subtract(pauseStartTime);
                            totalPauseTime = Convert.ToInt32(diff.TotalSeconds);
                            SqlCommand cmdunPause = ParafaitUtils.Utilities.getCommand();
                            cmdunPause.CommandText = @"update gameplayinfo 
                                                      set IsPaused='N',
                                                          PauseStartTime=NULL,
                                                          TotalPauseTime=isnull(TotalPauseTime,0)+@totalpausetime,
                                                          last_update_by = @last_update_by,
                                                          last_update_date = @last_update_date
                                                    where gameplay_id=@gameplay_id";
                            cmdunPause.Parameters.AddWithValue("@gameplay_id", gameplayID);
                            cmdunPause.Parameters.AddWithValue("@totalpausetime", totalPauseTime);
                            cmdunPause.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                            cmdunPause.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);

                            cmdunPause.ExecuteNonQuery();
                            AuditManager.logAction("Restart", "Game Play", gameplayID, "Y", ParafaitEnv.LoginID);
                            statusCheck = 0;
                        }
                        else
                        {
                            retMessage = "Not paused, attempting to restart..";
                            statusCheck = 1;
                        }
                    }
                }
                else if (dtGamePlayInfo.Rows.Count == 0)
                {
                    retMessage = "No game play info lines, cannot unpause";
                    statusCheck = -1;
                }
                else
                {
                    retMessage = "Multiple game play info lines";
                    statusCheck = -1;
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception: " + ex.Message;
                statusCheck = -1;
            }
            return statusCheck;
        }
        public int EndGame(ref string retMessage)
        {
            int statusCheck = 0;
            int gameplayID = currentPlayer.GamePlayId;
            string loginId = ParafaitUtils.ParafaitEnv.LoginID;

            try
            {
                SqlCommand cmdcheckGamePlayIdExists = ParafaitUtils.Utilities.getCommand();
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
                        SqlCommand cmdGameEnd = ParafaitUtils.Utilities.getCommand();
                        cmdGameEnd.CommandText = @"update gameplayinfo 
                                                      set GameEndTime = getDate(), 
                                                          last_update_by = @last_update_by,
                                                          last_update_date = @last_update_date
                                                    where gameplay_id=@gameplay_id";
                        cmdGameEnd.Parameters.AddWithValue("@gameplay_id", gameplayID);
                        cmdGameEnd.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                        cmdGameEnd.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                        cmdGameEnd.ExecuteNonQuery();
                        AuditManager.logAction("Game End", "Game Play", gameplayID, "Y", ParafaitEnv.LoginID);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Already game ended, attempting to end again..";
                        statusCheck = 1;
                    }
                }
                else
                {
                    if (dtGamePlayInfo.Rows.Count == 0)
                    {
                        SqlCommand cmdGameEnd = ParafaitUtils.Utilities.getCommand();
                        cmdGameEnd.CommandText = @"insert into gameplayinfo 
                                                    (gameplay_id, GameEndTime, last_update_by, last_update_date) values (@gameplayid, getdate(), @last_update_by, @last_update_date)";
                        cmdGameEnd.Parameters.AddWithValue("@gameplayid", gameplayID);
                        cmdGameEnd.Parameters.AddWithValue("@last_update_date", ParafaitUtils.Utilities.getServerTime());
                        cmdGameEnd.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                        cmdGameEnd.ExecuteNonQuery();
                        AuditManager.logAction("Game End", "Game Play", gameplayID, "Y", ParafaitEnv.LoginID);
                        statusCheck = 0;
                    }
                    else
                    {
                        retMessage = "Multiple game play info lines";
                        statusCheck = -1;

                    }
                }
            }
            catch (Exception ex)
            {
                retMessage = "Exception";
                statusCheck = -1;
                throw ex;
            }
            return statusCheck;
        }
        public int GameMachineId
        {
            get { return machineId; }
            set { machineId = value; }
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
            get { if (isActive) return "Y"; else return "N";}
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
            get {
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

    public class GameMachineList
    {
        GameMachine[] listOfGameMachines;
        List<string> gameNameList = new List<string>(); 
        public GameMachineList()
        {
            SqlCommand cmdMachines = ParafaitUtils.Utilities.getCommand();
            cmdMachines.CommandText = @"select  m.machine_id machineId, 
                                                m.machine_name machineName, 
                                                g.game_name gameName, 
                                                m.active_flag activeFlag,
                                                isnull(m.serialnumber, m.machine_id) sortOrderCol
                                           from game_profile gp, 
                                                games g, 
                                                machines m
                                          where gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id
                                            and dbo.GetMachineProfileValue(m.machine_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                          order by m.serialnumber, m.machine_id";
            SqlDataAdapter daMachines = new SqlDataAdapter(cmdMachines);
            DataTable dtMachines = new DataTable();
            daMachines.Fill(dtMachines);
            listOfGameMachines = new GameMachine[dtMachines.Rows.Count];
            for (int i = 0; i < dtMachines.Rows.Count; i++)
            {
                listOfGameMachines[i] = new GameMachine(Convert.ToInt32(dtMachines.Rows[i]["machineId"]), dtMachines.Rows[i]["machineName"].ToString(), dtMachines.Rows[i]["gameName"].ToString(), dtMachines.Rows[i]["activeFlag"].ToString(), dtMachines.Rows[i]["sortOrderCol"].ToString());
                if (gameNameList.Any(listOfGameMachines[i].GameNameOfMachine.Contains) == false)
                    gameNameList.Add(listOfGameMachines[i].GameNameOfMachine);
            }
        }
        public void refreshGameMachineList()
        {
            SqlCommand cmdMachines = ParafaitUtils.Utilities.getCommand();
            cmdMachines.CommandText = @"select  m.machine_id machineId, 
                                                m.machine_name machineName, 
                                                g.game_name gameName, 
                                                m.active_flag activeFlag,
                                                isnull(m.serialnumber, m.machine_id) sortOrderCol
                                           from game_profile gp, 
                                                games g, 
                                                machines m
                                          where gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id
                                            and dbo.GetMachineProfileValue(m.machine_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                          order by m.serialnumber, m.machine_id";
            SqlDataAdapter daMachines = new SqlDataAdapter(cmdMachines);
            DataTable dtMachines = new DataTable();
            daMachines.Fill(dtMachines);
            for (int i = 0; i < dtMachines.Rows.Count; i++)
            {
                GameMachine currentGameMachine = IdentifyGameMachine(Convert.ToInt32(dtMachines.Rows[i]["machineId"]));
                currentGameMachine.ActiveFlag = dtMachines.Rows[i]["activeFlag"].ToString();
                currentGameMachine.GameNameOfMachine =  dtMachines.Rows[i]["gameName"].ToString();
                currentGameMachine.LaneName = dtMachines.Rows[i]["machineName"].ToString();
                int sortOrderColValue = -1;
                bool parseSortOrderStatus = int.TryParse(dtMachines.Rows[i]["sortOrderCol"].ToString(), out sortOrderColValue);
                if (parseSortOrderStatus)
                    currentGameMachine.SerialNumber = sortOrderColValue;
                else
                    currentGameMachine.SerialNumber = -1;
            }
        }
        public void ClearPlayers()
        {
            for(int i=0; i < listOfGameMachines.Length; i++)
                listOfGameMachines[i].ClearCurrentPlayer();
        }
        public GameMachine IdentifyGameMachine(int machineIdPassed)
        {
            for (int i = 0; i < listOfGameMachines.Length; i++)
            {
                if (listOfGameMachines[i].GameMachineId == machineIdPassed)
                    return listOfGameMachines[i];
            }
            return null;
        }
        public int TotalEntries
        {
            get { return listOfGameMachines.Length; }
        }
        public GameMachine RetrieveGameMachine(int indexOfGM)
        {
            if (indexOfGM < listOfGameMachines.Length)
                return listOfGameMachines[indexOfGM];
            else
                return null;
        }
        public GameMachine[] GetAllGameMachines()
        {
            return listOfGameMachines.OrderBy(x => x.SerialNumber).ToArray(); 
        }
        public string[] GetAllGameMachinesName()
        {
            return gameNameList.ToArray();
        }
    }

    public class PlayersList
    {
        Player[] listOfPlayers;
        int totalEntries;
        public PlayersList(int totalPlayers)
        {
            listOfPlayers = new Player[totalPlayers];
            totalEntries = 0;
        }
        public Player CheckLastPlayerOnMachine(int machineIdToSearch)
        {
            int lastEntryWaitTime = -1;
            Player returnPlayer = null;
            for (int i = 0; i < totalEntries; i++)
            {
                if (listOfPlayers[i].GameMachineAssigned == machineIdToSearch)
                {
                    if (listOfPlayers[i].WaitTime > lastEntryWaitTime)
                        returnPlayer = listOfPlayers[i];
                }
            }
            return returnPlayer;
        }
        public int TotalEntries
        {
            get { return totalEntries; }
        }
        public void AddPlayer(Player playerPassed)
        {
            if (totalEntries < listOfPlayers.Length)
            {
                listOfPlayers[totalEntries] = playerPassed;
                totalEntries++;
            }
        }
        public Player[] GetAllPlayers()
        {
            return listOfPlayers;
        }
        private int FindPlayer(int playerIdentifier)
        {
            int playerNumber = -1;
            bool playerFound = false;
            for (int i = 0; (i < totalEntries) && (playerFound == false); i++)
            {
                if (listOfPlayers[i].QueueEntryId == playerIdentifier)
                {
                    playerNumber = i;
                    playerFound = true;
                }
            }
            return playerNumber;
        }
        public int RemovePlayer(int playerIdentifier)
        {
            int removalStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            if (playerIndex != -1)
                removalStatus = listOfPlayers[playerIndex].RemoveFromQueue();
            return removalStatus;
        }
        public int UpdateTeamName(int playerIdentifier, string teamNameToSet)
        {
            int updateTeamNameStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            if (playerIndex != -1)
                updateTeamNameStatus = listOfPlayers[playerIndex].UpdateTeamName(teamNameToSet);
            return updateTeamNameStatus;
        }
        public int AdvancePlayer(int playerIdentifier, int timeToAdvance)
        {
            int playerIndex = FindPlayer(playerIdentifier);
            int advancePlayerStatus = -1;
            if (playerIndex != -1)
                advancePlayerStatus = listOfPlayers[playerIndex].AdvancePlayer(timeToAdvance);
            return advancePlayerStatus;
        }
        public int ShiftPlayer(int playerIdentifier, int newMachineId, string gameNamePassed)
        {
            int playerIndex = FindPlayer(playerIdentifier);
            int shiftPlayerStatus = -1;
            if (playerIndex != -1)
                shiftPlayerStatus = listOfPlayers[playerIndex].ShiftPlayer(newMachineId, gameNamePassed, ParafaitUtils.Utilities.getServerTime());
            return shiftPlayerStatus;
        }

        public int PrintTokens(int playerIdentifier)
        {
            int printStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            if (playerIndex != -1)
            {
                if (listOfPlayers[playerIndex].PrintCount > 0)
                {
                    listOfPlayers[playerIndex].PrintToken();
                }
                else
                {
                    if (listOfPlayers[playerIndex].PrintCount == 0)
                    {
                        if (string.Compare(listOfPlayers[playerIndex].TeamName, "") != 0)
                        {
                            for (int i = 0; i < totalEntries; i++)
                            {
                                if ((listOfPlayers[playerIndex].GameMachineAssigned == listOfPlayers[i].GameMachineAssigned) &&
                                        (string.Compare(listOfPlayers[playerIndex].TeamName, listOfPlayers[i].TeamName) == 0))
                                {
                                    if ((listOfPlayers[i].PrintCount == 0) || (listOfPlayers[playerIndex].QueueEntryId == listOfPlayers[i].QueueEntryId))
                                        listOfPlayers[i].PrintToken();
                                }
                            }
                        }
                        else
                            listOfPlayers[playerIndex].PrintToken();
                    }
                    
                }
            }
            return printStatus;                
        }

    }

    public class QueueManager
    {
        GameMachineList gameMachinesList;
        PlayersList playersList;
        int gamePlayPastTime = Convert.ToInt32(ParafaitUtils.Utilities.getParafaitDefaults("GAMEPLAY_END_WAIT_TIME"));
        
        /// <summary>
        /// Featches all the machines that are part of the queue system
        /// </summary>
        public QueueManager()
        {
            gameMachinesList = new GameMachineList();
        }
        public GameMachineList GetGameMachineList()
        {
            return gameMachinesList;
        }
        public GameMachine[] getMachineDetails()
        {            
            return gameMachinesList.GetAllGameMachines();
        }
        public Player[] getPlayersList()
        {
            return playersList.GetAllPlayers();
        }

        public string[] GetListOfGame()
        {
            return gameMachinesList.GetAllGameMachinesName();
        }
        public void PrintTokens(int playerIdentifier)
        {
            playersList.PrintTokens(playerIdentifier);
        }
        public void refreshPlayersOnGameMachine()
        {
            gameMachinesList.refreshGameMachineList();
            gameMachinesList.ClearPlayers();
            try
            {
                SqlCommand cmdActivePlayers = ParafaitUtils.Utilities.getCommand();
                cmdActivePlayers.CommandText = @"select gmp.machine_id machine_id, 
                                                        gmp.play_date play_date,
                                                        gmp.gameplay_id gameplay_id, 
                                                        isnull(ca.OptionalAttribute,0) playTime, 
                                                        gmp.card_id card_id,
                                                        c.card_number cardNumber,
                                                        datediff(mi,gmp.play_date,getdate()) datedifference, 
                                                        cu.customer_name customer_name,
                                                        isnull(gplayinfo.GameEndTime,0) GameEndTime,    
                                                        gplayinfo.IsPaused pauseStatus, 
                                                        isnull(gplayinfo.PauseStartTime,0) PauseStartTime,
                                                        isnull(gplayinfo.TotalPauseTime,0) TotalPauseTime, 
                                                        cq.custqueue_id playerQueueId,
                                                        cq.customer_group_name teamName,
                                                        cq.promised_wait promisedWait,
                                                        cq.play_request playRequestTime,
                                                        isnull(cq.print_count,0)  printCount,
                                                        cu.username userName
                                                   from machines mac, 
                                                        cards c, 
                                                        customers cu, 
                                                        cardgames ca, 
                                                        (gameplay gmp left outer join gameplayinfo gplayinfo on gmp.gameplay_id = gplayinfo.gameplay_id),
                                                        (
                                                            select m.machine_id, m.machine_name, g.game_name, max(gmp.play_date) play_date
                                                              from game_profile gp, games g, machines m, gameplay gmp
                                                             where gp.game_profile_id = g.game_profile_id
                                                               and g.game_id = m.game_id
                                                               and dbo.GetGameProfileValue(g.game_Id, 'QUEUE_SETUP_REQUIRED') = '1'
                                                               and gmp.machine_id = m.machine_id
                                                               and gmp.play_date > dateadd(mi,@minutesAddParam,getdate())
                                                             group by m.machine_id, m.machine_name, g.game_name) gps,
                                                        customerqueue cq
                                                  where gps.play_date = gmp.play_date
                                                    and gps.machine_id = gmp.machine_id
                                                    and mac.machine_id = gmp.machine_id
                                                    and gmp.card_id = c.card_id
                                                    and cu.customer_id = c.customer_id 
                                                    and ca.card_id = c.card_id
                                                    and ca.card_game_id = cq.card_game_id";

                cmdActivePlayers.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));
                SqlDataAdapter daActivePlayers = new SqlDataAdapter(cmdActivePlayers);
                DataTable dtActivePlayers = new DataTable();
                daActivePlayers.Fill(dtActivePlayers);
                for (int i = 0; i < dtActivePlayers.Rows.Count; i++)
                {
                    Player playerOnMachine = new Player(Convert.ToInt32(dtActivePlayers.Rows[i]["playerQueueId"].ToString()), 
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["card_id"].ToString()),
                                                        dtActivePlayers.Rows[i]["cardNumber"].ToString(), 
                                                        dtActivePlayers.Rows[i]["customer_name"].ToString(),
                                                        dtActivePlayers.Rows[i]["teamName"].ToString(),
                                                        dtActivePlayers.Rows[i]["userName"].ToString(),
                                                        Convert.ToDateTime(dtActivePlayers.Rows[i]["playRequestTime"].ToString()),
                                                        DateTime.Parse("01/01/1900"),
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["playTime"].ToString()),
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["promisedWait"].ToString()),
                                                        Convert.ToInt32(dtActivePlayers.Rows[i]["printCount"].ToString()));
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generates the customer queue
        /// </summary>
        public string generateCustomerQueue()
        {

            int machineAssigned = 0;
            int waitTime = 0;
            int groupWaitTime = 0;
            int lastGroupWaitTime = 0;
            int groupWaitNumberLocal = 0;
            string custname = string.Empty;
            int totalBufferTimeToAdd = Convert.ToInt32(ParafaitUtils.Utilities.getParafaitDefaults("QUEUE_BUFFER_TIME")) + Convert.ToInt32(ParafaitUtils.Utilities.getParafaitDefaults("QUEUE_SETUP_TIME"));
            int queueMaxEntries = Convert.ToInt32(ParafaitUtils.Utilities.getParafaitDefaults("QUEUE_MAX_ENTRIES"));

            int lastCustomerMachineAssigned = 0;
            string lastCustomerTeamName = string.Empty;
            string lastCustomerGameName = string.Empty;

            string errorMessage = string.Empty;

            try
            {
                SqlCommand cmdCustQueue = ParafaitUtils.Utilities.getCommand();
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
                                                        q.username userName
                                                from customerqueue x, 
                                                    CardGames y, 
                                                    games g, 
                                                    customers q,
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
                cmdCustQueue.Parameters.AddWithValue("minutesAddParam", (-1 * gamePlayPastTime));
                SqlDataAdapter daCustQueue = new SqlDataAdapter(cmdCustQueue);
                DataTable dtCustQueue = new DataTable();
                daCustQueue.Fill(dtCustQueue);

                playersList = new PlayersList(dtCustQueue.Rows.Count);
                GameMachine playerGameMachine;

                for (int i = 0; i < dtCustQueue.Rows.Count; i++)
                {
                    playerGameMachine = null;
                    DateTime manualOverrideTime;
                    if (dtCustQueue.Rows[i]["manual_override_time"] == DBNull.Value)
                        manualOverrideTime = DateTime.Parse("01/01/1900");
                    else
                        manualOverrideTime = Convert.ToDateTime(dtCustQueue.Rows[i]["manual_override_time"].ToString());

                    Player playerOnMachine = new Player(Convert.ToInt32(dtCustQueue.Rows[i]["customerQueueId"].ToString()), 
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["card_id"].ToString()),
                                                        dtCustQueue.Rows[i]["cardNumber"].ToString(),
                                                        dtCustQueue.Rows[i]["customer_name"].ToString(),
                                                        dtCustQueue.Rows[i]["teamName"].ToString(),
                                                        dtCustQueue.Rows[i]["userName"].ToString(),
                                                        Convert.ToDateTime(dtCustQueue.Rows[i]["playrequest"].ToString()),
                                                        manualOverrideTime,
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["playTime"].ToString()),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["promisedWait"].ToString()),
                                                        Convert.ToInt32(dtCustQueue.Rows[i]["printCount"].ToString()));

                    if ((dtCustQueue.Rows[i]["manual_override"] != DBNull.Value) && (string.Compare(dtCustQueue.Rows[i]["manual_override"].ToString(),"Y") == 0))
                    {
                        int machineToSearch = Convert.ToInt32(dtCustQueue.Rows[i]["machine_assigned"]);
                        playerGameMachine = gameMachinesList.IdentifyGameMachine(machineToSearch);
                        if (playerGameMachine != null)
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
                            SqlCommand cmdGamePlayerOfTeam = ParafaitUtils.Utilities.getCommand();
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
                                SqlCommand cmdCheckManualOverride = ParafaitUtils.Utilities.getCommand();
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
                            if (dtCustQueue.Rows[i]["machine_assigned"] != DBNull.Value)
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
                            return "Queue has maxed out ";
                        }
                        else
                        {

                            playerOnMachine.GameMachineAssigned = playerGameMachine.GameMachineId;
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
                return "GenerateCustomerQueue " + custname+Environment.NewLine + ex.Message;
            }
            return errorMessage;
        }
        /// <summary>
        /// This event is fired when a card is issued to the customer
        /// </summary>
        public void OnTransactionEvent()
        {
            int promiseWaitTime = 9999;
            int queueIdShortest = -1;
            try
            {
                SqlCommand cmdTransaction = ParafaitUtils.Utilities.getCommand();
                cmdTransaction.CommandText = @"select c.card_id card_id, cg.card_game_id card_game_id, cg.last_update_date last_update_date,
                                                      g.game_name game_name, c.customer_id customerID
                                                 from cardgames cg, cards c, games g, customers cu
                                                where cg.last_update_date > DATEADD(dd,-1,getdate())
                                                  and dbo.GetGameProfileValue(g.game_Id, 'QUEUE_SETUP_REQUIRED') = '1'
                                                  and cg.card_id = c.card_id
                                                  and g.game_id = cg.game_id
                                                  and c.customer_id = cu.customer_id
                                                  and cg.card_game_id not in
                                                    (select card_game_id
                                                       from customerqueue
                                                      where last_update_date > DATEADD(dd,-1,getdate()))";
               
                SqlDataAdapter daTransaction = new SqlDataAdapter(cmdTransaction);
                DataTable dtTransaction = new DataTable();
                daTransaction.Fill(dtTransaction);
                if (dtTransaction.Rows.Count > 0)
                {
                    SqlCommand cmdCustomerQueue = ParafaitUtils.Utilities.getCommand();
                    cmdCustomerQueue.CommandText = @"Insert into customerqueue (play_request, promised_wait,
                                                        manual_override, manual_override_time, machine_id_assigned, card_id,
                                                        card_game_id, customer_group_name, last_update_date, last_update_by, play_complete) 
                                                        values (@play_request, @promised_wait, 
                                                        @manual_override, @manual_override_time, @machine_id_assigned, @card_id,  
                                                        @card_game_id, null, @last_update_date, @last_update_by, 0)";
                    for (int i = 0; i < dtTransaction.Rows.Count; i++)
                    {
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
                        cmdCustomerQueue.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                        cmdCustomerQueue.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //This event is fired when the lane allocator manipulates a lane(takes it offline/online)
        public void updateActiveLanes(int machineIdPassed, char activeFlagPassed)
        {
            GameMachine playerGameMachine = gameMachinesList.IdentifyGameMachine(machineIdPassed);
            if (playerGameMachine != null)
                playerGameMachine.SetStatusOfMachine(activeFlagPassed);
        }         

        //This method is called when the user removes a player from the queue
        public void updateCustomerQueuePlayerRemoval(int customerQueueIdPassed)
        {
            playersList.RemovePlayer(customerQueueIdPassed);
          
        }
        // This method is used when the user advances a player by x minutes
        public void updateCustomerAdvancedTime(int advancedTime, int customerQueueIdPassed)
        {
            playersList.AdvancePlayer(customerQueueIdPassed, advancedTime);
        }

        //This method is mainly used to determine the shortest wait time of a game queue
        public void shortestWaitMachine(string gameNameToSearch, ref int queueIdShortest, ref int waitTimeShortest)
        {
            int waitTime = 9999;
            int tempWaitTime = 9999;
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
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        //This method is used when the user moves a player to the priority queue
        public int moveToPriority(int customerQueueIdPassed, string loginID, ref string errorMessage)
        {
            
            string gameNameToSearch = "PRIORITY";
            int machineAssigned = -1;
            int waitTimeShortest = 9999;
            int returnCode = 0;
            
            shortestWaitMachine(gameNameToSearch, ref  machineAssigned, ref waitTimeShortest);
            GameMachine playerGameMachine = gameMachinesList.IdentifyGameMachine(machineAssigned);

            if (machineAssigned == -1)
            {
                errorMessage = "Could not assign any machine";
                returnCode = -1;
            }
            else if (waitTimeShortest != 0)
            {
                errorMessage = "Queue is occupied, cannot make the move";
                returnCode = -2;
            }
            else
            {
                playersList.ShiftPlayer(customerQueueIdPassed, playerGameMachine.GameMachineId, playerGameMachine.GameNameOfMachine);
            }
            return returnCode;
        }
        /// <summary>
        /// This event is fired when the lane allocator uses the pause feature
        /// </summary>
        /// <param name="gameplayID"></param>
        public int pause(int gameMachineIdPassed,  ref string errorMessage)
        {
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.PauseMachine(ref errorMessage);
            return statusCheck;
        }
        // This event fires when the lane allocator unpauses the machine
        public int unPause(int gameMachineIdPassed, ref string errorMessage)
        {
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.RestartMachine(ref errorMessage);
            return statusCheck;


        }
        public int GameEnd(int gameMachineIdPassed, ref string errorMessage)
        {
            int statusCheck = 0;
            GameMachine gameMachineId = gameMachinesList.IdentifyGameMachine(gameMachineIdPassed);
            statusCheck = gameMachineId.EndGame(ref errorMessage);
            return statusCheck;

        }
        public void updateGroupName(string newGroupName, int queueId)
        {
            playersList.UpdateTeamName(queueId, newGroupName);
        }

    }
    public class Players
    {
        int gamePlayPastTime = Convert.ToInt32(ParafaitUtils.Utilities.getParafaitDefaults("GAMEPLAY_END_WAIT_TIME"));
        public Players()
        {

        }

        public DataTable GetRemovedExpiredPlayers(DateTime fromDate, DateTime toDate, string cardNumber, string custName, bool isRemoved, bool isExpired)
        {
            SqlCommand cmdGetRemovedExpiredPlayers = ParafaitUtils.Utilities.getCommand();
            cmdGetRemovedExpiredPlayers.CommandText = @"select 'Removed' Status, cu.customer_name Name, c.card_number 'Card Number', cq.play_request 'Play Request', cq.custqueue_id 'QueueId'
                                   from customerqueue cq, cards c, customers cu
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
                                   from customerqueue cq, cards c, customers cu, CardGames cg
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

            return dtGetRemovedExpiredPlayers;
            
        }
        public void ReInstate(int customerQueueId)
        {
           
            try
            {
                SqlCommand cmdReinstatePlayers = ParafaitUtils.Utilities.getCommand();
                cmdReinstatePlayers.CommandText = @"update customerqueue 
                                                        set play_request = getdate(),
                                                            last_update_by = @loginId,
                                                            last_update_date = getdate(),
                                                            play_complete = 3,
                                                            manual_override = null,
                                                            machine_id_assigned = null,
                                                            advancedtime = null,
                                                            print_count = null
                                                     where custqueue_id = @customerQueueId";
                cmdReinstatePlayers.Parameters.AddWithValue("@loginId", ParafaitEnv.LoginID);
                cmdReinstatePlayers.Parameters.AddWithValue("@customerQueueId", customerQueueId);

                cmdReinstatePlayers.ExecuteNonQuery();
                AuditManager.logAction("ReInstate", "Customer", customerQueueId, DBNull.Value.ToString(), ParafaitEnv.LoginID);
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
