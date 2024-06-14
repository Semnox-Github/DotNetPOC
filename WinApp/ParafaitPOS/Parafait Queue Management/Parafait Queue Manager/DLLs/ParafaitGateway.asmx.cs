/********************************************************************************************
 * Project Name - Parafait POS 
 * Description  - Card Entitlements
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        11-Sep-2019      Jinto Thomas         Added logger for methods
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using ParafaitUtils;

namespace ParafaitGateway
{
    /// <summary>
    /// Summary description for ParafaitGateway
    /// </summary>
    [WebService(Namespace = "http://semnox.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class ParafaitGateway : System.Web.Services.WebService
    {
        public class Entitlements
        {
            public string GameName;
            public string Type;
            public double Balance;
            public double BalanceTime;
        }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public List<Entitlements> ReturnAllCardEntitlements(string CardNumber, ref string Message)
        {
            log.LogMethodEntry(CardNumber);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.LogMethodExit(null);
                return null;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            DataTable dt = gw.ReturnAllCardEntitlements(CardNumber);
            if (dt == null)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
                log.LogMethodExit(null);
                return null;
            }
            else
            {
                List<Entitlements> list = new List<Entitlements>();
                foreach(DataRow dr in dt.Rows)
                {
                    Entitlements ent = new Entitlements();
                    ent.GameName = dr["game_name"].ToString();
                    ent.Type = dr["Type"].ToString();
                    ent.Balance = Convert.ToDouble(dr["Balance"]);
                    if (ent.Type == "TIME")
                        ent.BalanceTime = ent.Balance * 60;
                    else
                    {
                        try
                        {
                            ent.BalanceTime = Convert.ToDouble(dr["OptionalAttribute"]);
                        }
                        catch { }
                    }
                    log.LogVariableState("ent", ent);
                    list.Add(ent);
                }
                log.LogMethodExit(list);
                return list;
            }
        }

        [WebMethod]
        public double ReturnCardEntitlement(string CardNumber, string MachineName, ref string EntitlementType, ref int BalanceTime, ref string Message)
        {
            log.LogMethodEntry(CardNumber,MachineName);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.LogMethodExit(-1);
                return -1;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.ReturnCardEntitlementDllMode(CardNumber, MachineName, ref EntitlementType, ref BalanceTime);
            if (EntitlementType == "TIME")
                BalanceTime = BalanceTime * 60;
            if (val == -1)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod]
        public double DeductCardEntitlement(string CardNumber, string MachineName, ref string Message)
        {
            log.LogMethodEntry(CardNumber, MachineName);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-1);
                return -1;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.DeductCardEntitlementDllMode(CardNumber, MachineName);
            if (val == -1)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod(MessageName = "CashPaymentParafaitPrice")]
        public double PurchaseProductCashPayment(string CardNumber, string ProductReference, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(CardNumber, ProductReference, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductCashPayment(CardNumber, ProductReference, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod(MessageName = "CashPaymentUserPrice")]
        public double PurchaseProductCashPaymentUserPrice(string CardNumber, string ProductReference, double Price, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(CardNumber, ProductReference, Price, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error("Message", Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductCashPaymentUserPrice(CardNumber, ProductReference, Price, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod (MessageName = "CashPaymentParafaitPriceNoCard")]
        public double PurchaseProductCashPaymentNoCard(string ProductReference, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(ProductReference, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductCashPaymentNoCard(ProductReference, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod (MessageName = "CashPaymentUserPriceNoCard")]
        public double PurchaseProductCashPaymentNoCardUserPrice(string ProductReference, double Price, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(ProductReference, Price, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductCashPaymentNoCardUserPrice(ProductReference, Price, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod (MessageName = "CardPaymentParafaitPrice")]
        public double PurchaseProduct(string CardNumber, string ProductReference, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(CardNumber, ProductReference, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductDllMode(CardNumber, ProductReference, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod (MessageName = "CardPaymentUserPrice")]
        public double PurchaseProductUserPrice(string CardNumber, string ProductReference, double Price, string POSReference, string TrxReference, ref string Message)
        {
            log.LogMethodEntry(CardNumber, ProductReference, Price, POSReference, TrxReference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-5);
                return -5;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            double val = gw.PurchaseProductDllModeUserPrice(CardNumber, ProductReference, Price, POSReference, TrxReference);
            if (val < 0)
            {
                Message = gw.LastMessageDetails();
                log.LogVariableState("Message", Message);
            }
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod]
        public bool ReversePurchase(double TrxId, string POSReference, string Reference, ref string Message)
        {
            log.LogMethodEntry(TrxId, POSReference, Reference);
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(false);
                return false;
            }

            ParafaitPaymentGateway.Gateway gw = new ParafaitPaymentGateway.Gateway();
            bool val = gw.ReversePurchase(TrxId, POSReference, Reference);
            Message = gw.LastMessageDetails();
            log.LogVariableState("Message", Message);
            log.LogMethodExit(val);
            return val;
        }

        [WebMethod]
        public double EndGame(string MachineReference, ref string Message)
        {
            log.LogMethodEntry(MachineReference);
            double statusCheck = 0;

            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;

            try
            {
                ParafaitUtils.Utilities.Connection.Open();
                updateAuditTable("Debug End Game", MachineReference, 0, MachineReference, "Semnox");
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-1);
                return -1;
            }
		
			// MachineReference = "MBLane03";
            try
            {
                string retMessage = string.Empty;
                int gamePlayId = getGamePlayId(MachineReference, ref retMessage);
                if (gamePlayId != -1)
                {
                    SqlCommand cmdcheckGamePlayIdExists = ParafaitUtils.Utilities.getCommand();
                    cmdcheckGamePlayIdExists.CommandText = "select GameEndTime from gameplayinfo where gameplay_id=@gameplayid";
                    cmdcheckGamePlayIdExists.Parameters.AddWithValue("@gameplayid", gamePlayId);
                    SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdcheckGamePlayIdExists);
                    DataTable dtGamePlayInfo = new DataTable();
                    daGamePlayInfo.Fill(dtGamePlayInfo);
                    if (dtGamePlayInfo.Rows.Count == 1)
                    {
                        if (dtGamePlayInfo.Rows[0]["GameEndTime"] == DBNull.Value)
                        {
                            SqlCommand cmdGameEnd = ParafaitUtils.Utilities.getCommand();
			                cmdGameEnd.CommandText = @"update gameplayinfo
	                                                  set GameEndTime = getDate(),
                                                              last_update_by = @last_update_by,
                                                              last_update_date = @last_update_date
                                                        where gameplay_id=@gameplay_id";
                            cmdGameEnd.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                            cmdGameEnd.Parameters.AddWithValue("@last_update_date", DateTime.Now);
                            cmdGameEnd.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                            cmdGameEnd.ExecuteNonQuery();
                            updateAuditTable("Game End", "Game Play", gamePlayId, "Y", ParafaitEnv.LoginID);
                            statusCheck = 0;
                        }
                        else
                        {
                            Message = "Already game ended, attempting to end again..";
                            log.Debug(Message);
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
                            cmdGameEnd.Parameters.AddWithValue("@gameplayid", gamePlayId);
                            cmdGameEnd.Parameters.AddWithValue("@last_update_date", DateTime.Now);
                            cmdGameEnd.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                            cmdGameEnd.ExecuteNonQuery();
                            updateAuditTable("Game End", "Game Play", gamePlayId, "Y", ParafaitEnv.LoginID);
                            statusCheck = 0;
                        }
                        else
                        {
                            Message = "Multiple game play info lines";
                            log.Debug(Message);
                            statusCheck = -1;
                        }
                    }
                }
                else
                {
                    Message = retMessage;
                    log.Debug(Message);
                    statusCheck = -1;
                }

            }
            catch (Exception ex)
            {
                Message = "Exception " + ex.Message;
                log.Error(Message);
                statusCheck = -1;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }

        [WebMethod]
        public double PauseGame(string MachineReference, ref string Message)
        {
            log.LogMethodEntry(MachineReference);
            double statusCheck = 0;
            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
                updateAuditTable("Debug Pause", MachineReference, 0, MachineReference, "Semnox");

            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-1);
                return -1;
            }
           // MachineReference = "MBLane03";
            try
            {
                string retMessage = string.Empty;
                int gamePlayId = getGamePlayId(MachineReference, ref retMessage);
                if (gamePlayId != -1)
                {
                    SqlCommand cmdcheckGamePlayIdExists = ParafaitUtils.Utilities.getCommand();
                    cmdcheckGamePlayIdExists.CommandText = "select isnull(isPaused,'N') PauseCheck, PauseStartTime, GameEndTime from gameplayinfo where gameplay_id=@gameplayid";
                    cmdcheckGamePlayIdExists.Parameters.AddWithValue("@gameplayid", gamePlayId);
                    SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdcheckGamePlayIdExists);
                    DataTable dtGamePlayInfo = new DataTable();
                    daGamePlayInfo.Fill(dtGamePlayInfo);
                    if (dtGamePlayInfo.Rows.Count == 1)
                    {
                        string isPausedStatus = dtGamePlayInfo.Rows[0]["PauseCheck"].ToString();
                        if (dtGamePlayInfo.Rows[0]["GameEndTime"] == DBNull.Value)
                        {
                            if (string.Compare(isPausedStatus, "N") == 0)
                            {
	                            SqlCommand cmdPause = ParafaitUtils.Utilities.getCommand();
	                            cmdPause.CommandText = @"update gameplayinfo
	                                                    set IsPaused='Y',
	                                                        PauseStartTime=getdate(),
	                                                        last_update_date=@last_update_date,
	                                                        last_update_by=@last_update_by
	                                                  where gameplay_id=@gameplay_id";
	                            cmdPause.Parameters.AddWithValue("@gameplay_id", gamePlayId);
	                            cmdPause.Parameters.AddWithValue("@last_update_date", DateTime.Now);
	                            cmdPause.Parameters.AddWithValue("@last_update_by", ParafaitEnv.LoginID);

	                            cmdPause.ExecuteNonQuery();
                                updateAuditTable("Pause", "Game Play", gamePlayId, "Y", ParafaitEnv.LoginID);
                                statusCheck = 0;
                            }
                            else
                            {
                                Message = "Already Paused, attempting to pause again..";
                                log.Debug(Message);
                                statusCheck = 1;
                            }
                        }
                        else
                        {
                            Message = "Game has ended, attempting to pause again..";
                            log.Debug(Message);
                            statusCheck = -1;
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
                       	    cmdPause.Parameters.AddWithValue("@gameplayid", gamePlayId);
                            cmdPause.Parameters.AddWithValue("@last_update_date", DateTime.Now);
                            cmdPause.Parameters.AddWithValue("@last_update_by", ParafaitEnv.LoginID);
                            cmdPause.ExecuteNonQuery();
                            updateAuditTable("Pause", "Game Play", gamePlayId, "Y", ParafaitEnv.LoginID);
                            statusCheck = 0;
                        }
                        else
                        {
                            Message = "Multiple game play info lines";
                            log.Debug(Message);
                            statusCheck = -1;
                        }
                    }
                }
                else
                {
                    Message = retMessage;
                    log.Debug(Message);
                    statusCheck = -1;
                }
            }
            catch (Exception ex)
            {
                Message = "Exception " + ex.Message;
                log.Error(Message);
                statusCheck = -1;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }

        [WebMethod]
        public double RestartGame(string MachineReference, ref string Message)
        {
            log.LogMethodEntry(MachineReference);
            double statusCheck = 0;
            TimeSpan diff = new TimeSpan();
            int totalPauseTime = 0;

            ParafaitUtils.Utilities.ConnectionString = getConnectionString();
            ParafaitUtils.Utilities.Connection = new SqlConnection();
            ParafaitUtils.Utilities.Connection.ConnectionString = ParafaitUtils.Utilities.ConnectionString;
            try
            {
                ParafaitUtils.Utilities.Connection.Open();
                updateAuditTable("Debug Restart", MachineReference, 0, MachineReference, "Semnox");
            }
            catch (Exception ex)
            {
                Message = ex.Message + ParafaitUtils.Utilities.ConnectionString;
                log.Error(Message);
                log.LogMethodExit(-1);
                return -1;
            }
          //  MachineReference = "MBLane03";
            try
            {
                string retMessage = string.Empty;
                int gamePlayId = getGamePlayId(MachineReference, ref retMessage);
                if (gamePlayId != -1)
                {
                    DateTime pauseStartTime;
                    SqlCommand cmdgetPauseStartTime = ParafaitUtils.Utilities.getCommand();
                    cmdgetPauseStartTime.CommandText = "select isnull(isPaused,'N') PauseCheck, PauseStartTime, isnull(TotalPauseTime, 0) TotalPauseTime, GameEndTime from gameplayinfo where gameplay_id=@gameplay_id";
                    cmdgetPauseStartTime.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                    SqlDataAdapter daGamePlayInfo = new SqlDataAdapter(cmdgetPauseStartTime);
                    DataTable dtGamePlayInfo = new DataTable();
                    daGamePlayInfo.Fill(dtGamePlayInfo);
                    if (dtGamePlayInfo.Rows.Count == 1)
                    {
                        if (dtGamePlayInfo.Rows[0]["GameEndTime"] == DBNull.Value)
                        {
                            if (string.Compare(dtGamePlayInfo.Rows[0]["PauseCheck"].ToString(), "Y") == 0)
                            {
                                pauseStartTime = Convert.ToDateTime(dtGamePlayInfo.Rows[0]["PauseStartTime"]);
                                diff = DateTime.Now.Subtract(pauseStartTime);
                                totalPauseTime = Convert.ToInt32(diff.TotalSeconds);
                                SqlCommand cmdunPause = ParafaitUtils.Utilities.getCommand();
                            	cmdunPause.CommandText = @"update gameplayinfo
                            	                          set IsPaused='N',
                            	                              PauseStartTime=NULL,
                            	                              TotalPauseTime=isnull(TotalPauseTime,0)+@totalpausetime,
                            	                              last_update_by = @last_update_by,
                            	                              last_update_date = @last_update_date
                            	                        where gameplay_id=@gameplay_id";
                            	cmdunPause.Parameters.AddWithValue("@gameplay_id", gamePlayId);
                            	cmdunPause.Parameters.AddWithValue("@totalpausetime", totalPauseTime);
                            	cmdunPause.Parameters.AddWithValue("@last_update_date", DateTime.Now);
                            	cmdunPause.Parameters.AddWithValue("@last_update_by", ParafaitUtils.ParafaitEnv.LoginID);
                                cmdunPause.ExecuteNonQuery();

                                updateAuditTable("Restart", "Game Play", gamePlayId, "Y", ParafaitEnv.LoginID);
                                statusCheck = 0;
                            }
                            else
                            {
                                Message = "Not paused, attempting to un-pause again..";
                                log.Debug(Message);
                                statusCheck = 1;
                            }
                        }
                        else
                        {
                            Message = "Game has ended, attempting to pause again..";
                            log.Debug(Message);
                            statusCheck = -1;
                        }
                    }
                    else if (dtGamePlayInfo.Rows.Count == 0)
                    {
                        Message = "No game play info lines, cannot unpause";
                        log.Debug(Message);
                        statusCheck = -1;
                    }
                    else
                    {
                        Message = "Multiple game play info lines";
                        log.Debug(Message);
                        statusCheck = -1;
                    }
                }
                else
                {
                    Message = retMessage;
                    log.Debug(Message);
                    statusCheck = -1;
                }
            }
            catch (Exception ex)
            {
                Message = "Exception " + ex.Message + ex.StackTrace;
                log.Error(Message);
                statusCheck = -1;
            }
            log.LogMethodExit(statusCheck);
            return statusCheck;
        }

        private string getConnectionString()
        {
            log.LogMethodEntry();
            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            log.LogMethodExit(connectionString);
            return connectionString;
        }

        private void updateAuditTable(string actionPerformed, string objectModified, int idOfObjectModified, string valueModified, string userUpdating)
        {
            log.LogMethodEntry();
            ParafaitUtils.EventLog.logEvent("QUEUE SYSTEM", 'M', idOfObjectModified.ToString(), actionPerformed, objectModified, 0);
            log.LogMethodExit();
        }

        private int getGamePlayId(string machineReference, ref string returnMessage)
        {
            log.LogMethodEntry(machineReference);
            int gamePlayId = -1;
            try
            {
                SqlCommand cmdFindGamePlay = ParafaitUtils.Utilities.getCommand();
                cmdFindGamePlay.CommandText = @"select y.gameplay_id gameplayid, y.play_date
                                                 from gameplay y, machines z, CustomDataView cdv
                                                where y.machine_id = z.machine_id
                                                  and y.play_date < DATEADD(hh, @timeWindow, getdate())
                                                  and z.active_flag = 'Y'
                                                  and z.CustomDataSetId = cdv.CustomDataSetId
                                                  and cdv.Name = 'External System Identifier'
                                                  and cdv.CustomDataText = @machineIdentifier
                                                order by y.play_date desc";
                cmdFindGamePlay.Parameters.AddWithValue("@machineIdentifier", machineReference);
                cmdFindGamePlay.Parameters.AddWithValue("@timeWindow", 3);
                SqlDataAdapter daFindGamePlay = new SqlDataAdapter(cmdFindGamePlay);
                DataTable dtFindGamePlay = new DataTable();
                daFindGamePlay.Fill(dtFindGamePlay);
                if (dtFindGamePlay.Rows.Count > 0)
                    gamePlayId = Convert.ToInt32(dtFindGamePlay.Rows[0]["gameplayid"]);
                else
                {
                    returnMessage = "No game play found";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                returnMessage = "Exception " + ex.Message;

            }
            log.LogMethodExit(gamePlayId);
            return gamePlayId;
        }
    }
}
