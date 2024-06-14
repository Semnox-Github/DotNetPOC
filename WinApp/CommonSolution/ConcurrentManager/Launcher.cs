
/********************************************************************************************
 * Project Name - Launcher File
 * Description  - User interface for Concurrent Program Lancher
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        25-Feb-2016   Jeevan         Created 
 *1.00        18-jul-2016   Rakshith       Modified 
 *1.10        22-March-2017   Amaresh       Modified 
 ********************************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Messaging;
using System.Diagnostics;
using System.Threading;
//using Toolkit;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.JobUtils;
using System.Globalization;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// Launcher Class File
    /// </summary> 
    public class Launcher
    {
        Utilities Utilities;
        SendEmailUI SendEmailUI;//Used to send emails
        Core.Utilities.ExecutionContext executionContext;

        ConcurrentProgramsDTO conProgramDTO;
        ConcurrentRequestsDTO conRequestDTO;
        SqlParameter[] sqlParameter;
        string sqlArguments;

        /// <summary>
        /// Phase of the program
        /// </summary>
        public enum Phase
        {
            /// <summary>
            /// indicates Request is pending
            /// </summary>
            Pending,
            /// <summary>
            /// indicates Request is Running
            /// </summary>
            Running,
            /// <summary>
            /// indicates Request is Complete
            /// </summary>
            Complete
        };

        /// <summary>
        /// Status of the program
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Request is in Normal condition
            /// </summary>
            Normal,
            /// <summary>
            /// Request ends with Error
            /// </summary>
            Error,
            /// <summary>
            /// Request is Aborted
            /// </summary>
            Aborted
        };

        string LogDirectory;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        /// <param name="inconProgDTO"></param>
        /// <param name="inconReqDTO"></param>
        /// <param name="inLogDirectory"></param>
        /// <param name="ExeExist"></param>
        public Launcher(Utilities inUtilities, ConcurrentProgramsDTO inconProgDTO, ConcurrentRequestsDTO inconReqDTO, string inLogDirectory, bool ExeExist, Core.Utilities.ExecutionContext executionContext)
        {
            Utilities = inUtilities;
            conProgramDTO = inconProgDTO;
            conRequestDTO = inconReqDTO;
            LogDirectory = inLogDirectory;
            this.executionContext = executionContext;
        }
        /// <summary>
        /// Parameterized constructor of Launcher Class
        /// </summary>
        /// <param name="inUtilities">Utilities object as parameter</param>
        /// <param name="inconProgDTO">Utilities object as parameter</param>
        /// <param name="inconReqDTO">Utilities object as parameter</param>
        /// <param name="inLogDirectory">Utilities object as parameter</param>
        public Launcher(Utilities inUtilities, ConcurrentProgramsDTO inconProgDTO, ConcurrentRequestsDTO inconReqDTO, string inLogDirectory, Core.Utilities.ExecutionContext executionContext)
        {

            Utilities = inUtilities;
            conProgramDTO = inconProgDTO;
            conRequestDTO = inconReqDTO;
            LogDirectory = inLogDirectory;
            this.executionContext = executionContext;

            log("Starts-Launcher() parameterized constructor.");

            switch (conProgramDTO.ExecutionMethod.ToString())
            {
                case "P": execStoredProc(); break;
                case "E": execExe(); break;
                case "L": execLibraryMethod(); break;
                default: break;
            }
            log("ends-Launcher( ) parameterized constructor.");
        }

        void execStoredProc()
        {
            log("Starts-execStoredProc() method.");

            int tid = Thread.CurrentThread.ManagedThreadId;
            Console.Write(tid.ToString());
            try
            {
                startRequest(0); // update request as running
                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;

                UpdateProcedureParameters();

                string command;
                DataTable dt;

                log(conProgramDTO.ExecutableName + "is running");

                //Run Procedure with arguments if present
                if (!string.IsNullOrEmpty(sqlArguments) && sqlParameter != null && sqlParameter.Length > 0)
                {
                    command = "exec " + conProgramDTO.ExecutableName + " " + sqlArguments;
                    dt = Utilities.executeDataTable(command, sqlParameter);
                }
                else
                {
                    //Run Procedure without arguments
                    command = "exec " + conProgramDTO.ExecutableName;
                    dt = Utilities.executeDataTable(command);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    //Log first row of datatable
                    log(dt.Rows[0][0].ToString());
                }

                log(conProgramDTO.ExecutableName + " is completed");

                //Update request as complete
                endRequest(Status.Normal.ToString());

                //Send request status to mail 
                SendEmail("Completed successfully", true);
            }
            catch (Exception ex)
            {
                log("Error :" + ex.Message);

                if (conRequestDTO.ErrorCount < 3)
                {
                    //update request as error
                    UpdateErrorRequest();
                }

                log("Error count : " + conRequestDTO.ErrorCount);

                //When error occured 3 times update request as complete with errors
                if (conRequestDTO.ErrorCount >= 3)
                {
                    endRequest(Status.Error.ToString());

                    log("Completed with error :" + ex.Message);

                    //Send error details to mail 
                    SendEmail("Completed with error :" + ex.Message + " <br/><br/> Please check", false);
                }
            }
            log("ends-execStoredProc() method.");
        }

        void execExe()
        {
            log("Starts-execExe() method.");

            string exe;
            try
            {
                startRequest(0); // update request as running

                string root = Application.StartupPath.ToString();

                exe = conProgramDTO.ExecutableName.ToString();

                if (!(File.Exists(root + "\\" + exe)))
                {
                    log("File not found or invalid executable: " + conProgramDTO.ExecutableName.ToString());
                    endRequest(Status.Error.ToString());

                    SendEmail("Completed with error : " + conProgramDTO.ExecutableName + " file not found" + " <br/><br/> Please check", false);
                    return;
                }

                if (Process.GetProcesses().Any(cp => cp.ProcessName.ToLower().Contains(exe.ToLower().Replace(".exe", ""))) == true)
                {
                    return;
                }

                //Added to run exe when its called from windows service
                string applicationName = root + "\\" + exe;

                //applicationName = "cmd.exe";
                ApplicationLoader.PROCESS_INFORMATION procInfo;
                ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);
                //Process p;
                //p = System.Diagnostics.Process.Start(root + "\\" + exe);
                log(conProgramDTO.ExecutableName + " is running");

                //Updated rakshith 12-07-2016
                conRequestDTO.Phase = "Running";
                conRequestDTO.Status = "Normal";

                int processId = 0;
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.ToLower().Contains(conProgramDTO.ExecutableName.ToLower().Replace(".exe", "")))
                    {
                        processId = clsProcess.Id;
                    }
                }

                conRequestDTO.ProcessId = processId;
                conRequestDTO.ActualStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
                ConcurrentRequests concurrentRequests = new ConcurrentRequests(Utilities.ExecutionContext, conRequestDTO);
                concurrentRequests.Save();

                log(conProgramDTO.ExecutableName + " is running.");

                Thread.Sleep(15000); //sleep thread for 15sec

                endRequest(Status.Normal.ToString());

                //Send request status to mail 
                SendEmail("Completed successfully", true);

                log("ends-execExe() method.");
            }

            catch (Exception ex)
            {
                log("Error: " + ex.Message);

                if (conRequestDTO.ErrorCount < 3)
                {
                    //update request as error
                    UpdateErrorRequest();
                }
                log("Error Count: " + conRequestDTO.ErrorCount);

                //When error occured 3 times update request as complete with errors
                if (conRequestDTO.ErrorCount >= 3)
                {
                    endRequest(Status.Error.ToString());

                    log("Request completed with error");

                    //Send error details to mail 
                    SendEmail("Completed with error :" + ex.Message + " <br/><br/> Please check", false);
                }
                log("ends-execExe() method.");
            }
        }

        void execLibraryMethod()
        {
            log("Starts-execLibraryMethod() method.");
            try
            {
                startRequest(0);
                ConcLib concLib = new ConcLib(Utilities, executionContext);
                Type thisType = concLib.GetType();
                MethodInfo theMethod = thisType.GetMethod(conProgramDTO.ExecutableName.ToString());
                if (theMethod == null)
                {
                    log("Invalid executable: " + conProgramDTO.ExecutableName.ToString());
                    return;
                }
                string ret = theMethod.Invoke(concLib, getArguments()).ToString();

                log(ret);
                if (ret.Equals("Success"))
                {
                    endRequest(Status.Normal.ToString());
                }
                else
                {
                    log(ret);
                    endRequest(Status.Error.ToString());
                }
                log("ends-execLibraryMethod() method.");

            }
            catch (Exception ex)
            {
                log("Error :" + ex.Message);
                endRequest(Status.Error.ToString());
                log("ends-execLibraryMethod() method.");
            }
        }

        //Get the sql arguments for running stored procedure
        void UpdateProcedureParameters()
        {
            log("starts-UpdateProcedureParameters() method.");

            List<string> argumentList = GetRequestArgumentsList();

            if (argumentList.Count > 0)
            {
                sqlParameter = new SqlParameter[argumentList.Count];
                for (int i = 0; i < argumentList.Count; i++)
                {
                    sqlArguments = sqlArguments + "@argument" + i.ToString() + ", ";
                    sqlParameter[i] = new SqlParameter("@argument" + i.ToString(), argumentList[i].ToString());
                }

                if (!string.IsNullOrEmpty(sqlArguments.Trim()))
                {
                    sqlArguments = sqlArguments.Substring(0, sqlArguments.Length - 2);
                }
            }

            log("ends-getExeArguments() method.");
        }

        //Get the arguments for non system programs
        List<string> GetRequestArgumentsList()
        {
            log("Starts GetRequestArgumentsList()");

            List<string> lstArguments = new List<string>();
            if (!string.IsNullOrEmpty(conRequestDTO.Argument1))
            {
                lstArguments.Add(conRequestDTO.Argument1);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument2))
            {
                lstArguments.Add(conRequestDTO.Argument2);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument3))
            {
                lstArguments.Add(conRequestDTO.Argument3);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument4))
            {
                lstArguments.Add(conRequestDTO.Argument4);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument5))
            {
                lstArguments.Add(conRequestDTO.Argument5);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument6))
            {
                lstArguments.Add(conRequestDTO.Argument6);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument7))
            {
                lstArguments.Add(conRequestDTO.Argument7);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument8))
            {
                lstArguments.Add(conRequestDTO.Argument8);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument9))
            {
                lstArguments.Add(conRequestDTO.Argument9);
            }
            if (!string.IsNullOrEmpty(conRequestDTO.Argument10))
            {
                lstArguments.Add(conRequestDTO.Argument10);
            }

            log("Ends- GetRequestArgumentsList()");
            return lstArguments;
        }

        //get the arguments for sysytem running program
        object[] getArguments()
        {
            log("starts-getArguments() method.");
            log("Starts- getArguments() for running system programs");

            int argCount = (conProgramDTO.ArgumentCount == -1) ? 0 : conProgramDTO.ArgumentCount;
            object[] args = new object[argCount + 2];

            if (argCount > 0)
            {
                for (int i = 1; i <= argCount; i++)
                {
                    switch (i)
                    {
                        case 1:
                            args[i - 1] = conRequestDTO.Argument1;
                            break;
                        case 2:
                            args[i - 1] = conRequestDTO.Argument2;
                            break;
                        case 3:
                            args[i - 1] = conRequestDTO.Argument3;
                            break;
                        case 4:
                            args[i - 1] = conRequestDTO.Argument4;
                            break;
                        case 5:
                            args[i - 1] = conRequestDTO.Argument5;
                            break;
                        case 6:
                            args[i - 1] = conRequestDTO.Argument6;
                            break;
                        case 7:
                            args[i - 1] = conRequestDTO.Argument7;
                            break;
                        case 8:
                            args[i - 1] = conRequestDTO.Argument8;
                            break;
                        case 9:
                            args[i - 1] = conRequestDTO.Argument9;
                            break;
                        case 10:
                            args[i - 1] = conRequestDTO.Argument10;
                            break;
                    }
                }
            }

            args[argCount] = conRequestDTO.RequestId;
            args[argCount + 1] = getLogFileName(conRequestDTO.RequestId);

            log("ends-getArguments() method.");
            log("Ends- getArguments() for running system programs");
            return args;
        }

        /// <summary>
        /// to Request status to registered mail
        /// </summary>
        /// <param name="message">error message to find</param>
        /// <param name="sucessEmail">check error mail or success</param>
        public void SendEmail(string message, bool sucessEmail)
        {
            try
            {
                string toMailId;

                if (sucessEmail)
                {
                    toMailId = conProgramDTO.SuccessNotificationMailId;
                }
                else
                {
                    toMailId = conProgramDTO.ErrorNotificationMailId;
                }

                if (!string.IsNullOrEmpty(toMailId))
                {
                    log("Start Sending Email notification from concurrent engine to " + toMailId);

                    string bodyText = conProgramDTO.ProgramName + " status as follows: <br/><br/> Status :" + (conRequestDTO.Status == "Normal" ? "Completed" : conRequestDTO.Status) + "<br/><br/> Start time : " + conRequestDTO.StartTime + "<br/><br/> End time : " + conRequestDTO.EndTime;
                    string subject = (string.IsNullOrEmpty(Utilities.ParafaitEnv.SiteName) ? "" : Utilities.ParafaitEnv.SiteName + ": ") + "Concurrent Request status: " + DateTime.Now.ToString("dd-MM-yyyy h:mm tt");

                    string[] MailId = toMailId.Split(',');
                    foreach (string email in MailId)
                    {
                        //  notification Email
                        SendEmailUI = new SendEmailUI(email,
                            "", "", subject, "Hi, <br/><br/> " + bodyText + " <br/><br/>" + message, "", "", true, Utilities);
                    }

                    log("End Sending Email notification from concurrent engine  to " + toMailId);
                }
            }

            catch (Exception ex)
            {
                log("Error: " + ex.Message);
                Utilities.EventLog.logEvent("ConcurrentProgramStatus", 'E', "Error: Sending Summary email: " + ex.Message, "", "ConcurrentProgramStatus", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
        }

        string getLogFileName(int RequestId)
        {
            return LogDirectory + RequestId.ToString() + ".log";
        }

        void startRequest(int ProcessId)
        {
            log("starts-startRequest(ProcessId) method.");

            string logFile = getLogFileName(conRequestDTO.RequestId);
            string logText = conProgramDTO.ProgramName.ToString();

            object[] args = getArguments();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                    logText += Environment.NewLine +
                    "Arg" + i.ToString() + ": " + args[i].ToString();
            }

            log(logFile, logText);

            // update the request status when request starts
            conRequestDTO.Phase = "Running";
            conRequestDTO.Status = "Normal";
            conRequestDTO.ProcessId = ProcessId;
            conRequestDTO.ActualStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture);
            ConcurrentRequests concurrentRequests = new ConcurrentRequests(Utilities.ExecutionContext, conRequestDTO);
            concurrentRequests.Save();

            log("ends-startRequest(ProcessId) method.");
        }

        /// <summary>
        /// Update the request as pending
        /// </summary>
        public void UpdateErrorRequest()
        {
            log("starts-UpdateErrorRequest() method.");
            conRequestDTO.Phase = "Pending";
            conRequestDTO.Status = "Error";
            conRequestDTO.ErrorCount = (conRequestDTO.ErrorCount == -1 ? 0 : conRequestDTO.ErrorCount) + 1;
            conRequestDTO.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            ConcurrentRequests concurrentRequests = new ConcurrentRequests(Utilities.ExecutionContext, conRequestDTO);
            concurrentRequests.Save();

            log("Update the request to error, error count:" + conRequestDTO.ErrorCount);

            string logFile = getLogFileName(conRequestDTO.RequestId);
            log(logFile, " Error occured while running the " + conProgramDTO.ProgramName + "Error count :" + conRequestDTO.ErrorCount);

            log("Ends-UpdateErrorRequest() method.");

        }

        /// <summary>
        /// endRequest method
        /// </summary>
        /// <param name="status"></param>
        public void endRequest(string status)
        {
            log("starts-endRequest(status) method.");

            // update the request status when request ends successfully 
            conRequestDTO.Phase = "Complete";
            conRequestDTO.Status = status;
            conRequestDTO.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            ConcurrentRequests concurrentRequests = new ConcurrentRequests(Utilities.ExecutionContext, conRequestDTO);
            concurrentRequests.Save();

            if (status == "Normal")
            {
                conProgramDTO.LastExecutedOn = conRequestDTO.StartTime;
                conProgramDTO.LastUpdatedDate = DateTime.Now;
                ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(Utilities.ExecutionContext,conProgramDTO);
                concurrentPrograms.Save();
            }

            string logFile = getLogFileName(conRequestDTO.RequestId);
            log(logFile, "Request Ended with status: " + status);

            log("ends-endRequest(status) method.");
        }

        void log(string fileName, string Message)
        {
            try
            {
                System.IO.File.AppendAllText(fileName, DateTime.Now.ToString("dd-MMM-yyyy H:mm:ss") + " - " + Message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                log("Error :" + ex.Message);
            }
        }

        //Adding Request logs
        void log(string Message)
        {
            try
            {
                string fileName = getLogFileName(conRequestDTO.RequestId);
                log(fileName, Message);
            }
            catch { }
        }
    }
}
