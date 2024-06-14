/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - FormQueueManagement - The class managing the player
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        01-Oct-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;


namespace ParafaitQueueManagement
{
    public class Player
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int PHYSICAL_PRINT = 2;
        private const int DIGITAL_TOKEN_PRINT = 1;
        private const int FORCE_UNDO_TOKEN = 3;
        private const int DIGITAL_TOKEN_UNDO = 2;
        private const bool TIME = false;
        private const bool OVERS = true;

        private int cardId;
        private int customerId;
        private string cardNumber;
        private int playerIdentifier;
        private string name;
        private string teamName;
        private string entitledGameName;
        private DateTime queueEntryTime;
        private DateTime manualOverrideTime;
        private int promisedWaitTime;
        private int machineAssignedInQueue;
        private string machineAssignedSerialNumber;
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
        private bool customerIntimated;
        private AuditManager auditMgr;
        private int numberOfOvers;
        private bool entitlementTypeFlag;
      //  Utilities parafaitUtility = new Utilities();
      //  ParafaitEnv parafaitEnv = new ParafaitEnv(parafaitUtility);
        public Player()
        {
            log.LogMethodEntry();
            cardId = -1;
            log.LogMethodExit();
        }

        public Player(int customerQueueId, ref string retMessage)
        {
            log.LogMethodEntry(customerQueueId);
            Common.ParafaitEnv.Initialize();
            //parafaitEnv = new ParafaitEnv(parafaitUtility);
            auditMgr = new AuditManager();
            SqlCommand cmdFetchPlayerDetails = Common.Utilities.getCommand();
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
                                                            z.print_count printCount,
                                                            z.customer_initmidated customerInformed
                                                    from    cards x, 
	                                                        CustomerView(@PassPhrase) y,
	                                                        customerqueue z
                                                   where    x.customer_id = y.customer_id
                                                     and    x.valid_flag = 'Y'  
                                                     and    x.card_id = z.card_id
                                                     and    z.Custqueue_id = @customerQueueId";
            cmdFetchPlayerDetails.Parameters.AddWithValue("customerQueueId", customerQueueId);
            string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;
            cmdFetchPlayerDetails.Parameters.Add(new SqlParameter("@PassPhrase", passPhrase));
            SqlDataAdapter daPlayerDetails = new SqlDataAdapter(cmdFetchPlayerDetails);
            DataTable dtPlayerDetails = new DataTable();
            daPlayerDetails.Fill(dtPlayerDetails);
            if (dtPlayerDetails.Rows.Count == 0)
            {
                cardId = -1;
                retMessage = "No such customer id exists";
                log.Debug("retMessage : " + retMessage);
            }
            else
            {
                if (dtPlayerDetails.Rows.Count > 1)
                {
                    cardId = -1;
                    retMessage = "Multiple entries of customer id exists";
                    log.Debug("retMessage : " + retMessage);
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
                    int customerInformedCount = Convert.ToInt32(dtPlayerDetails.Rows[0]["customerInformed"].ToString());
                    customerIntimated = (customerInformedCount != 2) ? false : true;
                }

            }
            log.LogMethodExit();
        }
        public Player(int customerQueueId, int cardIdOfPlayer, string cardNumberOfPlayer, string nameOfPlayer, string teamNameOfPlayer,
                        string userNameOfPlayer, string entitledGameNameOfPlayer, DateTime playerQueueEntryTime, DateTime playerManualOverrideTime, int playerGamePlayTime, int playerPromisedWaitTime,
                        int printCountOfPlayer, int advanceTimeOfPlayer, int playerintimated, string entitlementType, int entitlementCount)
        {
            log.LogMethodEntry(customerQueueId, cardIdOfPlayer, cardNumberOfPlayer, nameOfPlayer, teamNameOfPlayer, userNameOfPlayer, entitledGameNameOfPlayer,
                playerQueueEntryTime, playerManualOverrideTime, playerGamePlayTime, playerPromisedWaitTime, printCountOfPlayer, advanceTimeOfPlayer, playerintimated,
                entitlementType, entitlementCount);
            playerIdentifier = customerQueueId;
            cardId = cardIdOfPlayer;
            cardNumber = cardNumberOfPlayer;
            name = nameOfPlayer;
            teamName = teamNameOfPlayer;
            entitledGameName = entitledGameNameOfPlayer;
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
            advancedTime = advanceTimeOfPlayer;
            customerIntimated = (playerintimated != 2) ? false : true;
            entitlementTypeFlag = (String.Compare(entitlementType, "TIME") == 0) ? TIME : OVERS;
            numberOfOvers = (String.Compare(entitlementType, "OVERS") == 0) ? entitlementCount : -1;
            log.LogMethodExit();
        }

