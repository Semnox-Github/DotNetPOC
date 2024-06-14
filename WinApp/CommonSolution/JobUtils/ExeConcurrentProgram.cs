/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - Exe Concurrent Program Class for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *2.150.6       09-Nov-2023   Abhishek            Modified : The exe based application invoke, as a fix to remote backup.
 *********************************************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    class ExeConcurrentProgram : ConcurrentPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ERROR_STATUS = "Error";
        private const string COMPLETE_PHASE = "Complete";
        private const string RUNNING_PHASE = "Running";
        private const string PENDING_PHASE = "Pending";
        private const string NORMAL_STATUS = "Normal";
        private string fileName = string.Empty;

        /// <summary>
        /// ExeConcurrentProgram
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="conProgramDTO"></param>
        /// <param name="logFileName"></param>
        public ExeConcurrentProgram(Utilities utilities, ConcurrentProgramsDTO concurrentProgramsDTO, ConcurrentRequestsDTO concurrentRequestsDTO,
           string logFileName, DatabaseConnectorDTO dbConnectorDTO)
           : base(utilities.ExecutionContext, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO, utilities)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public override void LaunchRequests()
        {
            log.LogMethodEntry();
            //Debugger.Launch();
            string exe;
            try
            {
                fileName = getLogFileName();
                string root = dbConnectorDTO.ApplicationFolderPath;
                exe = concurrentProgramsDTO.ExecutableName.ToString();
                ConcurrentProgramHelper.WriteToLog(concurrentProgramsDTO.ExecutableName.ToString(), fileName);
                string applicationName = root + "\\" + exe;
                if (!(File.Exists(applicationName)))
                {
                    string logMessage = "File: " + applicationName + " does not exist. ";
                    string emailMsg = "Completed with error : " + concurrentProgramsDTO.ExecutableName + " file not found" + " <br/><br/> Please check";
                    ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                    log.LogMethodExit("Application exe not found: " + applicationName);
                    return;
                }
                ConcurrentProgramHelper.WriteToLog("Start the process", fileName);
                if (concurrentProgramsDTO.MutlipleInstanceRunAllowed == false)
                {
                    try
                    {
                        ConcurrentProgramHelper.AnotherInstanceOfProgramIsRunning(executionContext, concurrentProgramsDTO.ProgramId, concurrentRequestsDTO.Guid, concurrentProgramsDTO.ProgramName);
                    }
                    catch (AnotherInstanceOfProgramIsRunningException ex)
                    {
                        ConcurrentProgramHelper.UpdateRequestCompletePhaseForAnotherInstanceRunningEx(utilities.ExecutionContext, concurrentRequestsDTO.Guid);
                        log.Error("AnotherInstanceOfProgramIsRunningException Error: " + ex);
                        return;
                    }
                    catch (Exception e)
                    {
                        ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, ERROR_STATUS, COMPLETE_PHASE);
                        log.Error("Exception: " + e);
                        return;
                    }
                }
                Process process = Process.Start(applicationName, concurrentRequestsDTO.Guid);
                int processId = process.Id;
                try
                {
                    if (processId > 0)
                    {
                        ConcurrentProgramHelper.UpdateConcurrentRequest(executionContext, concurrentRequestsDTO.RequestId, NORMAL_STATUS, RUNNING_PHASE,
                            null, processId);
                        ConcurrentProgramHelper.WriteToLog("Process Id" + processId, fileName);
                        int count = -1;
                        while (count < 3)
                        {
                            try
                            {
                                Process p = Process.GetProcessById(processId);
                                ConcurrentProgramHelper.WriteToLog("Process is Running", fileName);
                                break;
                            }
                            catch (Exception ex)
                            {
                                ConcurrentProgramHelper.WriteToLog(count + ex.Message, fileName);
                                System.Threading.Thread.Sleep(50);
                                count++;
                                if (count >= 3)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        string logMessage = "Unable to launch the exe.";
                        string emailMsg = "Completed with error : " + concurrentProgramsDTO.ExecutableName + " Unable to launch the exe" + " <br/><br/> Please check";
                        ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string logMessage = ex.Message;
                    string emailMsg = "Unable to Launch the process." + " <br/><br/> Please check Event Log for more details";
                    ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string logMessage = ex.Message;
                string emailMsg = ex.Message + " <br/><br/> Please check";
                ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
            }
            finally
            {
                ConcurrentProgramHelper.WriteToLog("Ends Call request for" + concurrentProgramsDTO.ExecutableName, fileName);
            }
        }

        private string getLogFileName()
        {
            log.LogMethodEntry();
            log.LogMethodExit(logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log");
            return logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log";
        }
    }
}
