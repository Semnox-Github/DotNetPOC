using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace DayReport
{

    class DayReport
    {
        Utilities utilities;
        DataTable dTbale = new DataTable();
        DataTable dTbale1 = new DataTable();
        string time;
        string[] fileData;
        string[] BatchData;
        string fromDate, toDate;
        string[,] PosName;
        string sql = "";
        string tenantId;
        string sequence;
        string fileName;
        List<string> filenames = new List<string>();
        string failure_email_list;       
        string ftpUrl;
        string ftpUserName;
        string ftpPassword;
        string localDirectory;
        public bool error = false;
        bool ftpStatus = true;       
        SendEmailUI SendEmailUI;//Used to send emails
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger("DayReport");//(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public DayReport()
        {
            utilities = new Utilities(ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString);            
            try
            {
                log.Debug("Starts DayReport() default Contructor");
                failure_email_list = utilities.getParafaitDefaults("SALES_REPORT_FAILURE_MAIL_IDS");
                initialize();                
                error = false;
                log.Debug("Ends DayReport() default Contructor");
            }
            catch (Exception e)
            {
                log.Error("DayReport() Exception:"+e.ToString());
                error = true;
                try
                {
                    if (!string.IsNullOrEmpty(failure_email_list))
                    {
                        log.Info("DayReport() sending exception details to the configured mail ids");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in alert mail",e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                        SendEmailUI = new SendEmailUI(failure_email_list, "", "", "Error in FTP process.", "Hi, <br/><br/> Unable to FTP Sales Report. Please FTP file manually from site:" + utilities.ParafaitEnv.SiteName +" "+ utilities.ParafaitEnv.SiteAddress, "", "", true, utilities);
                        log.Info("DayReport() Sent exception details to the configured mail ids");
                    }
                    else
                    {
                        log.Fatal("DayReport() The Failure mail list is not found to Email");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in alert mail", "No email IDs specified to send FTP site connection failure failure alerts", "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    }
                }
                catch (Exception ex)
                {
                    log.Fatal("DayReport() Exception while sending email"+ex.ToString());
                    utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in alert mail", "Error:Sending FTP site connection failure alert:" + ex.Message, "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
            }
        }

        public bool processData(string RptDate = "")
        {
            log.Debug("Starts processData() method");
            try
            {
                ftpStatus = true;
                if (error)
                {
                    log.Error("processData():Due to error method throwing an exception");
                    throw new Exception();
                }
                if (!RptDate.Equals(""))
                {
                    log.Info("processData():Report generating manually.");
                    utilities.EventLog.logEvent("Daily Transaction Report", 'I', "Report generating information", "Report generating Manually." , "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    toDate = DateTime.Parse(RptDate).ToString("yyyy-MM-dd");
                    fromDate = DateTime.Parse(RptDate).AddDays(-1).ToString("yyyy-MM-dd");                    
                }
                else
                {
                    log.Info("processData():Report generating automatically.");
                    toDate = DateTime.Today.ToString("yyyy-MM-dd");
                    fromDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");                   
                }
                toDate = toDate + " " + time;
                fromDate = fromDate + " " + time;
                initSequences();
                getPosName();
                generateSql();
                generateData();                
                for (int i = 0; i < filenames.Count; i++)
                {
                    BatchData = new string[5];
                    BatchData[0] = "open " + ftpUrl.Replace("ftp://", "").Trim();
                    BatchData[1] = ftpUserName.Trim();
                    BatchData[2] = ftpPassword.Trim();
                    BatchData[3] = "put " + filenames[i];
                    BatchData[4] = "bye";
                    log.Debug("Url: "+ BatchData[0]);
                    if (BatchData[1].Length > 2)
                    {
                        log.Debug("UserName: "+ BatchData[1].Substring(BatchData[1].Length - 3).PadLeft(BatchData[1].Length, '*'));
                    }
                    if (BatchData[2].Length > 2)
                    {
                        log.Debug("Password: "+ BatchData[2].Substring(BatchData[2].Length - 3).PadLeft(BatchData[2].Length, '*'));
                    }
                    log.Debug("FileName: "+ BatchData[3]);
                    log.Debug("EndCmd: "+ BatchData[4]);
                    try
                    {
                        if (System.IO.File.Exists(@localDirectory + "sendFTP.txt"))
                        {
                            System.IO.File.Delete(@localDirectory + "sendFTP.txt");
                        }
                    }
                    catch
                    {
                    }
                    System.IO.File.WriteAllLines(@localDirectory + "sendFTP.txt", BatchData);
                    ExecuteCommand(filenames[i]);
                    if (System.IO.File.Exists(@localDirectory + "sendFTP.txt"))
                    {
                        System.IO.File.Delete(@localDirectory + "sendFTP.txt");
                    }                   
                }
                
                if (ftpStatus)
                {
                    log.Info("processData():Report generating process completed.");
                    utilities.EventLog.logEvent("Daily Transaction Report", 'I', "Report generating information", "Report generating process completed.", "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    return true;
                }
                else
                {
                    log.Error("processData():Report generating process failed.");
                    throw new  Exception("FTP failed.");
                }
            }
            catch (Exception e)
            {
                log.Error("processData():Exception during the report generation:"+e.ToString());
                try
                {
                    if (!string.IsNullOrEmpty(failure_email_list))
                    {
                        log.Info("processData() sending exception details to the configured mail ids");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in FTP process", e.ToString(), "SalesRptError",1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                        SendEmailUI = new SendEmailUI(failure_email_list, "", "", "Error in FTP process", "Hi, <br/><br/> Unable to FTP Sales Report. Please FTP file manually from site:" + utilities.ParafaitEnv.SiteName +" "+ utilities.ParafaitEnv.SiteAddress, "", "", true, utilities);
                        log.Info("processData() Sent exception details to the configured mail ids");
                    }
                    else
                    {
                        log.Fatal("processData() The Failure mail list is not found to Email");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in alert mail", "No email IDs specified to send FTP site connection failure failure alerts", "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                    }
                }
                catch (Exception ex)
                {
                    log.Fatal("processData() Exception while sending email" + ex.ToString());
                    utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in alert mail", "Error:Sending FTP site connection failure alert:" + ex.Message, "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                }
                return false;
            }
        }       
        private void initialize()
        {
            log.Debug("Starts initialize() method");
            try
            {
                utilities.ParafaitEnv.Initialize();
                time = utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME").PadLeft(2,'0') + ":00";//:00:000
                tenantId = utilities.getParafaitDefaults("TENANT_ID");                
                ftpUrl = utilities.getParafaitDefaults("SALES_REPORT_FTP_URL");
                ftpUserName = utilities.getParafaitDefaults("SALES_REPORT_FTP_USERNAME");
                ftpPassword = ParafaitDefaultContainer.GetDecryptedParafaitDefault(utilities.ExecutionContext, "SALES_REPORT_FTP_PASSWORD");
                toDate = DateTime.Today.ToString("yyyy-MM-dd");
                fromDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                toDate = toDate + " " + time;
                fromDate = fromDate + " " + time;
                localDirectory = ".\\SalesReport\\";
                log.Debug("Ends initialize() method");
            }
            catch (Exception e)
            {
                log.Fatal("initialize() Exception :" + e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in getPosName()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw e;
            }
        }
        private void getPosName()
        {
            log.Debug("Starts getPosName() method");
            try
            {
                dTbale.Dispose();
                dTbale = utilities.executeDataTable("SELECT POSMachineId,POSName FROM POSMachines");
                if (dTbale.Rows.Count > 0)
                {
                    PosName = new string[dTbale.Rows.Count, 2];
                    for (int i = 0; i < dTbale.Rows.Count; i++)
                    {
                        PosName[i, 0] = dTbale.Rows[i]["POSMachineId"].ToString();
                        PosName[i, 1] = dTbale.Rows[i]["POSName"].ToString();
                    }
                }
                log.Debug("Ends getPosName() method");
            }
            catch (Exception e)
            {
                log.Fatal("getPosName() Exception:" + e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in getPosName()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw e;
            }
        }
        private void initSequences()
        {
            log.Debug("Starts initSequences() method");
            //This part of the code is to add the records to the sequence table if not exists.
            try
            {
                dTbale.Dispose();
                dTbale = utilities.executeDataTable("SELECT POSMachineId,POSName FROM POSMachines");
                for (int i = 0; i < dTbale.Rows.Count; i++)
                {
                    dTbale1 = utilities.executeDataTable("SELECT 1 FROM Sequences WHERE SeqName='" + dTbale.Rows[i]["POSName"] + "'");
                    if (dTbale1.Rows.Count == 0)
                    {
                        utilities.executeScalar("insert into Sequences(SeqName,Seed,Incr,Currval,Guid) Values('" + dTbale.Rows[i]["POSName"].ToString() + "',0,1,1,NEWID())");
                    }
                }
                log.Debug("Ends initSequences() method");
            }
            catch (Exception e)
            {
                log.Fatal("initSequences() Exception:" + e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in initSequences()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw e;
            }
        }
        private void generateSql()
        {
            log.Debug("Starts generateSql() method");
            try
            {
                sql = "SELECT TrxId,textfield FROM (  ";
                dTbale.Dispose();
                dTbale = utilities.executeDataTable("SELECT query FROM dbo.DayReportStructure WHERE Active_Flag='Y' order by structure_Code");
                if (dTbale.Rows.Count > 0)
                {
                    for (int i = 0; i < dTbale.Rows.Count - 1; i++)
                    {
                        sql = sql + dTbale.Rows[i]["query"].ToString() + " Union All  ";
                    }
                    sql = sql + dTbale.Rows[dTbale.Rows.Count - 1]["query"].ToString();
                }
                sql = sql + " )as T order by CONVERT(int,TrxId),textfield";
                log.Debug("Ends generateSql() method");
            }
            catch (Exception e)
            {
                log.Fatal("generateSql() Exception:" + e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in generateSql()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw e;
            }
        }
        private void generateData()
        {
            log.Debug("Starts generateData() method");
            try
            {
                for (int i = 0; i < PosName.GetLength(0); i++)
                {
                    dTbale.Dispose();
                    dTbale = utilities.executeDataTable(sql, new SqlParameter("@FromDate", fromDate), new SqlParameter("@ToDate", toDate), new SqlParameter("@PosName", PosName[i, 1]));
                    if (dTbale.Rows.Count > 0)
                    {
                        fileData = new string[dTbale.Rows.Count + 2];
                        dTbale1.Dispose();
                        dTbale1 = utilities.executeDataTable("SELECT currval FROM Sequences WHERE SeqName='" + PosName[i, 1] + "'");
                        if (dTbale1.Rows.Count > 0)
                        {
                            sequence = dTbale1.Rows[0]["currval"].ToString();
                        }
                        //To generate the first line of the text file
                        dTbale1.Dispose();
                        dTbale1 = utilities.executeDataTable("SELECT top 1 isnull(convert(varchar(10),trx_no),'') as trx_no ,convert(varchar(8),year(GetDate()))+right('0'+convert(varchar,Month(GetDate())),2)+right('0'+convert(varchar,Day(GetDate())),2) as TrxDate,convert(varchar,GetDate(),108) as TrxTime, convert(varchar(8),(select isnull(loginid,'') from users u where u.user_id=th.user_id)) as user_id FROM trx_header th WHERE TrxID in (SELECT max(TrxID) FROM trx_header WHERE TrxDate<CAST('" + fromDate + "' AS DATETIME) and pos_machine='" + PosName[i, 1] + "')");
                        if (dTbale1.Rows.Count == 0)
                        {
                            log.Error("Ends generateData() method by throwing manual exception:\"Error in finding privious days records.\"");
                            throw new Exception("Error in finding privious days records.");
                        }
                        fileData[0] = "1|OPENED|" + tenantId + "|POS" + PosName[i, 0] + "|" + dTbale1.Rows[0]["trx_no"].ToString() + "|" + sequence + "|" + dTbale1.Rows[0]["TrxDate"].ToString() + "|" + dTbale1.Rows[0]["TrxTime"].ToString() + "|" + dTbale1.Rows[0]["user_id"].ToString()+"|"+DateTime.Parse(fromDate).ToString("yyyyMMdd");
                        //To generate the other lines of the text file
                        for (int j = 0; j < dTbale.Rows.Count; j++)
                        {
                            fileData[j + 1] = dTbale.Rows[j]["textfield"].ToString();
                        }
                        //To generate the last line of the text file
                        dTbale1.Dispose();
                        dTbale1 = utilities.executeDataTable("SELECT top 1 isnull(convert(varchar(10),trx_no),'') as trx_no ,convert(varchar(8),year(GetDate()))+right('0'+convert(varchar,Month(GetDate())),2)+right('0'+convert(varchar,Day(GetDate())),2) as TrxDate,convert(varchar,GetDate(),108) as TrxTime, convert(varchar(8),(select isnull(loginid,'') from users u where u.user_id=th.user_id)) as user_id FROM trx_header th WHERE TrxID in (SELECT max(TrxID) FROM trx_header WHERE TrxDate<CAST('" + toDate + "' AS DATETIME) and pos_machine='" + PosName[i, 1] + "')");
                        if (dTbale1.Rows.Count == 0)
                        {
                            log.Error("Ends generateData() method by throwing manual exception:\"Error in finding Todays records.\"");
                            throw new Exception("Error in finding Todays records.");
                        }
                        fileData[dTbale.Rows.Count + 1] = "1|CLOSED|" + tenantId + "|POS" + PosName[i, 0] + "|" + dTbale1.Rows[0]["trx_no"].ToString() + "|" + sequence + "|" + dTbale1.Rows[0]["TrxDate"].ToString() + "|" + dTbale1.Rows[0]["TrxTime"].ToString() + "|" + dTbale1.Rows[0]["user_id"].ToString() +"|"+ DateTime.Parse(fromDate).ToString("yyyyMMdd");
                        //file name generation
                        fileName = "t" + tenantId + "_POS" + PosName[i, 0] + "_" + sequence + "_" + DateTime.Now.ToString("yyMMddHHmm") + ".txt";
                        filenames.Add(fileName);
                        if (fileData.Length > 2)
                        {
                            if (generateFile())
                            {
                                utilities.executeScalar("UPDATE Sequences SET currval=currval+Incr WHERE SeqName='" + PosName[i, 1] + "'");
                                utilities.executeScalar("UPDATE Sequences SET currval=1 WHERE currval=10000 and SeqName='" + PosName[i, 1] + "'");//resetting the sequence.
                                utilities.EventLog.logEvent("Daily Transaction Report", 'I', "In generateData()", fileName + " file generation is successfull for the date FROM " + fromDate + " to " + toDate + ".", "SalesRptError", 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                            }
                            else
                            {
                                log.Error(fileName + " File generation is failed for the date FROM " + fromDate + " to " + toDate + ".");
                                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "In generateData()", fileName + " File generation is failed for the date FROM " + fromDate + " to " + toDate + ".", "SalesRptError", 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                            }
                        }
                    }
                    else
                    {
                        log.Info("There is no data to generate the text file for the pos :" + PosName[i, 1] + ".");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in generateData()", "There is no data to generate the text file for the pos :" + PosName[i, 1] + ".", "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    }
                }
                log.Debug("Ends generateData() method");

            }
            catch (Exception e)
            {
                log.Fatal("generateData() Exception:" + e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in generateData()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                throw e;
            }
        }
        private bool generateFile()
        {
            try
            {
                log.Debug("Starts generateFile() method");
                if (!System.IO.Directory.Exists(@localDirectory))
                {
                    System.IO.Directory.CreateDirectory(@localDirectory);
                }
                System.IO.File.WriteAllLines(@localDirectory + fileName, fileData);
                log.Debug("Ends generateFile() method by returning true");
                return true;
            }
            catch (Exception e)
            {
                log.Fatal("Ends generateFile() method exception:"+e.ToString());
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in generateFile()", e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                return false;
            }
        }




        void ExecuteCommand(string name)
        {
            log.Debug("Starts ExecuteCommand() method");
            int counter = 0;
            ProcessStartInfo processInfo;
            Process process;
            string output = "";
            string error = "";
            bool status = false;
            while (counter < 3)
            {
                try
                {
                    processInfo = new ProcessStartInfo();
                    processInfo.WorkingDirectory = Application.StartupPath + "\\SalesReport";
                    processInfo.FileName = "cmd.exe";
                    processInfo.CreateNoWindow = true;
                    processInfo.UseShellExecute = false;
                    processInfo.Arguments = "/C ftp -v -s:sendFTP.txt&&exit";
                    // *** Redirect the output ***
                    processInfo.RedirectStandardError = true;
                    processInfo.RedirectStandardOutput = true;                    
                    process = Process.Start(processInfo);
                    status = process.WaitForExit(60 * 1000);
                    // *** Read the streams ***

                    if (status)
                    {
                        output = process.StandardOutput.ReadToEnd();
                    }
                    else
                    {
                        try
                        {
                            Process[] process1 = Process.GetProcesses();//killing ftp process
                            for (int i = 0; i < process1.Length; i++)
                            {
                                if (process1[i].ProcessName.Equals("ftp"))
                                {
                                    process1[i].Kill();
                                    process1[i].Close();
                                    process1[i].Dispose();
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }
                        process.Kill();
                        error = "FTP process killed.";
                    }
                    if (status)
                    {
                        error = process.StandardError.ReadToEnd();
                    }
                    process.Close();
                    process.Dispose();
                    if (!string.IsNullOrEmpty(error.Trim()))
                    {
                        log.Debug("Starts ExecuteCommand() method" + counter + " Time Error." + error+"during FTP preocess");
                        utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in ExecuteCommand()", counter + " Time Error." + error, "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                        counter++;
                    }
                    else
                    {
                        log.Debug("Starts ExecuteCommand() FTP is successfull of the file " + name);
                        utilities.EventLog.logEvent("Daily Transaction Report", 'I', "FTP successfull.", "FTP is successfull of the file " + name, "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                        counter = 3;
                    }
                }
                catch (Exception e)
                {
                    log.Warn("Exception occured during " + counter + " Time try.:" + e.ToString());
                    utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in ExecuteCommand()", counter + " Time Error." + e.ToString(), "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                    counter++;
                }
            }
            if (counter == 3 && !status)
            {
                ftpStatus = false;
                log.Error("FTP is failed for the file " + name);
                utilities.EventLog.logEvent("Daily Transaction Report", 'E', "Error in ExecuteCommand()", "FTP is failed for the file " + name + ". Try again manualy...", "SalesRptError", 1, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
            }
        }
        
    }

}