        // Setting up the wait times of the player
        public void SetupPlayerPlayTimes(int playerGamePlayId, DateTime playerGameStartTime, DateTime playerGameEndTime, string playerPauseStatus, DateTime playerPauseStartTime, int playerTotalPauseTime)
        {
            log.LogMethodEntry(playerGamePlayId, playerGameStartTime, playerGameEndTime, playerPauseStatus, playerPauseStartTime, playerTotalPauseTime);
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
                playTimeLeft = Convert.ToInt32(gameEndTime.Subtract(Common.Utilities.getServerTime()).TotalSeconds);
            else
            {
                if (pauseStatus == true)
                {
                    pauseDiff = pauseStartTime.Subtract(Convert.ToDateTime(gameStartTime));
                    playTimeLeft = gamePlayTime - Convert.ToInt32(pauseDiff.TotalSeconds) + totalPauseTime;
                }
                else
                {
                    pauseDiff = Common.Utilities.getServerTime().Subtract(Convert.ToDateTime(gameStartTime));
                    int finalDiff = Convert.ToInt32(pauseDiff.TotalSeconds) - totalPauseTime;
                    playTimeLeft = gamePlayTime - finalDiff;
                }
            }
            log.LogMethodExit();
        }
        public void SetupWaitDetails(int customerIdOfPlayer, string teamNameOfPlayer, int waitTimeForPlayer, int waitTimeForPlayerGroup, int groupWaitNumberOfPlayer, string gameNameOfPlayerMachine, string machineNameOfPlayerMachine)
        {
            log.LogMethodEntry(customerIdOfPlayer, teamNameOfPlayer, waitTimeForPlayer, waitTimeForPlayerGroup, groupWaitNumberOfPlayer, gameNameOfPlayerMachine, machineNameOfPlayerMachine);
            customerId = customerIdOfPlayer;
            teamName = teamNameOfPlayer;
            waitTime = waitTimeForPlayer;
            waitTimeOfGroup = waitTimeForPlayerGroup;
            waitNumberWithinGroup = groupWaitNumberOfPlayer;
            gameName = gameNameOfPlayerMachine;
            machineName = machineNameOfPlayerMachine;
            log.LogMethodExit();

        }
        public int RemoveFromQueue()
        {
            log.LogMethodEntry();
            try
            {
                SqlCommand cmd = Common.Utilities.getCommand();
                cmd.CommandText = @"update customerqueue set manual_override = null, 
                                                            manual_override_time = null, 
                                                            customer_intimated = 0,
                                                            play_complete=2, 
                                                            print_count = 0,
                                                            machine_id_assigned = null,
                                                            last_update_by=@last_update_by,
                                                            last_update_date=@last_update_date 
                                                        where custqueue_id=@customerQueueId";
                cmd.Parameters.AddWithValue("@customerQueueId", playerIdentifier);
                cmd.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmd.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                cmd.ExecuteNonQuery();
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Remove Customer", 0, playerIdentifier.ToString(), "", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                //auditMgr.logAction("Remove Customer", "Customer", playerIdentifier, "", Common.ParafaitEnv.LoginID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(0);
            return 0;
        }
        public int UpdateTeamName(string teamNamePassed)
        {
            log.LogMethodEntry(teamNamePassed);
            try
            {
                SqlCommand cmdUpdateGroupname = Common.Utilities.getCommand();
                cmdUpdateGroupname.CommandText = @"update   customerqueue set 
                                                            customer_group_name = @newgroupname,
                                                            last_update_by=@last_update_by,
                                                            last_update_date=@last_update_date                                                            
                                                    where custqueue_id = @queueId";
                cmdUpdateGroupname.Parameters.AddWithValue("@newgroupname", teamNamePassed);
                cmdUpdateGroupname.Parameters.AddWithValue("@queueId", playerIdentifier);
                cmdUpdateGroupname.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmdUpdateGroupname.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                cmdUpdateGroupname.ExecuteNonQuery();
                teamName = teamNamePassed;
               // auditMgr.logAction("Team Name", "Customer", playerIdentifier, teamName, Common.ParafaitEnv.LoginID);
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Team Name", 0, playerIdentifier.ToString(), teamName, Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(0);
            return 0;
        }
        public int AdvancePlayer(int timeToAdvance)
        {
            log.LogMethodEntry(timeToAdvance);
            string loginId = Common.ParafaitEnv.LoginID;

            try
            {
                SqlCommand cmdAdvanceTime = Common.Utilities.getCommand();
                cmdAdvanceTime.CommandText = @"update   customerqueue 
                                                  set   advancedtime=(@advancedtime+isnull(advancedtime,0)),
                                                        last_update_date=@last_update_date,
                                                        last_update_by=@last_update_by 
                                                where   custqueue_id=@customerQueueId";
                cmdAdvanceTime.Parameters.AddWithValue("@advancedtime", (timeToAdvance));
                cmdAdvanceTime.Parameters.AddWithValue("@customerQueueId", playerIdentifier);
                cmdAdvanceTime.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmdAdvanceTime.Parameters.AddWithValue("@last_update_by", loginId);
                cmdAdvanceTime.ExecuteNonQuery();
                advancedTime += (timeToAdvance * 60);
               // auditMgr.logAction("Advance Customer", "Customer", playerIdentifier, advancedTime.ToString(), loginId);
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Advance Customer", 0, playerIdentifier.ToString(), advancedTime.ToString(), Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(0);
            return 0;
        }
        public int ShiftPlayer(int machineIdPassed, string gameNamePassed, DateTime manualOverrideTime)
        {
            log.LogMethodEntry(machineIdPassed, gameNamePassed, manualOverrideTime);
            string loginId = Common.ParafaitEnv.LoginID;
            try
            {
                SqlCommand cmdUpdateManual = Common.Utilities.getCommand();
                cmdUpdateManual.CommandText = @"update customerqueue 
                                                   set manual_override=@manual_override,
                                                       manual_override_time=@manual_override_time,
                                                       machine_id_assigned=@machine_id_assigned,
                                                       last_update_date=@last_update_date,
                                                       last_update_by=@last_update_by
                                                 where Custqueue_id=@customeQueueId";
                cmdUpdateManual.Parameters.AddWithValue("@manual_override", "Y");
                cmdUpdateManual.Parameters.AddWithValue("@manual_override_time", manualOverrideTime);
                cmdUpdateManual.Parameters.AddWithValue("@machine_id_assigned", machineIdPassed);
                cmdUpdateManual.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmdUpdateManual.Parameters.AddWithValue("@last_update_by", loginId);
                cmdUpdateManual.Parameters.AddWithValue("@customeQueueId", playerIdentifier);
                cmdUpdateManual.ExecuteNonQuery();
                //auditMgr.logAction("Move", "Customer", playerIdentifier, gameNamePassed, loginId);
                Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Move", 0, playerIdentifier.ToString(), gameNamePassed, loginId, Common.ParafaitEnv.POSMachine, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.LogMethodExit(0);
            return 0;
        }
        public void PrintToken(bool physicalPrintPerformed)
        {
            log.LogMethodEntry(physicalPrintPerformed);
            PrintDocument tokenPrintDoc = null;
            if (physicalPrintPerformed == true)
            {
                tokenPrintDoc = new PrintDocument();
                tokenPrintDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                tokenPrintDoc.DocumentName = "Token Receipt - " + name;
                //tokenPrintDoc.PrinterSettings.PrinterName = Utilities.getParafaitDefaults("PDF_WRITER_PRINTER");
                tokenPrintDoc.DefaultPageSettings.PaperSize = new PaperSize("custom", 300, 400);
            }
            try
            {
                if (physicalPrintPerformed == true)
                    tokenPrintDoc.Print();
                SqlCommand cmdUpdatePrintAttempts = Common.Utilities.getCommand();
                cmdUpdatePrintAttempts.CommandText = @"update   customerqueue set print_count = isnull(print_count, 0) + 1,
                                                                customer_intimated = @customerForcePrint,    
                                                                last_update_by=@last_update_by,
                                                                last_update_date=@last_update_date
                                                        where custqueue_id = @queueId";
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@queueId", playerIdentifier);
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@customerForcePrint", (physicalPrintPerformed == true) ? PHYSICAL_PRINT : DIGITAL_TOKEN_PRINT);
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                cmdUpdatePrintAttempts.ExecuteNonQuery();
               // auditMgr.logAction("Print Token", "Customer", playerIdentifier, "", Common.ParafaitEnv.LoginID);
                if (printCount == 0)
                {
                    SqlCommand cmdUpdateMachineId = Common.Utilities.getCommand();
                    cmdUpdateMachineId.CommandText = @"update customerqueue set machine_id_assigned=@machine_id_assigned,
                        last_update_date=@last_update_date,last_update_by=@last_update_by
                        where Custqueue_id=@customeQueueId";
                    cmdUpdateMachineId.Parameters.AddWithValue("@machine_id_assigned", GameMachineAssigned);
                    cmdUpdateMachineId.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                    cmdUpdateMachineId.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                    cmdUpdateMachineId.Parameters.AddWithValue("@customeQueueId", playerIdentifier);
                    cmdUpdateMachineId.ExecuteNonQuery();
                   // auditMgr.logAction("Move", "Customer", playerIdentifier, gameName, Common.ParafaitEnv.LoginID);
                    Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Print Token", 0, playerIdentifier.ToString(), gameName, Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                }
                printCount += 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        public void UndoPrintToken(bool customerForceRevoke)
        {
            log.LogMethodEntry(customerForceRevoke);
            try
            {
                SqlCommand cmdUpdatePrintAttempts = Common.Utilities.getCommand();
                cmdUpdatePrintAttempts.CommandText = @"update   customerqueue set print_count = 0,
                                                                customer_intimated = 0,
                                                                last_update_by=@last_update_by,
                                                                last_update_date=@last_update_date,
                                                                machine_id_assigned = null,
                                                                manual_override = null
                                                        where custqueue_id = @queueId
                                                           and isnull(customer_intimated, 0) != @customerForceUndo
                                                           and isnull(manual_override, 'N') != 'Y'";
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@queueId", playerIdentifier);
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@customerForceUndo", (customerForceRevoke == true) ? FORCE_UNDO_TOKEN : DIGITAL_TOKEN_UNDO);
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
                cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
                int rowsUpdated = cmdUpdatePrintAttempts.ExecuteNonQuery();
                if (rowsUpdated > 0)
                {
                  //  auditMgr.logAction("Undo Print Token", "Customer", playerIdentifier, "", Common.ParafaitEnv.LoginID);
                    Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Undo Print Token", 0, playerIdentifier.ToString(), "", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
                    printCount = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        
        public void ActivatePlayer()
        {
            log.LogMethodEntry();
            SqlCommand cmdUpdatePrintAttempts = Common.Utilities.getCommand();
            cmdUpdatePrintAttempts.CommandText = @"update   customerqueue set 
                                                                customer_intimated = 2, 
                                                                print_count = isnull(print_count, 0) + 1,   
                                                                last_update_by=@last_update_by,
                                                                last_update_date=@last_update_date
                                                        where custqueue_id = @queueId";
            cmdUpdatePrintAttempts.Parameters.AddWithValue("@queueId", playerIdentifier);
            cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_date", Common.Utilities.getServerTime());
            cmdUpdatePrintAttempts.Parameters.AddWithValue("@last_update_by", Common.ParafaitEnv.LoginID);
            cmdUpdatePrintAttempts.ExecuteNonQuery();
            //auditMgr.logAction("Activate Customer", "Customer", playerIdentifier, "", Common.ParafaitEnv.LoginID);
            Common.Utilities.EventLog.logEvent("QUEUE MGMT", 'M', "Customer", "", "Activate Customer", 0, playerIdentifier.ToString(), "", Common.ParafaitEnv.LoginID, Common.ParafaitEnv.POSMachine, null);
            log.LogMethodExit();
        }

        public Image siteLogo()
        {
            log.LogMethodEntry();
            Image img = null;
            try
                        {
                            SqlCommand cmdSiteLogo = Common.Utilities.getCommand();
                            cmdSiteLogo.CommandText = "select logo from site";
                            object siteImage = cmdSiteLogo.ExecuteScalar();
                            img = Common.Utilities.ConvertToImage(siteImage);
                        }
            catch
             {

             }
            log.LogMethodExit(img);
            return img;
        }

        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            Font printfont;
            string sitename = Common.ParafaitEnv.SiteName;

            string curDate = "Date: " + Common.Utilities.getServerTime().ToString(Common.Utilities.getParafaitDefaults("DATETIME_FORMAT"));
            string custName = "Customer Name: " + name;
            string TeamName = "Team Name: " + teamName;
            string LaneNo = "Lane No: " + machineAssignedSerialNumber;
            // string LaneName = "Lane Name: " + machineName;
            string LaneName = string.Empty;
            string tokenNo = "Token Number: " + playerIdentifier.ToString();

            string playDate = "Game Load Time: " + queueEntryTime.ToString(Common.Utilities.getParafaitDefaults("DATETIME_FORMAT"));
            string playOvers = "";
            if (entitlementTypeFlag == OVERS)
                playOvers = "Total Overs: " + numberOfOvers + " Overs";
            string playTime = "Total Play Time: " + (gamePlayTime / 60) + " minutes"; 
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
            System.Drawing.Image siteLogoImage=null;

            siteLogoImage = siteLogo();
           
           if(siteLogoImage !=null)
            {
             e.Graphics.DrawImage(siteLogo(), center,10,90,30);
            ypos += 20;
            }

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

            if (teamName.Trim() != string.Empty)
            {
                ypos = ypos + 20;
                stringsize = e.Graphics.MeasureString(TeamName, printfont);
                stringFormat.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(TeamName, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            }

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(LaneNo, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(LaneNo, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            //ypos = ypos + 20;
            //stringsize = e.Graphics.MeasureString(LaneName, printfont);
            //stringFormat.Alignment = StringAlignment.Near;
            //e.Graphics.DrawString(LaneName, printfont, Brushes.Black, new Rectangle((int)left+xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 20;
            stringsize = e.Graphics.MeasureString(tokenNo, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(tokenNo, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 35;
            stringsize = e.Graphics.MeasureString(playDate, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(playDate, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            
            ypos = ypos + 35;
            if (entitlementTypeFlag == OVERS)
            {                
                stringsize = e.Graphics.MeasureString(playOvers, printfont);
                stringFormat.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(playOvers, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
                ypos = ypos + 20;
            }
            
            stringsize = e.Graphics.MeasureString(playTime, printfont);
            stringFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawString(playTime, printfont, Brushes.Black, new Rectangle((int)left + xposwidth, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);

            ypos = ypos + 40;
            stringsize = e.Graphics.MeasureString(footerMsg, printfont);
            stringFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(footerMsg, printfont, Brushes.Black, new Rectangle((int)left, (int)ypos, recwidth, (int)stringsize.Height), stringFormat);
            log.LogMethodExit();
        }

        public int GameMachineAssigned
        {
            get { return machineAssignedInQueue; }
            set { machineAssignedInQueue = value; }
        }
        public string GameMachineAssignedSerialNumber
        {
            get { return machineAssignedSerialNumber; }
            set { machineAssignedSerialNumber = value; }
        }
        public int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }
        public int DisplayWaitTime
        {
            get { return (waitTime / 60); }
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
            get { return (gamePlayTime / 60); }
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
        public string EntitledGameName
        {
            get { return entitledGameName; }
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
            get { return (waitTimeOfGroup / 60); }
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
        public DateTime CalcQueueEntryTime
        {
            get { return queueEntryTime.AddSeconds(advancedTime * -1); }
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
        public bool CustomerIntimated
        {
            get { return customerIntimated; }
        }
    }

}
