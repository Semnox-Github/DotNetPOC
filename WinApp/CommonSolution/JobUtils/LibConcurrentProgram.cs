/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - Library Concurrent Program Class for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *********************************************************************************************/
using System;
using System.IO;
using System.Reflection;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    class LibConcurrentProgram : ConcurrentPrograms
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COMPLETE_PHASE = "Complete";
        private const string ERROR_STATUS = "Error";
        private string fileName = string.Empty;

        public LibConcurrentProgram(Utilities utilities, ConcurrentProgramsDTO concurrentProgramsDTO, ConcurrentRequestsDTO concurrentRequestsDTO,
                                    string logFileName, DatabaseConnectorDTO dbConnectorDTO)
            : base(utilities.ExecutionContext, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO, utilities)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public override void LaunchRequests()
        {
            log.LogMethodEntry();
            try
            {
                fileName = getLogFileName();
                string root = dbConnectorDTO.ApplicationFolderPath;
                string dll = concurrentProgramsDTO.ExecutableName + ".dll";
                ConcurrentProgramHelper.WriteToLog(concurrentProgramsDTO.ExecutableName, fileName);
                string libraryName = root + "\\" + dll;
                if (!(File.Exists(libraryName)))
                {
                    try
                    {
                        string logMessage = "File: " + libraryName + " does not exist. ";
                        string emailMsg = "Completed with error : " + concurrentProgramsDTO.ExecutableName + " file not found" + " <br/><br/> Please check";
                        ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                        return;
                    }
                    catch (Exception ex)
                    {
                        ConcurrentProgramHelper.WriteToLog(ex.Message, fileName);
                        return;
                    }
                }
                Assembly libFile = Assembly.UnsafeLoadFrom(libraryName);
                if (libFile != null)
                {
                    object[] arg = getArguments();
                    Type type = libFile.GetType(concurrentProgramsDTO.ExecutableName.ToString());
                    if (type != null)
                    {
                        ConstructorInfo constructorN = type.GetConstructor(new Type[] { concurrentRequestsDTO.RequestId.GetType(), logFileName.GetType() ,
                                                                                    utilities .GetType()});
                        if (constructorN == null)
                        {
                            ConcurrentProgramHelper.UpdateConcurrentRequest(executionContext, concurrentRequestsDTO.RequestId, ERROR_STATUS, COMPLETE_PHASE);
                            ConcurrentProgramHelper.SendEmail(executionContext, "Method not found", false,
                                concurrentRequestsDTO);
                            ConcurrentProgramHelper.WriteToLog("Method not found", fileName);
                            return;
                        }
                        constructorN.Invoke(arg);
                        ConcurrentProgramHelper.WriteToLog(libraryName + "Invoked", fileName);
                    }
                else
                {
                    string logMessage = "Unable to load the assembly, Type not found" + libraryName;
                    string emailMsg = "Completed with error : " + concurrentProgramsDTO.ExecutableName + "Unable to load the assembly,Type not found" + " <br/><br/> Please check";
                    ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                }
            }
                else
                {
                    string logMessage = "Unable to load the assembly" + libraryName;
                    string emailMsg = "Completed with error : " + concurrentProgramsDTO.ExecutableName + "Unable to load the assembly" + " <br/><br/> Please check";
                    ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                }
            }
            catch (Exception ex)
            {
                string logMessage = ex.Message;
                string emailMsg = ex.Message;
                ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
                log.Error(ex);
            }
            finally
            {
                ConcurrentProgramHelper.WriteToLog("Ends Call request for" + concurrentProgramsDTO.ExecutableName, fileName);
            }
            log.LogMethodExit();
        }


        private object[] getArguments()
        {
            log.LogMethodEntry();
            object[] args = new object[3];
            args[0] = concurrentRequestsDTO.RequestId;
            args[1] = getLogFileName();
            args[2] = utilities;
            log.LogMethodExit(args);
            return args;
        }


        private string getLogFileName()
        {
            log.LogMethodEntry();
            log.LogMethodExit(logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log");
            return logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log";
        }
    }
}
