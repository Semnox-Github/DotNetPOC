using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Messaging;
using System.Windows.Forms;
////using Semnox.Parafait.ConcurrentManager;
//using Semnox.Parafait.Context;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using System.Globalization;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// class of ConcurrentProgramsLaunch
    /// </summary>
    public class ConcurrentProgramsLaunch
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        Semnox.Core.Utilities.ExecutionContext machineUserContext;

        enum Phase { Pending, Running, Complete };
        enum Status { Normal, Error, Aborted };

        string LogDirectory;

        Dictionary<Thread, object> ThreadList = new Dictionary<Thread, object>();        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        public ConcurrentProgramsLaunch(Utilities _Utilities)
        {
            log.Debug("Concurrent statred to instialize");

            Utilities = _Utilities;

            machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

            if (Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            
            UsersList usersListBL = new UsersList(machineUserContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters;
            searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, "ConCurrent Job User"));
            List<UsersDTO> usersListDTO = usersListBL.GetAllUsers(searchParameters);
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            machineUserContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
            if (usersListDTO != null)
            {                
                machineUserContext.SetUserId(usersListDTO[0].LoginId);
                machineUserContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
                machineUserContext.SetUserPKId(-1);
            }
            //machineUserContext.SetUserId(Utilities.ParafaitEnv.LoginID);
            //machineUserContext.SetUserId(Utilities.ParafaitEnv.Username);
            Utilities.ParafaitEnv.Initialize();
            UpdateDirectory();

            log.Debug("Concurrent engine initialized");
        }

        /// <summary>
        /// first time load LoadConcurrentEngine()
        /// </summary>
        public void LoadConcurrentEngine()
        {
            //Debugger.Launch();
            log.Debug("Concurrent engine is loading");

            //Clean all requests
            CleanupRequests();

            //insert system programs
            InsertRequests(true);

            //check the request
            CheckRequests();

            //Wait for Message queue
            WaitForMessages();
        }

        /// <summary>
        /// WaitForMessages method 
        /// </summary>
        void WaitForMessages()
        {
            log.Debug("Starts-WaitForMessages() method.");

            MessageQueue messageQueue = MessageQueueUtils.GetMessageQueue("PrimaryServerCM");
            while (true)
            {
                System.Messaging.Message msg = null;
                try
                {
                    msg = messageQueue.Receive(TimeSpan.FromSeconds(0.2));
                }
                catch { }

                if (msg == null)
                    break;
            }

            messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(MyReceiveCompleted);
            messageQueue.BeginReceive();

            log.Debug("Ends-WaitForMessages() method.");
        }

        // Provides an event handler for the ReceiveCompleted event.
        private void MyReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            log.Debug("Starts-MyReceiveCompleted() method.");

            // Connect to the queue.
            MessageQueue queue = (MessageQueue)source;

            // End the asynchronous receive operation.
            System.Messaging.Message msg = queue.EndReceive(asyncResult.AsyncResult);

            msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
            string QueueMessage = msg.Body.ToString();
            if (!QueueMessage.Equals("SHUTDOWN"))
            {
                queue.BeginReceive();
            }

            log.Debug("Ends-MyReceiveCompleted() event.");
        }

        /// <summary>
        /// clean the request which are running
        /// </summary>
        void CleanupRequests()
        {
            log.Debug("Starts-CleanupRequests() method.");

            ConcurrentRequests concurrentRequests;
            ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestList.GetCleanupRequests();

            if (concurrentRequestsDTOList != null)
            {
                foreach (ConcurrentRequestsDTO conReqDTO in concurrentRequestsDTOList)
                {
                    try
                    {
                        log.Debug("Killing the running process");
                        System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(Convert.ToInt32(conReqDTO.ProcessId));
                        proc.Kill();
                    }
                    catch { }

                    //Update the request to complete with aborted
                    conReqDTO.Phase = Phase.Complete.ToString();
                    conReqDTO.Status = Status.Aborted.ToString();
                    conReqDTO.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    concurrentRequests = new ConcurrentRequests(machineUserContext, conReqDTO);
                    concurrentRequests.Save();
                    //End
                }
            }

            log.Debug("Ends-CleanupRequests() method");
        }

        void UpdateDirectory()
        {
            log.Debug("Starts-UpdateDirectory() method");

            string date = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + DateTime.Now.ToString("dd");
            string directory = Application.StartupPath + "\\log\\" + date;

            try
            {
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }
            }
            catch { }

            LogDirectory = directory + "\\";

            log.Debug("Ends-UpdateDirectory() method");
        }

        void InsertRequests(bool IncludeSystemPrograms)
        {
           
            log.Debug("Starts-InsertRequests(" + IncludeSystemPrograms + ") method.");

            ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
            ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO;

            ConcurrentProgramSchedules concurrentProgramSchedules;
            ConcurrentRequests concurrentRequests;

            // check and insert new requests
            DataTable dt = concurrentRequestList.GetConcurrentProgramScheduleDue(IncludeSystemPrograms);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                log.Debug("Adding new request");
                ConcurrentRequestsDTO concurrentRequestsDTO = new ConcurrentRequestsDTO();

                concurrentRequestsDTO.ProgramId = Convert.ToInt32(dt.Rows[i]["ProgramId"]);
                concurrentRequestsDTO.ProgramScheduleId = Convert.ToInt32(dt.Rows[i]["ProgramScheduleId"]);
                concurrentRequestsDTO.RequestedBy = Utilities.ParafaitEnv.Username;
                concurrentRequestsDTO.RequestedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                concurrentRequestsDTO.ActualStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                concurrentRequestsDTO.StartTime = Convert.ToDateTime(dt.Rows[i]["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                concurrentRequestsDTO.Phase = Phase.Pending.ToString();
                concurrentRequestsDTO.Status = Status.Normal.ToString();
                concurrentRequestsDTO.RelaunchOnExit = false;
                concurrentRequestsDTO.SynchStatus = false;
                concurrentRequestsDTO.ProcessId = 0;

                #region Assign Argumnet values
                ConcurrentProgramArgumentList programArguments = new ConcurrentProgramArgumentList();
                List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>(ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.PROGRAM_ID, concurrentRequestsDTO.ProgramId.ToString()));
                //searchParameters.Add(new KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>(ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters.IS_ACTIVE, "1"));
                List<ConcurrentProgramArgumentsDTO> programArgumentList = programArguments.GetConcurrentProgramArguments(searchParameters);
                if (programArgumentList != null && programArgumentList.Count > 0)
                {
                    int argumentNumber = 1;
                    foreach (ConcurrentProgramArgumentsDTO d in programArgumentList)
                    {
                        if (d.ArgumentType.Equals("D"))
                        {
                            if (string.IsNullOrEmpty(d.ArgumentValue) || (!string.IsNullOrEmpty(d.ArgumentValue) && d.ArgumentValue.Equals("null")))
                            {
                                log.Error("Date value not specified. You could specify getdate()- for current date or could give the format of datetime/date(yyyy-MM-27 12:mm) etc way. This will repalce the current date time values where the date time alphabets are present.");
                            }
                            else
                            {
                                if (d.ArgumentValue.ToLower().Equals("getdate()"))
                                {
                                    d.ArgumentValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//using getdate() one can pass the current datetime in the task parameter. 
                                }
                                else
                                {
                                    try
                                    {
                                        d.ArgumentValue = DateTime.Now.ToString(d.ArgumentValue);//Using this we can set some specipic date or month or year or time in a argument
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Error in argument convertion the datetime to " + d.ArgumentValue + " format.", ex);
                                    }
                                }
                            }
                        }
                        switch (argumentNumber)
                        {
                            case 1:
                                concurrentRequestsDTO.Argument1 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 2:
                                concurrentRequestsDTO.Argument2 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 3:
                                concurrentRequestsDTO.Argument3 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 4:
                                concurrentRequestsDTO.Argument4 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 5:
                                concurrentRequestsDTO.Argument5 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 6:
                                concurrentRequestsDTO.Argument6 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 7:
                                concurrentRequestsDTO.Argument7 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 8:
                                concurrentRequestsDTO.Argument8 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 9:
                                concurrentRequestsDTO.Argument9 = d.ArgumentValue;
                                argumentNumber++; break;
                            case 10:
                                concurrentRequestsDTO.Argument10 = d.ArgumentValue;
                                argumentNumber++; break;
                        }
                    }
                }
                #endregion

                // Insert new request
                concurrentRequests = new ConcurrentRequests(machineUserContext, concurrentRequestsDTO);
                concurrentRequests.Save();
                log.Debug("Request added " + concurrentRequestsDTO.RequestId);

                // update Latest start time to the program schedule 
                if (Convert.ToInt32(dt.Rows[i]["ProgramScheduleId"]) != 0)
                {
                    concurrentProgramSchedulesDTO = new ConcurrentProgramScheduleList(machineUserContext).GetConProgramSchedule(Convert.ToInt32(dt.Rows[i]["ProgramScheduleId"]));
                    concurrentProgramSchedulesDTO.LastExecutedOn = dt.Rows[i]["StartTime"].ToString();
                    concurrentProgramSchedulesDTO.LastUpdatedUser = Utilities.ParafaitEnv.Username;                    
                    concurrentProgramSchedules = new ConcurrentProgramSchedules(machineUserContext, concurrentProgramSchedulesDTO);
                    concurrentProgramSchedules.Save();
                }
            }
            log.Debug("Ends-InsertRequests(" + IncludeSystemPrograms + ") method.");
        }

        /// <summary>
        /// Check the request to run the programs
        /// </summary>
        public void CheckRequests()
        {
            //Debugger.Launch();
            log.Debug("----------------------------------------------------------------------------------------------");
            log.Debug("Starts-CheckRequests method.");

            // call insert requests to insert program requests including systm program
            InsertRequests(false);

            ConcurrentProgramsDTO concurrentProgramsDTO = new ConcurrentProgramsDTO();

            ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();

            log.Debug("Checking the request to run.");
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestList.GetConcurrentRequestsScheduledToRun(this.Utilities.ExecutionContext.GetSiteId());

            try
            {
                List<string> lstExe = new List<string>();
                if (concurrentRequestsDTOList != null)
                {
                    UpdateDirectory();

                    bool IsRunning = false;
                    int processId = 0;
                    // Loop through the requests scheduled to run
                    foreach (ConcurrentRequestsDTO conReqDTO in concurrentRequestsDTOList)
                    {
                        concurrentProgramsDTO = new ConcurrentProgramList(machineUserContext).GetConcurrentProgram(conReqDTO.ProgramId);

                        IsRunning = false;

                        if (concurrentProgramsDTO.ExecutionMethod == "E")
                        {
                            log.Debug("Checking " + concurrentProgramsDTO.ExecutableName + " running or not");

                            foreach (Process clsProcess in Process.GetProcesses())
                            {
                                if (clsProcess.ProcessName.ToLower().Contains(concurrentProgramsDTO.ExecutableName.ToLower().Replace(".exe", "")))
                                {
                                    IsRunning = true;
                                    processId = clsProcess.Id;
                                }
                            }
                        }

                        // Exclude same programs running 
                        if (lstExe.Contains(concurrentProgramsDTO.ExecutableName.ToLower()) == false && IsRunning == false)
                        {
                            log.Debug("created thread to run " + concurrentProgramsDTO.ExecutableName);

                            ThreadStart thread = delegate
                            {
                                new Launcher(Utilities, concurrentProgramsDTO, conReqDTO, LogDirectory, machineUserContext);
                            };

                            lstExe.Add(concurrentProgramsDTO.ExecutableName.ToLower());

                            if (concurrentProgramsDTO.ExecutionMethod == "L")
                            {
                                MessageQueueUtils.GetMessageQueue(conReqDTO.RequestId);
                            }

                            Thread thr = new Thread(new ThreadStart(thread));
                            ThreadList.Add(thr, conReqDTO.RequestId);
                            thr.Start();
                        }
                        else if (IsRunning == true && concurrentProgramsDTO.ExecutionMethod == "E")
                        {
                            log.Debug(concurrentProgramsDTO.ExecutableName + " is running update to request as complete");

                            Launcher launcher = new Launcher(Utilities, concurrentProgramsDTO, conReqDTO, LogDirectory, true, machineUserContext);

                            conReqDTO.ProcessId = processId;
                            ConcurrentRequests concurrentRequests = new ConcurrentRequests(machineUserContext, conReqDTO);
                            concurrentRequests.Save();
                            launcher.endRequest(Launcher.Status.Normal.ToString());
                        }
                    }
                }
            }
            catch (Exception ex){
                log.Error("Caught error while checking requests" + ex.Message);
            }
            log.Debug("Ends-CheckRequests() method.");
            log.Debug("----------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// shutdown the threads 
        /// </summary>
        public void CloseConcurrentProgramsLaunch()
        {
            log.Debug("Starts-CloseConcurrentProgramsLaunch() method.");

            //this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            foreach (KeyValuePair<Thread, object> thr in ThreadList)
            {
                if (thr.Key.IsAlive)
                {
                    log.Debug("Shutting down Request: " + thr.Value.ToString() + Environment.NewLine);
                    MessageQueueUtils.SendMessage(thr.Value, "SHUTDOWN");
                }
            }

            log.Debug("Giving 30 sec to all threads to complete the task");
            foreach (KeyValuePair<Thread, object> thr in ThreadList)
            {
                if (thr.Key.IsAlive)
                {
                    log.Debug("Waiting for Request " + thr.Value.ToString() + " to exit" + Environment.NewLine);
                    Application.DoEvents();
                    thr.Key.Join(30000);
                }
            }

            Application.DoEvents();
            foreach (KeyValuePair<Thread, object> thr in ThreadList)
            {
                if (thr.Key.IsAlive)
                {
                    log.Debug("killing the threads");
                    thr.Key.Abort();
                }
            }
            CleanupRequests();
        }
    }
}
